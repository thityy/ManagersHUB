using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(MusicData))]
    public class MusicDataEditor : BaseCustomEditor
    {
        private SerializedProperty m_Clip;
        private SerializedProperty m_Volume;

        private SerializedProperty m_ClipArt;
        private SerializedProperty m_ArtistName;
        private SerializedProperty m_SongName;

        private Sprite m_CurrentSpriteCoverAlbum;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            GetData(ref m_Clip, nameof(m_Clip));
            GetData(ref m_Volume, nameof(m_Volume));

            GetData(ref m_ClipArt, nameof(m_ClipArt));
            GetData(ref m_ArtistName, nameof(m_ArtistName));
            GetData(ref m_SongName, nameof(m_SongName));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "Music Data");

            ShowGroup(null, ShowBaseSettings);
            DrawSeparator();
            ShowGroup(null, ShowDisplayRadio);
            DrawSeparator();
            ShowGroup(null, ShowDisplayPreview);

            EditorGUILayout.EndVertical();
        }

        private void ShowBaseSettings()
        {
            ShowAdvancedSubtitle("Select the music for this data");
            EditorGUILayout.LabelField("Song", m_LabelTxtStyleLeft);
            GUILayout.Space(-10f);
            ShowAdvancedObjectField<AudioClip>(m_Clip, typeof(AudioClip), m_LabelTxtStyleLeft, false, 457512539, 1);
            m_Volume.floatValue = ShowAdvancedSlider("Volume", "Mute", "Max", 0.0f, 1.0f, m_Volume.floatValue, 1);
        }

        private void ShowDisplayRadio()
        {
            ShowAdvancedSubtitle("Set the values below for the radio display");
            ShowAdvancedSettings();
        }

        private void ShowDisplayPreview()
        {
            ShowAdvancedSubtitle("Preview of the radio display");
            Color32 colorValue = CurrentColorDark;
            colorValue.a = 80;
            GUI.color = colorValue;
            ShowGroup("", ShowPreview);
        }


        private void ShowAdvancedSettings()
        {
            EditorGUILayout.LabelField("Artist name", m_LabelTxtStyleLeft);
            m_ArtistName.stringValue = ShowAdvancedTextInsert(m_ArtistName.stringValue, 1);
            GUILayout.Space(5.0f);

            EditorGUILayout.LabelField("Song name", m_LabelTxtStyleLeft);
            m_SongName.stringValue = ShowAdvancedTextInsert(m_SongName.stringValue, 1);
            GUILayout.Space(5.0f);

            EditorGUILayout.LabelField("Album Cover", m_LabelTxtStyleLeft);
            GUILayout.Space(-10f);
            ShowAdvancedObjectField<Sprite>(m_ClipArt, typeof(Sprite), m_LabelTxtStyleLeft, false, 457512540, 1);
        }

        private void ShowPreview()
        {
            GUI.color = Color.white;
            if (m_ClipArt.objectReferenceValue != null)
            {
                m_CurrentSpriteCoverAlbum = m_ClipArt.objectReferenceValue as Sprite;
            }
            else
            {
                m_CurrentSpriteCoverAlbum = null;
            }

            GUI.backgroundColor = CurrentColor;
            EditorGUILayout.BeginHorizontal();
            if(m_ClipArt.objectReferenceValue == null)
            {
                GUILayout.Label("", GUI.skin.box, GUILayout.Width(50.0f), GUILayout.Height(50.0f));
            }
            else
            {
                GUILayout.Box(m_CurrentSpriteCoverAlbum.texture, GUILayout.Width(50.0f), GUILayout.Height(50.0f));
            }
            
            EditorGUILayout.BeginVertical();
            GUILayout.Label(m_ArtistName.stringValue, m_LabelTxtStyleLeft, GUILayout.Width(ScreenWidth - 70f));
            GUILayout.Label("<b>" + m_SongName.stringValue + "</b>", m_LabelTxtStyleLeft, GUILayout.Width(ScreenWidth - 70f));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
        }
    }
}
