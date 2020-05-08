using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/AudioManager/RadioData", fileName = "new RadioData", order = 1)]
    public class RadioData : ScriptableObject
    {
        [SerializeField]
        private List<MusicData> m_Musics = new List<MusicData>();
        [SerializeField]
        private string m_RadioName = string.Empty;
        [SerializeField]
        private bool m_AutoShuffle = false;

        public List<MusicData> GetMusics()
        {
            return m_Musics;
        }

        public bool WantToShuffle()
        {
            return m_AutoShuffle;
        }

        ///<summary> Shuffle all the music of the radio station [Good to do at the start of the radio]</summary>
        public void ShuffleRadio()
        {
            System.Random rng = new System.Random();
            int n = m_Musics.Count;
            while(n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                MusicData value = m_Musics[k];
                m_Musics[k] = m_Musics[n];
                m_Musics[n] = value;
            }
        }
    
        ///<summary> Check if the radio station have any music in it </summary>
        public bool HaveMusicInIt()
        {
            if(m_Musics.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

}

