using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ManagersHUB
{

    public class LoadingBar : MonoBehaviour
    {
        [SerializeField]
        private Image[] m_Bars = new Image[]{};
        [SerializeField]
        private Image[] m_Borders = new Image[]{};
        [SerializeField]
        private Color m_BarsColor = Color.black;
        [SerializeField]
        private Color m_BorderColor = Color.black;

        private float m_FillValue = 0.0f;
        
        private void Awake()
        {
            InitialiseColor();
        }

        private void InitialiseColor()
        {
            for (int i = 0; i < m_Bars.Length; i++)
            {
                m_Bars[i].color = m_BarsColor;
            }
            for (int i = 0; i < m_Borders.Length; i++)
            {
                m_Borders[i].color = m_BorderColor;
            }
        }

        public void UpdateFill(float aCurrentValue, float m_MinValue, float aMaxValue)
        {
            m_FillValue = 1f/(aMaxValue - m_MinValue);
            m_FillValue *= (aCurrentValue-m_MinValue);
            for(int i = 0; i < m_Bars.Length; i++)
            {
                m_Bars[i].fillAmount = m_FillValue;
            }
        }

    }
}
