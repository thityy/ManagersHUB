using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(LoadingTipData))]
    public class TipData_Editor : BaseCustomEditor
    {
        private SerializedProperty m_TipGroup;
        private SerializedProperty m_Tip;
        private SerializedProperty m_TextData;
        private SerializedProperty m_TextType;
        private AnimBool m_ShowWarning;

        private void OnEnable()
        {
            m_ShowWarning = new AnimBool(false);
            m_ShowWarning.valueChanged.AddListener(Repaint);
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
            GetData(ref m_TipGroup, nameof(m_TipGroup));
            GetData(ref m_Tip, nameof(m_Tip));
            GetData(ref m_TextData, nameof(m_TextData));
            GetData(ref m_TextType, nameof(m_TextType));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.SceneManager, "Tip Data");

            ShowGroup(null, ShowSelectType);
            DrawSeparator();
            ShowGroup(null, ShowStandardGUI);


            EditorGUILayout.EndVertical();
        }

        private void ShowSelectType()
        {
            ShowAdvancedSubtitle("Select the type of your TipData text");
            m_TextType.enumValueIndex = ShowAdvancedBtnChoice(new string[] { "String", "MultiLanguage Data" }, m_TextType.enumValueIndex, 0);
            if (m_TextType.enumValueIndex == (int)eSceneM_TextType.TextData)
            {
                m_ShowWarning.target = true;
            }
            else
            {
                m_ShowWarning.target = false;
            }
            ShowFadeGroup(ref m_ShowWarning, ShowWarningManager);
        }

        private void ShowWarningManager()
        {
            ShowNeedManagerMessage(eManagers.LanguageManager);
        }

        private void ShowStandardGUI()
        {
            ShowAdvancedSubtitle("Insert the tip value");
            if (m_TextType.enumValueIndex == (int)eSceneM_TextType.String)
            {
                m_Tip.stringValue = ShowAdvancedTextInsert(m_Tip.stringValue, 1);
            }
            else
            {
                ShowAdvancedObjectField<MultiLanguageData>(m_TextData, typeof(MultiLanguageData), m_LabelTxtStyleLeft, false, 457512540, 1);
            }
        }
    }
}

