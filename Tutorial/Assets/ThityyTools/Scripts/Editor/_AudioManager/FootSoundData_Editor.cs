using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{
    [CustomEditor(typeof(FootSoundData))]
    public class FootSoundData_Editor : BaseCustomEditor
    {

        private FootSoundData m_Self;
        private List<sFootSound> m_FootSounds;
        private List<sFootSoundWithData> m_FootSoundsData;

        private int m_GroundSelectId;
        private eAudioM_GroundType m_GroundSelect;
        private eAudioM_FootSoundType m_FootSoundType;

        private UnityEngine.Object m_TempObject;
        private int m_TempId;

        private AudioClip m_TempSound;
        private AudioData m_TempData;

        private eAudioM_AudioSelector m_AudioSelector;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
            EditorUtility.SetDirty((FootSoundData)target);
        }

        private void GetDatas()
        {
            m_Self = (FootSoundData)serializedObject.targetObject;
            m_FootSounds = m_Self.GetFootSounds();
            m_FootSoundsData = m_Self.GetFootSoundsData();
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.AudioManager, "Foot Sounds Data");
            ShowGroup("", ShowSelectAudioSelector);
            DrawSeparator();
            ShowGroup("", ShowSelectGroundType);
            DrawSeparator();
            ShowGroup("", ShowFootSounds);

            m_Self.EDITOR_SetFootSounds(m_FootSounds, m_FootSoundsData);
            EditorGUILayout.EndVertical();  
        }

        private void ShowSelectAudioSelector()
        {
            ShowAdvancedSubtitle("Select the type of your FootSound sound(s)");
            m_AudioSelector = (eAudioM_AudioSelector)ShowAdvancedBtnChoice(new string[] { "AudioData", "AudioClip" }, (int)m_AudioSelector, 1);
        }

        private void ShowSelectGroundType()
        {
            ShowAdvancedSubtitle("All of your foot sound(s)");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select ground material to edit:", m_LabelTxtStyleLeft, GUILayout.Width(ScreenWidth - 126f));
            EditorGUILayout.LabelField("[w] [r] [c] [c]  ", m_LabelTxtStyleRight, GUILayout.Width(86f));
            EditorGUILayout.EndHorizontal();
            ShowGroup("", ShowMaterialAvailable);


            DrawSeparator(true);

            EditorGUILayout.LabelField("Movement type ", m_LabelTxtStyleLeft);
            m_FootSoundType = (eAudioM_FootSoundType)ShowAdvancedBtnChoice(new string[] { "Walk", "Run", "Crouch", "Crawl" }, (int)m_FootSoundType, 1);
        }

        private void ShowMaterialAvailable()
        {
            for (int i = 0; i < EnumSystem.GetEnumCount(typeof(eAudioM_GroundType)); i++)
            {
                if (i == m_GroundSelectId)
                {
                    ShowAdvancedListBtn(((eAudioM_GroundType)i).ToString(), GetMaterialText((eAudioM_GroundType)i, m_AudioSelector), null, true, 2);
                }
                else
                {
                    if (ShowAdvancedListBtn(((eAudioM_GroundType)i).ToString(), GetMaterialText((eAudioM_GroundType)i, m_AudioSelector), null, false, 2))
                    {
                        m_GroundSelectId = i;
                    }
                }
            }

            m_GroundSelect = (eAudioM_GroundType)m_GroundSelectId;
        }

        private void ShowFootSounds()
        {
            ShowAdvancedSubtitle("FootSound edit mode");
            if (m_AudioSelector == eAudioM_AudioSelector.AudioClip)
            {
                m_FootSounds[(int)m_GroundSelect].FootVolumes = ShowFootVolumes(m_FootSounds[(int)m_GroundSelect].FootVolumes, m_FootSoundType);
                m_FootSounds[(int)m_GroundSelect].SetSoundsList(m_FootSoundType, ShowFootSound(m_FootSoundType.ToString() + " Sound(s)", m_FootSounds[(int)m_GroundSelect].GetSoundsList(m_FootSoundType)));
            }
            else
            {
                m_FootSoundsData[(int)m_GroundSelect].SetSoundsList(m_FootSoundType, ShowFootSound(m_FootSoundType.ToString() + " Sound(s)", m_FootSoundsData[(int)m_GroundSelect].GetSoundsList(m_FootSoundType)));
            }
        }

        private sVolumeFootSound ShowFootVolumes(sVolumeFootSound aVolumes, eAudioM_FootSoundType aType)
        {
            aVolumes.SetVolume(ShowAdvancedSlider(aType.ToString() + " volume", "Mute", "Max", 0.0f, 1.0f, aVolumes.GetVolume(aType), 1), aType);
            return aVolumes;
        }

        private List<AudioClip> ShowFootSound(string aListName, List<AudioClip> aSounds)
        {
            int toRemove = -1;
            Color colorValue;

            EditorGUILayout.LabelField(m_GroundSelect.ToString() + " | " + aListName, m_LabelTxtStyleLeft);

            for (int i = 0; i < aSounds.Count; i++)
            {
                m_TempObject = (UnityEngine.Object)aSounds[i];

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
                m_TempObject = EditorGUILayout.ObjectField(m_TempObject, typeof(AudioClip), false, GUILayout.Width(ScreenWidth - 100f), GUILayout.Height(20f));
                if (GUILayout.Button("Remove", m_ButtonStyle, GUILayout.Width(60f), GUILayout.Height(20f)))
                {
                    m_TempObject = null;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();

                //Set back into the list the new audio (if change)
                if (m_TempObject != null)
                {
                    aSounds[i] = (AudioClip)m_TempObject;
                }
                else
                {
                    toRemove = i;
                }
                GUILayout.Space(-2f);
            }

            DrawSeparatorDark(true);

            //Add a sound option
            EditorGUILayout.LabelField("Add new foot sound", m_LabelTxtStyleLeft);
            m_TempSound = (AudioClip)EditorGUILayout.ObjectField(m_TempSound, typeof(AudioClip), false, GUILayout.Width(ScreenWidth - 35f));
            if (m_TempSound != null)
            {
                aSounds.Add(m_TempSound);
                m_TempSound = null;
            }

            //Remove sound from list if null
            if (toRemove >= 0)
            {
                aSounds.RemoveAt(toRemove);
            }

            return aSounds;
        }

        private List<AudioData> ShowFootSound(string aListName, List<AudioData> aSounds)
        {
            int toRemove = -1;
            Color colorValue;

            EditorGUILayout.LabelField(m_GroundSelect.ToString() + " | " + aListName, m_LabelTxtStyleLeft);



            for (int i = 0; i < aSounds.Count; i++)
            {
                m_TempObject = (UnityEngine.Object)aSounds[i];

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
                m_TempObject = EditorGUILayout.ObjectField(m_TempObject, typeof(AudioData), false, GUILayout.Width(ScreenWidth - 100f), GUILayout.Height(20f));
                if (GUILayout.Button("Remove", m_ButtonStyle, GUILayout.Width(60f), GUILayout.Height(20f)))
                {
                    m_TempObject = null;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();

                //Set back into the list the new audio (if change)
                if (m_TempObject != null)
                {
                    aSounds[i] = (AudioData)m_TempObject;
                }
                else
                {
                    toRemove = i;
                }
                GUILayout.Space(-2f);
            }

            DrawSeparatorDark(true);

            //Add a sound option
            EditorGUILayout.LabelField("Add new foot sound", m_LabelTxtStyleLeft);
            m_TempData = (AudioData)EditorGUILayout.ObjectField(m_TempData, typeof(AudioData), false, GUILayout.Width(ScreenWidth - 35f));
            if (m_TempData != null)
            {
                aSounds.Add(m_TempData);
                m_TempData = null;
            }

            //Remove sound from list if null
            if (toRemove >= 0)
            {
                aSounds.RemoveAt(toRemove);
            }

            return aSounds;
        }

        private string GetMaterialText(eAudioM_GroundType aType, eAudioM_AudioSelector aSelector)
        {
            string textToReturn = "";
            switch (aSelector)
            {
                case eAudioM_AudioSelector.AudioClip:
                    {
                        textToReturn += GetAmountOfSound(aType, eAudioM_FootSoundType.Walk);
                        textToReturn += GetAmountOfSound(aType, eAudioM_FootSoundType.Run);
                        textToReturn += GetAmountOfSound(aType, eAudioM_FootSoundType.Crouch);
                        textToReturn += GetAmountOfSound(aType, eAudioM_FootSoundType.Crawling);
                        break;
                    }
                case eAudioM_AudioSelector.AudioData:
                    {
                        textToReturn += GetAmountOfSoundData(aType, eAudioM_FootSoundType.Walk);
                        textToReturn += GetAmountOfSoundData(aType, eAudioM_FootSoundType.Run);
                        textToReturn += GetAmountOfSoundData(aType, eAudioM_FootSoundType.Crouch);
                        textToReturn += GetAmountOfSoundData(aType, eAudioM_FootSoundType.Crawling);
                        break;
                    }
            }

            return textToReturn;
        }

        private string GetAmountOfSound(eAudioM_GroundType aType, eAudioM_FootSoundType aMovement)
        {
            int amount = m_FootSounds[(int)aType].GetSoundsList(aMovement).Count;
            if(amount >= 10)
            {
                return "[9+]";
            }
            else
            {
                return "[" + amount.ToString() + "]";
            }
        }

        private string GetAmountOfSoundData(eAudioM_GroundType aType, eAudioM_FootSoundType aMovement)
        {
            int amount = m_FootSoundsData[(int)aType].GetSoundsList(aMovement).Count;
            if(amount >= 10)
            {
                return "[9+]";
            }
            else
            {
                return "[" + amount.ToString() + "]";
            }
        }
    }


}

