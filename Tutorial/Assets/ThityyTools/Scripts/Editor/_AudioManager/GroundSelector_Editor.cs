using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(GroundSelector))]
    public class GroundSelector_Editor : BaseCustomEditor
    {
        private SerializedProperty m_GroundType;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_GroundType, nameof(m_GroundType));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "Ground Selector");
            
            
            ShowGroup("", ShowInsertData);
            EditorGUILayout.EndVertical();
        }

        private void ShowInsertData()
        {
            ShowAdvancedSubtitle("Be sure to select the same layer as the one in the FootSoundCaller");
            ShowAdvancedToolTip("Select the right ground type for this object");
            m_GroundType.enumValueIndex = ShowAdvancedEnumPopup(m_GroundType.enumValueIndex, typeof(eAudioM_GroundType), 1);

            if(((GroundSelector)serializedObject.targetObject).gameObject.GetComponent<Collider>() == null)
            {
                DrawSeparatorDark(true);
                ShowAdvancedWarningMessage("Need a collider component");
            }
        }
    }

}

