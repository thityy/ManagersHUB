using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class SFX_AudioSetup : MonoBehaviour
    {
        
        private AudioSource m_AudioSource = null;
        private float m_CurrentTime = 0.0f;
        private float m_Duration = 0.0f;
        private bool m_PerfMode = false;
        private System.Action<SFX_AudioSetup> m_PushBackAction;
        private bool m_InPlayMode = false;

        private void Update()
        {
            if(m_InPlayMode)
            {
                CheckSoundTime();
            }
        }

        public void SetAudioSource()
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void SetupAudio(AudioClip aClip, float aVolume, float aPitch, float a3DEffect, float aStereoPan)
        {
            if(m_AudioSource == null)
            {
                SetAudioSource();
            }
            m_AudioSource.clip = aClip;
            m_AudioSource.volume = aVolume;
            m_AudioSource.pitch = aPitch;
            m_AudioSource.panStereo = aStereoPan;
            m_Duration = aClip.length;
        }

        public void PlayAudio(ulong aDelay)
        {
            m_Duration += aDelay;
            m_PerfMode = false;
            m_InPlayMode = true;
            m_AudioSource.PlayDelayed(aDelay);
        }

        public void PlayPerfAudio(ulong aDelay, System.Action<SFX_AudioSetup> aPushBackInList)
        {
            m_Duration += aDelay;
            m_PerfMode = true;
            m_AudioSource.PlayDelayed(aDelay);
            m_InPlayMode = true;
            m_PushBackAction = aPushBackInList;
        }

        private void CheckSoundTime()
        {
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime >= m_Duration)
            {
                if (m_PerfMode && m_PushBackAction != null)
                {
                    m_PushBackAction(this);
                    m_InPlayMode = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
