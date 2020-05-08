using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{

    #region Audio Manager Enums

    public enum eAudioM_GroundType
    {
        Concrete,
        Grass,
        Water,
        Gravel,
        Ground,
        Metal,
        Snow,
        Wood,
        Sand
    }

    public enum eAudioM_FootSoundType
    {
        Walk,
        Run,
        Crouch,
        Crawling
    }

    public enum eAudioM_FootSoundDir
    {
        Left,
        Right
    }

    public enum eAudioM_AudioSelector
    {
        AudioData,
        AudioClip
    }

    #endregion

    #region Scene Manager Enums

    public enum eSceneM_SceneCallType
    {
        Path,
        SceneData
    }

    public enum eSceneM_FadeDir
    {
        FadeIn,
        FadeOut
    }

    public enum eSceneM_Music 
    { 
        WithMusic, 
        WithoutMusic, 
        KeepOldMusic 
    }
    
    public enum eSceneM_LoadingScreen 
    { 
        Default, 
        Overwrite, 
        Fade 
    }

    public enum eSceneM_LoadingType 
    { 
        WithTip, 
        WithoutTip
    }

    public enum eSceneM_TipOption 
    { 
        Random,
        Select 
    }
    
    public enum eSceneM_TipGroup 
    { 
        GroupAlpha, 
        GroupBravo, 
        GroupCharlie, 
        GroupDelta, 
        GroupEcho, 
        GroupFoxtrot, 
        GroupGolf, 
        GroupHotel, 
        GroupIndia, 
        GroupJuliett, 
        AllGroup
    }

    public enum eSceneM_TextType 
    { 
        String, 
        TextData 
    }

    #endregion

    #region Language Manager Enums

    public enum eLanguageM_LanguageType
    {
        Text,
        Audio,
        Image,
    }

    public enum eLanguageM_Platform
    {
        Xbox360, XboxOne, XboxProjectX,
        Playstation3, Playstation4, Playstation5,
        Windows, Linux, IosComputer,
        Android, IosMobile,
        Switch,
        Stadia,
        Unknown,
    }

    #endregion

    #region Spawn Manager Enums

    public enum eSpawnM_Types
    {
        Spawn_OnTeam,
        Spawn_OnSquad,
        Spawn_Hostile,
        Spawn_Safe
    }

    public enum eSpawnM_Teams
    {
        Team01,
        Team02,
        Team03,
        Team04,
        Team05,
        Team06,
        Team07,
        Team08,
    }

    public enum eSpawnM_Squads
    {
        SquadAlpha,
        SquadBravo,
        SquadCharlie,
        SquadDelta,
        SquadEcho,
        SquadFoxtrot,
        SquadGolf,
        SquadHotel,
    }

    #endregion

    public static class EnumSystem
    {
        ///<summary> Return the count of any enum </summary>
        public static int GetEnumCount(System.Type aEnumType)
        {
            return System.Enum.GetValues(aEnumType).Length;
        }
    }
}

