using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ManagersHUB
{
    public class PooledObjSettings : MonoBehaviour
    {
        private GameObject m_PoolOwner;
        public GameObject PoolOwner
        {
            get { return m_PoolOwner; }
        }

        public virtual void InitPooledObject(GameObject a_PoolOwner)
        {
            m_PoolOwner = a_PoolOwner;
            gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            ManagersHUB.PoolManager.Instance.ReturnedPooledObject(this, PoolOwner);
        }
    }
}