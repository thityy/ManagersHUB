using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ManagersHUB
{

    [System.Serializable]
    public class RadioStation
    {
        public List<MusicData> StationMusics = new List<MusicData>();
        public int CurrentMusicId;
        public float timeElapsed;
        public bool isPlaying;

        public RadioStation(RadioData aRadioData)
        {
            if (aRadioData != null)
            {
                StationMusics = aRadioData.GetMusics();
                isPlaying = false;
                CurrentMusicId = 0;
                timeElapsed = 0.0f;
                if (aRadioData.WantToShuffle())
                {
                    ShuffleRadio();
                }
            }
        }

        public void ShuffleRadio()
        {
            System.Random rng = new System.Random();
            int n = StationMusics.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                MusicData value = StationMusics[k];
                StationMusics[k] = StationMusics[n];
                StationMusics[n] = value;
            }
        }

        public MusicData GetNextMusic()
        {
            if (CurrentMusicId == StationMusics.Count - 1)
            {
                CurrentMusicId = 0;
            }
            else
            {
                CurrentMusicId++;
            }
            return StationMusics[CurrentMusicId];
        }

        public MusicData GetPreviousMusic()
        {
            if (CurrentMusicId == 0)
            {
                CurrentMusicId = StationMusics.Count - 1;
            }
            else
            {
                CurrentMusicId--;
            }
            return StationMusics[CurrentMusicId];
        }
    }

    public class AudioManager : Singleton<AudioManager>
    {

        //----------------------------------------------------------------------------------------------------------------
        //------------------------ ThityyDev® ---- Last Edit: 2020/01/23 13:57 ---- AudioManager® ------------------------
        //----------------------------------------------------------------------------------------------------------------


        //----------------------------------------------------------------------------------------------------------------
        //------------------------------------------   VARIABLES   -------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        [Header("Music System")]
        [SerializeField]
        private bool m_UseMusicSystem = false;          //If the manager instantiate and use the Music System
        private bool m_MusicIsInit = false;             //If the Music system is done initialising
        [SerializeField]
        private AudioSource m_MusicPlayer = null;       //The audioSource that we play music on.

        //----------------------------

        [Header("Option System")]
        [SerializeField]
        private bool m_UseOptionSystem = false;         //If the manager instantiate and use the Option System
        private bool m_OptionIsInit = false;            //If the Option system is done initialising
        [SerializeField]
        private AudioMixerGroup m_MasterGroup = null;   //The audioMixer for all sound in the game (master)
        [SerializeField]
        private AudioMixerGroup m_MusicGroup = null;    //The audioMixer for all music in the game (Music and Radio)
        [SerializeField]
        private AudioMixerGroup m_SFXGroup = null;      //The audioMixer for all sfx sound in the game
        [SerializeField]
        private AudioMixerGroup m_VoiceGroup = null;    //The audioMixer for all voice sound in the game

        //----------------------------

        [Header("SFX System")]
        [SerializeField]
        private bool m_UseSFXSystem = false;                                            //If the manager instantiate and use the SFX System
        private bool m_SFXIsInit = false;                                               //If the SFX system is done initialising
        [SerializeField]
        private Transform m_ListPerfSounds;                                             //The parent where we instantiate SFX prefab at start for perf sound
        [SerializeField]
        private int m_MaxAmountPerfSFX = 0;                                             //The max amount of sound to instantiate at start for perf sound    
        private List<SFX_AudioSetup> m_PerfSFXsAvailable = new List<SFX_AudioSetup>();  //The list of all perf sound available
        private List<SFX_AudioSetup> m_PerfSFXsInUse = new List<SFX_AudioSetup>();      //The list of all perf sound currently in use

        //----------------------------

        [Header("Foot System")]
        [SerializeField]
        private bool m_UseFootSystem = false;   //If the manager instantiate and use the Foot System
        private bool m_FootIsInit = false;      //If the Foot system is done initialising
        [SerializeField]
        private FootSoundData m_FootSoundsData = null; //The data that contains all foot sound 
        [SerializeField]
        private eAudioM_AudioSelector m_FootAudioSelector = eAudioM_AudioSelector.AudioClip; //Select if you want the foot system to use AudioClip or AudioData

        //----------------------------

        [Header("Radio System")]
        [SerializeField]
        private bool m_UseRadioSystem = false;                                      //If the manager instantiate and use the Radio System.
        private bool m_RadioIsInit = false;                                         //If the radio system is done initialising.
        private AudioSource m_RadioPlayer = null;                                   //The audioSource of the Radio System.
        [SerializeField]
        private List<RadioData> m_RadioStationsInput = new List<RadioData>();       //The list that contains all radio station give by the user in the manager.

        private Dictionary<RadioData, RadioStation> m_RadioStations = new Dictionary<RadioData, RadioStation>();    //The dictionnary that contains all radio station that have music in them.
        private List<RadioData> m_RadioStationIds = new List<RadioData>();                                          //The list that contains all Data to call the dictionnary.
        private int m_CurrentRadioId = 0;                                                                           //The current radio Id.
        private RadioStation m_CurrentRadioStation = null;                                                          //The radio station in use.

        [SerializeField]
        private RadioDisplay m_RadioDisplay = null; //The display of the radio music
        [SerializeField]
        private float m_DisplayLength = 2f; //The amount of time the display is show on the screen


        //----------------------------------------------------------------------------------------------------------------
        //------------------------------------------   FUNCTIONS   -------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            InitDebuging(eManagers.AudioManager);

            if (m_UseMusicSystem)
            {
                InitMusicSystem();
            }
            if (m_UseSFXSystem)
            {
                InitSFXSystem();
            }
            if (m_UseFootSystem)
            {
                InitFootSound();
            }
            if (m_UseRadioSystem)
            {
                InitRadioStation();
            }
            if (m_UseOptionSystem)
            {
                InitOptionSystem();
            }
        }

        private void Update()
        {
            if (Radio_IsInit(false))
            {
                Radio_UpdateTime();
            }
        }

        #endregion

        #region AudioManager Verifications [Include all Bool that tell you if the system is init and work correctly]

        ///<summary> Return TRUE if the music is init </summary>
        private bool Music_IsInit(bool aShowWarning)
        {
            if (!m_MusicIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Music System' isn't initialize."));
            }
            return m_MusicIsInit;
        }

        ///<summary> Return TRUE if the SFX is init </summary>
        private bool SFX_IsInit(bool aShowWarning)
        {
            if (!m_SFXIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'SFX System' isn't initialize."));
            }
            return m_SFXIsInit;
        }

        ///<summary> Return TRUE if the radio is init </summary>
        private bool Radio_IsInit(bool aShowWarning)
        {
            if (!m_RadioIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Radio System' isn't initialize."));
            }
            return m_RadioIsInit;
        }

        ///<summary> Return TRUE if the foot sound system is init </summary>
        private bool FootSound_IsInit(bool aShowWarning)
        {
            if (!m_FootIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'FootSound System' isn't initialize."));
            }
            return m_FootIsInit;
        }

        ///<summary> Return TRUE if the foot sound system is init </summary>
        private bool Option_IsInit(bool aShowWarning)
        {
            if (!m_OptionIsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Option System' isn't initialize."));
            }
            return m_OptionIsInit;
        }

        #endregion

        ///<summary>
        /// The Sound system let's you play a sound from anywhere in the game. You only need to have an AudioData and a position you want the sound to be played and
        /// the manager will do the rest for you. You have 2 options, 1. EasySound, it will play a sound no matter what, instantiate one and destroy it just after it get done;
        /// 2. PerfSound, it will pick a sound already instantiate (from the start of the game), will modify him to be as you wish, play it and then put it back into a list.
        /// This option is more performant, but you need to set the number of sound you want from the start and if you exceed the amount (play to many sound at the same time)
        /// the Manager will first warn you, but will need to stop the first sound played (actually) to be able to play the new one.
        ///</summary>
        #region SOUND Functions

        private void InitSFXSystem()
        {
            GameObject perfParent = Instantiate(new GameObject(), transform);
            perfParent.name = "PerfSounds[" + m_MaxAmountPerfSFX + "]";
            m_ListPerfSounds = perfParent.transform;

            SFX_AudioSetup audioInCreation = null;

            for (int i = 0; i < m_MaxAmountPerfSFX; i++)
            {
                audioInCreation = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity).AddComponent<SFX_AudioSetup>();
                audioInCreation.name = "PerfSound_" + i.ToString();
                audioInCreation.SetAudioSource();
                audioInCreation.transform.SetParent(m_ListPerfSounds);
                m_PerfSFXsAvailable.Add(audioInCreation);
            }

            m_SFXIsInit = true;
            Debug.Log(Debug_InitSucces("SFX System as been init"));
        }

        //--------------------------------------------------------------------------------------------

        ///<summary> 
        ///(Sphere Audio) Instiante a sound, play it and then destroy it
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///</summary>
        public void SoundEasy_Play(AudioData aSound, Vector3 aPosition)
        {
            if (!SFX_IsInit(true))
            {
                return;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return;
            }
            SFX_AudioSetup audio = Instantiate(new GameObject(), aPosition, Quaternion.identity).AddComponent<SFX_AudioSetup>();
            audio.SetAudioSource();
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayAudio(aSound.GetDelay());
        }

        ///<summary> 
        ///(Sphere Audio) Instiante a sound, return the transform of the sound to the 'Caller', play it and then destroy it
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///</summary>
        public Transform SoundEasy_PlayAndReturn(AudioData aSound, Vector3 aPosition)
        {
            if (!SFX_IsInit(true))
            {
                return null;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return null;
            }
            SFX_AudioSetup audio = Instantiate(new GameObject(), aPosition, Quaternion.identity).AddComponent<SFX_AudioSetup>();
            audio.SetAudioSource();
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayAudio(aSound.GetDelay());
            return audio.transform;
        }

        ///<summary> 
        ///(Sphere Audio) Instiante a sound, set it as a child of the parent parameters, play it and then destroy it
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///<param name="aParent">The parent of the instantiate sound</param>
        ///</summary>
        public void SoundEasy_PlayAsAChild(AudioData aSound, Vector3 aPosition, Transform aParent)
        {
            if (!SFX_IsInit(true))
            {
                return;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return;
            }
            SFX_AudioSetup audio = Instantiate(new GameObject(), aPosition, Quaternion.identity).AddComponent<SFX_AudioSetup>();
            audio.SetAudioSource();
            audio.transform.SetParent(aParent);
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayAudio(aSound.GetDelay());
        }

        //--------------------------------------------------------------------------------------------

        ///<summary> 
        ///(Sphere Audio) Pick a soundPrefab already instantiate from a list, play it and then put back in the list [PERFORMANCE++]
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///</summary>
        public void SoundPerf_Play(AudioData aSound, Vector3 aPosition)
        {
            if (!SFX_IsInit(true))
            {
                return;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return;
            }
            if (m_MaxAmountPerfSFX <= 0)
            {
                Debug.LogWarning(Debug_Warning("There's no perf sound available"));
                return;
            }
            SFX_AudioSetup audio = GetPerfSound();
            audio.transform.position = aPosition;
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayPerfAudio(aSound.GetDelay(), PutBackPerfSound);
        }

        ///<summary> 
        ///(Sphere Audio) Pick a soundPrefab already instantiate from a list, return the transform of the sound to the 'Caller', play it and then put back in the list [PERFORMANCE++]
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///</summary>
        public Transform SoundPerf_PlayAndReturn(AudioData aSound, Vector3 aPosition)
        {
            if (!SFX_IsInit(true))
            {
                return null;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return null;
            }
            if (m_MaxAmountPerfSFX <= 0)
            {
                Debug.LogWarning(Debug_Warning("There's no perf sound available"));
                return null;
            }
            SFX_AudioSetup audio = GetPerfSound();
            audio.transform.position = aPosition;
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayPerfAudio(aSound.GetDelay(), PutBackPerfSound);
            return audio.transform;
        }

        ///<summary> 
        ///(Sphere Audio) Pick a soundPrefab already instantiate from a list, set it as a child of the parent parameters, play it and then put back in the list [PERFORMANCE++]
        ///<param name="aSound">The AudioData, with all setting already set, that need to be play</param>
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///<param name="aParent">The parent of the instantiate sound</param>
        ///</summary>
        public void SoundPerf_PlayAsAChild(AudioData aSound, Vector3 aPosition, Transform aParent)
        {
            if (!SFX_IsInit(true))
            {
                return;
            }
            if (aSound == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return;
            }
            if (m_MaxAmountPerfSFX <= 0)
            {
                Debug.LogWarning(Debug_Warning("There's no perf sound available"));
                return;
            }
            SFX_AudioSetup audio = GetPerfSound();
            audio.transform.SetParent(aParent);
            audio.transform.position = aPosition;
            audio.SetupAudio(aSound.GetClip(), aSound.GetVolume(), aSound.GetPitch(), aSound.GetSpatialBlend(), aSound.GetStereoPan());
            audio.PlayPerfAudio(aSound.GetDelay(), PutBackPerfSound);
        }

        //--------------------------------------------------------------------------------------------

        private SFX_AudioSetup GetPerfSound()
        {
            SFX_AudioSetup perfSound = null;

            if (m_PerfSFXsAvailable.Count == 0)
            {
                Debug.LogWarning(Debug_Warning("You use to many PerfSound at the same time, consider grow the limit"));
                perfSound = m_PerfSFXsInUse[0];
                m_PerfSFXsInUse.RemoveAt(0);
                m_PerfSFXsInUse.Add(perfSound);
            }
            else
            {
                perfSound = m_PerfSFXsAvailable[0];
                m_PerfSFXsInUse.Add(perfSound);
                m_PerfSFXsAvailable.RemoveAt(0);
            }

            return perfSound;
        }

        private void PutBackPerfSound(SFX_AudioSetup aSFX)
        {
            aSFX.transform.SetParent(m_ListPerfSounds);
            m_PerfSFXsInUse.Remove(aSFX);
            m_PerfSFXsAvailable.Add(aSFX);
        }

        #endregion

        ///<summary>
        /// The Music system lets you play music from anywhere in the game.
        ///</summary>
        #region MUSIC Functions

        private void InitMusicSystem()
        {
            m_MusicPlayer = Instantiate(new GameObject(), transform).AddComponent<AudioSource>();
            if (m_MusicPlayer == null)
            {
                Debug.Log(Debug_InitFailed("Can't init the music system, there's no AudioSource in the MusicPlayer component slot! Drop one"));
                return;
            }
            m_MusicIsInit = true;
            Debug.Log(Debug_InitSucces("Music System as been init"));
        }

        ///<summary>
        ///Will play an audioClip only once on the Music AudioSource
        ///<param name="aClip">The audioClip that will be play </param>
        ///<param name="aPosition">The position where the SFX will be instantiate</param>
        ///</summary>
        public void Music_PlayOnce(AudioClip aClip)
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.PlayOneShot(aClip);
        }

        ///<summary>Press Play on the Music AudioSource </summary>
        public void Music_Play()
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.Play();
        }

        ///<summary>Press Stop on the Music AudioSource </summary>
        public void Music_Stop()
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.Stop();
        }

        ///<summary>Press Pause on the Music AudioSource </summary>
        public void Music_Pause()
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.Pause();
        }

        ///<summary>Set a new music to the Music AudioSource 
        ///<param name="aClip">The new audioClip that will be set</param>.
        ///<param name="aAutoPlay">If the music start automaticly after being set</param>
        ///</summary>
        public void Music_SetClip(AudioClip aClip, bool aAutoPlay)
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.clip = aClip;
            if (aAutoPlay)
            {
                Music_Play();
            }
        }

        ///<summary>Set the looping option on the MusicListener 
        ///<param name="aWantToLoop">If you want the music to loop or not</param>.
        ///</summary>
        public void Music_SetLoop(bool aWantToLoop)
        {
            if (!Music_IsInit(true))
            {
                return;
            }
            m_MusicPlayer.loop = aWantToLoop;
        }

        #endregion

        ///<summary>
        /// The radio system lets you set 10 stations that will play music automaticly. [you can add station by editing the eAudioM_RadioStation in the 'Enums' script]
        /// At start, the system will get all musicData in the folder "MusicDatas" and divide them into each station depend on the data you set.
        /// After, you only need to call if you want to go to next station, previous station or select station and the music will start automaticly, like a real-life radio.
        ///</summary>
        #region RADIO Functions

        ///<summary>Init all the radio station with their music in them</summary>
        private void InitRadioStation()
        {
            //Create the radio player
            GameObject radio = Instantiate(new GameObject(), transform);
            radio.AddComponent<AudioSource>();
            m_RadioPlayer = radio.GetComponent<AudioSource>();

            if (m_RadioPlayer == null)
            {
                Debug.LogError(Debug_InitFailed("Can't init the Radio System, Can't create an audioSource for the radio player"));
                return;
            }

            if (m_RadioStationsInput.Count == 0)
            {
                Debug.LogError(Debug_InitFailed("There's no RadioData set in the manager, can't use the Radio system!"));
                return;
            }

            //Check the radio stations given by the user
            for (int i = 0; i < m_RadioStationsInput.Count; i++)
            {
                RadioData currentRadio = m_RadioStationsInput[i];
                if (currentRadio.HaveMusicInIt())
                {
                    m_RadioStations.Add(currentRadio, new RadioStation(currentRadio));
                    m_RadioStationIds.Add(currentRadio);
                }
                else
                {
                    Debug.LogWarning(Debug_Warning("The radio station: '" + currentRadio.name + "' doesn't have any music in it! Won't be add to the radio system."));
                }
            }

            m_RadioIsInit = true;
            m_CurrentRadioId = 0;
            Debug.Log(Debug_InitSucces("Radio System as been init"));
            //Initialisation is finish
        }

        //--------------------------------------------------------------------------------------------

        ///<summary>Resume the music that was playing last time we exit this station</summary>
        public void Radio_ResumeMusic()
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            Radio_ChangeMusic(m_CurrentRadioStation.StationMusics[m_CurrentRadioStation.CurrentMusicId], m_CurrentRadioStation.timeElapsed);
        }

        ///<summary>Pause the music that is playing right now</summary>
        public void Radio_PauseMusic()
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            m_CurrentRadioStation.isPlaying = false;
            m_RadioPlayer.Pause();
        }

        ///<summary>Will change for the next music in the current radio station</summary>
        public void Radio_NextMusic()
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            Radio_ChangeMusic(m_CurrentRadioStation.GetNextMusic(), 0.00f);
        }

        ///<summary>Will change for the previous music in the current radio station</summary>
        public void Radio_PreviousMusic()
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            Radio_ChangeMusic(m_CurrentRadioStation.GetPreviousMusic(), 0.00f);
        }

        ///<summary>Will change for the next radio station</summary>
        public void Radio_NextStation(bool aAutoResume)
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            Radio_SelectStation(Radio_GetNextStation(), aAutoResume);
        }

        ///<summary>Will change for the previous radio station</summary>
        public void Radio_PreviousStation(bool aAutoResume)
        {
            if (!Radio_IsInit(true))
            {
                return;
            }
            Radio_SelectStation(Radio_GetPreviousStation(), aAutoResume);
        }

        ///<summary>Will change for the selected radio station</summary>
        public void Radio_SelectStation(RadioData aRadioStation, bool aAutoResume)
        {
            if (!Radio_IsInit(true))
            {
                return;
            }

            if (!m_RadioStations.ContainsKey(aRadioStation))
            {
                Debug.LogError(Debug_Error("The Radio system doesn't have this station: " + aRadioStation));
            }
            else if (aRadioStation == m_RadioStationIds[m_CurrentRadioId])
            {
                Debug.LogWarning(Debug_Warning("These radio station is already in-use: " + aRadioStation));
            }
            else
            {
                m_CurrentRadioStation = m_RadioStations[aRadioStation];
                m_CurrentRadioId = m_RadioStationIds.IndexOf(aRadioStation);
                if (aAutoResume)
                {
                    Radio_ResumeMusic();
                }
            }
        }

        //--------------------------------------------------------------------------------------------

        ///<summary>Get the next radio station that have music in it, will skip others</summary>
        private RadioData Radio_GetNextStation()
        {
            
            if (m_CurrentRadioId == m_RadioStationIds.Count-1)
            {
                m_CurrentRadioId = 0;
            }
            else
            {
                m_CurrentRadioId++;
            }

            return m_RadioStationIds[m_CurrentRadioId];
        }

        ///<summary>Get the previous radio station that have music in it, will skip others</summary>
        private RadioData Radio_GetPreviousStation()
        {
            if (m_CurrentRadioId == 0)
            {
                m_CurrentRadioId = m_RadioStationIds.Count - 1;
            }
            else
            {
                m_CurrentRadioId--;
            }

            return m_RadioStationIds[m_CurrentRadioId];
        }

        ///<summary>Set the new music in the radio and play it</summary>
        private void Radio_ChangeMusic(MusicData aNewMusic, float aTimeElapsed)
        {
            m_CurrentRadioStation.isPlaying = true;
            m_RadioPlayer.clip = aNewMusic.GetClip();
            m_RadioPlayer.time = aTimeElapsed;

            if(m_RadioDisplay != null && m_DisplayLength > 0)
            {
                m_RadioDisplay.ShowRadioInfo(aNewMusic, m_DisplayLength);
            }
            m_RadioPlayer.Play();
        }

        ///<summary>Update the music timer and change music when their are done</summary>
        private void Radio_UpdateTime()
        {
            if (m_CurrentRadioStation.isPlaying)
            {
                m_CurrentRadioStation.timeElapsed += Time.deltaTime;
            }
            if (m_CurrentRadioStation.timeElapsed > m_CurrentRadioStation.StationMusics[m_CurrentRadioStation.CurrentMusicId].GetClip().length)
            {
                Radio_NextMusic();
                m_CurrentRadioStation.timeElapsed = 0.0f;
            }
        }

        #endregion


        ///<summary>
        /// The option system lets you change volume for SFX, music, voice and master. You can also mute them if you want
        ///</summary>
        #region OPTION Functions

        private void InitOptionSystem()
        {
            if (m_MasterGroup == null || m_VoiceGroup == null || m_MusicGroup == null || m_SFXGroup == null)
            {
                Debug.Log(Debug_InitFailed("Can't init the option system, there's one or more groups missing, drop them."));
                return;
            }
            m_OptionIsInit = true;
            Debug.Log(Debug_InitSucces("Option system as been init"));
        }

        //--------------------------------------------------------------------------------------------

        ///<summary>Will change the Music volume of the game 
        ///<param name="aVolume">The new volume level (0 to 1)</param>
        ///</summary>
        public void Option_SetMusicGroupVolume(float aVolume)
        {
            if (!Option_IsInit(true))
            {
                return;
            }
            m_MusicGroup.audioMixer.SetFloat("musicVolume", CheckVolume(aVolume));
        }

        ///<summary>Will change the SFX volume of the game 
        ///<param name="aVolume">The new volume level (0 to 1)</param>
        ///</summary>
        public void Option_SetSFXGroupVolume(float aVolume)
        {
            if (!Option_IsInit(true))
            {
                return;
            }
            m_SFXGroup.audioMixer.SetFloat("sfxVolume", CheckVolume(aVolume));
        }

        ///<summary>Will change the Master volume of the game 
        ///<param name="aVolume">The new volume level (0 to 1)</param>
        ///</summary>
        public void Option_SetMasterGroupVolume(float aVolume)
        {
            if (!Option_IsInit(true))
            {
                return;
            }
            m_MasterGroup.audioMixer.SetFloat("masterVolume", CheckVolume(aVolume));
        }

        ///<summary>Will change the Voice volume of the game 
        ///<param name="aVolume">The new volume level (0 to 1)</param>
        ///</summary>
        public void Option_SetVoiceGroupVolume(float aVolume)
        {
            if (!Option_IsInit(true))
            {
                return;
            }
            m_VoiceGroup.audioMixer.SetFloat("voiceVolume", CheckVolume(aVolume));
        }

        //--------------------------------------------------------------------------------------------

        ///<summary>Check if the volume set in parameter for the option is between 0 and 1, if not, adjust it</summary>
        private float CheckVolume(float aVolume)
        {
            if (aVolume > 0.0f)
            {
                return 0.0f;
            }
            else if (aVolume < -80.0f)
            {
                return -80.0f;
            }

            return aVolume;
        }

        #endregion

        ///<summary>
        /// The foot sound system lets you play foot sound from any where in the game.
        /// The sound will be play automaticly with the right floor material (Wood, Water, Grass, etc.) [You cann add ground type by editing the eAudioM_GroundType in the 'Enums' script]
        /// It will also be played correctly with 3D effect (right or left foot)
        ///</summary>
        #region FootSound Functions

        private void InitFootSound()
        {
            if (!m_SFXIsInit)
            {
                Debug.Log(Debug_InitFailed("Can't init the FootSound System, you need to enable SFX system to be able to use it."));
                return;
            }
            if (m_FootSoundsData == null)
            {
                Debug.LogError(Debug_InitFailed("Can't init the FootSound System, there's no FootSounds Data in the m_FootSoundsData component slot! Drop one"));
                return;
            }

            m_FootIsInit = true;
            Debug.Log(Debug_InitSucces("FootSound System as been init"));
        }

        //--------------------------------------------------------------------------------------------

        ///<summary> 
        ///(Sphere Audio) Instiante a foot sound depend on which ground you are on, play it and then destroy it
        ///<param name="aPosition">The position where the sound will be instantiate</param>
        ///<param name="aGroundType">The type of ground you are moving on</param>
        ///<param name="aMovementType">The type of movement you are doing</param>
        ///<param name="aFootDir">The foot that will touch the ground and make sound</param>
        ///</summary>
        public void FootSound_Play(Vector3 aPosition, eAudioM_GroundType aGroundType, eAudioM_FootSoundType aMovementType, eAudioM_FootSoundDir aFootDir)
        {
            if (!FootSound_IsInit(true))
            {
                return;
            }

            AudioClip clip = null;

            switch (m_FootAudioSelector)
            {
                case eAudioM_AudioSelector.AudioClip:
                    {
                        clip = m_FootSoundsData.GetFootSound(aGroundType, aMovementType);
                        break;
                    }
                case eAudioM_AudioSelector.AudioData:
                    {
                        AudioData data = m_FootSoundsData.GetFootSoundData(aGroundType, aMovementType);
                        if (data == null)
                        {
                            Debug.LogWarning(Debug_Warning("There's no AudioData available for the ground '" + aGroundType + "' and the movement type '" + aMovementType + "'"));
                            return;
                        }
                        data.StereoPan = FootSound_GetStereoPan(aFootDir);
                        SoundEasy_Play(data, aPosition);
                        return;
                    }
            }

            if (clip == null)
            {
                Debug.LogWarning(Debug_Warning("There's no sound to play"));
                return;
            }

            GameObject obj = new GameObject();
            obj.transform.position = aPosition;
            SFX_AudioSetup audio = obj.AddComponent<SFX_AudioSetup>();

            audio.SetupAudio(clip, m_FootSoundsData.GetFootSoundVolume(aGroundType, aMovementType), 1.0f, 1.0f, FootSound_GetStereoPan(aFootDir));
            audio.PlayAudio(0);
        }

        ///<summary>Return the correct stereoPan with the ask footSound direction</summary>
        private float FootSound_GetStereoPan(eAudioM_FootSoundDir aDir)
        {
            float stereoPan = 0.0f;
            switch (aDir)
            {
                case eAudioM_FootSoundDir.Left:
                    {
                        stereoPan = -0.1f;
                        break;
                    }
                case eAudioM_FootSoundDir.Right:
                    {
                        stereoPan = 0.1f;
                        break;
                    }
            }

            return stereoPan;
        }


        #endregion
    }
}

