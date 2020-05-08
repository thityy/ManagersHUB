using UnityEngine;
using UnityEditor;
using System.IO;

namespace ManagersHUB
{
    public struct sToolDefaultRect
    {
        public float Btnheight;
        public float MargeTitle;
        public float MargeSide;
        public float MargeHeight;
        public float MinBtnWidth;
        public float MaxBtnWidth;
    }

    public class BaseEditor : EditorWindow
    {

        private const string m_CompanyName = "Thityy Tools®";
        private float m_BtnWidth;
        private float m_SpaceSize;
        protected Color32 m_CompanyColor01 = new Color32(35, 208, 185, 255);
        protected Color32 m_CompanyColor02 = new Color32(31, 31, 31, 255);
        private Color m_CompanyColor01Fade;

        #region style variable

        protected Font m_CustomFont;
        protected GUIStyle m_TitleStyle;
        protected GUIStyle m_TitleCenterStyle;
        protected GUIStyle m_SubtitleStyle;
        protected GUIStyle m_RichTxtStyle;
        protected GUIStyle m_WarningStyle;
        protected GUIStyle m_HelpBoxStyle;
        protected GUIStyle m_ToolBoxStyle;
        protected GUIStyle m_BtnStyle;
        protected GUIStyle m_ToolStyle;

        #endregion

        protected virtual void OnGUI()
        {
            CreateStyle();
            DrawBanner();
        }

        #region Set Tools

        protected sToolDefaultRect SetDefaultToolRect(float aBtnHeight, float aMargeTitle, float aMargeSide, float aMargeHeight, float aMinBtnWidth, float aMaxBtnWidth)
        {
            sToolDefaultRect toolRect = new sToolDefaultRect();
            toolRect.Btnheight = aBtnHeight;
            toolRect.MargeTitle = aMargeTitle;
            toolRect.MargeHeight = aMargeHeight;
            toolRect.MargeSide = aMargeSide;
            toolRect.MinBtnWidth = aMinBtnWidth;
            toolRect.MaxBtnWidth = aMaxBtnWidth;
            return toolRect;
        }

        private void CreateStyle()
        {
            m_CustomFont = Resources.Load("editorFont") as Font;
            m_TitleStyle = new GUIStyle(GUI.skin.label);
            m_TitleStyle.font = m_CustomFont;
            m_TitleStyle.fontSize = 11;
            m_TitleStyle.normal.textColor = Color.white;
            m_TitleStyle.alignment = TextAnchor.MiddleLeft;

            m_TitleCenterStyle = new GUIStyle(GUI.skin.label);
            m_TitleCenterStyle.font = m_CustomFont;
            m_TitleCenterStyle.fontSize = 13;
            m_TitleCenterStyle.normal.textColor = Color.white;
            m_TitleCenterStyle.alignment = TextAnchor.MiddleCenter;
            m_TitleCenterStyle.richText = true;

            m_SubtitleStyle = new GUIStyle(GUI.skin.label);
            m_SubtitleStyle.fontSize = 12;
            m_SubtitleStyle.normal.textColor = m_CompanyColor02;
            m_SubtitleStyle.alignment = TextAnchor.MiddleLeft;
            m_SubtitleStyle.richText = true;

            m_BtnStyle = new GUIStyle(GUI.skin.button);
            m_BtnStyle.richText = true;
            m_BtnStyle.fontSize = 12;
            m_BtnStyle.alignment = TextAnchor.MiddleCenter;

            m_ToolBoxStyle = new GUIStyle(GUI.skin.label);
            m_ToolBoxStyle.fontSize = 12;
            m_ToolBoxStyle.normal.textColor = m_CompanyColor02;
           
            m_ToolBoxStyle.alignment = TextAnchor.MiddleLeft;

            m_RichTxtStyle = new GUIStyle(GUI.skin.label);
            m_RichTxtStyle.richText = true;

            m_WarningStyle = new GUIStyle();
            m_WarningStyle = GUI.skin.label;
            m_WarningStyle.richText = true;
            m_WarningStyle.fontSize = 12;
            m_WarningStyle.alignment = TextAnchor.MiddleCenter;

            m_HelpBoxStyle = GUI.skin.GetStyle("HelpBox");
            if(!EditorGUIUtility.isProSkin)
            {   
                m_HelpBoxStyle.normal.textColor = m_CompanyColor02;
            }
            else
            {
                m_HelpBoxStyle.normal.textColor = Color.white;
            }
            m_HelpBoxStyle.richText = true;
        }

        protected string GetPath(string aFolderName, Object aObj)
        {
            aFolderName += "/";
            string path = AssetDatabase.GetAssetPath(m_CustomFont).Replace(m_CustomFont.name + ".otf", "") + aFolderName;
            if(!AssetDatabase.IsValidFolder(path))
            {
                string parentFolder = AssetDatabase.GetAssetPath(m_CustomFont).Replace(m_CustomFont.name + ".otf", "");
                CreateDirectoryWithPath(parentFolder + aFolderName);
            }
            int id = Resources.LoadAll(aFolderName).Length;
            path += aObj.GetType().Name + "_";
            while(!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path + id.ToString() + ".asset")))
            {
                id++;
            }
            return path + id.ToString() + ".asset";
        }

        protected string GetPathPrefab(string aFolderName, Object aObj)
        {
            aFolderName += "/";
            string path = AssetDatabase.GetAssetPath(m_CustomFont).Replace(m_CustomFont.name + ".otf", "") + aFolderName;
            if(!AssetDatabase.IsValidFolder(path))
            {
                string parentFolder = AssetDatabase.GetAssetPath(m_CustomFont).Replace(m_CustomFont.name + ".otf", "");
                CreateDirectoryWithPath(parentFolder + aFolderName);
            }
            int id = Resources.LoadAll(aFolderName).Length;
            path += aObj.GetType().Name + "_";
            while(!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path + id.ToString() + ".prefab")))
            {
                id++;
            }
            return path + id.ToString() + ".prefab";
        }

        protected string GetPath(string aFolderName)
        {
            aFolderName += "/";
            return AssetDatabase.GetAssetPath(m_CustomFont).Replace(m_CustomFont.name + ".otf", "") + aFolderName;
        }

        protected void CreateDirectoryWithPath(string aPathDir)
        {
            if(!Directory.Exists(aPathDir))
            {
                Directory.CreateDirectory(aPathDir);
                AssetDatabase.Refresh();
                Debug.Log("The directory -> " + aPathDir + " as been create to be use by your manager.");
            }
        }

        protected void GetDirectoryWithPath(string aFolderName)
        {
            if(!Directory.Exists(GetPath(aFolderName)))
            {
                Debug.LogError("The directory can't be found (has moved or has been deleted). A new one as been created");
                CreateDirectoryWithPath(GetPath(aFolderName));
            }
            else
            {
                AudioData obj = ScriptableObject.CreateInstance<AudioData>();
                string path = Path.Combine(GetPath(aFolderName, obj));
                AssetDatabase.CreateAsset(obj, path);
                Selection.activeObject = obj;
                EditorUtility.FocusProjectWindow();
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
            }
        }

        #endregion

        #region GetButtonPosition  

        private Rect GetBtnPos(int aIdPos, int aIdRow, int aNbOfBtnInRow, sToolDefaultRect aRect)
        {
            m_BtnWidth = (position.width - ((aRect.MargeSide * 4) + (aNbOfBtnInRow * 2))) / aNbOfBtnInRow;
            float checkMax = aRect.MaxBtnWidth;
            if (aNbOfBtnInRow > 5)
            {
                checkMax = aRect.MinBtnWidth;
            }
            if (m_BtnWidth > checkMax && aIdRow != 0)
            {
                m_BtnWidth = checkMax;
            }
            else if (m_BtnWidth < aRect.MinBtnWidth)
            {
                m_BtnWidth = aRect.MinBtnWidth;
            }
            float heightMarge = aRect.MargeTitle + (aIdRow * (aRect.Btnheight + (aRect.MargeHeight * 2.0f)));

            if (aIdPos <= 0)
            {
                return new Rect(aRect.MargeSide + (aRect.MargeSide * 0.5f), heightMarge, m_BtnWidth, aRect.Btnheight);
            }

            return new Rect(aRect.MargeSide + (aRect.MargeSide * 0.5f) + (aRect.MargeSide + m_BtnWidth) * (aIdPos), heightMarge, m_BtnWidth, aRect.Btnheight);
        }

        protected Rect GetBtnRect(int aIdPos, int aIdGroup, int aNbOfBtnInGroup, sToolDefaultRect aRect)
        {
            if (aIdGroup == 0)
            {
                return GetBtnPos(aIdPos, aIdGroup, aNbOfBtnInGroup, aRect);
            }
            int row = GetRowId(ref aIdPos, aNbOfBtnInGroup, aIdGroup, aRect);
            return GetBtnPos(aIdPos, row, aNbOfBtnInGroup, aRect);
        }

        private int GetRowId(ref int aIdPos, int aNbOfBtnInGroup, int aGroup, sToolDefaultRect aRect)
        {
            float width = position.width - (aRect.MargeSide * 2);
            float btnSize = aRect.MinBtnWidth + (aRect.MargeSide);
            while ((aIdPos + 1) * btnSize > width)
            {
                aIdPos -= Mathf.FloorToInt(width / btnSize);
                aGroup++;
            }
            return aGroup;
        }

        protected bool MouseOverBtn(Rect aBtn)
        {
            if (aBtn.Contains(Event.current.mousePosition))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Drawing Method

        protected void DrawTitleWithinRow(string aTitle, int aRow, sToolDefaultRect aRect)
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(10, GetBtnPos(0, aRow, 1, aRect).y, position.width - 20, 50), aTitle, m_TitleCenterStyle);
            GUI.color = Color.white;
        }

        protected void DrawBtnInfo(string aTitle, float aY, sToolDefaultRect aRect)
        {
            Rect baseRect = new Rect(aRect.MargeSide, aY + aRect.Btnheight + aRect.MargeHeight, position.width - aRect.MargeSide * 2f, 28);
            EditorGUI.DrawRect(baseRect, m_CompanyColor02);

            baseRect.x += aRect.MargeSide;
            EditorGUI.LabelField(baseRect, aTitle, m_TitleStyle);
            baseRect.x -= aRect.MargeSide;

            baseRect.y += baseRect.height;
            baseRect.height = 1;
            EditorGUI.DrawRect(baseRect, Color.white);

            baseRect.y += baseRect.height;
            baseRect.height = position.height - baseRect.height;
            m_CompanyColor01Fade = m_CompanyColor01;
            m_CompanyColor01Fade.a = 0.5f;
            EditorGUI.DrawRect(baseRect, m_CompanyColor01Fade);
        }

        protected void DrawToolTipBox(string aToolTipVO, string aToolTipVF, float aY, sToolDefaultRect aRect)
        {
            Vector2 vec = new Vector2();
            int amountOfLine = 0;
            Rect textBpxBack = new Rect();
            Rect textBoxSizeEN = new Rect();
            Rect textBoxSizeFR = new Rect();

            aToolTipVO = "<color=#FFFFFF>EN </color>" + aToolTipVO;
            aToolTipVF = "<color=#FFFFFF>FR </color>" + aToolTipVF;

            vec = m_HelpBoxStyle.CalcSize(new GUIContent(aToolTipVO));
            amountOfLine = Mathf.CeilToInt(vec.x / position.width);

            textBoxSizeEN = new Rect(10f, aY + aRect.Btnheight + aRect.MargeHeight + (aRect.Btnheight * 0.6f), position.width - 19, 19 * amountOfLine);

            vec = m_HelpBoxStyle.CalcSize(new GUIContent(aToolTipVF));
            amountOfLine = Mathf.CeilToInt(vec.x / position.width);

            textBoxSizeFR = new Rect(10f, textBoxSizeEN.yMax, position.width - 19, 19 * amountOfLine);


            textBpxBack = new Rect(aRect.MargeSide, textBoxSizeEN.yMin, position.width - aRect.MargeSide * 2, textBoxSizeEN.height + textBoxSizeFR.height + 8f);


            EditorGUI.DrawRect(textBpxBack, m_CompanyColor02);
            GUI.color = Color.white;
            EditorGUI.HelpBox(textBoxSizeEN, aToolTipVO, MessageType.None);
            EditorGUI.HelpBox(textBoxSizeFR, aToolTipVF, MessageType.None);
            GUI.color = Color.white;
            textBpxBack.y += textBpxBack.height;
            textBpxBack.height = 1;
            EditorGUI.DrawRect(textBpxBack, Color.white);
        }

        protected void DrawBackgroundMenu(int aRow, float aMinY, float aMaxY, sToolDefaultRect aRect)
        {
            if (aRow > 0)
            {
                aMinY -= aRect.MargeHeight;
            }
            aMinY -= aRect.MargeHeight;
            Rect backgroundRect = new Rect(aRect.MargeSide, aMinY, position.width - (aRect.MargeSide * 2f), aMaxY - aMinY + aRect.MargeHeight);
            EditorGUI.DrawRect(backgroundRect, m_CompanyColor02);
            backgroundRect.y += backgroundRect.height;
            backgroundRect.height = 2;
            EditorGUI.DrawRect(backgroundRect, Color.white);
        }

        protected void DrawButton(Rect aRect, Texture aTex, string aTitle, string aVETip, string aVFTip, System.Action aBtnCallBack, sToolDefaultRect aToolRect)
        {
            if (MouseOverBtn(aRect))
            {
                DrawBtnInfo(aTitle, aRect.y, aToolRect);
                if (SceneAutoLoader.IsUsingTooltip)
                {
                    DrawToolTipBox(aVETip, aVFTip, aRect.y, aToolRect);
                }
            }
            if (GUI.Button(aRect, aTex))
            {
                aBtnCallBack();
            }
        }

        protected void DrawButton(Rect aRect, string aText, string aTitle, string aVETip, string aVFTip, System.Action aBtnCallBack, sToolDefaultRect aToolRect)
        {
            if (MouseOverBtn(aRect))
            {
                DrawBtnInfo(aTitle, aRect.y, aToolRect);
                
                if (SceneAutoLoader.IsUsingTooltip)
                {
                    DrawToolTipBox(aVETip, aVFTip, aRect.y, aToolRect);
                }
            }
            if (GUI.Button(aRect, aText))
            {
                aBtnCallBack();
            }
        }

        private void DrawBanner()
        {
            GUI.color = m_CompanyColor02;
            EditorGUILayout.BeginVertical(GUI.skin.button);
            GUI.color = m_CompanyColor02;
            EditorGUILayout.BeginVertical(GUI.skin.label);
            GUI.color = m_CompanyColor01;
            EditorGUILayout.LabelField("Managers hub®", m_TitleCenterStyle);
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        protected void DrawManagerBtn(Rect aRect, eManagers aManager, Texture aIcon, bool aEnable, string aTitle, string aVETip, string aVFTip, System.Action aBtnCallBack, sToolDefaultRect aToolRect)
        {
            if (MouseOverBtn(aRect))
            {
                DrawBtnInfo(aTitle, aRect.y, aToolRect);
                
                if (SceneAutoLoader.IsUsingTooltip)
                {
                    DrawToolTipBox(aVETip, aVFTip, aRect.y, aToolRect);
                }
            }
            Color oldBack = GUI.backgroundColor;
            GUI.backgroundColor = ColorManager.GetColor(aManager, !aEnable);
            if(GUI.Button(aRect, aIcon))
            {
                aBtnCallBack();
            }
            GUI.backgroundColor = oldBack;
        }

        #endregion

    }

}

