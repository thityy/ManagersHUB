using UnityEngine;

namespace ManagersHUB
{
    public enum eManagers { GameManager, SceneManager, PoolManager, AudioManager, SpawnManager, LanguageManager, OptionManager };



    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected string m_DebugInit;

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance != null)
            {
                Destroy(gameObject);
            }

            m_Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void AwakeAlwaysExecute()
        {
            if (m_Instance != null)
            {
                Destroy(gameObject);
            }
            m_Instance = GetComponent<T>();
            if(Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        #region Debuging

        protected void InitDebuging(eManagers aManagerType)
        {
            string newWord = "";
            string[] split = System.Text.RegularExpressions.Regex.Split(aManagerType.ToString(), @"(?<!^)(?=[A-Z0-9])");
            for (int k = 0; k < split.Length; k++)
            {
                if(k != split.Length - 1)
                {
                    newWord += split[k] + " ";
                }
                else
                {
                    newWord += split[k];
                }
            }
            string debugTitle = ColorManager.GetColorText("•", aManagerType, true) + "[" + newWord.ToUpper() + "]";
            m_DebugInit = "<b>" + debugTitle + "</b> ";
            
        }

        protected string Debug_Warning(string aMessage)
        {
            return (m_DebugInit + "<b><color=#ba5b00>[WARNING]</color></b> " + aMessage + "\n");
        }

        protected string Debug_Error(string aMessage)
        {
            return (m_DebugInit + "<b><color=#930000>[ERROR]</color></b> " + aMessage + "\n");
        }

        protected string Debug_Message(string aMessage)
        {
            return (m_DebugInit + "<b><color=#197e7e>[LOG]</color></b> " + aMessage + "\n");
        }

        protected string Debug_InitFailed(string aMessage)
        {
            return (m_DebugInit + "<b><color=#930000>[FAILED]</color></b> " + aMessage + "\n");
        }

        protected string Debug_InitSucces(string aMessage)
        {
            return (m_DebugInit + "<b><color=#006f00>[SUCCES]</color></b> " + aMessage + "\n");
        }

        #endregion

        public static bool ManagerIsInit(eManagers aManagerToCheck)
        {
            switch (aManagerToCheck)
            {
                case eManagers.AudioManager:
                    {
                        if (AudioManager.Instance != null)
                        {
                            return true;
                        }
                        break;
                    }
                case eManagers.SceneManager:
                    {
                        if (AudioManager.Instance != null)
                        {
                            return true;
                        }
                        break;
                    }
                case eManagers.PoolManager:
                    {
                        if (AudioManager.Instance != null)
                        {
                            return true;
                        }
                        break;
                    }
                case eManagers.LanguageManager:
                    {
                        if (LanguageManager.Instance != null)
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;
        }

        protected virtual bool ManagerIsSpawn(eManagers aManagerToCheck)
        {
            return ManagerIsInit(aManagerToCheck);
        }
    }
}

