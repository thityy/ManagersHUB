using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/SceneManager/SceneData", fileName = "new SceneData", order = 0)]
    public class SceneData : ScriptableObject
    {
        [SerializeField]
        private string m_SceneObj = string.Empty;
        [SerializeField]
        private string m_ScenePath = string.Empty;

        /// option of the music for next scene
        [SerializeField]
        private bool m_UseMusicControle = false;
        public bool UseMusicControle
        {
            get {return m_UseMusicControle; }
        }
        [SerializeField]
        private bool m_WantMusic = false;
        public bool WantMusic
        {
            get {return m_WantMusic; }
        }

        
        [SerializeField]
        private AudioClip m_SceneMusic = null;
        [SerializeField]
        private eSceneM_Music m_MusicOption = eSceneM_Music.WithoutMusic;

        /// option of the pool for next scene
        [SerializeField]
        private bool m_UsePoolControle = false;
        public bool UsePoolControle
        {
            get {return m_UsePoolControle; }
        }
        [SerializeField]
        private PoolData m_ScenePool = null;

        /// Loading type is the option of the loading screen
        [SerializeField]
        private eSceneM_LoadingScreen m_LoadingScreen = eSceneM_LoadingScreen.Default;
        [SerializeField]
        private eSceneM_LoadingType m_LoadingType = eSceneM_LoadingType.WithoutTip;
        [SerializeField]
        private LoadingScreenData m_LoadingObject = null;

        /// option of the tip for the loading screen, if overwrite need a LoadingTipData
        [SerializeField]
        private Color m_FadeColor = new Color();
        [SerializeField]
        private eSceneM_TipOption m_TipOption = eSceneM_TipOption.Random;
        [SerializeField]
        private eSceneM_TipGroup m_TipGroup = eSceneM_TipGroup.AllGroup;
        [SerializeField]
        private LoadingTipData m_TipData = null;






        public string GetSceneName()
        {
            return m_SceneObj;
        }

        //--------

        public AudioClip GetSceneMusic()
        {
            return m_SceneMusic;
        }

        public eSceneM_Music GetMusicOption()
        {
            return m_MusicOption;
        }

        //--------

        public PoolData GetScenePool()
        {
            return m_ScenePool;
        }

        //--------

        public eSceneM_LoadingScreen GetLoadingScreenChoice()
        {
            return m_LoadingScreen;
        }

        public LoadingScreenObj GetLoadingObj()
        {
            return m_LoadingObject.GetLoadingScreen();
        }

        public eSceneM_LoadingType GetLoadingType()
        {
            return m_LoadingType;
        }

        //--------

        public LoadingTipData GetLoadingTipData()
        {
            return m_TipData;
        }

        public eSceneM_TipGroup GetTipGroup()
        {
            return m_TipGroup;
        }

        public eSceneM_TipOption GetTipOption()
        {
            return m_TipOption;
        }

        public Color GetFadeColor()
        {
            return m_FadeColor;
        }

    }
}
