using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [System.Serializable]
    public class sTipGroup
    {
        public List<LoadingTipData> TipData = new List<LoadingTipData>();
    }

    [CreateAssetMenu(menuName = "ManagersHUB/SceneManager/GroupTipsData", fileName = "New GroupTipsData")]
    public class GroupTipsData : ScriptableObject
    {
        [SerializeField]
        private List<sTipGroup> m_TipDatas;

        ///<summary>Return a random tip from a select group of tip</summary>
        public LoadingTipData GetTip(eSceneM_TipGroup aGroup)
        {
            if(m_TipDatas[(int)aGroup].TipData.Count != 0)
            {
                return m_TipDatas[(int)aGroup].TipData[Random.Range(0, m_TipDatas[(int)aGroup].TipData.Count - 1)];
            }

            Debug.LogError("There's no tip in the group: " + aGroup);
            return null;
        }

        ///<summary>Return all tip datas in this ScriptableObject</summary>
        public List<sTipGroup> GetAllTipDatas()
        {
            if(m_TipDatas == null || m_TipDatas.Count == 0)
            {
                Debug.Log("New init");
                m_TipDatas = new List<sTipGroup>();
                for(int i = 0; i < EnumSystem.GetEnumCount(typeof(eSceneM_TipGroup)); i++)
                {
                    m_TipDatas.Add(new sTipGroup());
                }
            }
            return m_TipDatas;
        }

        #region EDITOR
        ///<summary> EDITOR ONLY! Set the new list of tip after the inspector edit it</summary>
        public void EDITOR_SetAllTipDatas(List<sTipGroup> aDatas)
        {
            m_TipDatas = aDatas;
        }
        #endregion
    }
}
