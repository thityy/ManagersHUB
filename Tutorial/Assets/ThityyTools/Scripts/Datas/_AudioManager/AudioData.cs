using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/AudioManager/AudioData", fileName = "new AudioData", order = 0)]
    public class AudioData : ScriptableObject
    {

        //Default value
        private const float m_DefaultVolume = 1.0f;
        private const float m_DefaultSpatialBlend = 1.0f;
        private const float m_DefaultPitch = 1.0f;
        private const float m_DefaultStereoPan = 0.0f;
        private const float m_DefaultDelay = 0.0f;

        //Base Settings
        [SerializeField]
        private AudioClip m_Clip = null;
        [SerializeField]
        private int m_UseLanguageManager = 0;
        [SerializeField]
        private MultiLanguageData m_MultiLanguageClip = null;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_Volume = m_DefaultVolume;

        //Advanced Settings
        [SerializeField]
        private bool m_UseAdvancedSetting = false;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_SpatialBlend = m_DefaultSpatialBlend;
        [SerializeField]
        [Range(-3.0f, 3.0f)]
        private float m_Pitch = m_DefaultPitch;
        [SerializeField]
        [Range(-1.0f, 1.0f)]
        private float m_StereoPan = m_DefaultStereoPan;
        public float StereoPan
        {
            set { m_StereoPan = value; }
        }

        //Delay Settings
        [SerializeField]
        private bool m_UseDelay = false;
        [SerializeField]
        [Range(0.0f, Mathf.Infinity)]
        private float m_Delay = m_DefaultDelay;

        //Variante Settings
        [SerializeField]
        private bool m_UseVariante = false;

        [SerializeField]
        private bool m_UseVolumeVariante = false;
        [SerializeField]
        private float m_MinVolumeVariante = 0.0f;
        [SerializeField]
        private float m_MaxVolumeVariante = 1.0f;

        [SerializeField]
        private bool m_UsePitchVariante = false;
        [SerializeField]
        private float m_MinPitchVariante = 0.0f;
        [SerializeField]
        private float m_MaxPitchVariante = 1.0f;




        public AudioClip GetClip()
        {
            if(m_UseLanguageManager == 1)
            {
                return m_MultiLanguageClip.GetAudio(LanguageManager.Instance.CurrentLanguage);
            }
            return m_Clip;
        }

        public float GetSpatialBlend()
        {
            if(!m_UseAdvancedSetting)
            {
                return m_DefaultSpatialBlend;
            }
            return m_SpatialBlend;
        }

        public float GetVolume()
        {
            if (m_UseVolumeVariante && m_UseVariante)
            {
                return Random.Range(m_MinVolumeVariante, m_MaxVolumeVariante);
            }
            return m_Volume;
        }

        public float GetPitch()
        {
            if (m_UsePitchVariante && m_UseVariante)
            {
                return Random.Range(m_MinPitchVariante, m_MaxPitchVariante);
            }
            else if(!m_UseAdvancedSetting)
            {
                return m_DefaultPitch;
            }
            return m_Pitch;
        }

        public ulong GetDelay()
        {
            if(!m_UseDelay)
            {
                return (ulong)m_DefaultDelay;
            }
            return (ulong)m_Delay;
        }

        public float GetStereoPan()
        {
            if(!m_UseAdvancedSetting)
            {
                return m_DefaultStereoPan;
            }
            return m_StereoPan;
        }
    }
}

