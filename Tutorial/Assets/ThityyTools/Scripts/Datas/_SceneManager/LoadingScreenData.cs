using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/SceneManager/LoadingScreenData", fileName = "new LoadingScreenData", order = 0)]
    public class LoadingScreenData : ScriptableObject
    {
        [SerializeField]
        private LoadingScreenObj m_LoadingScreenPrefab = null;

        public LoadingScreenObj GetLoadingScreen()
        {
            return m_LoadingScreenPrefab;
        }
    }

}
