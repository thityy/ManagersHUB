using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class Pool_Creator : MonoBehaviour
    {
        private bool m_IsDoneInstantiate = false;
        public bool IsDoneInstantiate
        {
            get{return m_IsDoneInstantiate;}
        }

        [SerializeField]
        private PoolData m_Data = null;
        [SerializeField]
        private bool m_ShowPreview = false;

        private void Awake()
        {
            if(m_Data != null)
            {
                PoolManager.Instance.InitPools(m_Data);
            }
            else
            {
                m_ShowPreview = false;
                Debug.Log("There's no pool data in the Pool Creator");
            }

            //Only to erase the warning
            if(m_ShowPreview)
            {
                m_ShowPreview = true;
            }
        }
    }

}

