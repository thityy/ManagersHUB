using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ManagersHUB
{
    [CustomEditor(typeof(AudioData))]
    public class AudioData_Editor : BaseCustomEditor
    {
        #region Variables

        //Base Variables BEGIN
        private SerializedProperty m_Clip;
        private SerializedProperty m_UseLanguageManager;
        private AnimBool m_UseLanguage;
        private SerializedProperty m_MultiLanguageClip;
        private SerializedProperty m_Volume;
        //----
        private SerializedProperty m_UseAdvancedSetting;
        private AnimBool m_ShowAdvancedSettings;
        private SerializedProperty m_Pitch;
        private SerializedProperty m_SpatialBlend;
        private SerializedProperty m_StereoPan;
        //----
        private SerializedProperty m_UseDelay;
        private AnimBool m_ShowDelaySettings;
        private SerializedProperty m_Delay;
        //Standard Variables END

        //Variante Variables BEGIN
        private SerializedProperty m_UseVariante;
        private AnimBool m_ShowVarianteFields;
        //----
        private SerializedProperty m_UseVolumeVariante;
        private AnimBool m_UseVolumeVar;
        private SerializedProperty m_MinVolumeVariante;
        private SerializedProperty m_MaxVolumeVariante;
        //----
        private SerializedProperty m_UsePitchVariante;
        private AnimBool m_UsePitchVar;
        private SerializedProperty m_MinPitchVariante;
        private SerializedProperty m_MaxPitchVariante;
        //Variante Variables END

        const string cIconsPath = "/ThityyTools/Scripts/Editor/Icons/";

        #endregion

        private void OnEnable()
        {
            m_ShowVarianteFields = new AnimBool(false);
            m_ShowAdvancedSettings = new AnimBool(false);
            m_ShowDelaySettings = new AnimBool(false);
            m_UseLanguage = new AnimBool(false);
            m_UseVolumeVar = new AnimBool(false);
            m_UsePitchVar = new AnimBool(false);

            m_ShowVarianteFields.valueChanged.AddListener(Repaint);
            m_ShowAdvancedSettings.valueChanged.AddListener(Repaint);
            m_ShowDelaySettings.valueChanged.AddListener(Repaint);
            m_UseLanguage.valueChanged.AddListener(Repaint);
            m_UseVolumeVar.valueChanged.AddListener(Repaint);
            m_UsePitchVar.valueChanged.AddListener(Repaint);
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
            BeginInspector(eManagers.AudioManager, "Audio Data");

            ShowGroup(null, ShowSelectType);
            DrawSeparator();
            ShowGroup(null, ShowBaseSettings);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowAdvancedSettings, m_UseAdvancedSetting, "Advanced Settings", ShowAdvancedSetting);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowDelaySettings, m_UseDelay, "Delay", ShowDelay);
            DrawSeparator();
            ShowAdvancedFadeGroup(ref m_ShowVarianteFields, m_UseVariante, "Variant", ShowVariante);

            EditorGUILayout.EndVertical();
        }

        private void GetDatas()
        {
            //Basic Specification
            GetData(ref m_Clip, nameof(m_Clip));
            GetData(ref m_UseLanguageManager, nameof(m_UseLanguageManager));
            if(m_UseLanguageManager.intValue == 0)
            {
                m_UseLanguage.target = false;
            }
            else
            {
                m_UseLanguage.target = true;
            }
            GetData(ref m_MultiLanguageClip, nameof(m_MultiLanguageClip));
            GetData(ref m_Volume, nameof(m_Volume));

            //Advanced Specification
            GetData(ref m_UseAdvancedSetting, nameof(m_UseAdvancedSetting));
            m_ShowAdvancedSettings.target = m_UseAdvancedSetting.boolValue;
            GetData(ref m_Pitch, nameof(m_Pitch));
            GetData(ref m_SpatialBlend, nameof(m_SpatialBlend));
            GetData(ref m_StereoPan, nameof(m_StereoPan));

            //Delay
            GetData(ref m_UseDelay, nameof(m_UseDelay));
            m_ShowDelaySettings.target = m_UseDelay.boolValue;
            GetData(ref m_Delay, nameof(m_Delay));

            //Variantes
            GetData(ref m_UseVariante, nameof(m_UseVariante));
            m_ShowVarianteFields.target = m_UseVariante.boolValue;
            GetData(ref m_UseVolumeVariante, nameof(m_UseVolumeVariante));
            m_UseVolumeVar.target = m_UseVolumeVariante.boolValue;
            GetData(ref m_MinVolumeVariante, nameof(m_MinVolumeVariante));
            GetData(ref m_MaxVolumeVariante, nameof(m_MaxVolumeVariante));
            GetData(ref m_UsePitchVariante, nameof(m_UsePitchVariante));
            m_UsePitchVar.target = m_UsePitchVariante.boolValue;
            GetData(ref m_MinPitchVariante, nameof(m_MinPitchVariante));
            GetData(ref m_MaxPitchVariante, nameof(m_MaxPitchVariante));
        }

        private void ShowSelectType()
        {
            ShowAdvancedSubtitle("Select the type of your AudioData sound(s)");
            m_UseLanguageManager.intValue = ShowAdvancedBtnChoice(new string[]{"Standard Clip", "MultiLanguage Data"}, m_UseLanguageManager.intValue, 1);
            ShowFadeGroup(ref m_UseLanguage, ShowNeedLanguageManager);
            if(m_UseLanguageManager.intValue == 0)
            {
                m_UseLanguage.target = false;
            }
            else
            {
                m_UseLanguage.target = true;
            }
        }

        private void ShowNeedLanguageManager()
        {
            ShowNeedManagerMessage(eManagers.LanguageManager);
        }

        private void ShowBaseSettings()
        {
            ShowAdvancedSubtitle("Select the audio for this data");
            if(m_UseLanguageManager.boolValue)
            {
                ShowAdvancedObjectField<MultiLanguageData>(m_MultiLanguageClip, typeof(MultiLanguageData), m_LabelTxtStyleLeft, false, 457512539, 1);
            }
            else
            {
                ShowAdvancedObjectField<AudioClip>(m_Clip, typeof(AudioClip), m_LabelTxtStyleLeft, false, 457512540, 1);
            }
            
            m_Volume.floatValue = ShowAdvancedSlider("Volume", "Mute", "Max", 0.0f, 1.0f, m_Volume.floatValue, 1);
        }

        private void ShowAdvancedSetting()
        {
            ShowAdvancedSubtitle("Set advanced setting(s)");
            m_Pitch.floatValue = ShowAdvancedSlider("Pitch", "Low", "High", -3.0f, 3.0f, m_Pitch.floatValue, 1);
            m_SpatialBlend.floatValue = ShowAdvancedSlider("Spatial Blend", "2D", "3D", 0.0f, 1.0f, m_SpatialBlend.floatValue, 1);
            m_StereoPan.floatValue = ShowAdvancedSlider("Stereo Pan", "Left", "Right", -1.0f, 1.0f, m_StereoPan.floatValue, 1);
        }

        private void ShowDelay()
        {
            ShowAdvancedSubtitle("Set a delay");
            m_Delay.floatValue = ShowAdvancedFloatField("Delay [sec]", m_Delay.floatValue, 0, Mathf.Infinity, 1);
        }

        private void ShowVariante()
        {
            ShowAdvancedSubtitle("Set variante setting(s)");
            m_UseVolumeVariante.boolValue = ShowAdvancedToggle("Use Volume Variante", m_UseVolumeVariante.boolValue);
            m_UseVolumeVar.target = m_UseVolumeVariante.boolValue;
            ShowFadeGroup(ref m_UseVolumeVar, ShowVolumeVarSlider);
            m_UsePitchVariante.boolValue = ShowAdvancedToggle("Use Pitch Variante", m_UsePitchVariante.boolValue);
            m_UsePitchVar.target = m_UsePitchVariante.boolValue;
            ShowFadeGroup(ref m_UsePitchVar, ShowPitchVarSlider);
        }

        private void ShowVolumeVarSlider()
        {
            ShowGroup(null, CallShowVolume);
        }

        private void ShowPitchVarSlider()
        {
            ShowGroup(null, CallShowPitch);
        }

        private void CallShowVolume()
        {
            ShowAdvancedMinMaxSlider("Volume", "Mute", "Max", 0.0f, 1.0f, m_MinVolumeVariante, m_MaxVolumeVariante, 2);
        }

        private void CallShowPitch()
        {
            ShowAdvancedMinMaxSlider("Pitch", "Low", "High", -3.0f, 3.0f, m_MinPitchVariante, m_MaxPitchVariante, 2);
        }
    }
}
