using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

namespace ManagersHUB
{

    public class ThityyToolsBar : BaseEditor
    {
        #region Enum
        private enum eSceneToStart { currentScene, normalScene }
        private enum eMenuToOpen { AppLauncher, Manager, Doc, Scene }
        #endregion

        #region Paths

        const string cIconsPath = "/ThityyTools/Scripts/Editor/Icons/";
        const string cDocPath = "/ThityyTools/Scripts/Editor/Doc/";

        #endregion

        #region Variable

        private eSceneToStart m_SceneToStart = eSceneToStart.currentScene;
        private eMenuToOpen m_MenuToOpen = eMenuToOpen.AppLauncher;
        private SceneAsset m_SceneAppLauncher;
        private GameObject m_LevelManagerPrefab;
        private GameObject m_PoolManagerPrefab;
        private GameObject m_AudioManagerPrefab;
        private GameObject m_LanguageManagerPrefab;
        private GameObject m_LoadingScreenObjPrefab;
        private GameObject m_SpawnPoint;
        private GameObject m_OptionManagerPrefab;

        [Header("Drawing Button Variable(s)")]
        private Texture m_DocTex;
        private Texture m_AppTex;
        private Texture m_SceneTex;
        private Texture m_ManagerTex;
        private Texture m_ToolTex;
        private Texture m_FolderTex;

        private Texture m_AudioManagerTex;
        private Texture m_AudioDataTex;
        private Texture m_MusicDataTex;
        private Texture m_FootDataTex;
        private Texture m_RadioDataTex;

        private Texture m_PoolManagerTex;
        private Texture m_PoolDataTex;

        private Texture m_LevelManagerTex;
        private Texture m_SceneDataTex;
        private Texture m_TipDataTex;
        private Texture m_LoadingScreenDataTex;
        private Texture m_TipGroupDataTex;

        private Texture m_LanguageManagerTex;
        private Texture m_TextDataTex;
        private Texture m_AvailableLanguageTex;
        private Texture m_TagTex;
        private Texture m_SpriteSheetTex;

        private Texture m_OptionManagerTex;
        private Texture m_OptionDataTex;

        private Texture m_SpawnManagerTex;
        private Texture m_SpawnPointTex;

        [Header("DataPrefab")]
        private Object m_AudioDataPrefab;
        private GameObject m_SceneDataPrefab;
        private GameObject m_TipDataPrefab;
        private GameObject m_OptionDataPrefab;

        private sToolDefaultRect m_ToolRect;

        [Header("Flashing Button Variable(s)")]
        private float m_CurrentTime = 0;
        private const float cTimeBetweenFlash = 0.25f;
        private bool m_IsInFlash = false;
        private Color m_ColorFlash;

        private System.Type m_AudioManagerType = null;
        private System.Type m_PoolManagerType = null;
        private System.Type m_LevelManagerType = null;
        private System.Type m_LanguageManagerType = null;

        #endregion

        #region Windows Functions

        [MenuItem("ManagersHUB/ManagersHUB")]
        public static void ShowWindows()
        {
            EditorWindow.GetWindow(typeof(ThityyToolsBar), !SceneAutoLoader.IsUsingFixeWindow, "Managers HUB", true).Close();
            EditorWindow.GetWindow(typeof(ThityyToolsBar), !SceneAutoLoader.IsUsingFixeWindow, "Managers HUB", true).ShowPopup();
        }

        public static void ChangeWindowsFixe()
        {
            SceneAutoLoader.IsUsingFixeWindow = true;
            ShowWindows();
        }

        public static void ChangeWindowsFloating()
        {
            SceneAutoLoader.IsUsingFixeWindow = false;
            ShowWindows();
        }

        public static void ActivateTooltip()
        {
            SceneAutoLoader.IsUsingTooltip = true;
        }

        public static void DesactivateTooltip()
        {
            SceneAutoLoader.IsUsingTooltip = false;
        }

        public static void ActivateEnglish()
        {
            SceneAutoLoader.IsUsingEnglish = true;
        }

        public static void ActivateFrench()
        {
            SceneAutoLoader.IsUsingEnglish = false;
        }

        #endregion

        #region Behaviour

        private void Awake()
        {
            if (SceneAutoLoader.MasterScene == "Master.unity")
            {
                SetDefaultAppLauncherScene();
            }
        }

        private void OnEnable()
        {
            m_ToolRect = SetDefaultToolRect(40f, 34f, 4f, 4f, 40, Mathf.Infinity);
            GetTex();
            GetPrefab();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            ShowGUI();
            CheckManagers();
            ShowSubmenu();
        }

        private void Update()
        {
            Repaint();
        }

        private void OnInspectorUpdate()
        {
            FlashColor(Color.red);
        }

        #endregion

        #region Set

        private void SetDefaultAppLauncherScene()
        {
            SetNewAppLauncherScene(m_SceneAppLauncher);
        }

        private void GetTex()
        {
            string path = Application.dataPath + cIconsPath;
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }

            m_DocTex = LoadTexture(path + "DocumentationIcon.png");
            m_AppTex = LoadTexture(path + "AppLauncherIcon.png");
            m_ToolTex = LoadTexture(path + "ToolIcon.png");
            m_SceneTex = LoadTexture(path + "SceneIcon.png");
            m_ManagerTex = LoadTexture(path + "ManagerIcon.png");
            m_FolderTex = LoadTexture(path + "FolderIcon.png");

            m_AudioManagerTex = LoadTexture(path + "AudioManagerIconPlus.png");
            m_AudioDataTex = LoadTexture(path + "AudioDataIcon.png");
            m_MusicDataTex = LoadTexture(path + "MusicDataIcon.png");
            m_FootDataTex = LoadTexture(path + "FootSoundDataIcon.png");
            m_RadioDataTex = LoadTexture(path + "RadioDataIcon.png");

            m_PoolManagerTex = LoadTexture(path + "PoolManagerIconPlus.png");
            m_PoolDataTex = LoadTexture(path + "PoolDataIcon.png");

            m_LevelManagerTex = LoadTexture(path + "LevelManagerIconPlus.png");
            m_SceneDataTex = LoadTexture(path + "SceneDataIcon.png");
            m_TipDataTex = LoadTexture(path + "TipDataIcon.png");
            m_LoadingScreenDataTex = LoadTexture(path + "LoadingScreenObjIcon.png");
            m_TipGroupDataTex = LoadTexture(path + "TipGroupDataIcon.png");

            m_LanguageManagerTex = LoadTexture(path + "LanguageManagerIconPlus.png");
            m_TextDataTex = LoadTexture(path + "TextDataIcon.png");
            m_AvailableLanguageTex = LoadTexture(path + "AvailableLanguageDataIcon.png");
            m_TagTex = LoadTexture(path + "TagDataIcon.png");
            m_SpriteSheetTex = LoadTexture(path + "SpriteSheetDataIcon.png");
            
            m_OptionManagerTex = LoadTexture(path + "OptionManagerIconPlus.png");
            m_OptionDataTex = LoadTexture(path + "OptionDataIcon.png");

            m_SpawnManagerTex = LoadTexture(path + "SpawnManagerIconPlus.png");
            m_SpawnPointTex = LoadTexture(path + "SpawnPointIcon.png");
        }

        private Texture LoadTexture(string aPath)
        {
            return (Texture)AssetDatabase.LoadAssetAtPath(aPath, typeof(Texture));
        }

        private void GetPrefab()
        {
            m_SceneAppLauncher = Resources.Load("Scene/AppLauncher") as SceneAsset;

            m_LevelManagerPrefab = Resources.Load("Prefabs/Manager_Scene") as GameObject;
            m_PoolManagerPrefab = Resources.Load("Prefabs/Manager_Pool") as GameObject;
            m_AudioManagerPrefab = Resources.Load("Prefabs/Manager_Audio") as GameObject;
            m_LanguageManagerPrefab = Resources.Load("Prefabs/Manager_Language") as GameObject;
            m_OptionManagerPrefab = Resources.Load("Prefabs/Manager_Option") as GameObject;

            m_SpawnPoint = Resources.Load("Prefabs/SpawnPoint") as GameObject;
            m_AudioDataPrefab = Resources.Load("Audios/AudioPrefab") as Object;
            m_LoadingScreenObjPrefab = Resources.Load("Prefabs/LoadingObj") as GameObject;

            m_AudioManagerType = typeof(AudioManager);
            m_PoolManagerType = typeof(PoolManager);
            m_LevelManagerType = typeof(SceneManager);
            m_LanguageManagerType = typeof(LanguageManager);
        }

        #endregion

        private void ShowGUI()
        {

            DrawMainButtons();

            switch (m_MenuToOpen)
            {
                case eMenuToOpen.Manager:
                    {
                        DrawManagerMenu();
                        break;
                    }
                case eMenuToOpen.Scene:
                    {
                        DrawSceneMenu();
                        break;
                    }
                case eMenuToOpen.AppLauncher:
                    {
                        DrawAppLauncher();
                        break;
                    }
                case eMenuToOpen.Doc:
                    {
                        DrawDoc();
                        break;
                    }
            }


            DrawButtonsInfos();
            if (SceneAutoLoader.IsUsingTooltip)
            {
                DrawTooltips();
            }

        }

        private void ShowSubmenu()
        {
            if (Event.current.type == EventType.ContextClick)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Set new AppLauncher Scene"), false, ChangeAppLauncherScene);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Window/ Open as Floating Window"), !SceneAutoLoader.IsUsingFixeWindow, ChangeWindowsFloating);
                menu.AddItem(new GUIContent("Window/ Open as Dockable Window"), SceneAutoLoader.IsUsingFixeWindow, ChangeWindowsFixe);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Tooltip/ Enable"), SceneAutoLoader.IsUsingTooltip, ActivateTooltip);
                menu.AddItem(new GUIContent("Tooltip/ Disable"), !SceneAutoLoader.IsUsingTooltip, DesactivateTooltip);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Language/ English"), SceneAutoLoader.IsUsingEnglish, ActivateEnglish);
                menu.AddItem(new GUIContent("Language/ Français"), !SceneAutoLoader.IsUsingEnglish, ActivateFrench);
                menu.ShowAsContext();
            }
        }

        #region Draw Function(s)

        ///<summary> Draw all mains tools buttons </summary>
        private void DrawMainButtons()
        {
            DrawBackgroundMenu(0, GetBtnRect(0, 0, 4, m_ToolRect).y, GetBtnRect(4, 0, 4, m_ToolRect).yMax, m_ToolRect);

            //Tooltip Button
            GUI.color = Color.white;
            EditorGUI.BeginDisabledGroup(SceneAutoLoader.IsUsingTooltip);
            GUI.color = m_CompanyColor01;
            if (GUI.Button(new Rect(position.width - 55, 5, 48, 18), "<color=#FFFFFF>Info</color>", m_BtnStyle))
            {
                ActivateTooltip();
            }
            EditorGUI.EndDisabledGroup();
            if (MouseOverBtn(new Rect(position.width - 55, 5, 48, 18)) && Event.current.type == EventType.MouseUp && SceneAutoLoader.IsUsingTooltip)
            {
                DesactivateTooltip();
            }

            // AppLauncher Button
            if (IsInMenu(eMenuToOpen.Manager) && !IsInAppLauncher())
            {
                GUI.color = Color.red;
            }
            else
            {
                GUI.color = Color.white;
            }
            if (GUI.Button(GetBtnRect(0, 0, 4, m_ToolRect), new GUIContent(m_AppTex)))
            {
                if (IsInAppLauncher() && SceneAutoLoader.OldScenePath != "" && EditorSceneManager.GetActiveScene().path != SceneAutoLoader.OldScenePath)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        try
                        {
                            EditorSceneManager.OpenScene(SceneAutoLoader.OldScenePath);
                        }
                        catch
                        {
                            Debug.LogError(string.Format("error: can't open the previous scene", SceneAutoLoader.OldScenePath));
                        }
                    }
                }
                else if (!IsInAppLauncher())
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        SceneAutoLoader.OldScenePath = EditorSceneManager.GetActiveScene().path;
                        try
                        {
                            EditorSceneManager.OpenScene(SceneAutoLoader.MasterScene);
                        }
                        catch
                        {
                            Debug.LogError(string.Format("error: AppLauncher can't be found {0}", SceneAutoLoader.MasterScene));
                            this.ShowNotification(new GUIContent("AppLauncher can't be find!"));
                            ChangeAppLauncherScene();
                        }
                    }
                }
            }
            GUI.color = Color.white;

            // SceneChoice Button
            EditorGUI.BeginDisabledGroup(IsInMenu(eMenuToOpen.Scene));
            if (GUI.Button(GetBtnRect(1, 0, 4, m_ToolRect), new GUIContent(m_SceneTex)))
            {
                ChangeMenu(eMenuToOpen.Scene);
            }
            EditorGUI.EndDisabledGroup();
            if (IsInMenu(eMenuToOpen.Scene))
            {
                if (MouseOverBtn(GetBtnRect(1, 0, 4, m_ToolRect)) && Event.current.type == EventType.MouseUp)
                {
                    ChangeMenu(eMenuToOpen.AppLauncher);
                }
            }

            // Documentation Button
            if (GUI.Button(GetBtnRect(2, 0, 4, m_ToolRect), new GUIContent(m_DocTex)))
            {
                Application.OpenURL(Application.dataPath + cDocPath + "Documentation_ManagersHUB.pdf");
            }

            // ManagerCreator Button
            EditorGUI.BeginDisabledGroup(IsInMenu(eMenuToOpen.Manager));
            if (GUI.Button(GetBtnRect(3, 0, 4, m_ToolRect), new GUIContent(m_ManagerTex)))
            {
                if (SceneAutoLoader.WantHelpManager && !IsInAppLauncher())
                {
                    if (EditorUtility.DisplayDialog("HELP", "You need to be in the AppLauncher to create Manager(s)!", "ok", "Don't tell me again"))
                    {
                        SceneAutoLoader.WantHelpManager = true;
                    }
                    else
                    {
                        SceneAutoLoader.WantHelpManager = false;
                    }
                }
                ChangeMenu(eMenuToOpen.Manager);
            }
            EditorGUI.EndDisabledGroup();
            if (IsInMenu(eMenuToOpen.Manager))
            {
                if (MouseOverBtn(GetBtnRect(3, 0, 4, m_ToolRect)) && Event.current.type == EventType.MouseDown)
                {
                    ChangeMenu(eMenuToOpen.AppLauncher);
                }
            }

            /*
            if(GUI.Button(new Rect(50, 50, 50, 50), "Spawn"))
            {
                SpawnPoint obj = Instantiate(m_SpawnPoint, Vector3.zero, Quaternion.identity).GetComponent<SpawnPoint>();
                obj.m_VisualEditor.ChangeTeamColor(Color.yellow);
                obj.m_VisualEditor.ChangePlayerColor(Color.cyan);
            }
            */
        }

        ///<summary> Draw all the tooltip for each button </summary>
        private void DrawTooltips()
        {
            Rect checkRect;

            checkRect = GetBtnRect(0, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawToolTipBox(
                "Open the 'AppLauncher' scene. This scene let you create and/or edit your manager(s). If the scene 'AppLauncher' is already open, will load previous scene (if exist).",
                "Ouvre la scène 'AppLauncher'. Cette scène vous permet de créer et/ou modifier vos manager(s). Si la scène 'AppLauncher' est déjà ouverte, va charger la scène précédente (si elle existe).",
                checkRect.y, m_ToolRect);
            }

            checkRect = GetBtnRect(1, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawToolTipBox(
                "Let you choose if you want to load the game normaly or test the current scene with all your manager(s). <i>(only in editor)</i>",
                "Vous laisse choisir si vous désirez charger votre jeu normalement ou tester votre scène actuelle avec tout vos manager(s). <i>(seulement dans l'éditor)</i>",
                checkRect.y, m_ToolRect);
            }

            checkRect = GetBtnRect(2, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawToolTipBox(
                "Open the documentation for this tool.",
                "Ouvre la documentation lié à cet outil.",
                checkRect.y, m_ToolRect);
            }

            checkRect = GetBtnRect(3, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawToolTipBox(
                "Let you create managers for your game.",
                "Vous laisse créer des managers pour ton jeu.",
                checkRect.y, m_ToolRect);
            }
        }

        private void DrawButtonsInfos()
        {
            Rect checkRect;

            checkRect = GetBtnRect(0, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                if (IsInAppLauncher())
                {
                    DrawBtnInfo("App Launcher <color=#23D0B9><size=10>[ Active Scene ]</size></color>", checkRect.y, m_ToolRect);
                }
                else
                {
                    DrawBtnInfo("App Launcher", checkRect.y, m_ToolRect);
                }
            }

            checkRect = GetBtnRect(1, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                if (SceneAutoLoader.TestCurrentScene)
                {
                    DrawBtnInfo("Scene Selection <color=#23D0B9><size=10>[ Test Current Scene ]</size></color>", checkRect.y, m_ToolRect);
                }
                else
                {
                    DrawBtnInfo("Scene Selection <color=#23D0B9><size=10>[ Normal Start-up ]</size></color>", checkRect.y, m_ToolRect);
                }
            }

            checkRect = GetBtnRect(2, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawBtnInfo("Documentation", checkRect.y, m_ToolRect);
            }

            checkRect = GetBtnRect(3, 0, 4, m_ToolRect);
            if (MouseOverBtn(checkRect))
            {
                DrawBtnInfo("Manager(s) Creator", checkRect.y, m_ToolRect);
            }
        }

        #region ManagerMenu

        private void DrawManagerMenu()
        {
            /*
            ###################
            new button need to be add at the top (in reverse) so it get the tooltip get over other button correctly
            ###################
            */
            int nbOfBtn = 22;
            int btnId = nbOfBtn - 1;
            string disableTxt = "";
            string createdTxt = "";

            DrawBackgroundMenu(3, GetBtnRect(0, 1, nbOfBtn, m_ToolRect).y, GetBtnRect(nbOfBtn - 1, 2, nbOfBtn, m_ToolRect).yMax, m_ToolRect);
            DrawTitleWithinRow("Manager(s) Creator", 1, m_ToolRect);
            //-------------------------------------------

            #region SPAWN MANAGER BTN [TO ADD IN FUTURE]
            /*
            if (!SceneAutoLoader.HaveSpawnManager)
            {
                disableTxt = ManagerColors.GetColorText(" [ NEED SPAWN MANAGER ]", eManagers.SpawnManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ManagerColors.GetColorText(" [ Already Exist ]", eManagers.SpawnManager, false);
            }
            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HaveSpawnManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SpawnManager,
                m_SpawnPointTex,
                SceneAutoLoader.HaveSpawnManager,
                "Spawn Point +" + disableTxt,
                "Create a new SpawnPoint.",
                "Crée un nouveau SpawnPoint.",
                CreateOptionData,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HaveSpawnManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SpawnManager,
                m_SpawnManagerTex,
                SceneAutoLoader.HaveSpawnManager,
                "Spawn Manager +" + createdTxt,
                "Create a SpawnManager.",
                "Crée un SpawnManager.",
                CreateOptionManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            */
            #endregion

            //-------------------------------------------

            #region OPTION MANAGER BTN [TO ADD IN FUTURE]

            /*

            if (!SceneAutoLoader.HaveLanguageManager)
            {
                disableTxt = ManagerColors.GetColorText(" [ NEED OPTION MANAGER ]", eManagers.OptionManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ManagerColors.GetColorText(" [ Already Exist ]", eManagers.OptionManager, false);
            }

            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HaveOptionManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.OptionManager,
                m_OptionDataTex,
                SceneAutoLoader.HaveOptionManager,
                "Option Data +" + disableTxt,
                "Create a new OptionData.",
                "Crée un nouveau OptionData.",
                CreateOptionData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.OptionManager,
                m_FolderTex,
                SceneAutoLoader.HaveOptionManager,
                "Folder " + ManagerColors.GetColorText("[ Option Manager ]", eManagers.OptionManager, false) + disableTxt,
                "Open the option manager folder where the data is save.",
                "Ouvre le fichier contenant les datas de l'option manager.",
                OpenOptionFolder,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HaveOptionManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.OptionManager,
                m_OptionManagerTex,
                SceneAutoLoader.HaveOptionManager,
                "Option Manager +" + createdTxt,
                "Create an OptionManager.",
                "Crée un OptionManager.",
                CreateOptionManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            */
            #endregion

            //-------------------------------------------

            #region LANGUAGE MANAGER BTN

            if (!SceneAutoLoader.HaveLanguageManager)
            {
                disableTxt = ColorManager.GetColorText(" [ NEED LANGUAGE MANAGER ]", eManagers.LanguageManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ColorManager.GetColorText(" [ Already Exist ]", eManagers.LanguageManager, false);
            }

            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HaveLanguageManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_TagTex,
                SceneAutoLoader.HaveLanguageManager,
                "Multi-Platform Tag Data +" + disableTxt,
                "Create a new Multi-Platform Tag Data.",
                "Crée un nouveau Multi-Platform Tag Data.",
                CreateTagData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_SpriteSheetTex,
                SceneAutoLoader.HaveLanguageManager,
                "Multi-Platform Spritesheet Data +" + disableTxt,
                "Create a new Multi-Platform Spritesheet Data.",
                "Crée un nouveau Multi-Platform Spritesheet Data.",
                CreateSpriteSheetData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_AvailableLanguageTex,
                SceneAutoLoader.HaveLanguageManager,
                "Available Language Data +" + disableTxt,
                "Create a new AvailableLanguage Data.",
                "Crée un nouveau AvailableLanguage Data.",
                CreateAvailableLanguageData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_TextDataTex,
                SceneAutoLoader.HaveLanguageManager,
                "Text Data +" + disableTxt,
                "Create a new TextData.",
                "Crée un nouveau TextData.",
                CreateTextData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_FolderTex,
                SceneAutoLoader.HaveLanguageManager,
                "Folder " + ColorManager.GetColorText("[ Language Manager ]", eManagers.LanguageManager, false)+ disableTxt,
                "Open the language manager folder where the data is save.",
                "Ouvre le fichier contenant les datas du language manager.",
                OpenLanguageFolder,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HaveLanguageManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.LanguageManager,
                m_LanguageManagerTex,
                !SceneAutoLoader.HaveLanguageManager,
                "Language Manager +" + createdTxt,
                "Create a LanguageManager.",
                "Crée un LanguageManager.",
                CreateLanguageManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            #endregion

            //-------------------------------------------

            #region SCENE MANAGER BTN

            if (!SceneAutoLoader.HaveLevelManager)
            {
                disableTxt = ColorManager.GetColorText(" [ NEED SCENE MANAGER ]", eManagers.SceneManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ColorManager.GetColorText(" [ Already Exist ]", eManagers.SceneManager, false);
            }

            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HaveLevelManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_LoadingScreenDataTex,
                SceneAutoLoader.HaveLevelManager,
                "Loading Screen Prefab +" + disableTxt,
                "Create a new Loading Screen prefab that can be modify and use within the overwrite option of the SceneData Loading Setting(s).",
                "Crée un nouveau prefab 'Loading Screen' qui peut être modifié et utiliser avec l'option 'overwrite' dans un SceneData.",
                CreateLoadingObj,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_LoadingScreenDataTex,
                SceneAutoLoader.HaveLevelManager,
                "Loading Screen Data +" + disableTxt,
                "Create a new Loading Screen Data.",
                "Crée un nouveau Loading Screen Data.",
                CreateLoadingScreenData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_TipGroupDataTex,
                SceneAutoLoader.HaveLevelManager,
                "Group Tips Data +" + disableTxt,
                "Create a new Group Tips Data.",
                "Crée un nouveau Group Tips Data.",
                CreateGroupTipData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_TipDataTex,
                SceneAutoLoader.HaveLevelManager,
                "Tip Data +" + disableTxt,
                "Create a new TipLoadingData.",
                "Crée un nouveau TipLoadingData.",
                CreateTipData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_SceneDataTex,
                SceneAutoLoader.HaveLevelManager,
                "Scene Data +" + disableTxt,
                "Create a new SceneData.",
                "Crée un nouveau SceneData.",
                CreateSceneData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_FolderTex,
                SceneAutoLoader.HaveLevelManager,
                "Folder " + ColorManager.GetColorText("[ Scene Manager ]", eManagers.SceneManager, false)+ disableTxt,
                "Open the scene manager folder where the data is save.",
                "Ouvre le fichier contenant les datas du scene manager.",
                OpenSceneFolder,
                m_ToolRect
            );
            btnId--;

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HaveLevelManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.SceneManager,
                m_LevelManagerTex,
                !SceneAutoLoader.HaveLevelManager,
                "Level Manager +" + createdTxt,
                "Create a LevelManager.",
                "Crée un LevelManager.",
                CreateSceneManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            #endregion

            //-------------------------------------------

            #region POOL MANAGER BTN

            if (!SceneAutoLoader.HavePoolManager)
            {
                disableTxt = ColorManager.GetColorText(" [ NEED POOL MANAGER ]", eManagers.PoolManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ColorManager.GetColorText(" [ Already Exist ]", eManagers.PoolManager, false);
            }

            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HavePoolManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.PoolManager,
                m_PoolDataTex,
                SceneAutoLoader.HavePoolManager,
                "Pool Data +" + disableTxt,
                "Create a new PoolData.",
                "Crée un nouveau PoolData.",
                CreatePoolData,
                m_ToolRect
            );
            btnId--;

            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.PoolManager,
                m_FolderTex,
                SceneAutoLoader.HavePoolManager,
                "Folder <color=#d08223><size=11>[ Pool Manager ]</size></color>" + disableTxt,
                "Open the pool manager folder where the data is save.",
                "Ouvre le fichier contenant les datas du pool manager.",
                OpenPoolFolder,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HavePoolManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.PoolManager,
                m_PoolManagerTex,
                !SceneAutoLoader.HavePoolManager,
                "Pool Manager +" + createdTxt,
                "Create a PoolManager.",
                "Crée un PoolManager.",
                CreatePoolManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            #endregion

            //-------------------------------------------

            #region AUDIO MANAGER BTN

            if (!SceneAutoLoader.HaveAudioManager)
            {
                disableTxt = ColorManager.GetColorText(" [ NEED AUDIO MANAGER ]", eManagers.AudioManager, false);
                createdTxt = "";
            }
            else
            {
                disableTxt = "";
                createdTxt = ColorManager.GetColorText(" [ Already Exist ]", eManagers.AudioManager, false);
            }

            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.HaveAudioManager);
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_RadioDataTex,
                SceneAutoLoader.HaveAudioManager,
                "Radio Data +" + disableTxt,
                "Create a new RadioData.",
                "Crée un nouveau RadioData.",
                CreateRadioData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_FootDataTex,
                SceneAutoLoader.HaveAudioManager,
                "FootSound Data +" + disableTxt,
                "Create a new FootSoundData.",
                "Crée un nouveau FootSoundData.",
                CreateFootData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_MusicDataTex,
                SceneAutoLoader.HaveAudioManager,
                "Music Data +" + disableTxt,
                "Create a new MusicData.",
                "Crée un nouveau MusicData.",
                CreateMusicData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_AudioDataTex,
                SceneAutoLoader.HaveAudioManager,
                "Audio Data +" + disableTxt,
                "Create a new AudioData.",
                "Crée un nouveau AudioData.",
                CreateAudioData,
                m_ToolRect
            );
            btnId--;
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_FolderTex,
                SceneAutoLoader.HaveAudioManager,
                "Folder <color=#23d0b9><size=11>[ Audio Manager ]</size></color>" + disableTxt,
                "Open the audio manager folder where the data are save.",
                "Ouvre le fichier contenant les datas de l'audio manager.",
                OpenAudioFolder,
                m_ToolRect
            );
            btnId--;

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(SceneAutoLoader.HaveAudioManager || !IsInAppLauncher());
            DrawManagerBtn(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                eManagers.AudioManager,
                m_AudioManagerTex,
                !SceneAutoLoader.HaveAudioManager,
                "Audio Manager +" + createdTxt,
                "Create an AudioManager.",
                "Crée un AudioManager.",
                CreateAudioManager,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();

            #endregion
        }

        #region AUDIO MANAGER

        private void CreateAudioManager()
        {
            GameObject obj = Instantiate(m_AudioManagerPrefab);
            obj.name = obj.name.Replace("(Clone)", "");
            EditorSceneManager.MarkAllScenesDirty();
            CreateDirectoryWithPath(GetPath("AudioManager/AudioDatas"));
            CreateDirectoryWithPath(GetPath("AudioManager/MusicDatas"));
            CreateDirectoryWithPath(GetPath("AudioManager/FootSoundDatas"));
            CreateDirectoryWithPath(GetPath("AudioManager/RadioDatas"));
            this.ShowNotification(new GUIContent("AudioManager create."));
        }

        private void CreateAudioData()
        {
            AudioData obj = ScriptableObject.CreateInstance<AudioData>();
            string path = Path.Combine(GetPath("AudioManager/AudioDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateMusicData()
        {
            MusicData obj = ScriptableObject.CreateInstance<MusicData>();
            string path = Path.Combine(GetPath("AudioManager/MusicDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateFootData()
        {
            FootSoundData obj = ScriptableObject.CreateInstance<FootSoundData>();
            string path = Path.Combine(GetPath("AudioManager/FootSoundDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateRadioData()
        {
            RadioData obj = ScriptableObject.CreateInstance<RadioData>();
            string path = Path.Combine(GetPath("AudioManager/RadioDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void OpenAudioFolder()
        {
            GetDirectoryWithPath("AudioManager");
        }

        #endregion

        #region POOL MANAGER

        private void CreatePoolManager()
        {
            GameObject obj = Instantiate(m_PoolManagerPrefab);
            obj.name = obj.name.Replace("(Clone)", "");
            EditorSceneManager.MarkAllScenesDirty();
            CreateDirectoryWithPath(GetPath("PoolManager/PoolDatas"));
            this.ShowNotification(new GUIContent("PoolManager create."));
        }

        private void CreatePoolData()
        {
            PoolData obj = ScriptableObject.CreateInstance<PoolData>();
            string path = Path.Combine(GetPath("PoolManager/PoolDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void OpenPoolFolder()
        {
            GetDirectoryWithPath("PoolManager");
        }

        #endregion

        #region SCENE MANAGER

        private void CreateSceneManager()
        {
            GameObject obj = Instantiate(m_LevelManagerPrefab);
            obj.name = obj.name.Replace("(Clone)", "");
            EditorSceneManager.MarkAllScenesDirty();

            CreateDirectoryWithPath(GetPath("SceneManager/TipDatas"));
            CreateDirectoryWithPath(GetPath("SceneManager/SceneDatas"));
            CreateDirectoryWithPath(GetPath("SceneManager/LoadingScreenDatas"));
            CreateDirectoryWithPath(GetPath("SceneManager/LoadingScreenObjs"));
            CreateDirectoryWithPath(GetPath("SceneManager/GroupTipsDatas"));
            this.ShowNotification(new GUIContent("SceneManager create."));
        }

        private void CreateSceneData()
        {
            SceneData obj = ScriptableObject.CreateInstance<SceneData>();
            string path = Path.Combine(GetPath("SceneManager/SceneDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateTipData()
        {
            LoadingTipData obj = ScriptableObject.CreateInstance<LoadingTipData>();
            string path = Path.Combine(GetPath("SceneManager/TipDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateLoadingScreenData()
        {
            LoadingScreenData obj = ScriptableObject.CreateInstance<LoadingScreenData>();
            string path = Path.Combine(GetPath("SceneManager/LoadingScreenDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateGroupTipData()
        {
            GroupTipsData obj = ScriptableObject.CreateInstance<GroupTipsData>();
            string path = Path.Combine(GetPath("SceneManager/GroupTipsDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateLoadingObj()
        {
            string path = GetPath("SceneManager/LoadingScreenObjs");
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                bool success;
                Object root = PrefabUtility.InstantiatePrefab(m_LoadingScreenObjPrefab);
                path = Path.Combine(GetPathPrefab("SceneManager/LoadingScreenObjs", root));
                Object obj = PrefabUtility.SaveAsPrefabAsset((GameObject)root, path, out success);
                if (success)
                {
                    this.ShowNotification(new GUIContent("Loading Screen Copy created."));
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = obj;
                }
                else
                {
                    this.ShowNotification(new GUIContent("Operation can't be complete."));
                    Debug.LogWarning("The prefab can't be create!");
                }
                DestroyImmediate(root);
            }
        }

        private void OpenSceneFolder()
        {
            GetDirectoryWithPath("SceneManager");
        }

        #endregion

        #region LANGUAGE MANAGER

        private void CreateLanguageManager()
        {
            GameObject obj = Instantiate(m_LanguageManagerPrefab);
            obj.name = obj.name.Replace("(Clone)", "");
            EditorSceneManager.MarkAllScenesDirty();
            CreateDirectoryWithPath(GetPath("LanguageManager/TextDatas"));
            CreateDirectoryWithPath(GetPath("LanguageManager/AvailableLanguageDatas"));
            CreateDirectoryWithPath(GetPath("LanguageManager/MultiPlatform_SpriteSheetDatas"));
            CreateDirectoryWithPath(GetPath("LanguageManager/MultiPlatform_TagDatas"));
            this.ShowNotification(new GUIContent("LanguageManager create."));
        }

        private void CreateTextData()
        {
            MultiLanguageData obj = ScriptableObject.CreateInstance<MultiLanguageData>();
            string path = Path.Combine(GetPath("LanguageManager/TextDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateAvailableLanguageData()
        {
            AvailableLanguageData obj = ScriptableObject.CreateInstance<AvailableLanguageData>();
            string path = Path.Combine(GetPath("LanguageManager/AvailableLanguageDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateSpriteSheetData()
        {
            SpriteSheetPlatformData obj = ScriptableObject.CreateInstance<SpriteSheetPlatformData>();
            string path = Path.Combine(GetPath("LanguageManager/MultiPlatform_SpriteSheetDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void CreateTagData()
        {
            TagPlatformData obj = ScriptableObject.CreateInstance<TagPlatformData>();
            string path = Path.Combine(GetPath("LanguageManager/MultiPlatform_TagDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void OpenLanguageFolder()
        {
            GetDirectoryWithPath("LanguageManager");
        }

        #endregion

        #region OPTION MANAGER

        /*

        private void CreateOptionManager()
        {
            GameObject obj = Instantiate(m_OptionManagerPrefab);
            obj.name = obj.name.Replace("(Clone)", "");
            EditorSceneManager.MarkAllScenesDirty();
            CreateDirectoryWithPath(GetPath("OptionManager/OptionDatas"));
            this.ShowNotification(new GUIContent("OptionManager create."));
        }

        private void CreateOptionData()
        {
            OptionData obj = ScriptableObject.CreateInstance<OptionData>();
            string path = Path.Combine(GetPath("OptionManager/OptionDatas", obj));
            if (path == "")
            {
                this.ShowNotification(new GUIContent("Operation can't be complete."));
            }
            else
            {
                CreateNewObject(obj, path);
            }
        }

        private void OpenOptionFolder()
        {
            GetDirectoryWithPath("OptionManager");
        }

        */

        #endregion

        #endregion

        private void DrawSceneMenu()
        {
            int nbOfBtn = 2;
            int btnId = nbOfBtn - 1;
            string disableTxt = "";
            string createdTxt = "";

            //-------------------------------------------

            DrawBackgroundMenu(2, GetBtnRect(0, 1, nbOfBtn, m_ToolRect).y, GetBtnRect(nbOfBtn - 1, 2, nbOfBtn, m_ToolRect).yMax, m_ToolRect);
            DrawTitleWithinRow("Scene Selection", 1, m_ToolRect);

            if (IsInAppLauncher())
            {
                disableTxt = "<color=#d0233a><size=11>[ DISABLE in APP Launcher ]</size></color>";
                createdTxt = "";
            }
            else if (SceneAutoLoader.TestCurrentScene)
            {
                disableTxt = "";
                createdTxt = "<color=#23d0b9><size=11>[ selected ]</size></color>";
            }
            else
            {
                disableTxt = "";
                createdTxt = "";
            }
            EditorGUI.BeginDisabledGroup(SceneAutoLoader.TestCurrentScene || IsInAppLauncher());
            DrawButton(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                "Test Current Scene",
                createdTxt + disableTxt,
                "Unity will bypass the default scene give in the Launcher component, will load the AppLauncher Scene to get all managers and then load the scene that was open when you press Play. [ Will be bypass if you press play in the AppLauncher scene ]",
                "Unity va contourner la scène par défaut donné au Launcher, va charger l'AppLauncher pour implémenter tout les managers et va ensuite charger la scene qui était ouverte lorsque vous avez appuyez sur Jouer. [Ne fonctionneras pas si vous appuyez sur jouer dans la scène AppLauncher ]",
                TestCurrentScene,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();


            if (!SceneAutoLoader.TestCurrentScene)
            {
                disableTxt = "";
                createdTxt = "<color=#23d0b9><size=11>[ selected ]</size></color>";
            }
            else
            {
                disableTxt = "";
                createdTxt = "";
            }
            EditorGUI.BeginDisabledGroup(!SceneAutoLoader.TestCurrentScene);
            DrawButton(
                GetBtnRect(btnId, 2, nbOfBtn, m_ToolRect),
                "Normal start-up",
                createdTxt + disableTxt,
                "The game will start normally by loading the default scene give in the Launcher component.",
                "Le jeu va démarrer normalement en chargent la scène par défaut donné au 'Launcher' en component.",
                TestNormalScene,
                m_ToolRect
            );
            btnId--;
            EditorGUI.EndDisabledGroup();
        }

        private void TestCurrentScene()
        {
            SceneAutoLoader.TestCurrentScene = true;
        }

        private void TestNormalScene()
        {
            SceneAutoLoader.TestCurrentScene = false;
        }

        private void DrawAppLauncher()
        {
            switch (m_SceneToStart)
            {
                case eSceneToStart.currentScene:
                    {
                        break;
                    }
                case eSceneToStart.normalScene:
                    {
                        break;
                    }
            }
        }

        private void DrawDoc()
        {

        }

        #endregion

        #region Utils


        private void CreateNewObject(UnityEngine.Object aObject, string aPath)
        {
            this.ShowNotification(new GUIContent(aObject.name + " created."));
            AssetDatabase.CreateAsset(aObject, aPath);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = aObject;
        } 

        private void CheckManagers()
        {
            if (IsInAppLauncher())
            {
                SceneAutoLoader.HaveAudioManager = HaveManager(m_AudioManagerType);
                SceneAutoLoader.HavePoolManager = HaveManager(m_PoolManagerType);
                SceneAutoLoader.HaveLevelManager = HaveManager(m_LevelManagerType);
                SceneAutoLoader.HaveLanguageManager = HaveManager(m_LanguageManagerType);
                //SceneAutoLoader.HaveOptionManager = HaveManager(m_OptionManagerType);
            }
        }

        private void ChangeAppLauncherScene()
        {
            string[] filter = new string[] { "Unity scene file", "unity" };
            string objPath = EditorUtility.OpenFilePanelWithFilters("Get the new AppLauncher scene", Application.dataPath, filter);
            if (objPath.IndexOf("Assets/") > 0)
            {
                if (objPath.Substring(objPath.IndexOf("Assets/")) != "")
                {
                    string relativePath = objPath.Substring(objPath.IndexOf("Assets/"));
                    SetNewAppLauncherScene(relativePath);
                }
            }
        }

        private void SetNewAppLauncherScene(string aPath)
        {
            //Check if appLauncher is already set in
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].path == aPath)
                {
                    if (aPath != EditorSceneManager.GetActiveScene().path)
                    {
                        SceneAutoLoader.OldScenePath = EditorSceneManager.GetActiveScene().path;
                    }

                    SceneAutoLoader.SelectMasterScene(aPath);
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(SceneAutoLoader.MasterScene);
                    }
                    return;
                }
            }

            //Add in the build setting if not in
            List<EditorBuildSettingsScene> scenesValue = new List<EditorBuildSettingsScene>();
            scenesValue.Add(new EditorBuildSettingsScene(aPath, true));
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenesValue.Add(EditorBuildSettings.scenes[i]);
            }
            EditorBuildSettings.scenes = scenesValue.ToArray();

            if (aPath != EditorSceneManager.GetActiveScene().path)
            {
                SceneAutoLoader.OldScenePath = EditorSceneManager.GetActiveScene().path;
            }
            SceneAutoLoader.SelectMasterScene(aPath);
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(SceneAutoLoader.MasterScene);
            }
        }


        private void SetNewAppLauncherScene(SceneAsset aScene)
        {
            string path = AssetDatabase.GetAssetPath(aScene);
            SetNewAppLauncherScene(path);
        }

        private void FlashColor(Color aColor)
        {
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime > cTimeBetweenFlash)
            {
                if (!m_IsInFlash)
                {
                    m_IsInFlash = true;
                    m_ColorFlash = aColor;
                }
                else
                {
                    m_IsInFlash = false;
                    m_ColorFlash = Color.white;
                }

                m_CurrentTime = 0;
            }
        }

        private bool IsInMenu(eMenuToOpen aMenu)
        {
            if (aMenu == m_MenuToOpen)
            {
                return true;
            }
            return false;
        }

        private bool IsInAppLauncher()
        {
            if (EditorSceneManager.GetActiveScene().path == SceneAutoLoader.MasterScene)
            {
                return true;
            }
            return false;
        }

        private void ChangeMenu(eMenuToOpen aMenu)
        {
            m_MenuToOpen = aMenu;
        }

        private bool HaveManager(System.Type aManagerComponent)
        {
            Object[] obj = GameObject.FindObjectsOfType(aManagerComponent);
            if (obj.Length > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

    }


    #region SceneLoader

    [InitializeOnLoad]
    public static class SceneAutoLoader
    {

        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        ///<summary> Let you set the appLauncher scene </summary>
        public static void SelectMasterScene(string aScene)
        {
            string masterScene = aScene;
            masterScene = masterScene.Replace(Application.dataPath, "Assets");
            if (!string.IsNullOrEmpty(masterScene))
            {
                MasterScene = masterScene;
                LoadMasterOnPlay = true;
            }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            //Give to the appLauncher the scene to load
            if (TestCurrentScene)
            {
                if (EditorSceneManager.GetActiveScene().path == SceneAutoLoader.MasterScene && !EditorApplication.isPlaying)
                {
                    PlayerPrefs.SetString("SceneToLoad", "");
                }
                else
                {
                    PlayerPrefs.SetString("SceneToLoad", EditorSceneManager.GetActiveScene().path);
                }

            }
            else
            {
                PlayerPrefs.SetString("SceneToLoad", "");
            }

            //Load the appLauncher before launching the game
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PreviousScene = EditorSceneManager.GetActiveScene().path;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene(MasterScene);
                    }
                    catch
                    {
                        Debug.LogError(string.Format("error: scene not found: {0}", MasterScene));
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }

            //Load the previous Scene when the player stop playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                try
                {
                    EditorSceneManager.OpenScene(PreviousScene);
                }
                catch
                {
                    Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
                }
            }
        }


        private const string cEditorPrefLoadMasterOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
        private const string cEditorPrefTestCurrentScene = "SceneAutoLoader.TestCurrentScene";
        private const string cEditorPrefMasterScene = "SceneAutoLoader.MasterScene";
        private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";
        private const string cEditorPrefOriginalScene = "SceneAutoLoader.OriginalScene";
        private const string cEditorPrefWantHelpWithManager = "SceneAutoLoader.WantHelpManager";

        private const string cEditorPrefHavePoolManager = "HUD.PoolManager";
        private const string cEditorPrefHaveLevelManager = "HUD.LevelManager";
        private const string cEditorPrefHaveAudioManager = "HUD.AudioManager";
        private const string cEditorPrefHaveLanguageManager = "HUD.LanguageManager";
        private const string cEditorPrefHaveOptionManager = "HUD.OptionManager";
        private const string cEditorPrefHaveSpawnManager = "HUD.SpawnManager";

        private const string cEditorUseToolTip = "SceneAutoLoader.UseTooltip";
        private const string cEditorUseEnglish = "SceneAutoLoader.UseEnglish";
        private const string cEditorUseFixeWindow = "SceneAutoLoader.UseFixeWindow";
        private const string cEditorOldScenePath = "SceneAutoLoader.OldScenePath";

        private static bool LoadMasterOnPlay
        {
            get { return EditorPrefs.GetBool(cEditorPrefLoadMasterOnPlay, false); }
            set { EditorPrefs.SetBool(cEditorPrefLoadMasterOnPlay, value); }
        }

        public static bool WantHelpManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefWantHelpWithManager, true); }
            set { EditorPrefs.SetBool(cEditorPrefWantHelpWithManager, value); }
        }

        public static int SpawnGroundLayer
        {
            get { return EditorPrefs.GetInt("Spawn.GroundLayer", 0); }
            set { EditorPrefs.SetInt("Spawn.GroundLayer", value); }
        }

        public static bool HavePoolManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHavePoolManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHavePoolManager, value); }
        }

        public static bool HaveAudioManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHaveAudioManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHaveAudioManager, value); }
        }

        public static bool HaveLevelManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHaveLevelManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHaveLevelManager, value); }
        }

        public static bool HaveLanguageManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHaveLanguageManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHaveLanguageManager, value); }
        }

        public static bool HaveOptionManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHaveOptionManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHaveOptionManager, value); }
        }

        public static bool HaveSpawnManager
        {
            get { return EditorPrefs.GetBool(cEditorPrefHaveSpawnManager, false); }
            set { EditorPrefs.SetBool(cEditorPrefHaveSpawnManager, value); }
        }

        public static bool TestCurrentScene
        {
            get { return EditorPrefs.GetBool(cEditorPrefTestCurrentScene, false); }
            set { EditorPrefs.SetBool(cEditorPrefTestCurrentScene, value); }
        }

        public static bool IsUsingTooltip
        {
            get { return EditorPrefs.GetBool(cEditorUseToolTip, false); }
            set { EditorPrefs.SetBool(cEditorUseToolTip, value); }
        }

        public static bool IsUsingEnglish
        {
            get { return EditorPrefs.GetBool(cEditorUseEnglish, false); }
            set { EditorPrefs.SetBool(cEditorUseEnglish, value); }
        }

        public static bool IsUsingFixeWindow
        {
            get { return EditorPrefs.GetBool(cEditorUseFixeWindow, false); }
            set { EditorPrefs.SetBool(cEditorUseFixeWindow, value); }
        }

        public static string OldScenePath
        {
            get { return EditorPrefs.GetString(cEditorOldScenePath, ""); }
            set { EditorPrefs.SetString(cEditorOldScenePath, value); }
        }

        public static string MasterScene
        {
            get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Master.unity"); }
            set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
        }

        public static string SceneToLoad
        {
            get { return EditorPrefs.GetString(cEditorPrefOriginalScene, "Original.unity"); }
            set { EditorPrefs.SetString(cEditorPrefOriginalScene, value); }
        }

        private static string PreviousScene
        {
            get { return EditorPrefs.GetString(cEditorPrefPreviousScene, EditorSceneManager.GetActiveScene().path); }
            set { EditorPrefs.SetString(cEditorPrefPreviousScene, value); }
        }


    }
    #endregion
}

