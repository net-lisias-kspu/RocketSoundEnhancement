﻿using ModuleWheels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketSoundEnhancement
{
    class RSE_Wheels : PartModule
    {
        Dictionary<string, List<SoundLayer>> SoundLayerGroups = new Dictionary<string, List<SoundLayer>>();
        Dictionary<string, AudioSource> Sources = new Dictionary<string, AudioSource>();
        Dictionary<string, float> spools = new Dictionary<string, float>();

        GameObject audioParent;
        bool initialized;
        bool gamePaused;

        [KSPField(isPersistant = false)]
        public float volume = 1;

        ModuleWheelBase moduleWheel;
        ModuleWheelMotor moduleMotor;
        ModuleWheelDeployment moduleDeploy;

        public override void OnStart(StartState state)
        {
            if(state == StartState.Editor || state == StartState.None)
                return;

            string partParentName = part.name + "_" + this.moduleName;
            audioParent = part.gameObject.GetChild(partParentName);
            if(audioParent == null) {
                audioParent = new GameObject(partParentName);
                audioParent.transform.rotation = part.transform.rotation;
                audioParent.transform.position = part.transform.position;
                audioParent.transform.parent = part.transform;
            }

            moduleWheel = part.GetComponent<ModuleWheelBase>();
            moduleMotor = part.GetComponent<ModuleWheelMotor>();
            moduleDeploy = part.GetComponent<ModuleWheelDeployment>();

            var configNode = AudioUtility.GetConfigNode(part.partInfo.name, this.moduleName);

            foreach(var node in configNode.GetNodes()) {

                string _wheelState = node.name;

                var soundLayers = AudioUtility.CreateSoundLayerGroup(node.GetNodes("SOUNDLAYER"));
                if(soundLayers.Count > 0) {
                    if(SoundLayerGroups.ContainsKey(_wheelState)) {
                        SoundLayerGroups[_wheelState].AddRange(soundLayers);
                    } else {
                        SoundLayerGroups.Add(_wheelState, soundLayers);
                    }
                }
            }

            GameEvents.onGamePause.Add(onGamePause);
            GameEvents.onGameUnpause.Add(onGameUnpause);

            initialized = true;
        }

        bool running;
        bool motorEnabled;
        bool isConcrete = false;
        bool isRetracted = false;
        float driveOutput;

        public override void OnUpdate()
        {
            if(!initialized || !moduleWheel || !moduleWheel.Wheel || !audioParent || gamePaused)
                return;

            if(moduleMotor) {
                running = moduleMotor.state == ModuleWheelMotor.MotorState.Running;
                motorEnabled = moduleMotor.motorEnabled;
                driveOutput = moduleMotor.driveOutput;
            }

            if(moduleDeploy) {
                isRetracted = moduleDeploy.stateString == "Retracted";
            }

            WheelHit hit;
            if(moduleWheel.Wheel.wheelCollider.GetGroundHit(out hit)) {
                var groundTag = hit.collider.gameObject.tag.ToLower();
                if(groundTag.Contains("runway") || groundTag.Contains("ksc")) {
                    isConcrete = true;
                }
            }

            foreach(var soundLayerGroup in SoundLayerGroups) {
                string soundLayerKey = soundLayerGroup.Key;
                float control = 0;

                if(!isRetracted) {
                    switch(soundLayerGroup.Key) {
                        case "Torque":
                            control = running ? driveOutput / 100 : 0;
                            break;
                        case "Speed":
                            control = motorEnabled ? Mathf.Abs(moduleWheel.Wheel.WheelRadius * moduleWheel.Wheel.wheelCollider.angularVelocity) : 0;
                            break;
                        case "Steer":
                            control = moduleWheel.Wheel.steerState;
                            break;
                        case "Ground":
                            control = moduleWheel.isGrounded ? Mathf.Abs(moduleWheel.Wheel.speed) : 0;
                            break;
                        case "Slip":
                            control = moduleWheel.isGrounded ? Mathf.Abs(moduleWheel.slipDisplacement.magnitude) : 0;
                            break;
                        default:
                            continue;
                    }
                }

                foreach(var soundLayer in soundLayerGroup.Value) {
                    float finalControl = control;

                    if(soundLayerKey == "Ground" || soundLayerKey == "Slip") {
                        if(soundLayer.name.ToLower().Contains("-dirt") && isConcrete || soundLayer.name.ToLower().Contains("-concrete") && !isConcrete) {
                            finalControl = 0;
                        }
                    }

                    if(soundLayer.spool) {
                        if(!spools.ContainsKey(soundLayer.name)) {
                            spools.Add(soundLayer.name, 0);
                        }

                        spools[soundLayer.name] = Mathf.MoveTowards(spools[soundLayer.name], finalControl, soundLayer.spoolTime * TimeWarp.deltaTime);
                        finalControl = spools[soundLayer.name];
                    }

                    if(finalControl < 0.01f) {
                        if(Sources.ContainsKey(soundLayer.name)) {
                            UnityEngine.Object.Destroy(Sources[soundLayer.name]);
                            Sources.Remove(soundLayer.name);
                        }
                        continue;
                    }

                    AudioSource source;
                    if(Sources.ContainsKey(soundLayer.name)) {
                        source = Sources[soundLayer.name];
                    } else {
                        source = AudioUtility.CreateSource(audioParent, soundLayer);
                        Sources.Add(soundLayer.name, source);
                    }

                    source.volume = soundLayer.volume.Value(finalControl) * volume * HighLogic.CurrentGame.Parameters.CustomParams<Settings>().ShipVolume;
                    source.pitch = soundLayer.pitch.Value(finalControl);

                    AudioUtility.PlayAtChannel(source, soundLayer.channel, soundLayer.loop, soundLayer.loopAtRandom);
                }
            }

            if(Sources.Count > 0) {
                var sourceKeys = Sources.Keys.ToList();
                foreach(var source in sourceKeys) {
                    if(!Sources[source].isPlaying) {
                        UnityEngine.Object.Destroy(Sources[source]);
                        Sources.Remove(source);
                    }
                }
            }
        }

        private void onGameUnpause()
        {
            gamePaused = false;
            if(Sources.Count > 0) {
                foreach(var source in Sources.Values) {
                    source.UnPause();
                }
            }
        }

        private void onGamePause()
        {
            gamePaused = true;
            if(Sources.Count > 0) {
                foreach(var source in Sources.Values) {
                    source.Pause();
                }
            }
        }

        void OnDestroy()
        {
            if(Sources.Count() > 0) {
                foreach(var source in Sources.Keys) {
                    GameObject.Destroy(Sources[source]);
                }
            }

            GameEvents.onGamePause.Remove(onGamePause);
            GameEvents.onGameUnpause.Remove(onGameUnpause);
        }
    }
}
