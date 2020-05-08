using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ManagersHUB
{
    public class BaseCustomEditor : Editor
    {
        private static int m_idPicker;

        private const string CompanyName = "Thityy Tools®";

        protected Color32 CurrentColor;
        protected Color32 CurrentColorDark;

        const string cIconsPath = "/ThityyTools/Scripts/Editor/Icons/";

        #region style variable

        protected Font m_CustomFont;
        protected Font m_CustomFont2;
        protected GUIStyle m_TitleStyle;
        protected GUIStyle m_TitleCenterStyle;
        protected GUIStyle m_SubtitleStyle;
        protected GUIStyle m_RichTxtStyle;
        protected GUIStyle m_WarningStyle;
        protected GUIStyle m_HelpBoxStyle;

        protected GUIStyle m_LabelTxtStyleLeft;
        protected GUIStyle m_LabelTxtStyleCenter;
        protected GUIStyle m_LabelTxtStyleCenterSmall;
        protected GUIStyle m_LabelTxtStyleRight;
        protected GUIStyle m_ButtonStyle;
        protected GUIStyle m_ObjFieldStyle;
        protected GUIStyle m_ToggleStyle;
        protected GUIStyle m_FloatFieldStyle;
        protected GUIStyle m_EnumPopupStyle;
        protected GUIStyle m_ListBtnUnselect;
        protected GUIStyle m_ListBtnSelect;
        protected GUIStyle m_TextAreaStyle;
        protected GUIStyle m_ToolTipStyle;

        protected Texture m_WarningIcon;
        protected Texture m_AudioIcon;
        protected Texture m_SceneIcon;
        protected Texture m_PoolIcon;
        protected Texture m_LanguageIcon;

        protected float ScreenWidth
        {
            get{ return Screen.width * (1f / Screen.dpi * 96); }
        }
        #endregion

        #region style function

        protected virtual void OnGUI()
        {
            GetTexture();
            CreateStyle();
        }

        private void GetTexture()
        {
            string path = Application.dataPath + cIconsPath;
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }
            m_WarningIcon = (Texture)AssetDatabase.LoadAssetAtPath(path + "WarningIcon.png", typeof(Texture));
            m_AudioIcon = (Texture)AssetDatabase.LoadAssetAtPath(path + "AudioManagerIconPlus.png", typeof(Texture));
            m_SceneIcon = (Texture)AssetDatabase.LoadAssetAtPath(path + "SceneDataIcon.png", typeof(Texture));
            m_PoolIcon = (Texture)AssetDatabase.LoadAssetAtPath(path + "PoolManagerIconPlus.png", typeof(Texture));
            m_LanguageIcon = (Texture)AssetDatabase.LoadAssetAtPath(path + "LanguageManagerIconPlus.png", typeof(Texture));
        }

        private void CreateStyle()
        {
            m_CustomFont = Resources.Load("editorFont") as Font;
            m_CustomFont2 = Resources.Load("editorFont5") as Font;
            m_TitleStyle = new GUIStyle(GUI.skin.label);
            m_TitleStyle.font = m_CustomFont;
            m_TitleStyle.fontSize = 14;
            m_TitleStyle.normal.textColor = Color.white;
            m_TitleStyle.alignment = TextAnchor.MiddleLeft;

            m_TitleCenterStyle = new GUIStyle(GUI.skin.label);
            m_TitleCenterStyle.font = m_CustomFont;
            m_TitleCenterStyle.fontSize = 14;
            m_TitleCenterStyle.normal.textColor = Color.white;
            m_TitleCenterStyle.alignment = TextAnchor.MiddleCenter;

            m_SubtitleStyle = new GUIStyle(GUI.skin.label);
            m_SubtitleStyle.font = m_CustomFont2;
            m_SubtitleStyle.fontSize = 12;
            m_SubtitleStyle.normal.textColor = ColorManager.GetCompagnieBackColor();
            m_SubtitleStyle.alignment = TextAnchor.MiddleLeft;
            m_SubtitleStyle.richText = true;

            m_LabelTxtStyleLeft = new GUIStyle(GUI.skin.label);
            m_LabelTxtStyleLeft.font = m_CustomFont2;
            m_LabelTxtStyleLeft.fontSize = 12;
            m_LabelTxtStyleLeft.normal.textColor = ColorManager.GetCompagnieBackColor();
            m_LabelTxtStyleLeft.richText = true;
            m_LabelTxtStyleLeft.alignment = TextAnchor.MiddleLeft;

            m_LabelTxtStyleCenter = new GUIStyle(m_LabelTxtStyleLeft);
            m_LabelTxtStyleCenter.alignment = TextAnchor.MiddleCenter;

            m_LabelTxtStyleRight = new GUIStyle(m_LabelTxtStyleLeft);
            m_LabelTxtStyleRight.alignment = TextAnchor.MiddleRight;

            m_LabelTxtStyleCenterSmall = new GUIStyle(m_LabelTxtStyleCenter);
            m_LabelTxtStyleCenterSmall.fontSize = 8;

            m_RichTxtStyle = new GUIStyle(GUI.skin.label);
            m_RichTxtStyle.richText = true;

            m_WarningStyle = new GUIStyle();
            m_WarningStyle = GUI.skin.label;
            m_WarningStyle.richText = true;
            m_WarningStyle.fontSize = 12;
            m_WarningStyle.alignment = TextAnchor.MiddleCenter;

            m_ButtonStyle = new GUIStyle(GUI.skin.button);
            m_ButtonStyle.font = m_CustomFont2;
            m_ButtonStyle.fontSize = 12;
            m_ButtonStyle.alignment = TextAnchor.MiddleCenter;
            m_ButtonStyle.richText = true;
            m_ButtonStyle.normal.textColor = ColorManager.GetCompagnieBackColor();
            m_ButtonStyle.focused.textColor = CurrentColorDark;
            m_ButtonStyle.active.textColor = CurrentColor;

            m_ListBtnSelect = new GUIStyle(GUI.skin.textField);
            m_ListBtnSelect.font = m_CustomFont2;
            m_ListBtnSelect.fontSize = 12;
            m_ListBtnSelect.alignment = TextAnchor.MiddleLeft;
            m_ListBtnSelect.imagePosition = ImagePosition.ImageLeft;
            m_ListBtnSelect.richText = true;

            m_ListBtnUnselect = new GUIStyle(GUI.skin.label);
            m_ListBtnUnselect.font = m_CustomFont2;
            m_ListBtnUnselect.fontSize = 12;
            m_ListBtnUnselect.alignment = TextAnchor.MiddleLeft;
            m_ListBtnUnselect.imagePosition = ImagePosition.ImageLeft;
            m_ListBtnUnselect.richText = true;

            m_TextAreaStyle = new GUIStyle(GUI.skin.textArea);
            m_TextAreaStyle.font = m_CustomFont2;
            m_TextAreaStyle.fontSize = 12;
            m_TextAreaStyle.alignment = TextAnchor.UpperLeft;

            m_ToolTipStyle = new GUIStyle(GUI.skin.textArea);
            m_ToolTipStyle.font = m_CustomFont2;
            m_ToolTipStyle.fontSize = 10;
            m_ToolTipStyle.alignment = TextAnchor.UpperLeft;
            m_ToolTipStyle.richText = true;


            m_ObjFieldStyle = new GUIStyle(GUI.skin.label);
            m_ObjFieldStyle.font = m_CustomFont2;
            m_ObjFieldStyle.fontSize = 12;

            m_FloatFieldStyle = new GUIStyle(GUI.skin.textField);
            m_FloatFieldStyle.font = m_CustomFont2;
            m_FloatFieldStyle.fontSize = 11;
            m_FloatFieldStyle.alignment = TextAnchor.MiddleCenter;

            m_ToggleStyle = new GUIStyle(GUI.skin.label);
            m_ToggleStyle.font = m_CustomFont2;
            m_ToggleStyle.fontSize = 12;
            m_ToggleStyle.alignment = TextAnchor.MiddleLeft;

            m_HelpBoxStyle = GUI.skin.GetStyle("HelpBox");
            m_HelpBoxStyle.richText = true;
        }

        protected void DrawSeparator()
        {
            GUILayout.Space(1.0f);
            Color32 colorBack = GetColor(false);
            GUI.color = colorBack;
            EditorGUILayout.BeginHorizontal(GUI.skin.button);
            EditorGUILayout.LabelField("", GUILayout.Height(1f));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUILayout.Space(1.0f);
        }

        protected void DrawSeparatorInvisible(bool aWithSpace)
        {
            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }

            GUI.color = Color.white;
            Color32 test = Color.white;
            test.a = 80;
            GUI.color = test;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            EditorGUILayout.LabelField("", GUILayout.Height(1f));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }
        }

        protected void DrawSeparator(bool aWithSpace)
        {
            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }

            GUI.color = GetColor(false);
            Color32 test = GetColor(false);
            test.a = 80;
            GUI.color = test;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            EditorGUILayout.LabelField("", GUILayout.Height(1f));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }
        }

        protected void DrawSeparatorDark(bool aWithSpace)
        {
            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }
            
            GUI.color = GetColor(true);
            Color32 test = GetColor(true);
            test.a = 80;
            GUI.color = test;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            EditorGUILayout.LabelField("", GUILayout.Height(1f));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            if(aWithSpace)
            {
                GUILayout.Space(5.0f);
            }
        }

        ///<summary> Write a title with a tools infos</summary>
        protected void DrawTitleWithTips(string aTitle, Color32 aColor, string aToolInfos)
        {
            DrawTitle(aTitle, aColor);
            DrawToolsInfo(aToolInfos, false, false);
        }

        ///<summary> Write a title </summary>
        protected void DrawTitle(string aTitle, Color32 aColor)
        {
            GUI.color = ColorManager.GetCompagnieBackColor();
            EditorGUILayout.BeginVertical(GUI.skin.button);
            GUI.color = aColor;
            ShowAdvancedLabelCenter(aTitle, m_TitleCenterStyle);
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
            m_TitleCenterStyle.normal.textColor = Color.white;
        }

        protected void DrawSubtitle(string aText)
        {
            EditorGUILayout.LabelField("<i>" + aText + "</i>", m_SubtitleStyle);
        }

        protected void DrawToolsInfo(string aText, bool aBold, bool aItalic)
        {
            GUI.color = ColorManager.GetCompagnieBackColor();
            EditorGUILayout.BeginVertical(GUI.skin.button);
            GUI.color = Color.white;

            if (aBold && !aItalic)
                EditorGUILayout.HelpBox("<b>" + aText + "</b>", MessageType.Info);
            else if (aItalic && !aBold)
                EditorGUILayout.HelpBox("<i>" + aText + "</i>", MessageType.Info);
            else if (aItalic && aBold)
                EditorGUILayout.HelpBox("<b><i>" + aText + "</i></b>", MessageType.Info);
            else
                EditorGUILayout.HelpBox(aText, MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        private void DrawBanner()
        {
            GUI.color = ColorManager.GetCompagnieBackColor();
            EditorGUILayout.BeginVertical(GUI.skin.button);
            GUI.color = ColorManager.GetCompagnieColor();
            EditorGUILayout.LabelField(CompanyName, m_TitleCenterStyle);
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
        }

        protected void DrawWarning(string aText)
        {
            EditorGUILayout.TextArea(aText, m_WarningStyle);
        }

        ///<summary> Get the property of the SerializedObject property ask for [Make sure it's the same name on both script] </summary>
        protected void GetData(ref SerializedProperty aProperty, string aPropertyName)
        {
            aProperty = this.serializedObject.FindProperty(aPropertyName);
        }

        protected void UpdateProperty()
        {
            //EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        ///<summary> MUST CALL A EditorGUILayout.EndVertical() at the end </summary>
        protected void BeginInspector(eManagers aManagerType, string aInspectorTitle)
        {
            CurrentColor = ColorManager.GetColor(aManagerType, false);
            CurrentColorDark = ColorManager.GetColor(aManagerType, true);

            GUI.color = CurrentColor;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            DrawTitle(aInspectorTitle + "®", CurrentColor);
            GUILayout.Space(10);
        }

        protected void ShowGroup(string aGroupName, System.Action aShowFunction)
        {
            
            EditorGUILayout.BeginVertical(GUI.skin.textField);
            GUILayout.Space(5f);
            if (!string.IsNullOrEmpty(aGroupName))
            {
                EditorGUILayout.LabelField(aGroupName);
            }
            if (aShowFunction != null)
            {
                aShowFunction();
            }
            GUILayout.Space(10f);
            EditorGUILayout.EndVertical();
            
        }

        protected bool ShowFadeGroup(ref UnityEditor.AnimatedValues.AnimBool aAnimBool, System.Action aShowFunction)
        {
            if (EditorGUILayout.BeginFadeGroup(aAnimBool.faded) && aShowFunction != null)
            {
                aShowFunction();
            }
            EditorGUILayout.EndFadeGroup();


            return aAnimBool.value;
        }

        protected void ShowAdvancedFadeGroup(ref UnityEditor.AnimatedValues.AnimBool aAnimBool, SerializedProperty aEnableObj, string aGroupName, System.Action aShowFunction)
        {

            float sideWidth = 50.0f;
            float centerWidth = ((ScreenWidth) - (sideWidth * 2.0f) - 62.0f);

            EditorGUILayout.BeginVertical(GUI.skin.textField);
            EditorGUILayout.BeginHorizontal();

            string enableTxt = "Enable";
            string disableTxt = "Disable";
            if(!SceneAutoLoader.IsUsingEnglish)
                {
                    enableTxt = "Activé";
                    disableTxt = "Désactivé";
                }
                

            if (aAnimBool.target)
            {
                GUI.color = GetColor(false);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.LabelField(enableTxt, m_LabelTxtStyleCenterSmall, GUILayout.Width(sideWidth), GUILayout.Height(10));
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button(aGroupName, m_ButtonStyle, GUILayout.Width(centerWidth)))
                {
                    aAnimBool.target = !aAnimBool.target;
                }
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(disableTxt, m_LabelTxtStyleCenterSmall, GUILayout.Width(sideWidth), GUILayout.Height(10));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(enableTxt, m_LabelTxtStyleCenterSmall, GUILayout.Width(sideWidth), GUILayout.Height(10));
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button(aGroupName, m_ButtonStyle, GUILayout.Width(centerWidth)))
                {
                    aAnimBool.target = !aAnimBool.target;
                }
                GUI.color = GetColor(false);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.LabelField(disableTxt, m_LabelTxtStyleCenterSmall, GUILayout.Width(sideWidth), GUILayout.Height(10));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();


            if (EditorGUILayout.BeginFadeGroup(aAnimBool.faded) && aShowFunction != null)
            {
                aShowFunction();
            }
            EditorGUILayout.EndFadeGroup();


            if (aAnimBool.target)
            {
                DrawSeparator(false);
            }
            EditorGUILayout.EndVertical();
            if(aEnableObj != null)
            {
                aEnableObj.boolValue = aAnimBool.target;
            }
            
        }

        protected float ShowAdvancedSlider(string aSliderName, string aLeftText, string aRightText, float aMinSliderValue, float aMaxSliderValue, float aSliderValue, int aNbOfStage)
        {
            float sideWidth = 50.0f;
            float centerWidth = (ScreenWidth - sideWidth - 135.0f - (10f * aNbOfStage));
            float centerWidthSlider = (ScreenWidth - sideWidth - 30.0f) - (10f * aNbOfStage);

            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("[" + GetColorText(aLeftText, true) + "]", m_LabelTxtStyleLeft, GUILayout.Width(sideWidth));
            EditorGUILayout.LabelField(aSliderName, m_LabelTxtStyleCenter, GUILayout.Width(centerWidth));
            EditorGUILayout.LabelField("[" + GetColorText(aRightText, true) + "]", m_LabelTxtStyleRight, GUILayout.Width(sideWidth));

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(-6f);
            EditorGUILayout.BeginHorizontal();
            aSliderValue = GUILayout.HorizontalSlider(aSliderValue, aMinSliderValue, aMaxSliderValue, GUILayout.Width(centerWidthSlider));
            aSliderValue = EditorGUILayout.FloatField(aSliderValue, m_FloatFieldStyle, GUILayout.Width(sideWidth));
            EditorGUILayout.EndHorizontal();

            if(aSliderValue > aMaxSliderValue)
            {
                aSliderValue = aMaxSliderValue;
            }
            if(aSliderValue < aMinSliderValue)
            {
                aSliderValue = aMinSliderValue;
            }

            aSliderValue = FloatFieldLimitDecimal(aSliderValue, 3);

            return aSliderValue;
        }

        protected void ShowAdvancedMinMaxSlider(string aSliderName, string aLeftText, string aRightText, float aMinSliderLimit, float aMaxSliderLimit, SerializedProperty aMinValue, SerializedProperty aMaxValue, int aNbOfStage)
        {
            float sideWidth = 50.0f;
            float centerWidth = (ScreenWidth - (sideWidth * 4.0f) - 42.0f) - (10f * aNbOfStage);
            float centerWidthSlider = (ScreenWidth - (sideWidth * 2.0f) - 36.0f) - (10f * aNbOfStage);
            float minValue = aMinValue.floatValue;
            float maxValue = aMaxValue.floatValue;

            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("", m_LabelTxtStyleLeft, GUILayout.Width(sideWidth));
            EditorGUILayout.LabelField("[" + GetColorText(aLeftText, true) + "]", m_LabelTxtStyleLeft, GUILayout.Width(sideWidth));

            EditorGUILayout.LabelField(aSliderName, m_LabelTxtStyleCenter, GUILayout.Width(centerWidth));

            EditorGUILayout.LabelField("[" + GetColorText(aRightText, true) + "]", m_LabelTxtStyleRight, GUILayout.Width(sideWidth));
            EditorGUILayout.LabelField("", m_LabelTxtStyleRight, GUILayout.Width(sideWidth));

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(-2f);
            EditorGUILayout.BeginHorizontal();
            minValue = EditorGUILayout.FloatField(minValue, m_FloatFieldStyle, GUILayout.Width(sideWidth));
            EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, aMinSliderLimit, aMaxSliderLimit, GUILayout.Width(centerWidthSlider));
            maxValue = EditorGUILayout.FloatField(maxValue, m_FloatFieldStyle, GUILayout.Width(sideWidth));
            EditorGUILayout.EndHorizontal();

            if (minValue < aMinSliderLimit)
            {
                minValue = aMinSliderLimit;
            }
            if (maxValue > aMaxSliderLimit)
            {
                maxValue = aMaxSliderLimit;
            }
            if (minValue > maxValue)
            {
                minValue = maxValue;
            }
            if (maxValue < minValue)
            {
                maxValue = minValue;
            }

            minValue = FloatFieldLimitDecimal(minValue, 3);
            maxValue = FloatFieldLimitDecimal(maxValue, 3);

            aMinValue.floatValue = minValue;
            aMaxValue.floatValue = maxValue;
        }

        protected UnityEngine.Object ShowAdvancedObjectFieldSimple(UnityEngine.Object aValue, System.Type aObjType, GUIStyle aStyle, bool aAllowSceneObjects, int aNbOfStage)
        {
            float reduceSize = 127.0f + (aNbOfStage * 10.0f);
            GUILayout.Space(10.0f);

            Texture objIcon = GetObjectIcon(aObjType);
            aStyle.imagePosition = ImagePosition.ImageLeft;

            //--------------------

            EditorGUILayout.BeginHorizontal();

            if(aValue != null)
            {
                if(objIcon == null)
                {
                    EditorGUILayout.LabelField(new GUIContent(aValue.name, objIcon), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(" " + aValue.name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
            }
            else
            {
                if(SceneAutoLoader.IsUsingEnglish)
                {
                    EditorGUILayout.LabelField(new GUIContent(" No <i>" + aObjType.Name + "</i> selected", m_WarningIcon, "You need to select a(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(" Aucun <i>" + aObjType.Name + "</i> sélectionné", m_WarningIcon, "Vous devez sélectionnez un(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
            }
            EditorGUILayout.LabelField("<size=10>[" + GetColorText("Drag & Drop", true) + "]</size>", m_LabelTxtStyleCenter, GUILayout.Width(85.0f));

            EditorGUILayout.EndHorizontal();

            //--------------------

            aValue = EditorGUILayout.ObjectField(aValue, aObjType, aAllowSceneObjects, GUILayout.Width(ScreenWidth - reduceSize + 103f));
            GUILayout.Space(10f);
            return aValue;
        }

        protected void ShowAdvancedObjectField<T>(ref UnityEngine.Object aValue, System.Type aObjType, GUIStyle aStyle, bool aAllowSceneObjects, int aIdPicker, int aNbOfStage) where T : UnityEngine.Object
        {
            float reduceSize = 206.0f + (aNbOfStage * 10.0f);
            GUILayout.Space(10.0f);

            T tObj = aValue as T;

            Texture objIcon = GetObjectIcon(aObjType);
            aStyle.imagePosition = ImagePosition.ImageLeft;

            //--------------------

            EditorGUILayout.BeginHorizontal();

            if(tObj != null)
            {
                if(objIcon == null)
                {
                    EditorGUILayout.LabelField(new GUIContent(tObj.name, objIcon), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(" " + tObj.name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
            }
            else
            {
                if(SceneAutoLoader.IsUsingEnglish)
                {
                    EditorGUILayout.LabelField(new GUIContent(" No <i>" + aObjType.Name + "</i> selected", m_WarningIcon, "You need to select a(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(" Aucun <i>" + aObjType.Name + "</i> sélectionné", m_WarningIcon, "Vous devez sélectionnez un(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
            }
            EditorGUILayout.LabelField("<size=10>[" + GetColorText("Drag & Drop", true) + "]</size>", m_LabelTxtStyleCenter, GUILayout.Width(85.0f));

            EditorGUILayout.EndHorizontal();

            //--------------------

            EditorGUILayout.BeginHorizontal();
            
            aValue = EditorGUILayout.ObjectField(aValue, aObjType, aAllowSceneObjects, GUILayout.Width(ScreenWidth - reduceSize + 103f));
            if (GUILayout.Button("Select", m_ButtonStyle, GUILayout.Width(70f)))
            {
                EditorGUIUtility.ShowObjectPicker<T>(tObj, aAllowSceneObjects, "", aIdPicker);
            }
            EditorGUILayout.EndHorizontal();

            if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == aIdPicker)
                {
                    tObj = EditorGUIUtility.GetObjectPickerObject() as T;
                    aValue = tObj;
                }
            }
            GUILayout.Space(10f);

        }

        protected void ShowAdvancedObjectField<T>(SerializedProperty aValue, System.Type aObjType, GUIStyle aStyle, bool aAllowSceneObjects, int aIdPicker, int aNbOfStage) where T : UnityEngine.Object
        {
            UnityEngine.Object value = aValue.objectReferenceValue;
            ShowAdvancedObjectField<T>(ref value, aObjType, aStyle, aAllowSceneObjects, aIdPicker, aNbOfStage);
            aValue.objectReferenceValue = value;
        }

        protected SceneAsset ShowAdvancedObjectFieldSceneAsset<T>(SceneAsset aValue, System.Type aObjType, GUIStyle aStyle, bool aAllowSceneObjects, int aIdPicker, int aNbOfStage) where T : UnityEngine.Object
        {
            float reduceSize = 206.0f + (aNbOfStage * 10.0f);

            GUILayout.Space(10f);
            SceneAsset tObj = aValue;

            aStyle.imagePosition = ImagePosition.ImageLeft;
            

            EditorGUILayout.BeginHorizontal();
            if (tObj != null)
            {
                EditorGUILayout.LabelField(new GUIContent(" " + tObj.name, m_SceneIcon), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
            }
            else
            {
                if(SceneAutoLoader.IsUsingEnglish)
                {
                    EditorGUILayout.LabelField(new GUIContent(" No <i>" + aObjType.Name + "</i> selected", m_WarningIcon, "You need to select a(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(" Aucun <i>" + aObjType.Name + "</i> sélectionné", m_WarningIcon, "Vous devez sélectionnez un(n) " + aObjType.Name), aStyle, GUILayout.Width(ScreenWidth - reduceSize));
                }
            }
            EditorGUILayout.LabelField("<size=10>[" + GetColorText("Drag & Drop", true) + "]</size>", m_LabelTxtStyleCenter, GUILayout.Width(85.0f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            aValue = (SceneAsset)EditorGUILayout.ObjectField(tObj, aObjType, false, GUILayout.Width(ScreenWidth - reduceSize +  + 103f));
            if (GUILayout.Button("Select", m_ButtonStyle, GUILayout.Width(70f)))
            {
                EditorGUIUtility.ShowObjectPicker<T>(tObj, aAllowSceneObjects, "", aIdPicker);
            }
            EditorGUILayout.EndHorizontal();

            if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == aIdPicker)
                {
                    if (EditorGUIUtility.GetObjectPickerObject() != null)
                    {
                        tObj = (SceneAsset)EditorGUIUtility.GetObjectPickerObject();
                    }
                    else
                    {
                        tObj = null;
                    }
                    aValue = tObj;
                }
            }
            GUILayout.Space(10f);
            return aValue;

        }

        protected float ShowAdvancedFloatField(string aLabel, float aValue, float aMin, float aMax, int aNbOfStage)
        {
            float space = 203.0f + (aNbOfStage * 10.0f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(aLabel, m_LabelTxtStyleLeft, GUILayout.Width(170.0f));
            aValue = EditorGUILayout.FloatField(aValue, m_FloatFieldStyle, GUILayout.Width(ScreenWidth - space));
            EditorGUILayout.EndHorizontal();
            if (aValue > aMax)
            {
                aValue = aMax;
            }
            else if (aValue < aMin)
            {
                aValue = aMin;
            }
            return aValue;
        }

        protected int ShowAdvancedIntField(string aLabel, int aValue, int aMin, int aMax, int aNbOfStage)
        {
            float space = 203.0f + (aNbOfStage * 10.0f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(aLabel, m_LabelTxtStyleLeft, GUILayout.Width(170.0f));
            aValue = EditorGUILayout.IntField(aValue, m_FloatFieldStyle, GUILayout.Width(ScreenWidth - space));
            EditorGUILayout.EndHorizontal();
            if (aValue > aMax)
            {
                aValue = aMax;
            }
            else if (aValue < aMin)
            {
                aValue = aMin;
            }
            return aValue;
        }

        protected bool ShowAdvancedToggle(string aLabel, bool aValue)
        {
            EditorGUILayout.BeginHorizontal();
            aValue = EditorGUILayout.Toggle(aValue, GUILayout.MaxWidth(15f));
            EditorGUILayout.LabelField(aLabel, m_ToggleStyle);
            EditorGUILayout.EndHorizontal();
            return aValue;
        }

        protected float FloatFieldLimitDecimal(float aValue, int aNbOfDecimal)
        {
            if (aNbOfDecimal < 0)
            {
                aNbOfDecimal = 0;
            }

            string convert = aValue.ToString("F" + aNbOfDecimal.ToString());

            if (float.TryParse(convert, out aValue))
                return aValue;
            else
                return 0;
        }

        protected void ShowNeedManagerMessage(eManagers aManager)
        {
            GUILayout.Space(4f);
            Color32 colorValue = Color.black;
            colorValue.a = 200;
            GUI.backgroundColor = colorValue;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            if(SceneAutoLoader.IsUsingEnglish)
            {
                ShowAdvancedLabelCenter(new GUIContent(ColorManager.GetColorText("•", aManager, false) +"<color=#ffffff> Need the " + ColorManager.GetColorText(aManager.ToString(), aManager, false)  + " for this option</color> " + ColorManager.GetColorText("•", aManager, false)));
            }
            else
            {
                if(aManager == eManagers.AudioManager || aManager == eManagers.OptionManager)
                {
                    ShowAdvancedLabelCenter(new GUIContent(ColorManager.GetColorText("•", aManager, false) +"<color=#ffffff> Besoin de l'" + ColorManager.GetColorText(aManager.ToString(), aManager, false)  + " pour cette option</color> " + ColorManager.GetColorText("•", aManager, false)));
                }
                else
                {
                    ShowAdvancedLabelCenter(new GUIContent(ColorManager.GetColorText("•", aManager, false) +"<color=#ffffff> Besoin du " + ColorManager.GetColorText(aManager.ToString(), aManager, false)  + " pour cette option</color> " + ColorManager.GetColorText("•", aManager, false)));
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
        }

        protected void ShowUsefulManagerMessage(eManagers aManager)
        {
            GUILayout.Space(4f);
            Color32 colorValue = Color.black;
            colorValue.a = 200;
            GUI.backgroundColor = colorValue;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            if(SceneAutoLoader.IsUsingEnglish)
            {
                ShowAdvancedLabelCenter(new GUIContent(ColorManager.GetColorText("•", aManager, false) +"<color=#ffffff> Useful when combine with the " + ColorManager.GetColorText(aManager.ToString(), aManager, false)  + " </color> " + ColorManager.GetColorText("•", aManager, false)));
            }
            else
            {
                ShowAdvancedLabelCenter(new GUIContent(ColorManager.GetColorText("•", aManager, false) +"<color=#ffffff> Utile lorsque combiné avec le " + ColorManager.GetColorText(aManager.ToString(), aManager, false)  + " </color> " + ColorManager.GetColorText("•", aManager, false)));
            }
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
        }

        protected int ShowAdvancedEnumPopup(int aEnumIndex, System.Type aEnumType, int aNbOfStage)
        {
            float space = 30.0f + (5.0f * aNbOfStage);

            string[] options = System.Enum.GetNames(aEnumType);
            string newWord = "";

            for (int i = 0; i < options.Length; i++)
            {
                newWord = "";
                string[] split = System.Text.RegularExpressions.Regex.Split(options[i], @"(?<!^)(?=[A-Z])");
                
                for (int k = 0; k < split.Length; k++)
                {
                    newWord += split[k] + " ";
                }

                split = System.Text.RegularExpressions.Regex.Split(newWord,  @"(?<=[a-zA-Z])(?=\d)");
                newWord = "";

                for (int k = 0; k < split.Length; k++)
                {
                    newWord += split[k] + " ";
                }

                options[i] = newWord;
            }

            aEnumIndex = EditorGUILayout.Popup(aEnumIndex, options, m_FloatFieldStyle, GUILayout.Width(ScreenWidth - space));


            return aEnumIndex;
        }

        protected int ShowAdvancedEnumPopup(string[] aEnumChoices, int aEnumIndex, int aNbOfStage)
        {
            float space = 30.0f + (5.0f * aNbOfStage);
            return EditorGUILayout.Popup(aEnumIndex, aEnumChoices, m_FloatFieldStyle, GUILayout.Width(ScreenWidth - space));
        }

        protected int ShowAdvancedEnumPopupAdd(int aEnumIndex, System.Type aEnumType, int aNbOfStage, bool aAddBtnEnable, System.Action aAddAction)
        {

            float space = 100.0f + (5.0f * aNbOfStage);

            string[] options = System.Enum.GetNames(aEnumType);
            string newWord = "";

            for (int i = 0; i < options.Length; i++)
            {
                newWord = "";
                string[] split = System.Text.RegularExpressions.Regex.Split(options[i], @"(?<!^)(?=[A-Z0-9])");

                for (int k = 0; k < split.Length; k++)
                {
                    newWord += split[k] + " ";
                }

                split = System.Text.RegularExpressions.Regex.Split(newWord,  @"(?<=[a-zA-Z])(?=\d)");
                newWord = "";

                for (int k = 0; k < split.Length; k++)
                {
                    newWord += split[k] + " ";
                }

                options[i] = newWord;
            }

            string addWord = "Add";
            if(!SceneAutoLoader.IsUsingEnglish)
            {
                addWord = "Ajouter";
            }

            EditorGUILayout.BeginHorizontal();
            aEnumIndex = EditorGUILayout.Popup(aEnumIndex, options, m_FloatFieldStyle, GUILayout.Height(20f), GUILayout.Width(ScreenWidth - space));

            EditorGUI.BeginDisabledGroup(!aAddBtnEnable);
            if (GUILayout.Button(addWord, m_ButtonStyle, GUILayout.Width(60.0f), GUILayout.Height(20f)))
            {
                if (aAddAction != null)
                {
                    aAddAction();
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            return aEnumIndex;
        }

        protected bool ShowAdvancedListBtn(string aText, Texture aIconToShow, bool aSelect, int aNbOfStage)
        {
            GUIStyle tempStyle = m_ListBtnUnselect;
            Color oldBackgroundColor = GUI.backgroundColor;
            if (aSelect)
            {
                tempStyle = m_ListBtnSelect;
                GUI.backgroundColor = CurrentColor;
            }

            float space = 32.0f + (aNbOfStage * 10.0f);

            if (GUILayout.Button(new GUIContent(aText, aIconToShow), tempStyle, GUILayout.Height(19.0f), GUILayout.Width(ScreenWidth - space)))
            {
                return true;
            }

            if (aSelect)
            {
                GUI.backgroundColor = oldBackgroundColor;
            }

            return false;
        }

        protected bool ShowAdvancedListBtn(string aText, string aValue, Texture aIconToShow, bool aSelect, int aNbOfStage)
        {
            GUIStyle tempStyle = m_ListBtnUnselect;
            Color oldBackgroundColor = GUI.backgroundColor;
            if (aSelect)
            {
                tempStyle = m_ListBtnSelect;
                GUI.backgroundColor = CurrentColor;
            }

            float space = 125.0f + (aNbOfStage * 10.0f);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent(aText, aIconToShow), tempStyle, GUILayout.Height(19.0f), GUILayout.Width(ScreenWidth - space)))
            {
                aSelect = true;
            }
            EditorGUILayout.LabelField(aValue, m_LabelTxtStyleRight, GUILayout.Width(95));
            EditorGUILayout.EndHorizontal();

            if (aSelect)
            {
                GUI.backgroundColor = oldBackgroundColor;
            }

            return aSelect;
        }

        protected bool ShowAdvancedListBtnRemove(string aText, Texture aIconToShow, bool aSelect, bool aCanRemove, System.Action aRemoveAction, int aNbOfStage)
        {
            GUIStyle tempStyle = m_ListBtnUnselect;
            Color oldBackgroundColor = GUI.backgroundColor;
            if (aSelect)
            {
                tempStyle = m_ListBtnSelect;
                GUI.backgroundColor = CurrentColor;
            }

            float space = 100.0f + (aNbOfStage * 10.0f);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent(aText, aIconToShow), tempStyle, GUILayout.Height(19.0f), GUILayout.Width(ScreenWidth - space)))
            {
                aSelect = true;
            }
            EditorGUI.BeginDisabledGroup(!aCanRemove);
            string removeWord = "Remove";
            if(!SceneAutoLoader.IsUsingEnglish)
            {
                removeWord = "Retirer";
            }
            if (GUILayout.Button(removeWord, m_ButtonStyle, GUILayout.Width(60.0f), GUILayout.Height(20f)))
            {
                if (aRemoveAction != null)
                {
                    aRemoveAction();
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (aSelect)
            {
                GUI.backgroundColor = oldBackgroundColor;
            }

            return aSelect;
        }

        protected int ShowAdvancedBtnChoice(string[] aBtnName, int aCurrentBtnId, int aNbOfStage)
        {
            float btnWidth = (ScreenWidth - 40 - (10 * aNbOfStage))/aBtnName.Length;

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < aBtnName.Length; i++)
            {
                if (i == aCurrentBtnId)
                {
                    Color oldBackgroundColor = GUI.backgroundColor;
                    GUI.backgroundColor = CurrentColor;
                    if (GUILayout.Button(aBtnName[i], m_ButtonStyle, GUILayout.Width(btnWidth)))
                    {
                        aCurrentBtnId = i;
                    }
                    GUI.backgroundColor = oldBackgroundColor;
                }
                else
                {
                    if (GUILayout.Button(aBtnName[i], m_ButtonStyle, GUILayout.Width(btnWidth)))
                    {
                        aCurrentBtnId = i;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return aCurrentBtnId;
        }

        protected int ShowAdvancedBtnChoice(string aTitleChoice, string[] aBtnName, int aCurrentBtnId, int aNbOfStage)
        {
            EditorGUILayout.LabelField(aTitleChoice, m_LabelTxtStyleLeft);
            return ShowAdvancedBtnChoice(aBtnName, aCurrentBtnId, aNbOfStage);
        }

        protected void ShowAdvancedWarningMessage(string aMessage)
        {
            Color32 colorValue = Color.yellow;
            colorValue.a = 50;
            GUI.backgroundColor = colorValue;
            ShowAdvancedLabelCenter(new GUIContent(aMessage, m_WarningIcon), m_ToolTipStyle);
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
        }

        protected void ShowAdvancedToolTip(string aMessage)
        {
            GUILayout.Space(5f);
            Color32 color = CurrentColorDark;
            color.a = 80;
            GUI.backgroundColor = color;
            EditorGUILayout.LabelField("\n" + aMessage + "\n", m_ToolTipStyle);
            GUI.backgroundColor = Color.white;
        }

        protected Color ShowAdvancedColorSelection(string aMessage, Color aColor)
        {
            if (!string.IsNullOrEmpty(aMessage))
            {
                ShowAdvancedLabelCenter(aMessage);
            }
            return EditorGUILayout.ColorField(aColor);
        }

        protected void ShowTitle(string aTitle, eManagers aManager)
        {
            GUILayout.Space(22f);

            Color32 test = GetColor(false);
            test.a = 80;
            GUI.color = test;
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            EditorGUILayout.LabelField("", GUILayout.Height(0.01f));
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            GUILayout.Space(-27.5f);
            GUI.backgroundColor = new Color32(55,55,55,255);
            EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
            GUI.color = CurrentColor;
            ShowAdvancedLabelCenter(("<i><size=13>" + ColorManager.GetColorText(aTitle, aManager, false) + "</size></i>"));
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            GUILayout.Space(10f);
        }

        protected void ShowAdvancedLabelCenter(string aMessage)
        {
            ShowAdvancedLabelCenter(new GUIContent(aMessage), m_LabelTxtStyleCenter);
        }

        protected void ShowAdvancedSubtitle(string aMessage)
        {
            GUILayout.Space(5f);
            ShowAdvancedLabelCenter(aMessage);
            DrawSeparator(true);
        }

        protected void ShowAdvancedLabelCenter(string aMessage, GUIStyle aCustomStyle)
        {
            ShowAdvancedLabelCenter(new GUIContent(aMessage), aCustomStyle);
        }

        protected void ShowAdvancedLabelCenter(GUIContent aMessage, GUIStyle aCustomStyle)
        {
            EditorGUILayout.LabelField(aMessage, aCustomStyle);
        }

        protected void ShowAdvancedLabelCenter(GUIContent aMessage)
        {
            ShowAdvancedLabelCenter(aMessage, m_LabelTxtStyleCenter);
        }

        protected string ShowAdvancedTextInsert(string aMessage, int aNbOfStage)
        {
            float space = 24.0f + (aNbOfStage * 11.0f);
            return EditorGUILayout.TextArea(aMessage, m_TextAreaStyle, GUILayout.Width(ScreenWidth - space));
        }

        protected string ShowAdvancedTextInsert(string aTitle, string aMessage, int aNbOfStage)
        {
            float space = 200.0f + (aNbOfStage * 10.0f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(aTitle, m_LabelTxtStyleLeft, GUILayout.Width(170));
            string message = EditorGUILayout.TextArea(aMessage, m_TextAreaStyle, GUILayout.Width(ScreenWidth - space));
            EditorGUILayout.EndHorizontal();

            return message;
        }

        protected bool isNull(Object aObject)
        {
            if(aObject == null)
            {
                return true;
            }
            return false;
        }

        #region Language Text

        protected string[] EditorTextFR = new string[]{};
        protected string[] EditorTextEN = new string[]{};

        protected void NewEditorText(string[] aTexts, bool aIsForEnglish)
        {
            if(aIsForEnglish)
            {
                EditorTextEN = aTexts;
            }
            else
            {
                EditorTextFR = aTexts;
            }
        }

        protected bool EditorTextIsSet()
        {
            if(EditorTextFR.Length > 0 && EditorTextEN.Length > 0)
            {
                return true;
            }

            return false;
        }

        protected string GetEditorText(int aId)
        {
            if(SceneAutoLoader.IsUsingEnglish)
            {
                if(aId < EditorTextEN.Length)
                    return EditorTextEN[aId];
                else
                    return "No text";
            }
            else
            {
                if(aId < EditorTextFR.Length)
                    return EditorTextFR[aId];
                else
                    return "Aucun texte";
            }
        }

        #endregion

        #region Color Changer

        protected Color32 GetColor(bool aDarkColor)
        {
            if (!aDarkColor)
            {
                return CurrentColor;
            }
            else
            {
                return CurrentColorDark;
            }
        }

        protected string GetColorRich(bool aDarkColor)
        {
            return ColorUtility.ToHtmlStringRGB(GetColor(aDarkColor));
        }

        protected string GetColorText(string aText, bool aDarkColor)
        {
            return "<color=#" + GetColorRich(aDarkColor) + ">" + aText + "</color>";
        }

        protected Texture GetObjectIcon(System.Type aType)
        {
            if(aType == typeof(MusicData) || aType == typeof(AudioClip) || aType == typeof(AudioData))
            {
                return GetManagersIcon(eManagers.AudioManager);
            }

            return null;
        }

        protected Texture GetManagersIcon(eManagers aManager)
        {
            switch(aManager)
            {
                case eManagers.AudioManager:
                {
                    return m_AudioIcon;
                }
                case eManagers.SceneManager:
                {
                    return m_SceneIcon;
                }
            }

            return null;
        }

        #endregion

        #endregion

    }
}
