using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class RadioDisplay : MonoBehaviour
    {

        [Header("Base setting(s)")]
        [SerializeField]
        [Tooltip("[Parent]\n RadioDisplay is the gameObject parent that have all object that need to be display")]
        private GameObject m_RadioDisplay = null;                              //The display element that will be show to the player when you change music or radio station
        [SerializeField]
        [Tooltip("[Child of RadioDisplay]\n ArtistNameDisplay is the object TextMeshProUGUI that will show the artist name")]
        private TMPro.TextMeshProUGUI m_ArtistNameDisplay = null;              //The display element that show the name of the artist
        [SerializeField]
        [Tooltip("[Child of RadioDisplay]\n SongNameDisplay is the object TextMeshProUGUI that will show the song name")]
        private TMPro.TextMeshProUGUI m_SongNameDisplay = null;                //The display element that show the name of a song
        [SerializeField]
        [Tooltip("[Child of RadioDisplay]\n AlbumCoverDisplay is the object Image that will show the sprite of the album cover")]
        private UnityEngine.UI.Image m_AlbumCoverDisplay = null;               //The display element that show the album cover


        //_______________________ IMPORTANT ____________________\\
        /*
            This script doesn't have a custom editor to make it simple for you to edit as you wish! You can add function to it without no problem.
            The only thing you need to don't brake, it's the "ShowRadioInfo(MusicData aData, float aLapse)" function, cause it's the function that
            get call by the manager; but you can change what this function do if you wish!
        */

        private void Awake()
        {
            HideDisplay();
        }

        ///<summary>Show the radio information to the player </summary>
        public void ShowRadioInfo(MusicData aData, float aLapse)
        {
            StopAllCoroutines();

            ChangeInfo(aData);
            m_RadioDisplay.SetActive(true);

            StartCoroutine(ShowRadio(aLapse));
        }

        ///<summary> Change the info display on the screen with the new one </summary>
        private void ChangeInfo(MusicData aData)
        {
            if (m_ArtistNameDisplay != null)
            {
                m_ArtistNameDisplay.text = aData.GetArtistName();
            }
            if (m_SongNameDisplay != null)
            {
                m_SongNameDisplay.text = aData.GetSongName();
            }
            if(m_AlbumCoverDisplay != null)
            {
                m_AlbumCoverDisplay.sprite = aData.GetClipArt();
            }
        }

        ///<summary> Show the displayer on the screen for the amount of time ask (aLapse) </summary>
        private IEnumerator ShowRadio(float aLapse)
        {
            yield return new WaitForSeconds(aLapse);
            HideDisplay();
        }

        ///<summary> Hide the displayer from the screen </summary>
        private void HideDisplay()
        {
            m_RadioDisplay.SetActive(false);
        }
    }
}

