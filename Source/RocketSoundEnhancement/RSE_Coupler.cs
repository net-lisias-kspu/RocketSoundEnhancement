using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketSoundEnhancement
{
    public class RSE_Coupler : PartModule
    {
        public List<SoundLayer> SoundLayers = new List<SoundLayer>();
        public Dictionary<string, AudioSource> Sources = new Dictionary<string, AudioSource>();

        FXGroup fxGroup;
        GameObject audioParent;
        bool isDecoupler;

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

            ConfigNode configNode = AudioUtility.GetConfigNode(part.partInfo.name, this.moduleName);

            SoundLayers = AudioUtility.CreateSoundLayerGroup(configNode.GetNodes("SOUNDLAYER"));

            if(part.isLaunchClamp()) {
                fxGroup = part.findFxGroup("activate");
                isDecoupler = true;
            }

            if(part.GetComponent<ModuleDecouplerBase>()) {
                fxGroup = part.findFxGroup("decouple");
                isDecoupler = true;
            }

            if(fxGroup != null) {
                if(SoundLayers.Where(x => x.name == fxGroup.name).Count() > 0) {
                    SoundLayer soundLayer = SoundLayers.Find(x => x.name == fxGroup.name);
                    if(soundLayer.audioClips != null) {
                        AudioClip clip = GameDatabase.Instance.GetAudioClip(soundLayer.audioClips[0]);
                        if(clip != null) {
                            fxGroup.sfx = clip;
                            fxGroup.audio = AudioUtility.CreateOneShotSource(
                                audioParent,
                                soundLayer.volume * HighLogic.CurrentGame.Parameters.CustomParams<Settings>().ShipVolume,
                                soundLayer.pitch,
                                soundLayer.maxDistance,
                                soundLayer.spread);

                            Sources.Add(soundLayer.name, fxGroup.audio);
                        }
                    }
                }
            }

            GameEvents.onGameUnpause.Add(onGameUnpause);
            GameEvents.onDockingComplete.Add(onDock);
            GameEvents.onPartUndockComplete.Add(onUnDock);
        }

        private void onUnDock(Part data)
        {
            if(part.flightID == data.flightID && !isDecoupler) {
                PlaySound("undock");
            }
        }

        private void onDock(GameEvents.FromToAction<Part, Part> data)
        {
            if(part.flightID == data.from.flightID && !isDecoupler) {
                PlaySound("dock");
            }
        }

        private void onGameUnpause()
        {
            foreach(SoundLayer sound in SoundLayers) {
                if(Sources.ContainsKey(sound.name)) {
                    Sources[sound.name].volume = sound.volume * HighLogic.CurrentGame.Parameters.CustomParams<Settings>().ShipVolume;
                }
            }
        }

        public override void OnUpdate()
        {
            if(!HighLogic.LoadedSceneIsFlight)
                return;

            KeyValuePair<string, AudioSource>[] itemsToRemove = Sources.Where(x => x.Key != "decouple" && x.Key != "activate" && !Sources[x.Key].isPlaying).ToArray();
            foreach(KeyValuePair<string, AudioSource> item in itemsToRemove) {
                UnityEngine.Object.Destroy(Sources[item.Key]);
                Sources.Remove(item.Key);
            }
        }

        public void PlaySound(string action)
        {
            if(SoundLayers.Where(x => x.name == action).Count() > 0) {
                SoundLayer soundLayer = SoundLayers.Find(x => x.name == action);

                if(soundLayer.audioClips == null)
                    return;

                AudioSource source;
                if(Sources.ContainsKey(action)) {
                    source = Sources[action];
                } else {
                    source = AudioUtility.CreateOneShotSource(
                        audioParent,
                        soundLayer.volume * HighLogic.CurrentGame.Parameters.CustomParams<Settings>().ShipVolume,
                        soundLayer.pitch,
                        soundLayer.maxDistance,
                        soundLayer.spread);
                    Sources.Add(soundLayer.name, source);
                }

                AudioClip clip = GameDatabase.Instance.GetAudioClip(soundLayer.audioClips[0]);
                if(clip != null) {
                    source.PlayOneShot(clip);
                }
            }
        }

        void OnDestroy()
        {
            foreach(string source in Sources.Keys) {
                GameObject.Destroy(Sources[source]);
            }

            GameEvents.onGameUnpause.Remove(onGameUnpause);
            GameEvents.onDockingComplete.Remove(onDock);
            GameEvents.onPartUndockComplete.Remove(onUnDock);
        }
    }
}
