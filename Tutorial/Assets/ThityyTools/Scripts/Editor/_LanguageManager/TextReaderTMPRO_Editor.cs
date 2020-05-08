using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using TMPro;

namespace ManagersHUB
{
    [CustomEditor(typeof(TextReaderTMPRO))]
    public class TextReaderTMPRO_Editor : BaseCustomEditor
    {

        private SerializedProperty m_TextData;
        private SerializedProperty m_ShowPreview;
        private SerializedProperty m_TextDisplay;

        private MultiLanguageData m_LanguageData;

        private AnimBool m_UseTextData;

        private void OnEnable()
        {
            m_UseTextData = new AnimBool(false);
            m_UseTextData.valueChanged.AddListener(Repaint);

            
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
            GetData(ref m_TextData, nameof(m_TextData));
            GetData(ref m_ShowPreview, nameof(m_ShowPreview));
            GetData(ref m_TextDisplay, nameof(m_TextDisplay));
            m_UseTextData.target = m_ShowPreview.boolValue;
            m_LanguageData = (MultiLanguageData)m_TextData.objectReferenceValue;
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Text Reader TMPro");
            if (Selection.activeGameObject.GetComponent<TextMeshProUGUI>() == null)
            {
                ShowGroup("", ShowWarningUGUI);
            }
            else
            {
                ShowGroup("", ShowSelectData);
                DrawSeparator();
                if (m_LanguageData == null || m_TextData.objectReferenceValue == null)
                {
                    m_UseTextData.target = false;
                }
                ShowAdvancedFadeGroup(ref m_UseTextData, m_ShowPreview, "Preview", CallShowTestData);
            }


            EditorGUILayout.EndVertical();
        }

        private void ShowWarningUGUI()
        {
            ShowAdvancedWarningMessage("You need to set a TextMeshPro_UGUI for this script to work");
        }

        private void ShowSelectData()
        {
            ShowAdvancedSubtitle("Select the MultiLanguage data that will be use");
            Object value = m_TextData.objectReferenceValue;
            ShowAdvancedObjectField<MultiLanguageData>(ref value, typeof(MultiLanguageData), m_LabelTxtStyleLeft, false, 457512540, 1);
            m_TextData.objectReferenceValue = value; ;
        }

        private void CallShowTestData()
        {
            if (m_LanguageData == null || m_TextData.objectReferenceValue == null)
            {
                return;
            }
            ShowFadeGroup(ref m_UseTextData, ShowTestData);
        }

        private void ShowTestData()
        {
            List<string> allTextValue = new List<string>();
            SystemLanguageMultiLanguageValueDictionnary values = m_LanguageData.EDITOR_GetMultiLanguageValue();
            foreach (KeyValuePair<SystemLanguage, sMultiLanguageValue> element in values)
            {
                if (!string.IsNullOrEmpty(element.Value.m_Txt))
                {
                    string[] line = element.Value.m_Txt.Split('\n');
                    EditorGUILayout.LabelField(element.Key.ToString() + " : " + line[0], m_LabelTxtStyleLeft);
                }
                else
                {
                    EditorGUILayout.LabelField(element.Key.ToString() + " : <i>Empty</i>", m_LabelTxtStyleLeft);
                }
                allTextValue.Add(element.Value.m_Txt);
            }
        }
    }
}
