using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [System.Serializable]
    public class sFootSound
    {
        [SerializeField]
        private List<AudioClip> Walk;
        [SerializeField]
        private List<AudioClip> Run;
        [SerializeField]
        private List<AudioClip> Crouch;
        [SerializeField]
        private List<AudioClip> Crawl;

        [SerializeField]
        private sVolumeFootSound m_FootVolumes;
        public sVolumeFootSound FootVolumes
        {
            set{m_FootVolumes = value;}
            get{return m_FootVolumes;}
        }

        public sFootSound(bool aInit)
        {
            Walk = new List<AudioClip>();
            Run = new List<AudioClip>();
            Crouch = new List<AudioClip>();
            Crawl = new List<AudioClip>();
            m_FootVolumes = new sVolumeFootSound();
        }

        public List<AudioClip> GetSoundsList(eAudioM_FootSoundType aType)
        {
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        if (Walk == null)
                        {
                            Walk = new List<AudioClip>();
                        }
                        return Walk;
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        if (Run == null)
                        {
                            Run = new List<AudioClip>();
                        }
                        return Run;
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        if (Crouch == null)
                        {
                            Crouch = new List<AudioClip>();
                        }
                        return Crouch;
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        if (Crawl == null)
                        {
                            Crawl = new List<AudioClip>();
                        }
                        return Crawl;
                    }
            }

            return null;
        }

        public void SetSoundsList(eAudioM_FootSoundType aType, List<AudioClip> aSoundsList)
        {
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        Walk = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        Run = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        Crouch = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        Crawl = aSoundsList;
                        break;
                    }
            }


        }

        public AudioClip GetFootSound(eAudioM_FootSoundType aType)
        {
            int maxRandomValue = 0;
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        maxRandomValue = Walk.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Walk' type in this Data!");
                            return null;
                        }
                        if(Walk[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Walk' type in this Data!");
                            return null;
                        }
                        return Walk[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        maxRandomValue = Run.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Run' type in this Data!");
                            return null;
                        }
                        if(Run[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Run' type in this Data!");
                            return null;
                        }
                        return Run[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        maxRandomValue = Crouch.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crouch' type in this Data!");
                            return null;
                        }
                        if(Crouch[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crouch' type in this Data!");
                            return null;
                        }
                        return Crouch[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        maxRandomValue = Crawl.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crawl' type in this Data!");
                            return null;
                        }
                        if(Crawl[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crawl' type in this Data!");
                            return null;
                        }
                        return Crawl[Random.Range(0, maxRandomValue)];
                    }
            }

            return null;
        }
    
        public float GetFootSoundVolume(eAudioM_FootSoundType aType)
        {
            return m_FootVolumes.GetVolume(aType);
        }
    }

    [System.Serializable]
    public class sFootSoundWithData
    {
        [SerializeField]
        private List<AudioData> Walk;
        [SerializeField]
        private List<AudioData> Run;
        [SerializeField]
        private List<AudioData> Crouch;
        [SerializeField]
        private List<AudioData> Crawl;

        public sFootSoundWithData(bool aInit)
        {
            Walk = new List<AudioData>();
            Run = new List<AudioData>();
            Crouch = new List<AudioData>();
            Crawl = new List<AudioData>();
        }

        public List<AudioData> GetSoundsList(eAudioM_FootSoundType aType)
        {
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        if (Walk == null)
                        {
                            Walk = new List<AudioData>();
                        }
                        return Walk;
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        if (Run == null)
                        {
                            Run = new List<AudioData>();
                        }
                        return Run;
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        if (Crouch == null)
                        {
                            Crouch = new List<AudioData>();
                        }
                        return Crouch;
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        if (Crawl == null)
                        {
                            Crawl = new List<AudioData>();
                        }
                        return Crawl;
                    }
            }

            return null;
        }

        public void SetSoundsList(eAudioM_FootSoundType aType, List<AudioData> aSoundsList)
        {
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        Walk = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        Run = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        Crouch = aSoundsList;
                        break;
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        Crawl = aSoundsList;
                        break;
                    }
            }
        }

        public AudioData GetFootSound(eAudioM_FootSoundType aType)
        {
            int maxRandomValue = 0;
            switch (aType)
            {
                case eAudioM_FootSoundType.Walk:
                    {
                        maxRandomValue = Walk.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Walk' type in this Data!");
                            return null;
                        }
                        if(Walk[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Walk' type in this Data!");
                            return null;
                        }
                        return Walk[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Run:
                    {
                        maxRandomValue = Run.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Run' type in this Data!");
                            return null;
                        }
                        if(Run[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Run' type in this Data!");
                            return null;
                        }
                        return Run[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Crouch:
                    {
                        maxRandomValue = Crouch.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crouch' type in this Data!");
                            return null;
                        }
                        if(Crouch[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crouch' type in this Data!");
                            return null;
                        }
                        return Crouch[Random.Range(0, maxRandomValue)];
                    }
                case eAudioM_FootSoundType.Crawling:
                    {
                        maxRandomValue = Crawl.Count;
                        if (maxRandomValue <= 0)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crawl' type in this Data!");
                            return null;
                        }
                        if(Crawl[0] == null)
                        {
                            Debug.LogWarning("There's no AudioClip for 'Crawl' type in this Data!");
                            return null;
                        }
                        return Crawl[Random.Range(0, maxRandomValue)];
                    }
            }

            return null;
        }
    }

    [System.Serializable]
    public class sVolumeFootSound
    {
        [SerializeField]
        private float m_VolumeWalk;
        [SerializeField]
        private float m_VolumeRun;
        [SerializeField]
        private float m_VolumeCrouch;
        [SerializeField]
        private float m_VolumeCrawl;

        public void SetVolume(float aVolume, eAudioM_FootSoundType aType)
        {
            switch(aType)
            {
                case eAudioM_FootSoundType.Walk:
                {
                    m_VolumeWalk = aVolume;
                    break;
                }
                case eAudioM_FootSoundType.Run:
                {
                    m_VolumeRun = aVolume;
                    break;
                }
                case eAudioM_FootSoundType.Crouch:
                {
                    m_VolumeCrouch = aVolume;
                    break;
                }
                case eAudioM_FootSoundType.Crawling:
                {
                    m_VolumeCrawl = aVolume;
                    break;
                }
            }
        }

        public float GetVolume(eAudioM_FootSoundType aType)
        {
            switch(aType)
            {
                case eAudioM_FootSoundType.Walk:
                {
                    return m_VolumeWalk;
                }
                case eAudioM_FootSoundType.Run:
                {
                    return m_VolumeRun;
                }
                case eAudioM_FootSoundType.Crouch:
                {
                    return m_VolumeCrouch;
                }
                case eAudioM_FootSoundType.Crawling:
                {
                    return m_VolumeCrawl;
                }
            }

            return 1.0f;
        }
    }


    [CreateAssetMenu(menuName = "ManagersHUB/AudioManager/FootSoundData", fileName = "new FootSoundData", order = 0)]
    public class FootSoundData : ScriptableObject
    {


        //NEW VERSION
        [SerializeField]
        private List<sFootSound> m_FootSounds;
        [SerializeField]
        private List<sFootSoundWithData> m_FootSoundsWithData;

        ///<summary>Return a foot sound depend on the moving type of the player and the ground is on</summary>
        public AudioClip GetFootSound(eAudioM_GroundType aGround, eAudioM_FootSoundType aType)
        {
            return m_FootSounds[(int)aGround].GetFootSound(aType);
        }

        ///<summary>Return a foot sound data depend on the moving type of the player and the ground is on</summary>
        public AudioData GetFootSoundData(eAudioM_GroundType aGround, eAudioM_FootSoundType aType)
        {
            return m_FootSoundsWithData[(int)aGround].GetFootSound(aType);
        }

        ///<summary>Return all FootSounds set for audioClip</summary>
        public List<sFootSound> GetFootSounds()
        {
            if(m_FootSounds == null || m_FootSounds.Count == 0)
            {
                m_FootSounds = new List<sFootSound>();
                for(int i = 0; i < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)); i++)
                {
                    m_FootSounds.Add(new sFootSound(true));
                }
            }
            if(m_FootSounds.Count < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)))
            {
                for(int i = m_FootSounds.Count; i < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)); i++)
                {
                    m_FootSounds.Add(new sFootSound(true));
                }
            }
            return m_FootSounds;
        }

        public float GetFootSoundVolume(eAudioM_GroundType aGround, eAudioM_FootSoundType aType)
        {
            return m_FootSounds[(int)aGround].GetFootSoundVolume(aType);
        }

        ///<summary>Return all FootSounds set for audioData</summary>
        public List<sFootSoundWithData> GetFootSoundsData()
        {
            if(m_FootSoundsWithData == null || m_FootSoundsWithData.Count == 0)
            {
                m_FootSoundsWithData = new List<sFootSoundWithData>();
                for(int i = 0; i < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)); i++)
                {
                    m_FootSoundsWithData.Add(new sFootSoundWithData(true));
                }
            }
            if(m_FootSoundsWithData.Count < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)))
            {
                for(int i = m_FootSoundsWithData.Count; i < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)); i++)
                {
                    m_FootSoundsWithData.Add(new sFootSoundWithData(true));
                }
            }
            return m_FootSoundsWithData;
        }

        ///<summary>EDITOR ONLY! Use to set the new list of sound</summary>
        public void EDITOR_SetFootSounds(List<sFootSound> aSounds, List<sFootSoundWithData> aSoundsData)
        {
            m_FootSounds = aSounds;
            m_FootSoundsWithData = aSoundsData;
        }
    }
}

