using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [System.Serializable]
    public struct sPool
    {
        public GameObject prefab;
        public int amount;
        public bool overlapse;
    }

    
    [CreateAssetMenu(menuName = "ManagersHUB/PoolManager/PoolData", fileName = "new PoolData", order = 0)]
    public class PoolData : ScriptableObject
    {

        private void OnEnable()
        {
            if(m_Pools == null)
            {
                m_Pools = new List<sPool>();
            }
        }

        [SerializeField]
        private List<sPool> m_Pools;

        public List<sPool> GetPoolList()
        {
            return m_Pools;
        }
    }
}


