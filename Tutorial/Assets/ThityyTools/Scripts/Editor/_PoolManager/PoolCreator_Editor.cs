using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace ManagersHUB
{
    [CustomEditor(typeof(Pool_Creator))]
    public class PoolCreator_Editor : BaseCustomEditor
    {
        private SerializedProperty m_Data;
        private SerializedProperty m_ShowPreview;
        private AnimBool m_CanShowEdit;

        private PoolData m_DataCopy;

        private void OnEnable()
        {
            m_CanShowEdit = new AnimBool(false);
            m_CanShowEdit.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.PoolManager, "Pool Creator");
            ShowGroup(null, ShowData);
            EditorGUILayout.EndVertical();
            
        }

        private void GetDatas()
        {
            GetData(ref m_Data, nameof(m_Data));
            GetData(ref m_ShowPreview, nameof(m_ShowPreview));
            if (m_Data.objectReferenceValue == null)
            {
                m_CanShowEdit.target = false;
            }
            else
            {
                m_CanShowEdit.target = m_ShowPreview.boolValue;
                m_DataCopy = (PoolData)m_Data.objectReferenceValue;
            }
        }

        private void ShowData()
        {
            ShowAdvancedSubtitle("Select the pool data for this scene");
            m_Data.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_Data.objectReferenceValue, typeof(PoolData), m_ObjFieldStyle, false, 1);
            if (m_Data.objectReferenceValue != null)
            {
                ShowAdvancedFadeGroup(ref m_CanShowEdit, m_ShowPreview, "Preview", ShowPreview);
            }
        }

        private void ShowPreview()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("[Amount]", m_LabelTxtStyleLeft, GUILayout.Width(80));
            EditorGUILayout.LabelField("[Prefab]", m_LabelTxtStyleCenter, GUILayout.Width(ScreenWidth - 225));
            EditorGUILayout.LabelField("[Auto-Create]", m_LabelTxtStyleRight, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10f);

            List<sPool> currentList = m_DataCopy.GetPoolList();
            string nameObj = "";
            string overlapse = "[√]";
            for(int i = 0; i < currentList.Count; i++)
            {
                if(currentList[i].prefab == null)
                {
                    nameObj = "Null";
                }
                else
                {
                    nameObj = currentList[i].prefab.name;
                }
                if(!currentList[i].overlapse)
                {
                    overlapse = "[  ]";
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(currentList[i].amount.ToString(), m_LabelTxtStyleLeft, GUILayout.Width(50f));
                EditorGUILayout.LabelField(nameObj, m_LabelTxtStyleCenter, GUILayout.Width(ScreenWidth - 150));
                EditorGUILayout.LabelField(overlapse, m_LabelTxtStyleRight, GUILayout.Width(50f));
                EditorGUILayout.EndHorizontal();
            }
        }


    }
}