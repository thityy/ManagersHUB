using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{

    [System.Serializable]
    public struct sPlatformTag
    {
        //_______ PC _______\\
        public string WindowsTxt;
        public string LinuxTxt;
        public string IOSComputerTxt;

        //_____ MOBILE _____\\
        public string AndroidTxt;
        public string IOSMobileTxt;

        //______ XBOX ______\\
        public string Xbox360Txt;
        public string XboxOneTxt;
        public string XboxProjectXTxt;

        //___ PLAYSTATION ___\\
        public string Playstation3Txt;
        public string Playstation4Txt;
        public string Playstation5Txt;

        //____ NINTENDO ____\\
        public string SwitchTxt;

        //_____ GOOGLE _____\\
        public string StadiaTxt;

        //_____ OTHERS _____\\
        public string UnknowTxt;

        public void SetNewValue(eLanguageM_Platform aPlatform, string aValue)
        {
            switch(aPlatform)
            {

                //_____ XBOX ______________________\\

                case eLanguageM_Platform.Xbox360:
                {
                    Xbox360Txt = aValue;
                    break;
                }
                case eLanguageM_Platform.XboxOne:
                {
                    XboxOneTxt = aValue;
                    break;
                }
                case eLanguageM_Platform.XboxProjectX:
                {
                    XboxProjectXTxt = aValue;
                    break;
                }

                //_____ PLAYSTATION ______________________\\

                case eLanguageM_Platform.Playstation3:
                {
                    Playstation3Txt = aValue;
                    break;
                }
                case eLanguageM_Platform.Playstation4:
                {
                    Playstation4Txt = aValue;
                    break;
                }
                case eLanguageM_Platform.Playstation5:
                {
                    Playstation5Txt = aValue;
                    break;
                }

                //_____ PC ______________________\\

                case eLanguageM_Platform.Windows:
                {
                    WindowsTxt = aValue;
                    break;
                }
                case eLanguageM_Platform.Linux:
                {
                    LinuxTxt = aValue;
                    break;
                }
                case eLanguageM_Platform.IosComputer:
                {
                    IOSComputerTxt = aValue;
                    break;
                }

                //_____ MOBILE ______________________\\

                case eLanguageM_Platform.Android:
                {
                    AndroidTxt = aValue;
                    break;
                }
                case eLanguageM_Platform.IosMobile:
                {
                    IOSMobileTxt = aValue;
                    break;
                }

                //_____ NINTENDO ______________________\\

                case eLanguageM_Platform.Switch:
                {
                    SwitchTxt = aValue;
                    break;
                }

                //_____ GOOGLE ______________________\\

                case eLanguageM_Platform.Stadia:
                {
                    StadiaTxt = aValue;
                    break;
                }

                //_____ OTHERS ______________________\\

                case eLanguageM_Platform.Unknown:
                {
                    UnknowTxt = aValue;
                    break;
                }

            }
        }
    }
    
    [System.Serializable]
    public class StringPlatformTagDictionnary : SerializableDictionary<string, sPlatformTag> { }

    [ExecuteInEditMode]
    [CreateAssetMenu(menuName = "ManagersHUB/LanguageManager/MultiPlatform_TagData", fileName = "new TagData", order = 1)]
    public class TagPlatformData : ScriptableObject
    {
        [SerializeField]
        private StringPlatformTagDictionnary m_PlatformTags;      //The dictionnary that contains multiple tag call for each platform option

        //----------------- Get Function(s) --------------------

        ///<summary> Return all the replace value for the ask TAG (argument aTag) for every platform </summary>
        public sPlatformTag GetTagValuesForEachPlatform(string aTag)
        {
            return m_PlatformTags[aTag];
        }

        ///<summary> Return the replace value for the ask TAG (argument aTag) for the current platform in use (set in the Language Manager) </summary>
        public string GetTagValueForCurrentPlatform(string aTag)
        {
            return GetTagValue(aTag, LanguageManager.Instance.CurrentPlatform);
        }
    
        public string GetTagValue(string aTag, eLanguageM_Platform aWantPlatform)
        {
            if(!m_PlatformTags.ContainsKey(aTag))
            {
                Debug.LogWarning("the tag '" + aTag + "' do no exist in the current Tag Platform Data");
                return "Tag do not exist";
            }
            sPlatformTag value = m_PlatformTags[aTag];

            switch(aWantPlatform)
            {

                //_____ XBOX ______________________\\

                case eLanguageM_Platform.Xbox360:
                {
                    return value.Xbox360Txt;
                }
                case eLanguageM_Platform.XboxOne:
                {
                    return value.XboxOneTxt;
                }
                case eLanguageM_Platform.XboxProjectX:
                {
                    return value.XboxProjectXTxt;
                }

                //_____ PLAYSTATION ______________________\\

                case eLanguageM_Platform.Playstation3:
                {
                    return value.Playstation3Txt;
                }
                case eLanguageM_Platform.Playstation4:
                {
                    return value.Playstation4Txt;
                }
                case eLanguageM_Platform.Playstation5:
                {
                    return value.Playstation5Txt;
                }

                //_____ PC ______________________\\

                case eLanguageM_Platform.Windows:
                {
                    return value.WindowsTxt;
                }
                case eLanguageM_Platform.Linux:
                {
                    return value.LinuxTxt;
                }
                case eLanguageM_Platform.IosComputer:
                {
                    return value.IOSComputerTxt;
                }

                //_____ MOBILE ______________________\\

                case eLanguageM_Platform.Android:
                {
                    return value.AndroidTxt;
                }
                case eLanguageM_Platform.IosMobile:
                {
                    return value.IOSMobileTxt;
                }

                //_____ NINTENDO ______________________\\

                case eLanguageM_Platform.Switch:
                {
                    return value.SwitchTxt;
                }

                //_____ GOOGLE ______________________\\

                case eLanguageM_Platform.Stadia:
                {
                    return value.StadiaTxt;
                }

            }

            return value.UnknowTxt;
        }



        //----------------- Editor Function(s) --------------------

        public StringPlatformTagDictionnary EDITOR_GetTagCallsDictionnary()
        {
            return m_PlatformTags;
        }

        public void EDITOR_SetTagCallsDictionnary(StringPlatformTagDictionnary aPlatformDic)
        {
            m_PlatformTags = aPlatformDic;
        }

        public void EDITOR_SetPlatformTag(sPlatformTag aPlatformTag, string aTag)
        {
            sPlatformTag tempValue;
            if(m_PlatformTags.TryGetValue(aTag, out tempValue))
            {
                m_PlatformTags[aTag] = aPlatformTag;
            }
            else
            {
                m_PlatformTags.Add(aTag, aPlatformTag);
            }
        }

        public void EDITOR_EditTag(string aOldTag, string aNewTag)
        {
            sPlatformTag tempValue;

            if(m_PlatformTags.TryGetValue(aOldTag, out tempValue))
            {
                m_PlatformTags.Remove(aOldTag);
                m_PlatformTags.Add(aNewTag, tempValue);
            }
            else
            {
                m_PlatformTags.Add(aNewTag, new sPlatformTag());
            }

            
            

        }

    }
}

