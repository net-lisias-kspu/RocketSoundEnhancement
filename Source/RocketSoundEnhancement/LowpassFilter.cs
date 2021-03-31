﻿using System.Reflection;
using UnityEngine;

namespace RocketSoundEnhancement
{
    public class LowpassFilterSettings : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Audio Muffler"; } }
        public override string Section { get { return "Rocket Sound Enhancement"; } }
        public override string DisplaySection { get { return "Rocket Sound Enhancement"; } }
        public override int SectionOrder { get { return 2; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override bool HasPresets { get { return false; } }

        [GameParameters.CustomParameterUI("Enable Muffling")]
        public bool EnableMuffling = true;

        [GameParameters.CustomIntParameterUI("Internal Camera - Atmosphere Muffling", toolTip = "Amount of outside environment muffling while Internal Camera (IVA) is Active in an Atmosphere (hz)", minValue = 10, maxValue = 22200)]
        public int InteriorMufflingAtm = 3000;

        [GameParameters.CustomIntParameterUI("Internal Camera - Vacuum Muffling", toolTip = "Amount of outside environment muffling while the Internal Camera (IVA) is Active in a Vacuum (hz)", minValue = 10, maxValue = 22200)]
        public int InteriorMufflingVac = 1500;

        [GameParameters.CustomIntParameterUI("Vacuum", toolTip = "Amount of Vacuum Muffling (hz)", minValue = 10, maxValue = 22200)]
        public int VacuumMuffling = 300;

        [GameParameters.CustomParameterUI("Muffle Chatterer", toolTip = "Muffle Chatterer when Internal Camera (IVA) is not Active")]
        public bool MuffleChatterer = false;

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return true;
        }
        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }
    }

    /// <summary>
    /// Thanks to Iron-Warrior for source: https://forum.unity.com/threads/custom-low-pass-filter-using-onaudiofilterread.976326/
    /// </summary>
    [RequireComponent(typeof(AudioBehaviour))]
    public class LowpassFilter : MonoBehaviour
    {
        private float[] inputHistoryLeft = new float[2];
        private float[] inputHistoryRight = new float[2];


        private float[] outputHistoryLeft = new float[3];
        private float[] outputHistoryRight = new float[3];

        private float c, a1, a2, a3, b1, b2;

        public float cutoffFrequency = 22200;
        public float lowpassResonanceQ = 1;

        int sampleRate;

        private void Awake()
        {
            sampleRate = AudioSettings.outputSampleRate;
            inputHistoryLeft[1] = 0;
            inputHistoryLeft[0] = 0;

            outputHistoryLeft[2] = 0;
            outputHistoryLeft[1] = 0;
            outputHistoryLeft[0] = 0;

            inputHistoryRight[1] = 0;
            inputHistoryRight[0] = 0;

            outputHistoryRight[2] = 0;
            outputHistoryRight[1] = 0;
            outputHistoryRight[0] = 0;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            for(int i = 0; i < data.Length; i++) {
                data[i] = AddInput(data[i], i);
            }
        }

        float AddInput(float newInput, int index)
        {
            float finalCutOff = Mathf.Clamp(cutoffFrequency, 10, 22200);
            float finalResonance = Mathf.Clamp(lowpassResonanceQ, 0.5f, 10);

            c = 1.0f / (float)Mathf.Tan(Mathf.PI * finalCutOff / sampleRate);
            a1 = 1.0f / (1.0f + finalResonance * c + c * c);
            a2 = 2f * a1;
            a3 = a1;
            b1 = 2.0f * (1.0f - c * c) * a1;
            b2 = (1.0f - finalResonance * c + c * c) * a1;

            float newOutput = 0;
            if(index % 2 == 0) {
                newOutput = a1 * newInput + a2 * inputHistoryLeft[0] + a3 * inputHistoryLeft[1] - b1 * outputHistoryLeft[0] - b2 * outputHistoryLeft[1];

                inputHistoryLeft[1] = inputHistoryLeft[0];
                inputHistoryLeft[0] = newInput;

                outputHistoryLeft[2] = outputHistoryLeft[1];
                outputHistoryLeft[1] = outputHistoryLeft[0];
                outputHistoryLeft[0] = newOutput;
            } else {
                newOutput = a1 * newInput + a2 * inputHistoryRight[0] + a3 * inputHistoryRight[1] - b1 * outputHistoryRight[0] - b2 * outputHistoryRight[1];

                inputHistoryRight[1] = inputHistoryRight[0];
                inputHistoryRight[0] = newInput;

                outputHistoryRight[2] = outputHistoryRight[1];
                outputHistoryRight[1] = outputHistoryRight[0];
                outputHistoryRight[0] = newOutput;
            }

            return newOutput;
        }
    }
}
