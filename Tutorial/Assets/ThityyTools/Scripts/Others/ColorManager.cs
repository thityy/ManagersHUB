using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public static class ColorManager
    {
        private static Color32 CompanyColor = new Color32(35, 208, 185, 255);
        private static Color32 CompanyColorDark = new Color32(20, 121, 107, 255);
        private static Color32 CompanyColorBack = new Color32(31, 31, 31, 255);

        private static Color32 AudioColor = new Color32(35, 208, 185, 255);
        private static Color32 AudioColorDark = new Color32(20, 121, 107, 255);

        private static Color32 PoolColor = new Color32(208, 108, 35, 255);
        private static Color32 PoolColorDark = new Color32(150, 95, 30, 255);

        private static Color32 SceneColor = new Color32(208, 35, 58, 255);
        private static Color32 SceneColorDark = new Color32(94, 15, 26, 255);

        private static Color32 LanguageColor = new Color32(167, 208, 35, 255);
        private static Color32 LanguageColorDark = new Color32(122, 153, 26, 255);

        private static Color32 OptionColor = new Color32(141, 111, 225, 255);
        private static Color32 OptionColorDark = new Color32(103, 81, 165, 255);

        private static Color32 SpawnColor = new Color32(227, 123, 192, 255);
        private static Color32 SpawnColorDark = new Color32(166, 90, 141, 255);

        #region Color Changer

        public static Color32 GetCompagnieColor()
        {
            if(UnityEditor.EditorGUIUtility.isProSkin)
            {
                return CompanyColor;
            }
            else
            {
                return CompanyColorDark;
            }
        }

        public static Color32 GetCompagnieBackColor()
        {
            if(UnityEditor.EditorGUIUtility.isProSkin)
            {
                return Color.white;
            }
            else
            {
                return CompanyColorBack;
            }
        }

        public static Color32 GetColor(eManagers aManagerType, bool aDarkColor)
        {
            switch (aManagerType)
            {
                case eManagers.AudioManager:
                    {
                        if (aDarkColor)
                            return AudioColorDark;
                        else
                            return AudioColor;
                    }
                case eManagers.SceneManager:
                    {
                        if (aDarkColor)
                            return SceneColorDark;
                        else
                            return SceneColor;
                    }
                case eManagers.PoolManager:
                    {
                        if (aDarkColor)
                            return PoolColorDark;
                        else
                            return PoolColor;
                    }
                case eManagers.LanguageManager:
                {
                    if(aDarkColor)
                        return LanguageColorDark;
                    else
                        return LanguageColor;    
                }
                case eManagers.OptionManager:
                {
                    if(aDarkColor)
                        return OptionColorDark;
                    else
                        return OptionColor;
                }
                case eManagers.SpawnManager:
                {
                    if(aDarkColor)
                        return SpawnColorDark;
                    else
                        return SpawnColor;
                }
            }

            return Color.white;
        }

        public static  string GetColorRich(eManagers aManagerType, bool aDarkColor)
        {
            return ColorUtility.ToHtmlStringRGB(GetColor(aManagerType, aDarkColor));
        }

        public static  string GetColorRich()
        {
            return ColorUtility.ToHtmlStringRGB(GetCompagnieColor());
        }

        public static  string GetColorText(string aText, eManagers aManagerType, bool aDarkColor)
        {
            return "<color=#" + GetColorRich(aManagerType, aDarkColor) + ">" + aText + "</color>";
        }

        public static string GetCompagnieColorText(string aText)
        {
            return "<color=#" + GetColorRich() + ">" + aText + "</color>";
        }

        public static Color32 GetSpawnColor(eSpawnM_Teams aTeam)
        {
            return GetSpawnColor((int)aTeam);
        }

        public static Color32 GetSpawnColor(eSpawnM_Squads aSquad)
        {
            return GetSpawnColor((int)aSquad);
        }

        public static Color32 GetSpawnColor(int aId)
        {
            switch (aId)
            {
                case 0:
                    {
                        return new Color32(51, 153, 255, 255);
                    }
                case 1:
                    {
                        return new Color32(255, 51, 51, 255);
                    }
                case 2:
                    {
                        return new Color32(255, 153, 51, 255);
                    }
                case 3:
                    {
                        return new Color32(51, 255, 51, 255);
                    }
                case 4:
                    {
                        return new Color32(255, 255, 102, 255);
                    }
                case 5:
                    {
                        return new Color32(255, 51, 153, 255);
                    }
                case 6:
                    {
                        return new Color32(102, 255, 255, 255);
                    }
                case 7:
                    {
                        return new Color32(192, 192, 192, 255);
                    }
            }

            return ColorManager.GetColor(eManagers.SpawnManager, false);
        }


        #endregion
    }
}

