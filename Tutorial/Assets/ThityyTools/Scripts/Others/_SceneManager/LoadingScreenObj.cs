using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenObj : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TipBox = null;
    [SerializeField]
    private TextMeshProUGUI m_TipTxtDisplay = null;

    public void ShowTipPox(string aTip)
    {
        m_TipTxtDisplay.text = aTip;
        m_TipBox.SetActive(true);
    }

    public void HideTipBox()
    {
        m_TipBox.SetActive(false);
    }
}
