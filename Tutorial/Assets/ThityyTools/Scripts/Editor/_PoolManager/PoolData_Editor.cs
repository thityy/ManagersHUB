using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(PoolData))]
    public class PoolData_Editor : BaseCustomEditor
    {

        private SerializedProperty m_Pools;

        private int m_CurrentObjectSelectId;

        private int m_Amount;
        private bool m_Overlapse;
        private Object m_Prefab;

        private Vector2 m_ScrollViewObject;
        private float m_scrollSize;

        private AnimBool m_CanShowEdit;
        private Texture m_BtnIcon;

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
            BeginInspector(eManagers.PoolManager, "Pool Data");
            ShowGroup(null, ShowPrefabList);
            DrawSeparator();
            ShowGroup(null, ShowEditPage);
            EditorGUILayout.EndVertical();

        }

        private void GetDatas()
        {
            GetData(ref m_Pools, nameof(m_Pools));
        }

        private void ShowPrefabList()
        {
            ShowAdvancedSubtitle("Select prefabs for the pool");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("[Amount]", m_LabelTxtStyleLeft, GUILayout.Width(80));
            EditorGUILayout.LabelField("[Prefab]", m_LabelTxtStyleCenter, GUILayout.Width(ScreenWidth - 225));
            EditorGUILayout.LabelField("[Auto-Create]", m_LabelTxtStyleRight, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Pooled Object(s):", m_LabelTxtStyleLeft);
            ShowGroup(null, ShowObjectInPool);
            if (GUILayout.Button("Add a Prefab in the pool", m_ButtonStyle))
            {
                AddPrefabInPool();
            }
        }


        private void ShowObjectInPool()
        {
            if (m_Pools.arraySize < 8)
            {
                m_scrollSize = m_Pools.arraySize * 25.0f;
            }
            else
            {
                m_scrollSize = 8 * 25.0f;
            }

            m_ScrollViewObject = EditorGUILayout.BeginScrollView(m_ScrollViewObject, GUILayout.Height(m_scrollSize));

            for (int i = 0; i < m_Pools.arraySize; i++)
            {
                if (m_Pools.GetArrayElementAtIndex(i).FindPropertyRelative("prefab").objectReferenceValue == null)
                {
                    m_BtnIcon = m_WarningIcon;
                }
                else
                {
                    m_BtnIcon = null;
                }

                if (i == m_CurrentObjectSelectId)
                {
                    if (ShowAdvancedListBtnRemove(GetDisplayName(i), m_BtnIcon, true, true, RemoveCurrentObject, 2))
                    {
                        m_CurrentObjectSelectId = i;
                    }
                }
                else
                {
                    if (ShowAdvancedListBtn(GetDisplayName(i), m_BtnIcon, false, 2))
                    {
                        m_CurrentObjectSelectId = i;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void AddPrefabInPool()
        {

            m_Pools.InsertArrayElementAtIndex(m_Pools.arraySize);
            m_CurrentObjectSelectId = m_Pools.arraySize - 1;
            m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("prefab").objectReferenceValue = null;
            m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("amount").intValue = 1;
            m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("overlapse").boolValue = false;
            
        }

        private string GetDisplayName(int aObjectId)
        {
            
            m_Prefab = m_Pools.GetArrayElementAtIndex(aObjectId).FindPropertyRelative("prefab").objectReferenceValue;
            m_Amount = m_Pools.GetArrayElementAtIndex(aObjectId).FindPropertyRelative("amount").intValue;
            m_Overlapse = m_Pools.GetArrayElementAtIndex(aObjectId).FindPropertyRelative("overlapse").boolValue;



            string displayName = "NULL";
            if (m_Prefab != null)
            {
                displayName = m_Prefab.name;
            }
            if (m_Overlapse)
                return " [" + m_Amount.ToString() + "] - [" + displayName + "] - [√]";

            return " [" + m_Amount.ToString() + "] - [" + displayName + "] - [  ]";
        }

        private void RemoveCurrentObject()
        {
            int tempId = m_CurrentObjectSelectId;

            m_Pools.DeleteArrayElementAtIndex(tempId);
            m_CurrentObjectSelectId -= 1;
        }

        private void ShowEditPage()
        {

            if (m_CurrentObjectSelectId < m_Pools.arraySize && m_CurrentObjectSelectId >= 0)
            {
                ShowAdvancedSubtitle("Edit Selected Prefab Pool");
                m_CanShowEdit.target = true;
                ShowFadeGroup(ref m_CanShowEdit, ShowPoolObject);
            }
            else
            {
                ShowAdvancedSubtitle("[Select a prefab to edit]");
                m_CanShowEdit.target = false;
            }

        }

        private void ShowPoolObject()
        {
            ShowAdvancedObjectField<GameObject>(m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("prefab"), typeof(GameObject), m_LabelTxtStyleLeft, false, 45402120, 1);
            if(PrefabAlreadyExist((Object)m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("prefab").objectReferenceValue))
            {
                m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("prefab").objectReferenceValue = null;
                EditorUtility.DisplayDialog("WARNING!", "This prefab is already use in the data. Augmented the amount instead.", "Continue");
                return;
            }
            m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("amount").intValue = ShowAdvancedIntField("Amount: ", m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("amount").intValue, 1, 1000, 1);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Auto-Create when exceeds: ", m_LabelTxtStyleLeft);
            m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("overlapse").boolValue = EditorGUILayout.Toggle(m_Pools.GetArrayElementAtIndex(m_CurrentObjectSelectId).FindPropertyRelative("overlapse").boolValue, GUILayout.Width(20f));
            EditorGUILayout.EndHorizontal();
        }

        private bool PrefabAlreadyExist(Object aPrefab)
        {
            if(aPrefab == null)
            {
                return false;
            }
            int amountOfCopy = 0;
            for(int i = 0; i < m_Pools.arraySize; i++)
            {
                if(((Object)m_Pools.GetArrayElementAtIndex(i).FindPropertyRelative("prefab").objectReferenceValue) == aPrefab)
                {
                    amountOfCopy++;
                }
                if(amountOfCopy >= 2)
                {
                    return true;
                }
            }
            return false;
        }



    }
}

