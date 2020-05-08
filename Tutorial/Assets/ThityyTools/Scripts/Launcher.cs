using UnityEngine;

namespace ManagersHUB
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField]
        private SceneData m_SceneToLoad = null;

        private string m_SceneName;

        
        private void Start()
        {
            m_SceneName = PlayerPrefs.GetString("SceneToLoad");
            
            if (m_SceneName != "" && m_SceneName != null)
            {
                if(SceneManager.Instance.SceneExist(m_SceneName))
                {
                    SceneManager.Instance.CallLoadScene(m_SceneName);
                }
                else
                {
                    Debug.LogWarning("Scene don't exist");
                }
            }
            else
            {
                SceneManager.Instance.CallLoadScene(m_SceneToLoad);
            }
        }
        
    }
}

