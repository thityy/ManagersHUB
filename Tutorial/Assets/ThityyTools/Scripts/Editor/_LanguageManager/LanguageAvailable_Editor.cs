using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(AvailableLanguageData))]
    public class LanguageAvailable_Editor : BaseCustomEditor
    {

        private SerializedProperty m_Languages;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_Languages, nameof(m_Languages));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Available Language Data");
            ShowGroup(null, ShowInfo);
            DrawSeparator();
            ShowGroup(null, ShowLanguages);
            EditorGUILayout.EndVertical();
        }

        private void ShowInfo()
        {
            ShowAdvancedSubtitle("READ ONLY");
            ShowAdvancedToolTip("AvailableLanguage Data are use to select which language your game is available in. Can't edit from here, use it in the Language Manager.");
        }

        private void ShowLanguages()
        {
            ShowAdvancedSubtitle("Language available in Data");
            for(int i = 0; i < m_Languages.arraySize; i++)
            {
                SystemLanguage language = (SystemLanguage)m_Languages.GetArrayElementAtIndex(i).intValue;
                EditorGUILayout.LabelField("- " + language.ToString(), m_LabelTxtStyleLeft);
            }
        }
    }
}


