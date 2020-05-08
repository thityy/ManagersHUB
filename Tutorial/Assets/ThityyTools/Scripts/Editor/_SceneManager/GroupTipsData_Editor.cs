using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(GroupTipsData))]
    public class GroupTipsData_Editor : BaseCustomEditor
    {

        private eSceneM_TipGroup m_CurrentGroup;

        private List<sTipGroup> m_TipDatas;
        private GroupTipsData m_Self;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
            EditorUtility.SetDirty((GroupTipsData)target);
        }

        private void GetDatas()
        {
            m_Self = (GroupTipsData)serializedObject.targetObject;
            m_TipDatas = m_Self.GetAllTipDatas();
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.SceneManager, "Group Tips Data");

            ShowGroup(null, ShowGroupSelector);
            DrawSeparator();
            ShowGroup(null, ShowGroupInEdit);

            EditorGUILayout.EndVertical();

            m_Self.EDITOR_SetAllTipDatas(m_TipDatas);
        }

        private void ShowGroupSelector()
        {
            ShowAdvancedSubtitle("Select the group to edit");

            string[] groupTxt = new string[]{"Group Alpha", "Group Bravo", "Group Charlie", "Group Delta", "Group Echo", "Group Foxtrot", "Group Golf","Group Hotel", "Group India", "Group Juliett"};
            m_CurrentGroup = (eSceneM_TipGroup)ShowAdvancedEnumPopup(groupTxt, (int)m_CurrentGroup, 1);
        }

        private void ShowGroupInEdit()
        {
            ShowAdvancedSubtitle("Drop all the tips of this group");
            EditorGUILayout.LabelField("Tip(s) of " + m_CurrentGroup.ToString(), m_LabelTxtStyleLeft);
            ShowTipList();
        }

        private void ShowTipList()
        {
            int toRemove = -1;
            Color colorValue;
            List<LoadingTipData> listInUse = m_TipDatas[(int)m_CurrentGroup].TipData;

            if(listInUse.Count == 0)
            {
                GUILayout.Space(15f);
                ShowAdvancedLabelCenter("Empty");
            }

            for(int i = 0; i < listInUse.Count; i++)
            {
                LoadingTipData objFromList = listInUse[i];

                if(i % 2 == 0)
                {
                    colorValue = CurrentColor;
                }
                else
                {
                    colorValue = CurrentColorDark;
                }
                colorValue.a = 0.5f;

                GUI.backgroundColor = colorValue;
                EditorGUILayout.BeginHorizontal();
                objFromList = (LoadingTipData)EditorGUILayout.ObjectField(objFromList, typeof(LoadingTipData), false, GUILayout.Width(ScreenWidth - 103), GUILayout.Height(20f));
                if(GUILayout.Button("Remove", m_ButtonStyle, GUILayout.Width(60f), GUILayout.Height(20f)))
                {
                    objFromList = null;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();

                if(objFromList == null)
                {
                    toRemove = i;
                }
                GUILayout.Space(-2f);
            }

            DrawSeparatorDark(true);

            EditorGUILayout.LabelField("Add a new tip ", m_LabelTxtStyleLeft);
            LoadingTipData tipData = null;
            tipData = (LoadingTipData)EditorGUILayout.ObjectField(tipData, typeof(LoadingTipData), false, GUILayout.Width(ScreenWidth - 35f));
            if(tipData != null)
            {
                listInUse.Add(tipData);
            }

            if(toRemove >= 0)
            {
                listInUse.RemoveAt(toRemove);
            }

            m_TipDatas[(int)m_CurrentGroup].TipData = listInUse;
        }
    }
}

