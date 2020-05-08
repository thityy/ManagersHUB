using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{

    [System.Serializable]
    public class PlatformSpriteSheetDictionnary : SerializableDictionary<eLanguageM_Platform, TMPro.TMP_SpriteAsset> { }

    [ExecuteInEditMode]
    [CreateAssetMenu(menuName = "ManagersHUB/LanguageManager/MultiPlatform_SpriteSheetData", fileName = "new SpriteSheetsData", order = 1)]
    public class SpriteSheetPlatformData : ScriptableObject
    {
        [SerializeField]
        private PlatformSpriteSheetDictionnary m_PlatformSpriteSheets = new PlatformSpriteSheetDictionnary();      //The dictionnary that contains multiple sprite sheet for each platform option


        public TMPro.TMP_SpriteAsset GetSpriteAsset(eLanguageM_Platform aPlatform)
        {
            return m_PlatformSpriteSheets[aPlatform];
        }

        public PlatformSpriteSheetDictionnary EDITOR_GetSpriteSheetDictionnary()
        {
            return m_PlatformSpriteSheets;
        }

        public void EDITOR_InitDic()
        {
            if(m_PlatformSpriteSheets.Count > 0)
            {
                return;
            }

            for(int i = 0; i < EnumSystem.GetEnumCount(typeof(eLanguageM_Platform)); i++)
            {
                m_PlatformSpriteSheets.Add((eLanguageM_Platform)i, null);
            }
        }
    }
}

