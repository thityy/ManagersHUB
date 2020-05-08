using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(TagPlatformData))]
    public class TagPlatformData_Editor : BaseCustomEditor
    {

        private SerializedProperty m_PlatformTags;
        private TagPlatformData m_Data;
        private StringPlatformTagDictionnary m_DicPlatform;

        private sPlatformTag m_EditPlatformTags = new sPlatformTag();
        private string[] m_TagAvailables;

        private int m_IdToRemove = -1;
        private string m_NewTagValue = string.Empty;
        private string m_NewTagValueForCreation = string.Empty;

        private int m_CurrentTagSelectId = -1;
        private string m_newTagEdit;

        private float m_scrollSize;
        private Vector2 m_ScrollViewTag;

        private string m_NewTag;

        private AnimBool m_CanShowEditTag;

        private void OnEnable()
        {
            m_CanShowEditTag = new AnimBool(false);
            m_CanShowEditTag.valueChanged.AddListener(Repaint);
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
            GetData(ref m_PlatformTags, nameof(m_PlatformTags));
            m_Data = (TagPlatformData)m_PlatformTags.serializedObject.targetObject;
            m_DicPlatform = m_Data.EDITOR_GetTagCallsDictionnary();
            m_TagAvailables = GetAvailableTags();

            if (m_CurrentTagSelectId >= 0)
            {
                m_CanShowEditTag.target = true;
            }
            else
            {
                m_CanShowEditTag.target = false;
            }
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Multi-Platform Tag Data");

            ShowAdvancedLabelCenter("Tag Call");
            ShowGroup("", AddTag);
            ShowGroup("", ShowTags);

            EditorGUILayout.EndVertical();
        }

        private string[] GetAvailableTags()
        {
            List<string> availableTag = new List<string>();
            foreach (KeyValuePair<string, sPlatformTag> entry in m_Data.EDITOR_GetTagCallsDictionnary())
            {
                availableTag.Add(entry.Key);
            }

            return availableTag.ToArray();
        }

        private void AddTag()
        {
            m_NewTag = ShowAdvancedTextInsert("Name of a new Tag: ", m_NewTag, 1);
            EditorGUI.BeginDisabledGroup(!TagNameIsAvailable(m_NewTag));
            if (GUILayout.Button("Add Tag Call"))
            {
                m_NewTagValueForCreation = m_NewTag;
            }
            EditorGUI.EndDisabledGroup();
        }

        private void ShowTags()
        {
            if (m_Data.EDITOR_GetTagCallsDictionnary().Values.Count < 4)
            {
                m_scrollSize = m_Data.EDITOR_GetTagCallsDictionnary().Values.Count * 25.0f;
            }
            else
            {
                m_scrollSize = 4 * 25.0f;
            }
            
            m_ScrollViewTag = EditorGUILayout.BeginScrollView(m_ScrollViewTag, GUILayout.Height(m_scrollSize));

            for (int i = 0; i < m_Data.EDITOR_GetTagCallsDictionnary().Values.Count; i++)
            {
                if (m_CurrentTagSelectId == i)
                {
                    if (ShowAdvancedListBtnRemove(m_TagAvailables[i].ToString(), null, true, true, RemoveTags, 1))
                    {
                        m_CurrentTagSelectId = i;
                    }
                }
                else
                {
                    if (ShowAdvancedListBtn(m_TagAvailables[i].ToString(), null, false, 2))
                    {
                        m_CurrentTagSelectId = i;
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10f);
            DrawSeparator(false);
            GUILayout.Space(20f);
            if (m_CurrentTagSelectId < m_TagAvailables.Length && m_CurrentTagSelectId >= 0)
            {
                ShowTagInEdit();
                DrawSeparatorDark(true);

                ShowFadeGroup(ref m_CanShowEditTag, CallShowEditTagValues);
            }

            if(!string.IsNullOrEmpty(m_NewTagValueForCreation))
            {
                m_Data.EDITOR_SetPlatformTag(new sPlatformTag(), m_NewTagValueForCreation);
                m_NewTagValueForCreation = string.Empty;
            }
            if(m_IdToRemove >= 0)
            {
                m_CurrentTagSelectId = -1;
                m_Data.EDITOR_GetTagCallsDictionnary().Remove(m_TagAvailables[m_IdToRemove]);
                m_IdToRemove = -1;
            }
            if(!string.IsNullOrEmpty(m_NewTagValue))
            {
                m_Data.EDITOR_EditTag(m_TagAvailables[m_CurrentTagSelectId], m_NewTagValue);
                m_NewTagValue = string.Empty;
            }

        }


        private void RemoveTags()
        {
            m_IdToRemove = m_CurrentTagSelectId;
        }


        private void ShowTagInEdit() //Ask for the tag in use.
        {
            m_newTagEdit = ShowAdvancedTextInsert("Edit tag call: ", m_newTagEdit, 1);

            if (GUILayout.Button("Change Tag Call"))
            {
                for (int i = 0; i < m_TagAvailables.Length; i++)
                {
                    if (!TagNameIsAvailable(m_newTagEdit))
                    {
                        if (EditorUtility.DisplayDialog("WARNING!", "'" + m_newTagEdit + "' is already in use.", "Ok"))
                        {

                        }
                        return;
                    }
                }
                m_NewTagValue = m_newTagEdit;
            }
        }

        private bool TagNameIsAvailable(string aTag)
        {
            if(string.IsNullOrEmpty(aTag))
            {
                return false;
            }
            for (int i = 0; i < m_TagAvailables.Length; i++)
            {
                if (!string.IsNullOrEmpty(aTag))
                {
                    if (aTag.ToUpper() == m_TagAvailables[i].ToUpper())
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void CallShowEditTagValues()
        {
            ShowGroup("", ShowEditTagValues);
        }

        private void ShowEditTagValues()
        {
            m_EditPlatformTags = m_Data.GetTagValuesForEachPlatform(m_TagAvailables[m_CurrentTagSelectId]);


            for (int i = 0; i < EnumSystem.GetEnumCount(typeof(eLanguageM_Platform)); i++)
            {
                eLanguageM_Platform platformInUse = (eLanguageM_Platform)i;


                if (platformInUse == eLanguageM_Platform.Unknown)
                {
                    DrawSeparatorDark(true);
                }

                EditorGUILayout.BeginHorizontal();
                m_EditPlatformTags.SetNewValue(platformInUse, ShowAdvancedTextInsert(platformInUse.ToString(), m_Data.GetTagValue(m_TagAvailables[m_CurrentTagSelectId], platformInUse), 2));
                EditorGUILayout.EndHorizontal();

                if (platformInUse == eLanguageM_Platform.Unknown)
                {
                    ShowAdvancedLabelCenter("Return Unknow value if ask platform is null or empty");
                    DrawSeparatorDark(true);
                }

            }

            m_Data.EDITOR_SetPlatformTag(m_EditPlatformTags, m_TagAvailables[m_CurrentTagSelectId]);

        }


    }
}


