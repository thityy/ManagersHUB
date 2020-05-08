using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{

    public static class AvailableLanguage
    {
        private const string cEditorPrefPathAvailableLanguage = "HUD.AvailableLanguage";

        public static string AvailableLanguagePath
        {
            get { return EditorPrefs.GetString(cEditorPrefPathAvailableLanguage, ""); }
            set { EditorPrefs.SetString(cEditorPrefPathAvailableLanguage, value); }
        }
    }

    [CustomEditor(typeof(LanguageManager))]
    public class LanguageManager_Editor : BaseCustomEditor
    {

        private SerializedProperty m_DefaultLanguage;
        private SerializedProperty m_AvailableLanguageData;
        private SerializedProperty m_UseDeviceLanguage;

        private SerializedProperty m_DefaultPlatform;
        private SerializedProperty m_TagData;
        private SerializedProperty m_SpriteSheetData;

        private AvailableLanguageData m_LanguageData;
        private int m_CurrentLanguageIdToAdd = 0;
        private int m_CurrentLanguageSelect;
        private int m_CurrentDefaultLanguageId;

        private LanguageManager m_LanguageManager;

        private SerializedProperty m_TagCall;
        private SerializedProperty m_CurrentSystem;
        private SerializedProperty m_CurrentDevice;
        private SerializedProperty m_DefaultSpriteAsset;

        private int m_UseDeviceLanguageId;
        private bool m_NewDataAlreadyCheck = false;

        private Vector2 m_ScrollViewLanguage;
        private float m_scrollSize;

        private PlatformSpriteAssetDictionnary m_SpriteAssetsPlatform;

        private AnimBool m_CanShowOptions;

        private void OnEnable()
        {
            m_CurrentLanguageSelect = -1;

            m_CanShowOptions = new AnimBool(false);
            m_CanShowOptions.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_DefaultLanguage, nameof(m_DefaultLanguage));
            GetData(ref m_AvailableLanguageData, nameof(m_AvailableLanguageData));
            GetData(ref m_UseDeviceLanguage, nameof(m_UseDeviceLanguage));
            if (m_UseDeviceLanguage.boolValue)
            {
                m_UseDeviceLanguageId = 0;
            }
            else
            {
                m_UseDeviceLanguageId = 1;
            }

            GetData(ref m_TagCall, nameof(m_TagCall));
            GetData(ref m_CurrentSystem, nameof(m_CurrentSystem));
            GetData(ref m_CurrentDevice, nameof(m_CurrentDevice));

            GetData(ref m_DefaultPlatform, nameof(m_DefaultPlatform));
            GetData(ref m_TagData, nameof(m_TagData));
            GetData(ref m_SpriteSheetData, nameof(m_SpriteSheetData));
            GetData(ref m_DefaultSpriteAsset, nameof(m_DefaultSpriteAsset));


            m_LanguageManager = (LanguageManager)serializedObject.targetObject;
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Language Manager");


            ShowGroup("", ShowSetData);
            DrawSeparator(true);

            CheckIfDataExist();

            ShowGroup("", CallShowAvailableLanguage);
            DrawSeparator(true);
            ShowGroup("", CallShowDefaultLanguage);
            DrawSeparator(true);
            ShowGroup("", CallShowTagOption);

            EditorGUILayout.EndVertical();


        }

        private void CallShowAvailableLanguage()
        {
            ShowFadeGroup(ref m_CanShowOptions, ShowAvailableLanguage);
        }

        private void CallShowDefaultLanguage()
        {
            ShowFadeGroup(ref m_CanShowOptions, ShowDefaultLanguage);
        }

        private void CallShowTagOption()
        {
            ShowFadeGroup(ref m_CanShowOptions, ShowTagOption);
        }

        //_______________ Set AvailableLanguageData Functions _______________\\

        ///<summary> Show the option to set the AvailableLanguageData</summary>
        private void ShowSetData()
        {
            ShowTitle("Language Data", eManagers.LanguageManager);

            ShowAdvancedLabelCenter("Set Available Language Data", m_LabelTxtStyleLeft);
            m_AvailableLanguageData.objectReferenceValue = AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData));
            ShowAdvancedObjectField<AvailableLanguageData>(m_AvailableLanguageData, typeof(AvailableLanguageData), m_LabelTxtStyleLeft, false, 457512540, 2);
            ShowAdvancedToolTip("You need to set an AvailableLanguageData so the manager can tell your project and MultiLanguage_Datas which languages can be use.");

            if (m_AvailableLanguageData.objectReferenceValue != AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData)) && !m_NewDataAlreadyCheck)
            {
                ShowWarningChangeAvailableLanguage();
            }
            else
            {
                m_NewDataAlreadyCheck = false;
            }
        }

        ///<summary> Show a Message Box that show you a warning before you change the language data</summary>
        private void ShowWarningChangeAvailableLanguage()
        {
            if ((AvailableLanguageData)AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData)) == null)
            {
                AvailableLanguage.AvailableLanguagePath = AssetDatabase.GetAssetPath(m_AvailableLanguageData.objectReferenceValue);
                return;
            }
            string[] miss = MissLanguage((AvailableLanguageData)AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData)), (AvailableLanguageData)m_AvailableLanguageData.objectReferenceValue);
            if (miss.Length > 0)
            {
                string text;
                if (miss.Length == 1)
                {
                    text = "Your new AvailableLanguageData didn't have this language: \n\n- " + miss[0] + "\nIf you continue, you will lose all values set for this language across all MultiLanguage Data!";
                }
                else
                {
                    text = "Your new AvailableLanguageData didn't have those languages: \n\n";
                    for (int i = 0; i < miss.Length; i++)
                    {
                        text += "- " + miss[i] + "\n";
                    }
                    text += "If you continue, you will lose all values set for those languages across all MultiLanguage Data!";
                }

                bool choose = EditorUtility.DisplayDialog("WARNING!", text, "Continue", "Cancel");
                if (choose)
                {
                    AvailableLanguage.AvailableLanguagePath = AssetDatabase.GetAssetPath(m_AvailableLanguageData.objectReferenceValue);
                }
                else
                {
                    m_NewDataAlreadyCheck = true;
                }
            }
            else
            {
                AvailableLanguage.AvailableLanguagePath = AssetDatabase.GetAssetPath(m_AvailableLanguageData.objectReferenceValue);
            }
        }

        ///<summary> Check if there's a valid AvailableLanguageData set in the manager</summary>
        private void CheckIfDataExist()
        {
            if (m_AvailableLanguageData.objectReferenceValue != null)
            {
                m_CanShowOptions.target = true;
                m_LanguageData = (AvailableLanguageData)m_AvailableLanguageData.objectReferenceValue;
                m_LanguageData.EDITOR_InitLanguageIsNeed();
            }
            else
            {
                m_CanShowOptions.target = false;
            }
        }

        //_______________ Show Default Options Functions _______________\\

        ///<summary> Show the full option for the default Language</summary>
        private void ShowDefaultLanguage()
        {
            ShowTitle("Options de démarrage", eManagers.LanguageManager);

            ShowAdvancedLabelCenter("Set start-up language", m_LabelTxtStyleLeft);
            m_UseDeviceLanguageId = ShowAdvancedBtnChoice(new string[] { "Use device", "Use pre-selected" }, m_UseDeviceLanguageId, 1);
            ShowAdvancedToolTip("Choose if you want your project to start on the <b>" + ColorManager.GetColorText("device", eManagers.LanguageManager, true) + "</b> language or on a <b>" + ColorManager.GetColorText("pre-selected", eManagers.LanguageManager, true) + "</b> language.");

            DrawSeparatorInvisible(true);
            
            if (m_UseDeviceLanguageId == 0)
            {
                m_UseDeviceLanguage.boolValue = true;
                EditorGUILayout.LabelField("Select the default language (If device language isn't in the list)", m_LabelTxtStyleLeft);
            }
            else
            {
                m_UseDeviceLanguage.boolValue = false;
                EditorGUILayout.LabelField("Select the default language", m_LabelTxtStyleLeft);
            }
            m_CurrentDefaultLanguageId = ShowAdvancedEnumPopup(GetAvailableLanguageList(), m_CurrentDefaultLanguageId, 4);
            m_DefaultLanguage.enumValueIndex = (int)m_LanguageData.GetLanguage(m_CurrentDefaultLanguageId);
            ShowAdvancedToolTip("Select the<b> " + ColorManager.GetColorText("language your project will start on", eManagers.LanguageManager, true) + "</b> if you are using 'pre-selected' option or if the device language isn't set for your project when using 'device' option.");

            DrawSeparatorInvisible(true);

            EditorGUILayout.LabelField("Select platform", m_LabelTxtStyleLeft);
            m_DefaultPlatform.enumValueIndex = (int)ShowAdvancedEnumPopup(m_DefaultPlatform.enumValueIndex, typeof(eLanguageM_Platform), 4);
            ShowAdvancedToolTip("Select the <b> " + ColorManager.GetColorText("platform in use for this build", eManagers.LanguageManager, true) + "</b>. Change this <b> " + ColorManager.GetColorText("each time", eManagers.LanguageManager, true) + " </b>you build your game for a specific platform (Xbox, Playstation, Pc, etc.)");
        }

        //_______________ Show Edit AvailableLanguage Functions _______________\\

        ///<summary> RETURN an array with all Language in the AvailableLanguageData</summary>
        private string[] GetAvailableLanguageList()
        {
            string[] lists = new string[m_LanguageData.GetAvailableLanguageCount()];
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = m_LanguageData.GetAvailableLanguages()[i].ToString();
            }

            return lists;
        }

        ///<summary> Show the full option for the actual AvailableLanguageData</summary>
        private void ShowAvailableLanguage()
        {
            ShowTitle("Available Language(s)", eManagers.LanguageManager);
            ShowAdvancedLabelCenter("Edit Available(s) Language(s)", m_LabelTxtStyleLeft);
            ShowGroup("", ShowListDeleteLanguage);
            DrawSeparatorDark(true);
            ShowAddLanguageOption();
            ShowAdvancedToolTip("Select <b>" + ColorManager.GetColorText("all", eManagers.LanguageManager, true) +"</b> languages you want for your project.");
            ShowAdvancedWarningMessage("Before removing a language from the list, make sure you won't use it anymore, cause it would delete all text data link to this language.");
        }

        ///<summary> Show the option that lets you add a language in a data</summary>
        private void ShowAddLanguageOption()
        {
            bool canAdd = true;
            if (HaveLanguage((SystemLanguage)m_CurrentLanguageIdToAdd))
            {
                canAdd = false;
            }
            m_CurrentLanguageIdToAdd = ShowAdvancedEnumPopupAdd(m_CurrentLanguageIdToAdd, typeof(SystemLanguage), 2, canAdd, AddLanguage);
        }

        ///<summary> Show a Message Box that show you a warning before destroying a language from the data</summary>
        private void ShowWarningDeleteLanguage()
        {
            bool choose = EditorUtility.DisplayDialog("WARNING!", "You are about to remove a language (" + m_LanguageData.GetLanguage(m_CurrentLanguageSelect).ToString() + ") from the available List. It will delete all value for all MultiLanguage Data", "Remove it", "Cancel");
            if (choose)
            {
                if (m_DefaultLanguage.enumValueIndex == (int)m_LanguageData.GetLanguage(m_CurrentLanguageSelect))
                {
                    m_DefaultLanguage.enumValueIndex = (int)m_LanguageData.GetLanguage(0);
                    m_CurrentDefaultLanguageId = 0;
                }
                m_LanguageData.GetAvailableLanguages().RemoveAt(m_CurrentLanguageSelect);
            }
        }

        ///<summary> RETURN an array of missing language from the new language data</summary>
        private string[] MissLanguage(AvailableLanguageData aOldData, AvailableLanguageData aNewData)
        {
            List<string> miss = new List<string>();
            if (aNewData == null)
            {
                for (int i = 0; i < aOldData.GetAvailableLanguageCount(); i++)
                {
                    miss.Add(aOldData.GetLanguage(i).ToString());
                }
            }
            else
            {
                bool haveIn = false;

                for (int i = 0; i < aOldData.GetAvailableLanguageCount(); i++)
                {
                    for (int k = 0; k < aNewData.GetAvailableLanguageCount(); k++)
                    {
                        if (aOldData.GetLanguage(i) == aNewData.GetLanguage(k))
                        {
                            k = aNewData.GetAvailableLanguageCount();
                            Debug.Log(aOldData.GetLanguage(i) + " " + aNewData.GetLanguage(k));
                            haveIn = true;
                        }
                    }
                    if (!haveIn)
                    {
                        miss.Add(aOldData.GetLanguage(i).ToString());
                    }
                }
            }

            return miss.ToArray();
        }

        ///<summary> RETURN true if the current Language Data have the language in it</summary>
        private bool HaveLanguage(SystemLanguage aLanguage)
        {
            return m_LanguageData.GetAvailableLanguages().Contains(aLanguage);
        }

        ///<summary> Show a list of all language in the language data and show option to remove them</summary>
        private void ShowListDeleteLanguage()
        {
            bool needScroll = false;
            if (m_LanguageData.GetAvailableLanguageCount() < 5)
            {
                if (m_LanguageData.GetAvailableLanguageCount() == 1)
                {
                    m_scrollSize = 40.0f;
                }
                else
                {
                    m_scrollSize = m_LanguageData.GetAvailableLanguageCount() * 22.5f;
                }
            }
            else
            {
                m_scrollSize = 4 * 21.1f;
                needScroll = true;
            }

            m_ScrollViewLanguage = EditorGUILayout.BeginScrollView(m_ScrollViewLanguage, GUILayout.Height(m_scrollSize));

            for (int i = 0; i < m_LanguageData.GetAvailableLanguageCount(); i++)
            {
                if (m_CurrentLanguageSelect == i)
                {
                    bool canRemove = false;
                    if (m_LanguageData.GetAvailableLanguages().Count > 1 && m_CurrentLanguageSelect >= 0)
                    {
                        canRemove = true;
                    }

                    if (needScroll)
                    {
                        if (ShowAdvancedListBtnRemove(m_LanguageData.GetLanguage(i).ToString(), null, true, canRemove, RemoveLanguage, 5))
                        {
                            m_CurrentLanguageSelect = i;
                        }
                    }
                    else
                    {
                        if (ShowAdvancedListBtnRemove(m_LanguageData.GetLanguage(i).ToString(), null, true, canRemove, RemoveLanguage, 3))
                        {
                            m_CurrentLanguageSelect = i;
                        }
                    }

                }
                else
                {
                    if (needScroll)
                    {
                        if (ShowAdvancedListBtn(m_LanguageData.GetLanguage(i).ToString(), null, false, 5))
                        {
                            m_CurrentLanguageSelect = i;
                        }
                    }
                    else
                    {
                        if (ShowAdvancedListBtn(m_LanguageData.GetLanguage(i).ToString(), null, false, 4))
                        {
                            m_CurrentLanguageSelect = i;
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        ///<summary> Add the language in the LanguageData</summary>
        private void AddLanguage()
        {
            m_LanguageData.AddLanguage((SystemLanguage)m_CurrentLanguageIdToAdd);
        }

        ///<summary> Removve the language in the LanguageData</summary>
        private void RemoveLanguage()
        {
            ShowWarningDeleteLanguage();
            m_CurrentLanguageSelect = -1;
        }

        //_______________ Show Tag Options Functions _______________\\

        private void ShowTagOption()
        {
            ShowTitle("Sprite system", eManagers.LanguageManager);
            ShowAdvancedToolTip("Tag options lets you call TextMeshPro Sprite depend on the platform you're on.");
            EditorGUILayout.LabelField("Default SpriteAsset", m_LabelTxtStyleLeft);
            GUILayout.Space(-10f);
            ShowAdvancedObjectField<TMPro.TMP_SpriteAsset>(m_DefaultSpriteAsset, typeof(TMPro.TMP_SpriteAsset), m_LabelTxtStyleLeft, false, 457512549, 2);
            DrawSeparator(true);
            ShowTagChoice();
            if (m_TagCall.enumValueIndex == (int)eTagCallOption.MultipleSpriteAsset)
            {
                ShowSetSpriteSheet();
            }
            else
            {
                ShowSetTagData();
            }
        }

        private void ShowTagChoice()
        {
            List<string> choice = new List<string>();
            for (int i = 0; i < EnumSystem.GetEnumCount(typeof(eTagCallOption)); i++)
            {
                eTagCallOption current = (eTagCallOption)i;
                choice.Add(current.ToString());
            }
            m_TagCall.enumValueIndex = ShowAdvancedBtnChoice(choice.ToArray(), m_TagCall.enumValueIndex, 1);
        }

        private void ShowSetTagData()
        {
            ShowAdvancedObjectField<TagPlatformData>(m_TagData, typeof(TagPlatformData), m_LabelTxtStyleLeft, false, 457512547, 2);
        }

        private void ShowSetSpriteSheet()
        {
            ShowAdvancedObjectField<SpriteSheetPlatformData>(m_SpriteSheetData, typeof(SpriteSheetPlatformData), m_LabelTxtStyleLeft, false, 457512548, 2);
        }


    }
}
