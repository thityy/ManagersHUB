using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{

    [CustomEditor(typeof(LoadingScreenData))]
    public class LoadingScreenData_Editor : BaseCustomEditor
    {

        private SerializedProperty m_LoadingScreenPrefab;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void ShowGUI()
        {

            BeginInspector(eManagers.SceneManager, "Loading Screen Data");

            ShowGroup("", ShowToolTip);
            DrawSeparator();
            ShowGroup("", ShowMessage);
            
            EditorGUILayout.EndVertical();

        }

        private void GetDatas()
        {
            GetData(ref m_LoadingScreenPrefab, nameof(m_LoadingScreenPrefab));
        }

        private void ShowToolTip()
        {
            ShowAdvancedToolTip("Loading Screen Data are there to make sure you use a correct loading screen prefab for the loading. Select a prefab with 'LoadingScreenObj.cs' script on it and set it correctly to make it work for your needs.");
        }

        private void ShowMessage()
        {
            ShowAdvancedSubtitle("Select a Loading screen");
            ShowAdvancedObjectField<LoadingScreenObj>(m_LoadingScreenPrefab, typeof(LoadingScreenObj), m_LabelTxtStyleLeft, false, 457512539, 1);
        }
    }
}
