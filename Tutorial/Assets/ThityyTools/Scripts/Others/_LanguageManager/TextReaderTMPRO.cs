using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ManagersHUB
{

    public class TextReaderTMPRO : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_TextDisplay = null;
        [SerializeField]
        private MultiLanguageData m_TextData = null;
        [SerializeField]
        private bool m_ShowPreview = false;
        public bool ShowPreview
        {
            get { return m_ShowPreview; }
        }

        private void Start()
        {
            try
            {
                m_TextDisplay = GetComponent<TextMeshProUGUI>();
            }
            catch
            {
                Debug.LogError("There's no TextMeshProUGUI on this gameObject -> " + gameObject.name + ", if you're using the Unity default Text Component, use TextReader instead.");
            }

            if (m_TextDisplay != null)
            {
                SetSpriteAsset();
                ShowTxt();
            }
        }

        private void SetSpriteAsset()
        {
            m_TextDisplay.spriteAsset = LanguageManager.Instance.Tag_GetSpriteAsset();
        }

        private void ShowTxt()
        {
            if (m_TextData != null)
            {
                m_TextDisplay.text = LanguageManager.Instance.Tag_GetTextWithSymbolInclude(m_TextData.GetText(LanguageManager.Instance.CurrentLanguage));
            }
            else
            {
                m_TextDisplay.text = LanguageManager.Instance.Tag_GetTextWithSymbolInclude(m_TextDisplay.text);
            }
        }

        public void SetNewText(MultiLanguageData aTextData)
        {
            m_TextData = aTextData;
            ShowTxt();
        }
    }
}