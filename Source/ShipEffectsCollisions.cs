﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketSoundEnhancement
{
    public enum CollisionType
    {
        CollisionEnter,
        CollisionStay,
        CollisionExit
    }

    public class ShipEffectsCollisions : PartModule
    {
        Dictionary<CollisionType, List<SoundLayer>> SoundLayerGroups = new Dictionary<CollisionType, List<SoundLayer>>();
        Dictionary<string, AudioSource> Sources = new Dictionary<string, AudioSource>();

        public bool collided;

        public override void OnStart(StartState state)
        {
            if(state == StartState.Editor || state == StartState.None)
                return;

            var configNode = AudioUtility.GetConfigNode(part.partInfo.name, this.moduleName);

            foreach(var groupNode in configNode.GetNodes()) {
                var soundLayerNodes = groupNode.GetNodes("SOUNDLAYER");
                CollisionType collisionType;

                if(Enum.TryParse(groupNode.name, out collisionType)) {
                    var soundLayers = AudioUtility.CreateSoundLayerGroup(soundLayerNodes);
                    if(SoundLayerGroups.ContainsKey(collisionType)) {
                        SoundLayerGroups[collisionType].AddRange(soundLayers);
                    } else {
                        SoundLayerGroups.Add(collisionType, soundLayers);
                    }
                }
            }
        }

        public override void OnUpdate()
        {
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

        void OnDestroy()
        {
            if(Sources.Count > 0) {
                foreach(var source in Sources.Values) {
                    source.Stop();
                    UnityEngine.Object.Destroy(source);
                }
            }
        }

        void OnCollisionEnter(Collision col)
        {
            var collisionType = AudioUtility.GetCollidingType(col.collider);

            if(SoundLayerGroups.ContainsKey(CollisionType.CollisionEnter)) {
                PlaySounds(CollisionType.CollisionEnter, col.relativeVelocity.magnitude, collisionType, true);
            }

            collided = true;
        }

        void OnCollisionStay(Collision col)
        {
            var collisionType = AudioUtility.GetCollidingType(col.collider);
            if(SoundLayerGroups.ContainsKey(CollisionType.CollisionStay)) {
                PlaySounds(CollisionType.CollisionStay, col.relativeVelocity.magnitude, collisionType);
            }
        }

        void OnCollisionExit(Collision col)
        {
            if(SoundLayerGroups.ContainsKey(CollisionType.CollisionStay)) {
                foreach(var layer in SoundLayerGroups[CollisionType.CollisionStay]) {
                    if(Sources.ContainsKey(layer.name)) {
                        Sources[layer.name].Stop();
                    }
                }
            }

            var collisionType = AudioUtility.GetCollidingType(col.collider);
            if(SoundLayerGroups.ContainsKey(CollisionType.CollisionExit)) {
                PlaySounds(CollisionType.CollisionExit, col.relativeVelocity.magnitude, collisionType, true);
            }
            collided = false;
        }

        void PlaySounds(CollisionType collisionType, float control, CollidingObject collidingObjectType = CollidingObject.None, bool oneshot = false)
        {
            bool concreteLayerData = SoundLayerGroups[collisionType].Where(x => x.data.ToLower().Contains("concrete")).Count() > 0;
            bool vesselLayerData = SoundLayerGroups[collisionType].Where(x => x.data.ToLower().Contains("vessel")).Count() > 0;

            foreach(var soundLayer in SoundLayerGroups[collisionType]) {
                float finalVolume = soundLayer.volume.Value(control) * soundLayer.massToVolume.Value(control);

                var layerMaskName = soundLayer.data.ToLower();
                switch(collidingObjectType) {
                    case CollidingObject.Concrete:
                        if(!layerMaskName.Contains("concrete") && concreteLayerData)
                            finalVolume = 0;
                        break;
                    case CollidingObject.Vessel:
                        if(!layerMaskName.Contains("vessel") && vesselLayerData)
                            finalVolume = 0;
                        break;
                    case CollidingObject.None:
                        if(layerMaskName.Contains("concrete") || layerMaskName.Contains("vessel"))
                            finalVolume = 0;
                        break;
                }

                if(finalVolume > float.Epsilon) {
                    if(!Sources.ContainsKey(soundLayer.name)) {
                        Sources.Add(soundLayer.name, AudioUtility.CreateSource(part.gameObject, soundLayer));
                    }

                    var source = Sources[soundLayer.name];

                    if(source == null)
                        return;

                    source.volume = finalVolume * HighLogic.CurrentGame.Parameters.CustomParams<Settings>().EffectsVolume;
                    source.pitch = soundLayer.pitch.Value(control) * soundLayer.massToPitch.Value(part.vessel.GetComponent<ShipEffects>().TotalMass);

                    bool loop = source.loop;
                    if(oneshot) {
                        source.volume *= UnityEngine.Random.Range(0.8f, 1.0f);
                        source.pitch *= UnityEngine.Random.Range(0.95f, 1.05f);
                        loop = false;
                    }

                    AudioUtility.PlayAtChannel(source, soundLayer.channel, loop, oneshot);
                } else {
                    if(Sources.ContainsKey(soundLayer.name) && Sources[soundLayer.name].isPlaying) {
                        Sources[soundLayer.name].Stop();
                    }
                }

            }

        }
    }
}
