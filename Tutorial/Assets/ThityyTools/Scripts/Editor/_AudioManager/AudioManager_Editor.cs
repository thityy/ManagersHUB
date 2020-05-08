using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : BaseCustomEditor
    {
        private SerializedProperty m_UseMusicSystem;
        private SerializedProperty m_UseOptionSystem;
        private SerializedProperty m_UseSFXSystem;
        private SerializedProperty m_UseFootSystem;
        private SerializedProperty m_UseRadioSystem;

        //Sound
        private SerializedProperty m_ListPerfSounds;
        private SerializedProperty m_MaxAmountPerfSFX;

        //Option
        private SerializedProperty m_MasterGroup;
        private SerializedProperty m_MusicGroup;
        private SerializedProperty m_SFXGroup;
        private SerializedProperty m_VoiceGroup;

        //Radio
        private SerializedProperty m_RadioStationsInput;
        //--
        private SerializedProperty m_RadioDisplay;
        private SerializedProperty m_DisplayLength;

        //Foot
        private SerializedProperty m_FootSoundsData;
        private SerializedProperty m_FootAudioSelector;


        private AnimBool m_ShowMusicSystem;
        private AnimBool m_ShowOptionSystem;
        private AnimBool m_ShowSoundSystem;
        private AnimBool m_ShowFootSystem;
        private AnimBool m_ShowRadioSystem;


        private void OnEnable()
        {
            m_ShowMusicSystem = new AnimBool(false);
            m_ShowOptionSystem = new AnimBool(false);
            m_ShowSoundSystem = new AnimBool(false);
            m_ShowFootSystem = new AnimBool(false);
            m_ShowRadioSystem = new AnimBool(false);

            m_ShowMusicSystem.valueChanged.AddListener(Repaint);
            m_ShowOptionSystem.valueChanged.AddListener(Repaint);
            m_ShowSoundSystem.valueChanged.AddListener(Repaint);
            m_ShowFootSystem.valueChanged.AddListener(Repaint);
            m_ShowRadioSystem.valueChanged.AddListener(Repaint);
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
            GetData(ref m_UseMusicSystem, nameof(m_UseMusicSystem));
            m_ShowMusicSystem.target = m_UseMusicSystem.boolValue;
            GetData(ref m_UseSFXSystem, nameof(m_UseSFXSystem));
            m_ShowSoundSystem.target = m_UseSFXSystem.boolValue;
            GetData(ref m_UseRadioSystem, nameof(m_UseRadioSystem));
            m_ShowRadioSystem.target = m_UseRadioSystem.boolValue;
            GetData(ref m_UseFootSystem, nameof(m_UseFootSystem));
            m_ShowFootSystem.target = m_UseFootSystem.boolValue;
            GetData(ref m_UseOptionSystem, nameof(m_UseOptionSystem));
            m_ShowOptionSystem.target = m_UseOptionSystem.boolValue;

            GetData(ref m_ListPerfSounds, nameof(m_ListPerfSounds));
            GetData(ref m_MaxAmountPerfSFX, nameof(m_MaxAmountPerfSFX));

            GetData(ref m_MasterGroup, nameof(m_MasterGroup));
            GetData(ref m_MusicGroup, nameof(m_MusicGroup));
            GetData(ref m_SFXGroup, nameof(m_SFXGroup));
            GetData(ref m_VoiceGroup, nameof(m_VoiceGroup));

            GetData(ref m_RadioStationsInput, nameof(m_RadioStationsInput));
            GetData(ref m_RadioDisplay, nameof(m_RadioDisplay));
            GetData(ref m_DisplayLength, nameof(m_DisplayLength));

            GetData(ref m_FootSoundsData, nameof(m_FootSoundsData));
            GetData(ref m_FootAudioSelector, nameof(m_FootAudioSelector));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "Audio Manager");

            ShowAdvancedFadeGroup(ref m_ShowMusicSystem, m_UseMusicSystem, "Music System", ShowMusicSystem);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowSoundSystem, m_UseSFXSystem, "Sound System", ShowSoundSystem);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowRadioSystem, m_UseRadioSystem, "Radio System", ShowRadioSystem);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowFootSystem, m_UseFootSystem, "FootSound System", ShowFootSystem);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowOptionSystem, m_UseOptionSystem, "Option System", ShowOptionSystem);

            EditorGUILayout.EndVertical();
        }

        private void ShowMusicSystem()
        {
            ShowAdvancedToolTip("The Music system lets you play, stop, pause and loop music from anywhere in the game.");
        }

        private void ShowOptionSystem()
        {
            ShowAdvancedToolTip("Option system lets you divide all of your sound between AudioMixer to be able to manage their volume by code (Music, Voice, SFX, Master)");
            DrawSeparator(true);
            EditorGUILayout.LabelField("Set the AudioMixerGroup:", m_LabelTxtStyleLeft);
            DrawSeparatorDark(true);
            Color32 color = CurrentColorDark;
            color.a = 80;
            GUI.backgroundColor = color;
            EditorGUILayout.LabelField("Master Group", m_LabelTxtStyleLeft);
            ShowGroup(null, ShowMasterGroup);
            EditorGUILayout.LabelField("Music Group", m_LabelTxtStyleLeft);
            ShowGroup(null, ShowMusicGroup);
            EditorGUILayout.LabelField("SFX Group", m_LabelTxtStyleLeft);
            ShowGroup(null, ShowSFXGroup);
            EditorGUILayout.LabelField("Voice Group", m_LabelTxtStyleLeft);
            ShowGroup(null, ShowVoiceGroup);
            GUI.backgroundColor = Color.white;
        }

        private void ShowMasterGroup()
        {
            GUILayout.Space(-13f);
            m_MasterGroup.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_MasterGroup.objectReferenceValue, typeof(AudioMixerGroup), m_LabelTxtStyleLeft, false, 2);
            GUILayout.Space(-13f);
        }

        private void ShowMusicGroup()
        {
            GUILayout.Space(-13f);
            m_MusicGroup.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_MusicGroup.objectReferenceValue, typeof(AudioMixerGroup), m_LabelTxtStyleLeft, false, 2);
            GUILayout.Space(-13f);
        }

        private void ShowSFXGroup()
        {
            GUILayout.Space(-13f);
            m_SFXGroup.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_SFXGroup.objectReferenceValue, typeof(AudioMixerGroup), m_LabelTxtStyleLeft, false, 2);
            GUILayout.Space(-13f);
        }

        private void ShowVoiceGroup()
        {
            GUILayout.Space(-13f);
            m_VoiceGroup.objectReferenceValue = ShowAdvancedObjectFieldSimple(m_VoiceGroup.objectReferenceValue, typeof(AudioMixerGroup), m_LabelTxtStyleLeft, false, 2);
            GUILayout.Space(-13f);
        }

        private void ShowSoundSystem()
        {
            ShowAdvancedToolTip("Sound system lets you play sound anywhere in the game with all setting already preset! You can play sound in easy mode or in perf mode.");
            DrawSeparator(true);
            m_MaxAmountPerfSFX.intValue = ShowAdvancedIntField("Amount of Perf Sound(s)", m_MaxAmountPerfSFX.intValue, 0, 100, 1);
        }

        private void ShowFootSystem()
        {
            ShowAdvancedToolTip("FootSounds system lets you play foot sound depend on your movement and on which texture you're moving on!");
            DrawSeparator(true);
            ShowAdvancedObjectField<FootSoundData>(m_FootSoundsData, typeof(FootSoundData), m_LabelTxtStyleLeft, false, 457512540, 1);

            DrawSeparator(true);
            EditorGUILayout.LabelField("Select how the data should be use to play sound", m_LabelTxtStyleLeft);
            m_FootAudioSelector.enumValueIndex = ShowAdvancedBtnChoice(new string[] { "AudioData", "AudioClip" }, m_FootAudioSelector.enumValueIndex, 1);
        }

        private void ShowRadioSystem()
        {
            ShowAdvancedToolTip("Radio system lets you play music with multiple station that the player (or you) can change anytime in game! It can also display the music in play with a custom interface, like in racing game. Only set the MusicData for the station you want and the manager will mix it for you!");
            DrawSeparator(true);
            EditorGUILayout.LabelField("Radio station(s)", m_LabelTxtStyleLeft);
            ShowRadioList();
            DrawSeparator(true);

            EditorGUILayout.LabelField("Display Setting(s)", m_LabelTxtStyleCenter);
            ShowAdvancedObjectField<RadioDisplay>(m_RadioDisplay, typeof(RadioDisplay), m_LabelTxtStyleLeft, true, 45421024, 1);
            if(m_RadioDisplay.objectReferenceValue != null)
            {
                m_DisplayLength.floatValue = ShowAdvancedFloatField("Time show on screen", m_DisplayLength.floatValue, 0, Mathf.Infinity, 1);
            }
        }

        private void ShowRadioList()
        {
            int toRemove = -1;
            Color colorValue;
            if(m_RadioStationsInput.arraySize == 0)
            {
                GUILayout.Space(15f);
                ShowAdvancedLabelCenter("Empty");
            }
            for (int i = 0; i < m_RadioStationsInput.arraySize; i++)
            {
                
                SerializedProperty tempObject = m_RadioStationsInput.GetArrayElementAtIndex(i);

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
                tempObject.objectReferenceValue = EditorGUILayout.ObjectField(tempObject.objectReferenceValue, typeof(RadioData), false, GUILayout.Width(ScreenWidth - 100f), GUILayout.Height(20f));
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
            EditorGUILayout.LabelField("Add a new station ", m_LabelTxtStyleLeft);
            RadioData m_TempData = null;
            m_TempData = (RadioData)EditorGUILayout.ObjectField(m_TempData, typeof(RadioData), false, GUILayout.Width(ScreenWidth - 35f));
            if (m_TempData != null)
            {
                m_RadioStationsInput.InsertArrayElementAtIndex(m_RadioStationsInput.arraySize);
                m_RadioStationsInput.GetArrayElementAtIndex(m_RadioStationsInput.arraySize-1).objectReferenceValue = m_TempData;
            }

            //Remove sound from list if null
            if (toRemove >= 0)
            {
                m_RadioStationsInput.DeleteArrayElementAtIndex(toRemove);
            }
        }
    }
}
