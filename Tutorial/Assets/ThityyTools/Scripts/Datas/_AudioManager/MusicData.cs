using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/AudioManager/MusicData", fileName = "new MusicData", order = 0)]
    public class MusicData : ScriptableObject
    {

        #region Variable(s)
        //Base settings
        [SerializeField]
        private AudioClip m_Clip = null;
        [SerializeField]
        private float m_Volume = 1.0f;

        //Display Radio Settings
        [SerializeField]
        private Sprite m_ClipArt = null;
        [SerializeField]
        private string m_ArtistName = string.Empty;
        [SerializeField]
        private string m_SongName = string.Empty;

        #endregion

        #region Get Function(s)

        ///<summary>Return the volume set for this music</summary>
        public float GetVolume()
        {
            return m_Volume;
        }

        ///<summary>Return the clip of this music</summary>
        public AudioClip GetClip()
        {
            return m_Clip;
        }

        ///<summary>Return the sprite cover of the album of this music</summary>
        public Sprite GetClipArt()
        {
            return m_ClipArt;
        }

        ///<summary>Return the artist name of this music</summary>
        public string GetArtistName()
        {
            if(string.IsNullOrEmpty(m_ArtistName))
                return "Unknow";
            return m_ArtistName;
        }

        ///<summary>Return the display name of this music</summary>
        public string GetSongName()
        {
            if(string.IsNullOrEmpty(m_SongName))
                return "Unknow";
            return m_SongName;
        }

        #endregion
    }
}

