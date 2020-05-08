using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(FootSoundCaller))]
    public class NewBehaviourScript : BaseCustomEditor
    {

        private SerializedProperty m_GroundLayer;
        private SerializedProperty m_GroundCheck_LeftFootPoint;
        private SerializedProperty m_GroundCheck_RightFootPoint;
        private SerializedProperty m_DistanceFromGround;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_GroundLayer, nameof(m_GroundLayer));
            GetData(ref m_GroundCheck_LeftFootPoint, nameof(m_GroundCheck_LeftFootPoint));
            GetData(ref m_GroundCheck_RightFootPoint, nameof(m_GroundCheck_RightFootPoint));
            GetData(ref m_DistanceFromGround, nameof(m_DistanceFromGround));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "FootSound Caller");

            ShowGroup("", ShowInsertData);
            EditorGUILayout.EndVertical();
        }

        private void ShowInsertData()
        {
            ShowAdvancedToolTip("Add this component on the same Object as the player animator, so you can acces it with event within your animation.");
            EditorGUILayout.PropertyField(m_GroundLayer);
            DrawSeparator(true);
            ShowAdvancedLabelCenter("Left foot Transform");
            EditorGUILayout.Space(-15f);
            m_GroundCheck_LeftFootPoint.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_GroundCheck_LeftFootPoint.objectReferenceValue, typeof(Transform), m_ObjFieldStyle, true, 1);
            ShowAdvancedLabelCenter("Right foot Transform");
            EditorGUILayout.Space(-15f);
            m_GroundCheck_RightFootPoint.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_GroundCheck_RightFootPoint.objectReferenceValue, typeof(Transform), m_ObjFieldStyle, true, 1);
            DrawSeparatorDark(true);
            m_DistanceFromGround.floatValue = ShowAdvancedFloatField("Distance from ground", m_DistanceFromGround.floatValue, Mathf.NegativeInfinity, Mathf.Infinity, 1);
        }
    }

}
