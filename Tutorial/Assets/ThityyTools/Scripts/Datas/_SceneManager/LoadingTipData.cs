using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    

    [CreateAssetMenu(menuName = "ManagersHUB/SceneManager/LoadingTipData", fileName = "New LoadingTipData")]
    public class LoadingTipData : ScriptableObject
    {
        [SerializeField]
        private eSceneM_TextType m_TextType = eSceneM_TextType.String;
        [SerializeField]
        private string m_Tip = string.Empty;
        [SerializeField]
        private MultiLanguageData m_TextData = null;

        public string GetTip()
        {
            switch(m_TextType)
            {
                case eSceneM_TextType.String:
                {
                    return m_Tip;
                }
                case eSceneM_TextType.TextData:
                {
                    if(LanguageManager.Instance != null)
                    {
                        return m_TextData.GetText(LanguageManager.Instance.CurrentLanguage);
                    }
                    else
                    {
                        return "Can't display preview when use MultiLanguage_Data";
                    }
                }
            }
            return "";
        }
    }
}

