using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;

namespace ManagersHUB
{
    [CustomEditor(typeof(MultiLanguageData))]
    public class LanguageData_Editor : BaseCustomEditor
    {

        private MultiLanguageData m_Language;

        private SerializedProperty m_AvailableLanguageData;
        private SerializedProperty m_MultiClips;
        private AvailableLanguageData m_AvailableLanguage;

        private SerializedProperty m_TypeChoice;

        private SystemLanguage m_CurrentLanguage;
        private int m_CurrentId;

        private string[] m_typeBtnChoice = new string[] { "Text", "Audio", "Image" };

        private Vector2 m_ScrollViewLanguage;
        private float m_scrollSize = 100.0f;

        private Color32 colorValue;

        private void OnEnable()
        {
            GetDatas();

            colorValue = ColorManager.GetColor(eManagers.LanguageManager, true);
            colorValue.a = 80;
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
            m_Language = (MultiLanguageData)serializedObject.targetObject;

            GetData(ref m_AvailableLanguageData, nameof(m_AvailableLanguageData));
            if((AvailableLanguageData)AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData)) != null)
            {
                m_AvailableLanguage = (AvailableLanguageData)AssetDatabase.LoadAssetAtPath(AvailableLanguage.AvailableLanguagePath, typeof(AvailableLanguageData));
                m_Language.EDITOR_UpdateDictionnary(m_AvailableLanguage);
            }
            else
            {
                m_AvailableLanguage = null;
            }

            GetData(ref m_TypeChoice, nameof(m_TypeChoice));
            GetData(ref m_MultiClips, nameof(m_MultiClips));
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Multi-Language Data");

            if(m_AvailableLanguage != null)
            {
                UpdateCurrentLanguage(m_CurrentId);
                ShowGroup(null, ShowSelectType);
                DrawSeparator();
                ShowGroup(null, ShowBaseSettings);
            }
            else
            {
                ShowGroup(null, ShowWarningEmpty);
            }

            EditorGUILayout.EndVertical();
        }

        private void ShowWarningEmpty()
        {
            ShowAdvancedSubtitle("WARNING! Can't edit");
                ShowAdvancedToolTip("There's no LanguageAvailableData set on the Language Manager");
        }

        private void ShowSelectType()
        {
            ShowAdvancedSubtitle("Select how this Multi-Language should be use in your game.");
            m_TypeChoice.enumValueIndex = ShowAdvancedBtnChoice(m_typeBtnChoice, m_TypeChoice.enumValueIndex, 1);
        }

        private void ShowBaseSettings()
        {
            EditorGUILayout.LabelField("Language to edit (available in your game)", m_LabelTxtStyleLeft);
            if (m_AvailableLanguage.GetAvailableLanguageCount() > 0)
            {
                
                ShowGroup(null, ShowLanguageToEdit);
                

                DrawSeparatorDark(true);

                
                ShowValueToEdit();
                
            }
        }

        private void ShowLanguageToEdit()
        {
            //Set size of the list
            if (m_AvailableLanguage.GetAvailableLanguageCount() < 4)
            {
                m_scrollSize = m_AvailableLanguage.GetAvailableLanguageCount() * 25.0f;
            }
            else
            {
                m_scrollSize = 4 * 25.0f;
            }

            m_ScrollViewLanguage = EditorGUILayout.BeginScrollView(m_ScrollViewLanguage, GUILayout.Height(m_scrollSize));

            //Init effect for button language
            Texture iconToShow = m_WarningIcon;
            string textValue = "";
            bool isSelect = false;

            //show all object in the inspector list
            for (int i = 0; i < m_AvailableLanguage.GetAvailableLanguageCount(); i++)
            {

                textValue = ObjectInInspectorText(i);
                if(ObjectInInspectorIsNULL(i))
                {
                    iconToShow = m_WarningIcon;
                }
                else
                {
                    iconToShow = null;
                }

                if (i == m_CurrentId)
                    isSelect = true;
                else
                    isSelect = false;

                if (ShowAdvancedListBtn(" " + m_AvailableLanguage.GetLanguage(i).ToString() + textValue, iconToShow, isSelect, 2))
                {
                    GUI.SetNextControlName("");
                    GUI.FocusControl("");
                    UpdateCurrentLanguage(i);
                }
            }

            EditorGUILayout.EndScrollView();
        }


        private void UpdateCurrentLanguage(int aId)
        {
            m_CurrentId = aId;
            m_CurrentLanguage = m_AvailableLanguage.GetLanguage(m_CurrentId);
        }
        

        private void ShowValueToEdit()
        {
            if (m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Text)
            {
                EditorGUILayout.LabelField("Text value for <i>" + m_AvailableLanguage.GetLanguage(m_CurrentId).ToString() + "</i>  language:", m_LabelTxtStyleLeft);
                ShowTextToEdit();
            }
            else if (m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Audio)
            {
                EditorGUILayout.LabelField("Audio value for <i>" + m_AvailableLanguage.GetLanguage(m_CurrentId).ToString() + "</i>  language:", m_LabelTxtStyleLeft);
                ShowGroup("", ShowClipToEdit);
            }
            else if (m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Image)
            {
                EditorGUILayout.LabelField("Sprite value for <i>" + m_AvailableLanguage.GetLanguage(m_CurrentId).ToString() + "</i>  language:", m_LabelTxtStyleLeft);
                ShowGroup("", ShowSpriteToEdit);
            }
        }

        private void ShowTextToEdit()
        {
            m_Language.EDITOR_SetText(m_CurrentLanguage, ShowAdvancedTextInsert(m_Language.EDITOR_GetText(m_CurrentLanguage), 1));
        }

        private void ShowClipToEdit()
        {
            UnityEngine.Object value = null;
            value = m_Language.EDITOR_GetAudio(m_CurrentLanguage);
            ShowAdvancedObjectField<AudioClip>(ref value, typeof(AudioClip), m_LabelTxtStyleLeft, false, 457512540 + m_CurrentId, 2);
            m_Language.EDITOR_SetAudio(m_CurrentLanguage, (AudioClip)value);
        }

        private void ShowSpriteToEdit()
        {
            UnityEngine.Object value = null;
            value = m_Language.EDITOR_GetImage(m_CurrentLanguage);
            ShowAdvancedObjectField<Sprite>(ref value, typeof(Sprite), m_LabelTxtStyleLeft, false, 457512540 + m_CurrentId, 2);
            m_Language.EDITOR_SetImage(m_CurrentLanguage, (Sprite)value);
        }

        ///<summary> Return TRUE if the object in the inspector is null</summary>
        private bool ObjectInInspectorIsNULL(int aId)
        {
            if(m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Text)
            {
                if(string.IsNullOrEmpty(m_Language.EDITOR_GetText(m_AvailableLanguage.GetLanguage(aId))))
                {
                    return true;
                }
            }
            else if(m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Audio)
            {
                if(m_Language.EDITOR_GetAudio(m_AvailableLanguage.GetLanguage(aId)) == null)
                {
                    return true;
                }
            }
            else if(m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Image)
            {
                if(m_Language.EDITOR_GetImage(m_AvailableLanguage.GetLanguage(aId)) == null)
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary> Return the value of the object in inspector</summary>
        private string ObjectInInspectorText(int aId)
        {
            if (m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Text)
            {
                if(!string.IsNullOrEmpty(m_Language.EDITOR_GetText(m_AvailableLanguage.GetLanguage(aId))))
                {
                    string[] line = m_Language.EDITOR_GetText(m_AvailableLanguage.GetLanguage(aId)).Split('\n');
                    return " → <i>" + line[0] + "</i>";
                }
                return " → " + "<i>No String Set</i>";
            }
            else if(m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Audio)
            {
                if(m_Language.EDITOR_GetAudio(m_AvailableLanguage.GetLanguage(aId)) == null)
                {
                    return " → <i>No Audio Select</i>";
                }
                return " → " + m_Language.EDITOR_GetAudio(m_AvailableLanguage.GetLanguage(aId)).name;
            }
            else if(m_TypeChoice.enumValueIndex == (int)eLanguageM_LanguageType.Image)
            {
                if(m_Language.EDITOR_GetImage(m_AvailableLanguage.GetLanguage(aId)) == null)
                {
                    return " → <i>No Image select</i>";
                }
                return " → " + m_Language.EDITOR_GetImage(m_AvailableLanguage.GetLanguage(aId)).name;
            }
            return "";
        }
    }
}

