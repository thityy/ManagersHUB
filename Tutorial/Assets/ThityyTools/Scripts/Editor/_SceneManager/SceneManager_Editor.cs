using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(SceneManager))]
    public class SceneManager_Editor : BaseCustomEditor
    {

        SerializedProperty m_DefaultLoadingScreenData;
        SerializedProperty m_MinLoadingTimeStandard;
        SerializedProperty m_UseTipSystem;
        SerializedProperty m_UseFadeSystem;
        SerializedProperty m_FadeScreenImg;
        SerializedProperty m_UseAdvancedLoadingSystem;
        SerializedProperty m_UseLoadingBarSystem;
        SerializedProperty m_GroupTip;

        private AnimBool m_TipSystem;
        private AnimBool m_FadeSystem;
        private AnimBool m_AdvancedSystem;
        private AnimBool m_BarSystem;

        private void OnEnable()
        {
            m_TipSystem = new AnimBool(false);
            m_TipSystem.valueChanged.AddListener(Repaint);

            m_FadeSystem = new AnimBool(false);
            m_FadeSystem.valueChanged.AddListener(Repaint);

            m_AdvancedSystem = new AnimBool(false);
            m_AdvancedSystem.valueChanged.AddListener(Repaint);

            m_BarSystem = new AnimBool(false);
            m_BarSystem.valueChanged.AddListener(Repaint);
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
            GetData(ref m_DefaultLoadingScreenData, nameof(m_DefaultLoadingScreenData));
            GetData(ref m_MinLoadingTimeStandard, nameof(m_MinLoadingTimeStandard));
            GetData(ref m_UseTipSystem, nameof(m_UseTipSystem));
            GetData(ref m_UseFadeSystem, nameof(m_UseFadeSystem));
            GetData(ref m_FadeScreenImg, nameof(m_FadeScreenImg));
            GetData(ref m_UseAdvancedLoadingSystem, nameof(m_UseAdvancedLoadingSystem));
            GetData(ref m_UseLoadingBarSystem, nameof(m_UseLoadingBarSystem));
            GetData(ref m_GroupTip, nameof(m_GroupTip));

            m_TipSystem.target = m_UseTipSystem.boolValue;
            m_FadeSystem.target = m_UseFadeSystem.boolValue;
            m_AdvancedSystem.target = m_UseAdvancedLoadingSystem.boolValue;
            m_BarSystem.target = m_UseLoadingBarSystem.boolValue;
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.SceneManager, "Scene Manager");
            ShowGroup("", ShowBaseSetting);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_TipSystem, m_UseTipSystem, "Tip & Trick System", ShowTip);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_FadeSystem, m_UseFadeSystem, "Fade System", ShowFade);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_AdvancedSystem, m_UseAdvancedLoadingSystem, "Overwrite System", ShowAdvancedLoading);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_BarSystem, m_UseLoadingBarSystem, "Loading Bar System", ShowLoadingBar);
            EditorGUILayout.EndVertical();
        }

        private void ShowBaseSetting()
        {
            ShowAdvancedSubtitle("Base Setting(s) of the manager");
            EditorGUILayout.LabelField("Default Loading Screen", m_LabelTxtStyleLeft);
            GUILayout.Space(-10f);
            ShowAdvancedObjectField<LoadingScreenData>(m_DefaultLoadingScreenData, typeof(LoadingScreenData), m_LabelTxtStyleLeft, false, 457512547, 1);
            m_MinLoadingTimeStandard.floatValue = ShowAdvancedFloatField("Minimum Loading Time", m_MinLoadingTimeStandard.floatValue, 0.0f, Mathf.Infinity, 1);
        }

        private void ShowLoadingBar()
        {
            ShowAdvancedToolTip("Loading bar lets you have a bar that display 'fake' loading to the player");
        }

        private void ShowAdvancedLoading()
        {
            ShowAdvancedToolTip("Overwrite option lets you have a custom loading screen for every scene, you just need to set a LoadingScreenData and the manager will create it for you!");
        }   

        private void ShowFade()
        {
            ShowAdvancedToolTip("Fade system lets you change scene with a fade (in/out) effect. When you choose this kind of loading, there's no 'minimum loading time', when the loading is done, the screen fade out automaticly!");
        }

        private void ShowTip()
        {
            ShowAdvancedToolTip("Tip & trick lets you set custom text within a loading screen. It can be choose randomly or with a data to show the player the tip you want them to know!");
            ShowAdvancedSubtitle("Set the tips group");
            ShowAdvancedObjectField<GroupTipsData>(m_GroupTip, typeof(GroupTipsData), m_LabelTxtStyleLeft, false, 457512548, 1);
        }
    }
}

