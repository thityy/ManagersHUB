using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{

    public enum eTagCallOption { MultipleSpriteAsset, MultipleTagCall };
    [System.Serializable]
    public class PlatformSpriteAssetDictionnary : SerializableDictionary<eLanguageM_Platform, TMPro.TMP_SpriteAsset> { }

    
    public class LanguageManager : Singleton<LanguageManager>
    {

        //________________________ AVAILABLE LANGUAGES VARIABLES ________________________\\

        [SerializeField]
        private AvailableLanguageData m_AvailableLanguageData = null;      //The data that save the language available
        public List<SystemLanguage> AvailableLanguage
        {
            get 
            { 
                if(m_AvailableLanguageData != null)
                    return m_AvailableLanguageData.GetAvailableLanguages();
                return null;
            }
        }

        //________________________ CURRENT LANGUAGE VARIABLES ________________________\\

        private SystemLanguage m_CurrentLanguage = SystemLanguage.Unknown;   //The current language the game is on
        public SystemLanguage CurrentLanguage
        {
            get { return m_CurrentLanguage; }
        }

        //________________________ DEFAULT VARIABLES ________________________\\

        [SerializeField]
        private SystemLanguage m_DefaultLanguage = SystemLanguage.Unknown;      //The language that the game will start on
        [SerializeField]
        private bool m_UseDeviceLanguage = false;                               //If you want to detect the device language and set the default one with it

        [SerializeField]
        private eLanguageM_Platform m_DefaultPlatform = eLanguageM_Platform.Unknown;   //The platform that the game will start on

        //________________________ TAG VARIABLES ________________________\\

        [SerializeField]
        private eTagCallOption m_TagCall = eTagCallOption.MultipleTagCall;  //If the tag use 'Multiple Sprite Assets' or 'multiple Tag Call' option
        public eTagCallOption TagCall
        {
            get { return m_TagCall; }
        }

        [SerializeField]
        private TMPro.TMP_SpriteAsset m_DefaultSpriteAsset = null;      //The Default spriteAsset use when the LanguageManager option is set on 'MultipleTagCall'

        [SerializeField]
        private TagPlatformData m_TagData = null;
        [SerializeField]
        private SpriteSheetPlatformData m_SpriteSheetData = null;

        [SerializeField]
        private PlatformSpriteAssetDictionnary m_SpriteAssetsPlatform = new PlatformSpriteAssetDictionnary();   //The dictionnary that contains the sprite assets for the device option

        [SerializeField]
        private eLanguageM_Platform m_CurrentPlatform = eLanguageM_Platform.Unknown;    //The current platform the game is on (Use when m_TagType = eTagTypeOption.Device)
        public eLanguageM_Platform CurrentPlatform
        {
            get { return m_CurrentPlatform; }
        }

        protected override void Awake()
        {
            AwakeAlwaysExecute();
            InitDebuging(eManagers.LanguageManager);
            if (m_UseDeviceLanguage)
            {
                Language_SetCurrentLanguage(Application.systemLanguage);
            }
            else
            {
                Language_SetCurrentLanguage(m_DefaultLanguage);
            }

            m_CurrentPlatform = m_DefaultPlatform;
            Debug.Log(Debug_InitSucces("Language Manager as been init"));
        }

        ///<summary>
        /// The Language system lets you set the current Language as you wish, The language only need to be set previously in the LanguageManager inspector
        ///</summary>
        #region LANGUAGE Functions

        ///<summary> Set the new language the game will be on </summary>
        public void Language_SetCurrentLanguage(SystemLanguage aLanguage)
        {
            if (AvailableLanguage != null)
            {
                if (AvailableLanguage.Contains(aLanguage))
                {
                    m_CurrentLanguage = aLanguage;
                }
            }

        }

        ///<summary> Set the new language the game will be on with the Defualt Language </summary>
        public void Language_ResetWithDefaultLanguage()
        {
            if (m_UseDeviceLanguage)
            {
                Language_ResetWithDeviceLanguage();
            }
            else
            {
                Language_SetCurrentLanguage(m_DefaultLanguage);
            }
        }

        ///<summary> Set the new language the game will be on with the Device Language </summary>
        public void Language_ResetWithDeviceLanguage()
        {
            Language_SetCurrentLanguage(Application.systemLanguage);
        }

        #endregion

        ///<summary>
        /// The Tag system lets you call many type of device/system symbol (use with TMPRo SpriteAsset) with only one Tag, this system come with multiple option:
        /// 1- You can choose between Device (more accurate[xbox360, xboxOne, xboxProjectX]) and System (more general[xbox, playstation, computer]) to render symbol
        /// 2- You can also choose if you want to use multiple sprite sheet and change them when you render text on screen depend on the platform (device/system)
        /// OR
        /// You can choose if you want to use multple Tag Call, so you only have one spriteAsset for all platform, but you set the different call on the LanguageManager inspector
        ///</summary>
        #region TAG Functions

        ///<summary> Set the new Device in use </summary>
        public void Tag_SetCurrentDevice(eLanguageM_Platform aPlatform)
        {
            m_CurrentPlatform = aPlatform;
        }

        ///<summary> RETURN the right symbol for the actual Device/System </summary>
        public string Tag_GetSymbolWithTag(string aTag)
        {
            string symbol = "No Symbol equals " + aTag;
            symbol = m_TagData.GetTagValueForCurrentPlatform(aTag);

            if (symbol == "No Symbol equals " + aTag)
            {
                return symbol;
            }
            return "<sprite name=" + symbol + ">";
        }

        ///<summary> RETURN the text with the symbol include in the text </summary>
        public string Tag_GetTextWithSymbolInclude(string aText)
        {
            return Tag_GetSymbolText(aText);
        }

        ///<summary> REPLACE all tag value with the right tag text and RETURN it</summary>
        private string Tag_GetSymbolText(string aText)
        {
            string[] words;
            words = aText.Split(' ');

            aText = "";
            for (int i = 0; i < words.Length; i++)
            {
                while (words[i].Contains("<tag="))
                {
                    string saveWord = words[i];
                    int start = words[i].IndexOf("<tag=", 0) + 5;
                    int end = words[i].IndexOf(">", start);
                    string beforeTag = Tag_GetTextBeforeTag(words[i]);
                    words[i] = words[i].Substring(start, end - start);
                    string afterTag = saveWord.Replace(beforeTag + "<tag=" + words[i] + ">", "");
                    words[i] = beforeTag + Tag_GetSymbolWithTag(words[i]) + afterTag;
                }
                aText += words[i];
                if (i != words.Length - 1)
                {
                    aText += " ";
                }
            }

            return aText;
        }

        ///<summary> RETURN the text that was juste before the tag </sumary>
        private string Tag_GetTextBeforeTag(string aText)
        {
            if (!string.IsNullOrWhiteSpace(aText))
            {
                int charLocation = aText.IndexOf("<tag=", System.StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return aText.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }

        ///<summary> RETURN the right spriteAsset for the actual Device/System </summary>
        public TMPro.TMP_SpriteAsset Tag_GetSpriteAsset()
        {
            switch (m_TagCall)
            {
                case eTagCallOption.MultipleSpriteAsset:
                    {
                        if (m_SpriteSheetData.GetSpriteAsset(m_CurrentPlatform) != null)
                        {
                            return m_SpriteSheetData.GetSpriteAsset(m_CurrentPlatform);
                        }
                        return m_DefaultSpriteAsset;
                    }

                case eTagCallOption.MultipleTagCall:
                    {
                        return m_DefaultSpriteAsset;
                    }
            }

            return null;
        }

        #endregion
    }
}


