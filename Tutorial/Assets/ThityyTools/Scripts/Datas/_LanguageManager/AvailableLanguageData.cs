using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [CreateAssetMenu(menuName = "ManagersHUB/LanguageManager/MultiLanguage_AvailableData", fileName = "new LanguageAvailableData", order = 2)]
    public class AvailableLanguageData : ScriptableObject
    {
        [SerializeField]
        private List<SystemLanguage> m_Languages = new List<SystemLanguage>();

        public bool EDITOR_InitLanguageIsNeed()
        {
            if(m_Languages.Count > 0)
            {
                return false;
            }
            
            m_Languages = new List<SystemLanguage>(){SystemLanguage.English};
            return true;
        }

        public List<SystemLanguage> GetAvailableLanguages()
        {
            return m_Languages;
        }

        public void AddLanguage(SystemLanguage aLanguage)
        {
            m_Languages.Add(aLanguage);
        }

        public SystemLanguage GetLanguage(int aId)
        {
            return m_Languages[aId];
        }

        public int GetAvailableLanguageCount()
        {
            return m_Languages.Count;
        }
    }
}

