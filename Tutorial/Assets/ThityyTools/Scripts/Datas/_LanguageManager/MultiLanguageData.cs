using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ManagersHUB
{

    [System.Serializable]
    public struct sMultiLanguageValue
    {
        [TextArea]
        public string m_Txt;
        public AudioClip m_Audio;
        public Sprite m_Sprite;
    }

    [System.Serializable]
    public class SystemLanguageMultiLanguageValueDictionnary : SerializableDictionary<SystemLanguage, sMultiLanguageValue> {}

    [ExecuteInEditMode]
    [CreateAssetMenu(menuName = "ManagersHUB/LanguageManager/MultiLanguage_TextData", fileName = "new TextData", order = 0)]
    public class MultiLanguageData : ScriptableObject
    {

        //----------------- Variable(s) --------------------

        [SerializeField]
        private SystemLanguageMultiLanguageValueDictionnary m_MultiLanguageValues = new SystemLanguageMultiLanguageValueDictionnary();
        [SerializeField]
        private AvailableLanguageData m_AvailableLanguageData;
        [SerializeField]
        private eLanguageM_LanguageType m_TypeChoice = eLanguageM_LanguageType.Text;

        private sMultiLanguageValue forEdit = new sMultiLanguageValue();

        //----------------- Get Function(s) --------------------

        ///<summary> Return the Audio use for the current language the game is running on </summary>
        public AudioClip GetAudio()
        {
            return GetAudio(LanguageManager.Instance.CurrentLanguage);
        }

        ///<summary> Return the Audio use for the ask language </summary>
        public AudioClip GetAudio(SystemLanguage aLanguage)
        {
            try
            {
                if (m_TypeChoice == eLanguageM_LanguageType.Audio)
                    return m_MultiLanguageValues[aLanguage].m_Audio;
                else
                {
                    Debug.LogWarning("This MultiLanguage Data (" + this.name + ") isn't configure to work with clip value");
                    return null;
                }
            }
            catch
            {
                Debug.LogWarning("Can't return any audioClip within the language: " + aLanguage);
                return null;
            }
            
        }

        ///<summary> Return the Text use for the current language the game is running on </summary>
        public string GetText()
        {
            return GetText(LanguageManager.Instance.CurrentLanguage);
        }

        ///<summary> Return the Text use for the ask language </summary>
        public string GetText(SystemLanguage aLanguage)
        {
            try
            {
                if (m_TypeChoice == eLanguageM_LanguageType.Text)
                    return m_MultiLanguageValues[aLanguage].m_Txt;
                else
                {
                    Debug.LogWarning("This MultiLanguage Data (" + this.name + ") isn't configure to work with text value");
                    return "This MultiLanguage Data isn't configure to work with text value";
                }
            }
            catch
            {
                Debug.LogWarning("Can't return any String within the language: " + aLanguage);
                return "No Text found for this language";
            }
        }

        ///<summary> Return the Sprite use for the current language the game is running on </summary>
        public Sprite GetImage()
        {
            return GetImage(LanguageManager.Instance.CurrentLanguage);
        }

        ///<summary> Return the Sprite use for the ask language </summary>
        public Sprite GetImage(SystemLanguage aLanguage)
        {
            try
            {
                if (m_TypeChoice == eLanguageM_LanguageType.Image)
                    return m_MultiLanguageValues[aLanguage].m_Sprite;
                else
                {
                    Debug.LogWarning("This MultiLanguage Data (" + this.name + ") isn't configure to work with sprite value");
                    return null;
                }
            }
            catch
            {
                Debug.LogWarning("Can't return any Sprite within the language: " + aLanguage);
                return null;
            }
        }

        //----------------- Editor Function(s) -------------------

        #if (UNITY_EDITOR)

        ///<summary> ONLY IN EDITOR, return the audioClip ask for </summary>
        public AudioClip EDITOR_GetAudio(SystemLanguage aLanguage)
        {
            return m_MultiLanguageValues[aLanguage].m_Audio;
        }

        ///<summary> [ONLY IN EDITOR], return the string ask for </summary>
        public string EDITOR_GetText(SystemLanguage aLanguage)
        {
            return m_MultiLanguageValues[aLanguage].m_Txt;
        }

        ///<summary> [ONLY IN EDITOR], return the sprite ask for </summary>
        public Sprite EDITOR_GetImage(SystemLanguage aLanguage)
        {
            return m_MultiLanguageValues[aLanguage].m_Sprite;
        }

        ///<summary> [ONLY IN EDITOR], set the new audio </summary>
        public void EDITOR_SetAudio(SystemLanguage aLanguage, AudioClip aAudio)
        {
            forEdit = m_MultiLanguageValues[aLanguage];
            forEdit.m_Audio = aAudio;
            m_MultiLanguageValues[aLanguage] = forEdit;
        }

        ///<summary> [ONLY IN EDITOR], set the new text </summary>
        public void EDITOR_SetText(SystemLanguage aLanguage, string aText)
        {
            forEdit = m_MultiLanguageValues[aLanguage];
            forEdit.m_Txt = aText;
            m_MultiLanguageValues[aLanguage] = forEdit;
        }

        ///<summary> [ONLY IN EDITOR], set the new sprite </summary>
        public void EDITOR_SetImage(SystemLanguage aLanguage, Sprite aSprite)
        {
            forEdit = m_MultiLanguageValues[aLanguage];
            forEdit.m_Sprite = aSprite;
            m_MultiLanguageValues[aLanguage] = forEdit;
        }

        ///<summary> [ONLY IN EDITOR], return all text set for all language </summary>
        public SystemLanguageMultiLanguageValueDictionnary EDITOR_GetMultiLanguageValue()
        {
            return m_MultiLanguageValues;
        }

        ///<summary> [ONLY IN EDITOR], update dictionnary with the available Language </summary>
        public void EDITOR_UpdateDictionnary(AvailableLanguageData aLanguages)
        {
            foreach (KeyValuePair<SystemLanguage, sMultiLanguageValue> entry in m_MultiLanguageValues.ToList())
            {
                if(!DictionaryHaveSameKey(entry, aLanguages))
                {
                    m_MultiLanguageValues.Remove(entry.Key);
                }
            }

            for(int i = 0; i < aLanguages.GetAvailableLanguageCount(); i++)
            {
                if(!HaveLanguage(aLanguages.GetLanguage(i)))
                {
                    AddLanguage(aLanguages.GetLanguage(i));
                }
            }
        }

        #endif

        //----------------- Update Dictionnary Function(s) ----------------

        ///<summary> If the dictionnary have the key in it </summary>
        private bool DictionaryHaveSameKey(KeyValuePair<SystemLanguage, sMultiLanguageValue> aEntry, AvailableLanguageData aAvailableLanguage)
        {
            for(int i = 0; i < aAvailableLanguage.GetAvailableLanguageCount(); i++)
                {
                    if(aAvailableLanguage.GetLanguage(i) == aEntry.Key)
                    {
                        return true;
                    }
                }
            return false;
        }

        ///<summary> Check if the ask language exist in the dictionnary </summary>
        private bool HaveLanguage(SystemLanguage aLanguage)
        {
            return m_MultiLanguageValues.ContainsKey(aLanguage);
        }

        ///<summary> Add the ask language into the dictionnary </summary>
        private void AddLanguage(SystemLanguage aLanguage)
        {
            m_MultiLanguageValues.Add(aLanguage, new sMultiLanguageValue());
        }

    }
}

