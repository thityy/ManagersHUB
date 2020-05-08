using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


namespace ManagersHUB
{
    public class SceneManager : Singleton<SceneManager>
    {

        //----------------------------------------------------------------------------------------------------------------
        //------------------------ ThityyDev® ---- Last Edit: 2020/01/23 13:53 ---- SceneManager® ------------------------
        //----------------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------------------------------------------------------------
        //------------------------------------------   VARIABLES   -------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        [Header("Loading System")]
        [SerializeField]
        private LoadingScreenData m_DefaultLoadingScreenData = null;    //The default loading screen data
        private LoadingScreenObj m_DefaultLoadingScreen = null;     //The default loading screen that will be display.
        [SerializeField]
        private float m_MinLoadingTimeStandard = 3;                 //The minimum time the loading need to lapse before disable the windows (paramaters).
        private float m_CurrentTime = 0;                            //The current time of the loading.
        private bool m_SceneIsLoad = false;                         //If the scene is Loading in memory and ready to play.
        [Header("Technicals Variables")]
        private eSceneM_SceneCallType m_CallType;                          //If the scene get call by path or with a sceneData
        private SceneData m_CurrentSceneData;                       //The scene data that is currently loading.
        private string m_ScenePath;                                 //The current scene that want to be loaded.
        private bool m_WantToLoadScene = false;                     //Check if the LevelManager want to load a scene.
        private bool m_IsInit = false;                              //If the manager is init.

        //----------------------------

        [Header("TipAndTrick System")]
        [SerializeField]
        private bool m_UseTipSystem = false;            //If the manager instantiate and use the Tip System
        private bool m_TipIsInit = false;               //If the Tip system is done initialising
        [SerializeField]
        private GroupTipsData m_GroupTip = null;        //The data that contains all tip and trick
        private string m_TipText = "";                  //The tip that will be display on the player screen.

        //----------------------------

        [Header("Fade System")]
        [SerializeField]
        private bool m_UseFadeSystem = false;           //If the manager instantiate and use the Fade System
        private bool m_FadeIsInit = false;              //If the Fade system is done initialising
        [SerializeField]
        private Image m_FadeScreenImg = null;           //The object that will be fadeIn and fadeOut if ask for a fade loading.
        private Color m_FadeScreenColor = new Color();  //The current color of the blackscreen.
        private eSceneM_FadeDir m_FadeDir = eSceneM_FadeDir.FadeIn;   //The current direction of the fading effect.
        private bool m_FadeIsDone = false;              //If the black screen is finish fadeIn or fadeOut.

        //----------------------------

        [Header("Advance Loading System")]
        [SerializeField]
        private bool m_UseAdvancedLoadingSystem = false;        //If the manager instantiate and use the AdvancedLoading System
        private bool m_AdvancedLoadingIsInit = false;           //If the AdvancedLoading system is done initialising
        private GameObject m_Canvas;                            //The Canvas that we use to spawn object.
        private Transform m_CanvasObj;                          //The Canvas transform that we use to render the overwrite Loading Screen.
        private LoadingScreenObj m_OverwriteLS;                 //The loading screen that overwrite the default one.

        private bool m_LoadingScreenIsOver = false;             //If the loading screen (total loading) is done or not. (loading screen removed)
        public bool LoadingIsOver                               //Acces from anywhere if the loading is done or not.
        {
            get { return m_LoadingScreenIsOver; }
        }

        //----------------------------

        [Header("LoadingBar Variables")]
        [SerializeField]
        private bool m_UseLoadingBarSystem = false; //If the manager instantiate and use the LoadingBar System
        private bool m_LoadingBarIsInit = false;    //If the LoadingBar system is done initialising
        private float m_LBCurrentTime = 0.0f;       //The current time of the loading bar.
        private float m_LbTimer = 10.0f;            //The max time of the loading bar.
        private bool m_LbIsUsing = false;           //If the loading bar is currently in use.
        public bool LoadingBarIsInUse               //Get if the loading bar is in use.
        {
            get { return m_LbIsUsing; }
        }
        private LoadingBar m_LbBar = null;          //The loading bar that need to be affect.
        private Action m_LbFinishAction = null;     //The action that get called when the loading bar is finish.


        //----------------------------------------------------------------------------------------------------------------
        //------------------------------------------   FUNCTIONS   -------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            InitDebuging(eManagers.SceneManager);
            InitSceneManager();
            if (SceneManager_IsInit(true))
            {
                if (m_UseTipSystem)
                {
                    InitTipAndTrick();
                }
                if (m_UseAdvancedLoadingSystem)
                {
                    InitAdvancedOption();
                }
                if (m_UseLoadingBarSystem)
                {
                    InitLoadingBar();
                }
                if (m_UseFadeSystem)
                {
                    InitFade();
                }
                else
                {
                    if (m_FadeScreenImg != null)
                    {
                        m_FadeScreenImg.enabled = false;
                    }
                }
            }
        }

        private void Update()
        {
            if (SceneManager_IsInit(false))
            {
                Verification_StartLoading();
                Verification_FinishLoading();
                if (m_LbIsUsing)
                {
                    UpdateLoadingBar();
                }
            }
        }

        #endregion

        ///<summary> Init the level manager and close the loading screen to be sure everything work fine </summary>
        private void InitSceneManager()
        {
            InitCanvas();

            if (m_DefaultLoadingScreenData == null)
            {
                Debug.LogError(Debug_InitFailed("Can't init the SceneManager, there's no default Loading Screen Data in the m_DefaultLoadingScreen component slot! Drop one"));
                return;
            }
            m_DefaultLoadingScreen = Instantiate(m_DefaultLoadingScreenData.GetLoadingScreen(), m_CanvasObj);
            m_DefaultLoadingScreen.name = "Default_LoadingScreen";
            if (m_DefaultLoadingScreen == null)
            {
                Debug.LogError(Debug_InitFailed("Can't init the SceneManager, there's no default Loading Screen Object set in ther actual Data! Drop one"));
                return;
            }
            try
            {
                DesactivateDefaultLoadingScreen();
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += LoadingIsDone;
            }
            catch
            {
                Debug.LogError(Debug_InitFailed("Can't init the SceneManager, there's a problem with UnityEngine.SceneManagement.SceneManager"));
                return;
            }

            m_IsInit = true;
            Debug.Log(Debug_InitSucces("Scene Manager as been init"));
        }


        private void InitCanvas()
        {
            GameObject tempObj = Instantiate(new GameObject(), transform);
            tempObj.name = "LoadingScreen_Canvas";
            Canvas tempCanvas = tempObj.AddComponent<Canvas>();
            CanvasScaler tempScaler = tempObj.AddComponent<CanvasScaler>();

            tempCanvas.sortingOrder = 50;
            tempCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            tempScaler.referenceResolution = new Vector2(1920, 1080);
            tempScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            m_Canvas = tempObj;
            m_CanvasObj = tempObj.transform;
        }

        #region SceneManager Verifications  [Include all Bool that tell you if the system is init and work correctly]

        ///<summary> Return TRUE if the SceneManager as been correctly init </summary>
        private bool SceneManager_IsInit(bool aShowWarning)
        {
            if (!m_IsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Scene Manager' isn't initialize."));
            }
            return m_IsInit;
        }

        ///<summary> Return TRUE if the Tip and Trick system as been correctly init </summary>
        private bool Tip_IsInit(bool aShowWarning)
        {
            if (!m_TipIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Tip & Trick System' isn't initialize."));
            }
            return m_TipIsInit;
        }

        ///<summary> Return TRUE if the Fade System as been correctly init </summary>
        private bool Fade_IsInit(bool aShowWarning)
        {
            if (!m_FadeIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Fade System' isn't initialize."));
            }
            return m_FadeIsInit;
        }

        ///<summary> Return TRUE if the AdvancedLoading System as been correctly init </summary>
        private bool AdvancedLoading_IsInit(bool aShowWarning)
        {
            if (!m_AdvancedLoadingIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Advanced Loading System' isn't initialize."));
            }
            return m_AdvancedLoadingIsInit;
        }

        ///<summary> Return TRUE if the Loading Bar system as been correctly init </summary>
        private bool LoadingBar_IsInit(bool aShowWarning)
        {
            if (!m_LoadingBarIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Loading Bar System' isn't initialize."));
            }
            return m_LoadingBarIsInit;
        }

        #endregion

        #region Tip & Trick System          [Include Initialisation and function for Tip & Trick]

        ///<summary> Initialise the tip and trick system </summary>
        private void InitTipAndTrick()
        {
            if (m_GroupTip == null)
            {
                Debug.LogError(Debug_Error("Can't init the tip and trick system, there's no Group Tip Data set"));
                return;
            }
            m_TipIsInit = true;
            Debug.Log(Debug_InitSucces("Tip & Trick System as been init"));
        }

        ///<summary> Get a random tip from the folder of list that equal the same group of the scene in load </summary>
        private void SetTip()
        {
            if (!Tip_IsInit(true))
            {
                return;
            }
            switch (m_CurrentSceneData.GetTipOption())
            {
                case eSceneM_TipOption.Random:
                    {
                        m_TipText = GetTipWithinGroup(m_CurrentSceneData.GetTipGroup());
                        break;
                    }
                case eSceneM_TipOption.Select:
                    {
                        if (m_CurrentSceneData.GetLoadingTipData() != null)
                        {
                            m_TipText = m_CurrentSceneData.GetLoadingTipData().GetTip();
                        }
                        break;
                    }
            }
        }

        ///<summary> Show the tip box on the loading screen </summary>
        private void ShowTip(ref LoadingScreenObj aLoadingScreen)
        {
            SetTip();
            if (m_TipText != "")
            {
                aLoadingScreen.ShowTipPox(m_TipText);
            }
            else
            {
                Debug.LogWarning("There's no tip available to show.");
                aLoadingScreen.HideTipBox();
            }
        }

        ///<summary> Hide the tip box from the loading screen </summary>
        private void HideTip(ref LoadingScreenObj aLoadingScreen)
        {
            aLoadingScreen.HideTipBox();
            m_TipText = "";
        }

        ///<summary> Return a tip within the group asked </summary>
        private string GetTipWithinGroup(eSceneM_TipGroup aGroup)
        {
            if (!Tip_IsInit(true))
            {
                return "";
            }
            return m_GroupTip.GetTip(aGroup).GetTip();
        }

        #endregion

        #region Fade System                 [Include Initialisation and function for Fade loading]

        ///<summary> Initialise the fade loading system </summary>
        private void InitFade()
        {
            m_FadeScreenImg = Instantiate(new GameObject(), m_CanvasObj).AddComponent<Image>();
            m_FadeScreenImg.name = "FadeScreen";
            m_FadeScreenImg.rectTransform.sizeDelta = new Vector2(1920, 1080);

            if (m_FadeScreenImg == null)
            {
                Debug.Log(Debug_InitFailed("Can't init the Fade System, there's no Image in the m_FadeScreenImg component slot! Drop one"));
                return;
            }
            m_FadeScreenImg.gameObject.SetActive(true);
            m_FadeScreenColor.a = 1.0f;
            m_FadeScreenImg.color = m_FadeScreenColor;

            m_FadeIsInit = true;
            Debug.Log(Debug_InitSucces("Fade System as been init"));
        }

        ///<summary> Ask to fade in and out the black screen </summary>
        private void AskToFade(eSceneM_FadeDir aDir)
        {
            StopAllCoroutines();
            m_FadeIsDone = false;
            m_FadeDir = aDir;
            m_FadeScreenColor = m_CurrentSceneData.GetFadeColor();
            StartCoroutine(FadeScreen(m_FadeDir));
        }

        ///<summary> Update the black Screen Color (fade in and out) </summary>
        private IEnumerator FadeScreen(eSceneM_FadeDir aDir)
        {
            if (!m_FadeIsDone)
            {
                switch (aDir)
                {
                    case eSceneM_FadeDir.FadeIn:
                        {
                            m_FadeScreenColor.a = 0.0f;
                            while (m_FadeScreenColor.a < 1)
                            {
                                m_FadeScreenColor.a += 0.02f;
                                m_FadeScreenImg.color = m_FadeScreenColor;
                                yield return new WaitForSeconds(0.01f);
                            }
                            m_FadeIsDone = true;
                            break;
                        }
                    case eSceneM_FadeDir.FadeOut:
                        {
                            m_FadeScreenColor.a = 1.0f;
                            while (m_FadeScreenColor.a > 0)
                            {
                                m_FadeScreenColor.a -= 0.02f;
                                m_FadeScreenImg.color = m_FadeScreenColor;
                                yield return new WaitForSeconds(0.01f);
                            }
                            m_FadeIsDone = true;
                            break;
                        }
                }
            }
            else if (LoadingIsOver)
            {
                m_FadeScreenColor.a = 0.0f;
                m_FadeScreenImg.color = m_FadeScreenColor;
            }
        }

        #endregion

        #region Advanced Loading System     [Include Initialisation and function for overwrite loading screen]

        ///<summary> Initialise the advanced loading system </summary>
        private void InitAdvancedOption()
        {
            if (m_CanvasObj == null)
            {
                Debug.Log(Debug_InitFailed("Can't init the Advanced Loading System, there's no Canvas in the m_CanvasObj component slot! Drop one"));
                return;
            }
            m_AdvancedLoadingIsInit = true;
            Debug.Log(Debug_InitSucces("Advanced Loading System as been init"));
        }

        ///<summary> Overwrite the loading screen with the one ask for within the SceneData </summary>
        private void OverwriteLoadingScreen(ref LoadingScreenObj aLoadingScreen)
        {
            if (m_CurrentSceneData.GetLoadingObj() != null)
            {
                m_OverwriteLS = Instantiate(m_CurrentSceneData.GetLoadingObj(), m_CanvasObj);
                aLoadingScreen = m_OverwriteLS;
            }
            else
            {
                Debug.LogWarning("There's no Loading Screen available to overwrite the default one.");
                m_DefaultLoadingScreen.gameObject.SetActive(true);
            }
        }

        ///<summary> Destroy the overwrite loading screen </summary>
        private void DestroyOverwriteLoadingScreen()
        {
            if (m_OverwriteLS != null)
            {
                Destroy(m_OverwriteLS.gameObject);
                m_OverwriteLS = null;
                m_FadeScreenColor.a = 0.0f;
                m_FadeScreenImg.color = m_FadeScreenColor;
            }
        }

        #endregion

        #region Loading Bar System          [Include Initialisation and function for the Loading bar]

        ///<summary> Initialise the loading bar system </summary>
        private void InitLoadingBar()
        {
            m_LoadingBarIsInit = true;
            Debug.Log(Debug_InitSucces("Loading Bar System as been init"));
        }

        ///<summary> Call a loading bar, and when it finish tell </summary>
        public void CallLoadingBar(LoadingBar aBar, System.Action aAction, float aTimer)
        {
            if (!LoadingBar_IsInit(true))
            {
                return;
            }
            if (!m_LbIsUsing)
            {
                m_LbBar = aBar;
                m_LbFinishAction = aAction;
                m_LbTimer = aTimer;
                m_LBCurrentTime = 0;
                m_LbIsUsing = true;
            }
            else
            {
                Debug.LogWarning("There's already a loading bar in action");
            }
        }

        ///<summary> Cancel the current loading bar </summary>
        public void CancelLoadingBar()
        {
            if (!LoadingBar_IsInit(true))
            {
                return;
            }
            m_LbIsUsing = false;
            m_LbBar = null;
            m_LbFinishAction = null;
            m_LbTimer = 0;
            m_LBCurrentTime = 0;
        }

        ///<summary> Update the timer of the loading bar </summary>
        private void UpdateLoadingBar()
        {
            if (!LoadingBar_IsInit(true))
            {
                return;
            }
            if (m_LBCurrentTime < m_LbTimer)
            {
                m_LBCurrentTime += Time.deltaTime;
                m_LbBar.UpdateFill(m_LBCurrentTime, 0, m_LbTimer);
            }
            else
            {
                m_LbFinishAction();
                CancelLoadingBar();
            }
        }

        #endregion

        #region Basic Scene Loading System  [Include all function needed for the SceneManager to be able to load a scene]

        ///<summary> Ask the loading of the next scene [SceneData]</summary>
        public void CallLoadScene(SceneData aScene)
        {
            if (!SceneManager_IsInit(true))
            {
                return;
            }
            if (SceneExist(aScene.GetSceneName()))
            {
                SetNewSceneData(aScene);
                LoadScene(aScene.GetSceneName());
            }
        }

        ///<summary> Ask the loading of the next scene [Path]</summary>
        public void CallLoadScene(string aScene)
        {
            if (!SceneManager_IsInit(true))
            {
                return;
            }
            if (SceneExist(aScene))
            {
                SetNewSceneData(null);
                LoadScene(aScene);
            }
        }

        ///<summary> Start the loading of the next scene </summary> 
        private void LoadScene(string aScene)
        {
            ShowLoadingScreen();
            m_ScenePath = aScene;
            m_WantToLoadScene = true;
        }

        ///<summary> Start the loading scene and load the new scene with the Unity.SceneManager </summary>
        private void StartLoading(string aScene)
        {
            ResetLoading();
            UnityEngine.SceneManagement.SceneManager.LoadScene(aScene);
        }

        ///<summary> Call back that Unity tell you when the scene is finish loading and ready to start </summary>
        private void LoadingIsDone(Scene aScene, LoadSceneMode aMode)
        {
            m_SceneIsLoad = true;
            if (m_CurrentSceneData != null)
            {
                if (ManagerIsSpawn(eManagers.PoolManager))
                {
                    if (m_CurrentSceneData.UsePoolControle)
                    {
                        ManagePool();
                    }
                }
                if (ManagerIsSpawn(eManagers.SpawnManager))
                {
                    ManageSpawn();
                }
            }

        }

        ///<summary> Close the loading screen and tell other managers that the scene is loaded and open </summary>
        private void FinishLoading()
        {
            HideLoadingScreen();

            if (m_CurrentSceneData != null)
            {
                if (ManagerIsSpawn(eManagers.AudioManager))
                {
                    if (m_CurrentSceneData.UseMusicControle)
                    {
                        ManageMusic();
                    }
                }
            }

            SetNewSceneData(null);
        }

        ///<summary> Show the loading screen (effect depend on the LoadingType Option) </summary>
        private void ShowLoadingScreen()
        {
            if (m_CurrentSceneData == null)
            {
                ActivateDefaultLoadingScreen();
                Debug.Log(Debug_Message("[SHOW] Loading Screen skip when you select 'Test Current Scene'"));
                return;
            }
            LoadingScreenObj obj = m_DefaultLoadingScreen;
            try
            {

                //Loading Scene Visual Choice
                switch (m_CurrentSceneData.GetLoadingScreenChoice())
                {
                    case eSceneM_LoadingScreen.Default:
                        {
                            ActivateDefaultLoadingScreen();
                            break;
                        }
                    case eSceneM_LoadingScreen.Overwrite:
                        {
                            if (!AdvancedLoading_IsInit(true))
                            {
                                ActivateDefaultLoadingScreen();
                            }
                            else
                            {
                                OverwriteLoadingScreen(ref obj);
                            }

                            break;
                        }
                    case eSceneM_LoadingScreen.Fade:
                        {
                            if (!Fade_IsInit(true))
                            {
                                ActivateDefaultLoadingScreen();
                            }
                            else
                            {
                                AskToFade(eSceneM_FadeDir.FadeIn);
                                return;
                            }

                            break;
                        }
                }

                //Loading Scene Options [Skip if Fade]
                switch (m_CurrentSceneData.GetLoadingType())
                {
                    case eSceneM_LoadingType.WithTip:
                        {
                            if (!Tip_IsInit(true))
                            {
                                HideTip(ref obj);
                            }
                            else
                            {
                                ShowTip(ref obj);
                            }
                            break;
                        }
                    case eSceneM_LoadingType.WithoutTip:
                        {
                            HideTip(ref obj);
                            break;
                        }
                }
            }
            catch
            {
                ActivateDefaultLoadingScreen();
                Debug.LogWarning(Debug_Warning("Problem with the ShowLoadingScreen() function"));
            }
        }

        ///<summary> Hide the loading screen (effect depend on the LoadingType Option) </summary>
        private void HideLoadingScreen()
        {
            if (m_CurrentSceneData == null)
            {
                DesactivateDefaultLoadingScreen();
                Debug.Log(Debug_Message("[HIDE] Loading Screen skip when you select 'Test Current Scene'"));
                return;
            }
            try
            {
                switch (m_CurrentSceneData.GetLoadingType())
                {
                    case eSceneM_LoadingType.WithTip:
                        {
                            if (!Tip_IsInit(true))
                            {
                                break;
                            }
                            else
                            {
                                HideTip(ref m_DefaultLoadingScreen);
                                break;
                            }
                        }
                }

                switch (m_CurrentSceneData.GetLoadingScreenChoice())
                {
                    case eSceneM_LoadingScreen.Default:
                        {
                            DesactivateDefaultLoadingScreen();
                            break;
                        }
                    case eSceneM_LoadingScreen.Overwrite:
                        {
                            if (!AdvancedLoading_IsInit(true))
                            {
                                DesactivateDefaultLoadingScreen();
                            }
                            else
                            {
                                DestroyOverwriteLoadingScreen();
                            }
                            break;
                        }
                    case eSceneM_LoadingScreen.Fade:
                        {
                            if (!Fade_IsInit(true))
                            {
                                DesactivateDefaultLoadingScreen();
                            }
                            else
                            {
                                AskToFade(eSceneM_FadeDir.FadeOut);
                            }
                            break;
                        }
                }

            }
            catch
            {
                DesactivateDefaultLoadingScreen();
                Debug.LogWarning(Debug_Warning("Problem with the HideLoadingScreen() function"));
            }

            m_LoadingScreenIsOver = true;
        }


        #endregion

        #region Managers Call               [Include all function that call other Manager]

        ///<summary> Manage all the option set within the SceneData that have link with the PoolManager </summary>
        private void ManagePool()
        {
            if (!SceneManager_IsInit(true))
            {
                return;
            }
            if (IsUsingSceneData())
            {
                PoolManager.Instance.InitPools(m_CurrentSceneData.GetScenePool());
            }
        }

        ///<summary> Manage all the option set within the SceneData that have link with the AudioManager </summary>
        private void ManageMusic()
        {
            if (!SceneManager_IsInit(true))
            {
                return;
            }
            if (IsUsingSceneData())
            {
                switch (m_CurrentSceneData.GetMusicOption())
                {
                    case eSceneM_Music.WithMusic:
                        {
                            if (m_CurrentSceneData.GetSceneMusic() != null)
                            {
                                AudioManager.Instance.Music_SetLoop(true);
                                AudioManager.Instance.Music_SetClip(m_CurrentSceneData.GetSceneMusic(), true);
                            }
                            else
                            {
                                Debug.LogWarning("There's no music in this SceneData you try to load!");
                                AudioManager.Instance.Music_SetClip(null, false);
                            }
                            break;
                        }
                    case eSceneM_Music.WithoutMusic:
                        {
                            AudioManager.Instance.Music_SetClip(null, false);
                            break;
                        }
                    case eSceneM_Music.KeepOldMusic:
                        {
                            AudioManager.Instance.Music_SetLoop(true);
                            break;
                        }
                }
            }
        }

        ///<summary> Manage all the option set within the SceneData that have link with the SpawnManager </summary>
        private void ManageSpawn()
        {
            if (!SceneManager_IsInit(true))
            {
                return;
            }
        }

        #endregion

        #region Utils                       [Include all function that make the code more simple to read and understand]

        ///<summary> Set a new SceneData for the LevelManager </summary>
        private void SetNewSceneData(SceneData aData)
        {
            m_CurrentSceneData = aData;

            if (m_CurrentSceneData != null)
            {
                m_CallType = eSceneM_SceneCallType.SceneData;
            }
            else
            {
                m_CallType = eSceneM_SceneCallType.Path;
            }
        }

        ///<summary> Reset all loading variables each time you want to load a new scene </summary>
        private void ResetLoading()
        {
            m_CurrentTime = 0.0f;
            m_WantToLoadScene = false;
            m_SceneIsLoad = false;
            m_LoadingScreenIsOver = false;
        }

        ///<summary> Check if the LevelManager is currently using a valid SceneData </summary>
        private bool IsUsingSceneData()
        {
            switch (m_CallType)
            {
                case eSceneM_SceneCallType.SceneData:
                    {
                        if (m_CurrentSceneData != null)
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        ///<summary> Check if the loading screen as been show to the player for enough time </summary>
        public bool SceneExist(string aSceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(aSceneName))
            {
                return true;
            }
            else
            {
                Debug.LogWarning("There's no scene with the name [" + aSceneName + "] in the Build of Unity!");
                return false;
            }
        }

        ///<summary> Check if the loading screen as been show to the player for enough time </summary>
        private void Verification_FinishLoading()
        {
            //Bypass the amount of time for loading screen when is fading
            if (m_CurrentSceneData != null)
            {
                if (Fade_IsInit(false) && m_SceneIsLoad && m_CurrentSceneData.GetLoadingScreenChoice() == eSceneM_LoadingScreen.Fade && m_FadeIsDone)
                {
                    m_SceneIsLoad = false;
                    FinishLoading();
                    return;
                }
            }

            //Update the amount of time we're in the laoding screen
            if (m_CurrentTime < m_MinLoadingTimeStandard)
            {
                m_CurrentTime += Time.deltaTime;
            }
            else if (m_SceneIsLoad)
            {
                m_SceneIsLoad = false;
                FinishLoading();
            }
        }

        ///<summary> Check if the LevelManager can load the scene </summary>
        private void Verification_StartLoading()
        {
            //Return if the fading isn't done or simply if the player doesn't want to load the next scene.
            if (!m_WantToLoadScene || (Fade_IsInit(false) && m_CurrentSceneData != null && m_CurrentSceneData.GetLoadingScreenChoice() == eSceneM_LoadingScreen.Fade && !m_FadeIsDone))
            {
                return;
            }

            StartLoading(m_ScenePath);
        }

        ///<summary> Show to the player the default loading screen [USE WHEN THERE'S A PROBLEM WITH LOADING OPTION]</summary>
        private void ActivateDefaultLoadingScreen()
        {
            m_DefaultLoadingScreen.gameObject.SetActive(true);
        }

        ///<summary> Hide to the player the default loading screen [USE WHEN THERE'S A PROBLEM WITH LOADING OPTION]</summary>
        private void DesactivateDefaultLoadingScreen()
        {
            m_DefaultLoadingScreen.gameObject.SetActive(false);
            if (m_UseFadeSystem && m_FadeIsInit)
            {
                m_FadeScreenColor.a = 0.0f;
                m_FadeScreenImg.color = m_FadeScreenColor;
            }
        }

        #endregion
    }
}
