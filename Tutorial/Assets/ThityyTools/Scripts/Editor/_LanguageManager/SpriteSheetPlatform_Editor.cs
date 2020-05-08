using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ManagersHUB
{

    [CustomEditor(typeof(SpriteSheetPlatformData))]
    public class SpriteSheetPlatform_Editor : BaseCustomEditor
    {

        private PlatformSpriteSheetDictionnary m_PlatformSpriteSheetDic;
        private SpriteSheetPlatformData m_Self;

        public override void OnInspectorGUI()
        {
            base.OnGUI();
            GetDatas();
            ShowGUI();
            UpdateProperty();
        }

        private void GetDatas()
        {
            m_Self = (SpriteSheetPlatformData)serializedObject.targetObject;
            m_PlatformSpriteSheetDic = m_Self.EDITOR_GetSpriteSheetDictionnary();
            m_Self.EDITOR_InitDic();
        }

        private void ShowGUI()
        {
            BeginInspector(eManagers.LanguageManager, "Multi-Platform Sprites_Sheet Data");
            ShowGroup("", ShowInsertPlatform);
            

            EditorGUILayout.EndVertical();
        }

        private void ShowInsertPlatform()
        {

            ShowAdvancedSubtitle("Insert Sprite Sheet");
            ShowAdvancedToolTip("Insert Sprite Sheet for each wanted platform. When you change platform In-Game, the sprite sheet in TMPro will change to.");
            DrawSeparator(true);
            eLanguageM_Platform currentPlatform;
            for(int i = 0; i < m_PlatformSpriteSheetDic.Count; i++)
            {
                currentPlatform = (eLanguageM_Platform)i;
                if(currentPlatform == eLanguageM_Platform.Unknown)
                {
                    DrawSeparatorDark(true);
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(currentPlatform.ToString(), m_LabelTxtStyleLeft, GUILayout.Width(150f));
                m_PlatformSpriteSheetDic[currentPlatform] = (TMPro.TMP_SpriteAsset)EditorGUILayout.ObjectField(m_PlatformSpriteSheetDic[currentPlatform], typeof(TMPro.TMP_SpriteAsset), false, GUILayout.Width(Screen.width * (1f / Screen.dpi * 96f) - 200f));
                EditorGUILayout.EndHorizontal();
                if(currentPlatform == eLanguageM_Platform.Unknown)
                {
                    ShowAdvancedLabelCenter("Return Unknow value if ask platform is null or empty");
                    DrawSeparatorDark(true);
                }
                
            }
        }
    }
}

