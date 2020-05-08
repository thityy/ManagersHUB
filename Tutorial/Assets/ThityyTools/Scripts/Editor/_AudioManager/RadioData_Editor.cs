using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(RadioData))]
    public class RadioData_Editor : BaseCustomEditor
    {

        SerializedProperty m_Musics;
        SerializedProperty m_AutoShuffle;
        SerializedProperty m_RadioName;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_Musics, nameof(m_Musics));
            GetData(ref m_AutoShuffle, nameof(m_AutoShuffle));
            GetData(ref m_RadioName, nameof(m_RadioName));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "Radio Data");

            ShowGroup("", ShowRadioSetting);
            DrawSeparator();
            ShowGroup("", ShowMusicList);

            EditorGUILayout.EndVertical();
        }

        private void ShowRadioSetting()
        {
            ShowAdvancedSubtitle("Edit the radio station");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Radio station name", m_LabelTxtStyleLeft, GUILayout.Width(ScreenWidth - 180.0f));
            m_AutoShuffle.boolValue = ShowAdvancedToggle("Auto Shuffle Radio", m_AutoShuffle.boolValue);
            EditorGUILayout.EndHorizontal();
            m_RadioName.stringValue = ShowAdvancedTextInsert(m_RadioName.stringValue, 1);
        }

        private void ShowMusicList()
        {
            ShowAdvancedSubtitle("Edit the music inside the station");
            int toRemove = -1;
            Color colorValue;

            EditorGUILayout.LabelField("Music(s) of " + m_RadioName.stringValue + " station", m_LabelTxtStyleLeft);

            if(m_Musics.arraySize == 0)
            {
                GUILayout.Space(15f);
                ShowAdvancedLabelCenter("Empty");
            }
            for (int i = 0; i < m_Musics.arraySize; i++)
            {
                SerializedProperty tempObject = m_Musics.GetArrayElementAtIndex(i);

                //Set color
                if (i % 2 == 0)
                {
                    colorValue = CurrentColor;
                }
                else
                {
                    colorValue = CurrentColorDark;
                }
                colorValue.a = 0.5f;

                //Show AudioClip with 'Remove' button
                GUI.backgroundColor = colorValue;
                EditorGUILayout.BeginHorizontal();
                tempObject.objectReferenceValue = EditorGUILayout.ObjectField(tempObject.objectReferenceValue, typeof(MusicData), false, GUILayout.Width(ScreenWidth - 100f), GUILayout.Height(20f));
                if (GUILayout.Button("Remove", m_ButtonStyle, GUILayout.Width(60f), GUILayout.Height(20f)))
                {
                    tempObject.objectReferenceValue = null;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();

                //Set back into the list the new audio (if change)
                if (tempObject.objectReferenceValue == null)
                {
                    toRemove = i;
                }
                GUILayout.Space(-2f);
            }

            DrawSeparatorDark(true);

            //Add a sound option
            EditorGUILayout.LabelField("Add a new music ", m_LabelTxtStyleLeft);
            MusicData m_TempData = null;
            m_TempData = (MusicData)EditorGUILayout.ObjectField(m_TempData, typeof(MusicData), false, GUILayout.Width(ScreenWidth - 35f));
            if (m_TempData != null)
            {
                m_Musics.InsertArrayElementAtIndex(m_Musics.arraySize);
                m_Musics.GetArrayElementAtIndex(m_Musics.arraySize-1).objectReferenceValue = m_TempData;
            }

            //Remove sound from list if null
            if (toRemove >= 0)
            {
                m_Musics.DeleteArrayElementAtIndex(toRemove);
            }
        }
    }
}
