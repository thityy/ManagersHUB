using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(PoolManager))]
    public class PoolManager_Editor : BaseCustomEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnGUI();
            ShowGUI();
            UpdateProperty();
        }

        private void ShowGUI()
        {

            BeginInspector(eManagers.PoolManager, "Pool Manager");

            ShowGroup("", ShowMessage);
            
            EditorGUILayout.EndVertical();

        }

        private void ShowMessage()
        {
            ShowAdvancedLabelCenter("Pool Manager is setting free. Enjoy!");
            DrawSeparator(false);
        }
    }
}


