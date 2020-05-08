using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(SceneData))]
    public class SceneData_Editor : BaseCustomEditor
    {

        private SerializedProperty m_SceneObj;
        private SerializedProperty m_SceneMusic;
        private SerializedProperty m_ScenePool;
        private SerializedProperty m_MusicOption;
        private SerializedProperty m_PoolOption;
        private SerializedProperty m_LoadingType;
        private SerializedProperty m_LoadingObject;
        private SerializedProperty m_TipOption;
        private SerializedProperty m_TipData;
        private SerializedProperty m_TipGroup;
        private SerializedProperty m_LoadingScreen;
        private SerializedProperty m_ScenePath;
        private SerializedProperty m_UseMusicControle;
        private SerializedProperty m_UsePoolControle;
        private SerializedProperty m_FadeColor;


        private AnimBool m_ShowMusicFields;
        private AnimBool m_ShowMusicOptions;
        private AnimBool m_ShowPoolFields;
        private AnimBool m_ShowPoolOptions;

        private AnimBool m_ShowOverwriteOptions;
        private AnimBool m_ShowFadeOptions;
        private AnimBool m_ShowDefaultOptions;

        private AnimBool m_ShowTip;
        private AnimBool m_ShowRandomTip;
        private AnimBool m_ShowSelectTip;

        private SceneAsset m_SceneToLoad;

        private string m_OldPath = "";

        private void OnEnable()
        {
            m_ShowMusicFields = new AnimBool(false);
            m_ShowMusicOptions = new AnimBool(false);
            m_ShowPoolFields = new AnimBool(false);
            m_ShowPoolOptions = new AnimBool(false);
            m_ShowOverwriteOptions = new AnimBool(false);
            m_ShowFadeOptions = new AnimBool(false);
            m_ShowDefaultOptions = new AnimBool(false);
            m_ShowTip = new AnimBool(false);
            m_ShowRandomTip = new AnimBool(false);
            m_ShowSelectTip = new AnimBool(false);

            m_ShowMusicFields.valueChanged.AddListener(Repaint);
            m_ShowPoolFields.valueChanged.AddListener(Repaint);
            m_ShowMusicOptions.valueChanged.AddListener(Repaint);
            m_ShowPoolOptions.valueChanged.AddListener(Repaint);
            m_ShowOverwriteOptions.valueChanged.AddListener(Repaint);
            m_ShowFadeOptions.valueChanged.AddListener(Repaint);
            m_ShowDefaultOptions.valueChanged.AddListener(Repaint);
            m_ShowTip.valueChanged.AddListener(Repaint);
            m_ShowRandomTip.valueChanged.AddListener(Repaint);
            m_ShowSelectTip.valueChanged.AddListener(Repaint);
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

            GetData(ref m_SceneObj, nameof(m_SceneObj));
            GetData(ref m_ScenePath, nameof(m_ScenePath));

            GetData(ref m_UseMusicControle, nameof(m_UseMusicControle));
            m_ShowMusicFields.target = m_UseMusicControle.boolValue;
            GetData(ref m_SceneMusic, nameof(m_SceneMusic));
            GetData(ref m_ScenePool, nameof(m_ScenePool));


            GetData(ref m_UsePoolControle, nameof(m_UsePoolControle));
            m_ShowPoolFields.target = m_UsePoolControle.boolValue;
            GetData(ref m_MusicOption, nameof(m_MusicOption));
            GetData(ref m_PoolOption, nameof(m_PoolOption));

            GetData(ref m_LoadingScreen, nameof(m_LoadingScreen));
            GetData(ref m_LoadingType, nameof(m_LoadingType));
            GetData(ref m_LoadingObject, nameof(m_LoadingObject));

            GetData(ref m_TipOption, nameof(m_TipOption));
            GetData(ref m_TipData, nameof(m_TipData));
            GetData(ref m_TipGroup, nameof(m_TipGroup));
            GetData(ref m_FadeColor, nameof(m_FadeColor));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.SceneManager, "Scene Data");

            ShowGroup(null, ShowSceneSettings);
            DrawSeparator();
            ShowGroup(null, ShowLoadingScreenSetting);
            DrawSeparator();
            ShowGroup(null, ShowLoadingOptions);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowMusicFields, m_UseMusicControle, "Music Controle", ShowMusic);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowPoolFields, m_UsePoolControle, "Pool Controle", ShowPool);

            EditorGUILayout.EndVertical();
        }

        ///<summary> Check if the scene exist in the Build setting </summary>
        private void TestScene()
        {
            m_OldPath = AssetDatabase.GetAssetPath(m_SceneToLoad);
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].path == AssetDatabase.GetAssetPath(m_SceneToLoad))
                {
                    m_SceneObj.stringValue = m_SceneToLoad.name;
                    m_ScenePath.stringValue = AssetDatabase.GetAssetPath(m_SceneToLoad);
                    return;
                }
            }
            ShowWarningScene("The scene '" + m_SceneToLoad.name + "' isn't in the Build Setting!");
        }

        ///<summary> Show to the player if the scene exist in the build setting and let him add it </summary>
        private void ShowWarningScene(string aWarningText)
        {
            bool choose = EditorUtility.DisplayDialog("WARNING!", aWarningText, "Add it", "Cancel");
            if (choose)
            {
                EditorBuildSettingsScene[] aScenes = EditorBuildSettings.scenes;
                AddScene();
            }
            else
            {
                ResetScene(false);
            }
        }

        ///<summary> Add the new scene in the build setting </summary>
        private void AddScene()
        {
            List<EditorBuildSettingsScene> scenesValue = new List<EditorBuildSettingsScene>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenesValue.Add(EditorBuildSettings.scenes[i]);
            }

            scenesValue.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(m_SceneToLoad), true));

            EditorBuildSettings.scenes = scenesValue.ToArray();
            EditorUtility.DisplayDialog("Scene Added", m_SceneToLoad.name + " as been add in the build setting.", "ok");

            m_SceneObj.stringValue = m_SceneToLoad.name;
            m_ScenePath.stringValue = AssetDatabase.GetAssetPath(m_SceneToLoad);
        }

        private void ShowSceneSettings()
        {
            ShowAdvancedSubtitle("Select the scene you want to load");
            if (m_ScenePath.stringValue != "")
            {
                m_SceneToLoad = AssetDatabase.LoadAssetAtPath(m_ScenePath.stringValue, typeof(SceneAsset)) as SceneAsset;
            }

            m_SceneToLoad = ShowAdvancedObjectFieldSceneAsset<SceneAsset>(m_SceneToLoad, typeof(SceneAsset), m_LabelTxtStyleLeft, false, 455454426, 1);

            if (m_OldPath != AssetDatabase.GetAssetPath(m_SceneToLoad) && m_SceneToLoad != null)
            {
                TestScene();
            }
            else if (m_SceneToLoad == null)
            {
                ResetScene(true);
            }
        }

        private void ResetScene(bool aHardReset)
        {
            m_SceneToLoad = null;
            m_SceneObj.stringValue = "";

            if (aHardReset)
            {
                m_OldPath = "";
                m_ScenePath.stringValue = "";
            }
        }

        //-----------------------------------------------

        private void ShowMusic()
        {
            ShowNeedManagerMessage(eManagers.AudioManager);
            ShowAdvancedSubtitle("Select your audio option");
            string[] optionName = m_MusicOption.enumDisplayNames;
            m_MusicOption.enumValueIndex = ShowAdvancedBtnChoice("Music Options", optionName, m_MusicOption.enumValueIndex, 1);

            if (m_MusicOption.enumValueIndex == (int)eSceneM_Music.WithMusic)
            {
                m_ShowMusicOptions.target = true;
            }
            else
            {
                m_ShowMusicOptions.target = false;
            }

            ShowFadeGroup(ref m_ShowMusicOptions, ShowMusicOption);
        }

        private void ShowMusicOption()
        {
            ShowAdvancedObjectField<AudioClip>(m_SceneMusic, typeof(AudioClip), m_LabelTxtStyleLeft, false, 457512540, 1);
        }

        //-----------------------------------------------

        private void ShowPool()
        {
            ShowNeedManagerMessage(eManagers.PoolManager);
            ShowAdvancedSubtitle("Select the poolData to load");
            ShowAdvancedObjectField<PoolData>(m_ScenePool, typeof(PoolData), m_LabelTxtStyleLeft, false, 457512541, 1);
        }

        //-----------------------------------------------

        private void ShowLoadingScreenSetting()
        {
            ShowAdvancedSubtitle("Edit the option for your scene loading");
            string[] optionName = m_LoadingScreen.enumDisplayNames;
            m_LoadingScreen.enumValueIndex = ShowAdvancedBtnChoice("Loading screen type", optionName, m_LoadingScreen.enumValueIndex, 1);

            if (m_LoadingScreen.enumValueIndex == 0)
            {
                m_ShowOverwriteOptions.target = false;
                m_ShowFadeOptions.target = false;
                if (m_ShowOverwriteOptions.faded <= 0 && m_ShowFadeOptions.faded <= 0)
                {
                    m_ShowDefaultOptions.target = true;
                }
            }
            else if (m_LoadingScreen.enumValueIndex == 1)
            {
                m_ShowFadeOptions.target = false;
                m_ShowDefaultOptions.target = false;
                if (m_ShowDefaultOptions.faded <= 0 && m_ShowFadeOptions.faded <= 0)
                {
                    m_ShowOverwriteOptions.target = true;
                }

            }
            else if (m_LoadingScreen.enumValueIndex == 2)
            {
                m_ShowOverwriteOptions.target = false;
                m_ShowDefaultOptions.target = false;
                if (m_ShowDefaultOptions.faded <= 0 && m_ShowOverwriteOptions.faded <= 0)
                {
                    m_ShowFadeOptions.target = true;
                }
            }
        }

        private void ShowLoadingOptions()
        {
            if (m_LoadingScreen.enumValueIndex == (int)eSceneM_LoadingScreen.Default)
            {
                ShowAdvancedSubtitle("Default screen option(s)");
            }
            else if (m_LoadingScreen.enumValueIndex == (int)eSceneM_LoadingScreen.Overwrite)
            {
                ShowAdvancedSubtitle("Overwrite screen option(s)");
            }
            else
            {
                ShowAdvancedSubtitle("Fade screen option(s)");
            }

            ShowFadeGroup(ref m_ShowDefaultOptions, ShowDefaultOption);
            ShowFadeGroup(ref m_ShowOverwriteOptions, ShowOverwriteOption);
            ShowFadeGroup(ref m_ShowFadeOptions, ShowFadeOption);
        }

        private void ShowDefaultOption()
        {
            EditorGUILayout.LabelField("Do you want Tip & Trick?", m_LabelTxtStyleLeft);

            string[] optionName = m_LoadingType.enumDisplayNames;
            m_LoadingType.enumValueIndex = ShowAdvancedBtnChoice(optionName, m_LoadingType.enumValueIndex, 1);

            if (m_LoadingType.enumValueIndex == 0)
            {
                m_ShowTip.target = true;
            }
            else
            {
                m_ShowTip.target = false;
            }

            ShowFadeGroup(ref m_ShowTip, ShowTip);
        }

        private void ShowOverwriteOption()
        {
            EditorGUILayout.LabelField("Overwrite loading screen", m_LabelTxtStyleLeft);
            GUILayout.Space(-10f);
            ShowAdvancedObjectField<LoadingScreenData>(m_LoadingObject, typeof(LoadingScreenData), m_LabelTxtStyleLeft, false, 457512542, 1);
            DrawSeparatorDark(true);
            ShowDefaultOption();
        }

        private void ShowTip()
        {
            ShowTipOption();
        }

        private void ShowFadeOption()
        {
            EditorGUILayout.LabelField("Color to Fade to", m_LabelTxtStyleLeft);
            m_FadeColor.colorValue = ShowAdvancedColorSelection(null, m_FadeColor.colorValue);
        }

        private void ShowTipOption()
        {
            string[] optionName = m_LoadingType.enumDisplayNames;
            optionName = m_TipOption.enumDisplayNames;
            EditorGUILayout.LabelField("How the Tip get select?", m_LabelTxtStyleLeft);
            m_TipOption.enumValueIndex = ShowAdvancedBtnChoice(optionName, m_TipOption.enumValueIndex, 1);
            if (m_TipOption.enumValueIndex == 0)
            {
                m_ShowSelectTip.target = false;
                if (m_ShowSelectTip.faded <= 0)
                {
                    m_ShowRandomTip.target = true;
                }
            }
            else if (m_TipOption.enumValueIndex == 1)
            {
                m_ShowRandomTip.target = false;
                if (m_ShowRandomTip.faded <= 0)
                {
                    m_ShowSelectTip.target = true;
                }
            }
            ShowFadeGroup(ref m_ShowRandomTip, ShowRandomTip);
            ShowFadeGroup(ref m_ShowSelectTip, ShowSelectTip);
        }

        private void ShowRandomTip()
        {
            EditorGUILayout.LabelField("From which group it came from?", m_LabelTxtStyleLeft);
            m_TipGroup.enumValueIndex = ShowAdvancedEnumPopup(m_TipGroup.enumValueIndex, typeof(eSceneM_TipGroup), 2);
        }

        private void ShowSelectTip()
        {
            ShowAdvancedObjectField<LoadingTipData>(m_TipData, typeof(LoadingTipData), m_LabelTxtStyleLeft, false, 457512543, 1);
            if (m_TipData.objectReferenceValue != null)
            {
                LoadingTipData temp = (LoadingTipData)m_TipData.objectReferenceValue;
                string[] texts = temp.GetTip().Split('\n');
                string text = texts[0];
                if (texts.Length > 1)
                {
                    text += "<b>...</b>";
                }
                EditorGUILayout.LabelField("Tip: " + text, m_LabelTxtStyleCenter);
            }
        }

    }
}