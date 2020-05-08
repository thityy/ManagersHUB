using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class PoolManager : Singleton<PoolManager>
    {
        private PoolData m_CurrentData = null;

        private List<sPool> m_Pools = new List<sPool>();

        private Dictionary<GameObject, List<GameObject>> m_PoolsInactiveObjects = new Dictionary<GameObject, List<GameObject>>();
        private Dictionary<GameObject, List<GameObject>> m_PoolsActiveObjects = new Dictionary<GameObject, List<GameObject>>();


        protected override void Awake()
        {
            base.Awake();
            InitDebuging(eManagers.PoolManager);
            Debug.Log(Debug_InitSucces("Pool Manager as been init"));
        }

        ///<summary> Initialise a new pools object </summary>
        public void InitPools(PoolData aPoolData)
        {
            ClearOlderPools();
            m_CurrentData = aPoolData;
            m_Pools = aPoolData.GetPoolList();
            CreatePools();
        }

        public void ClearOlderPools()
        {
            m_Pools.Clear();
            m_PoolsInactiveObjects.Clear();
            m_PoolsActiveObjects.Clear();
        }

        ///<summary>Create the pools for each object set in the data</summary>
        private void CreatePools()
        {
            for(int i = 0; i < m_Pools.Count; i++)
            {
                sPool pool = m_Pools[i];
                if(IsValidPoolObject(pool))
                {
                    m_PoolsInactiveObjects.Add(pool.prefab, new List<GameObject>());
                    m_PoolsActiveObjects.Add(pool.prefab, new List<GameObject>());
                    GrowPool(pool);
                }
            }
        }

        ///<summary> Check if the poolObject ask for is valid </summary>
        private bool IsValidPoolObject(sPool aPool)
        {
            return aPool.prefab != null;
        }   

        ///<summary>Extend the Pool by creating all poolObject ask for</summary>
        private void GrowPool(sPool aPool)
        {
            for(int i = 0; i < aPool.amount; i++)
            {
                CreatePooledObject(aPool.prefab);
            }
        }

        ///<summary>Create a gameObject with the ask prefab for the PoolManager</summary>
        private void CreatePooledObject(GameObject aPoolPrefab)
        {
            GameObject pooledObject = Instantiate(aPoolPrefab);
            PooledObjSettings pooledScript = pooledObject.AddComponent<PooledObjSettings>();

            if(pooledScript == null)
            {
                Debug.LogError(Debug_Error("Invalid Prefab for PoolManager"));
                Destroy(pooledObject);
            }
            else
            {
                pooledScript.InitPooledObject(aPoolPrefab);
                pooledObject.SetActive(false);

                m_PoolsInactiveObjects[aPoolPrefab].Add(pooledObject);
            }
        }

        ///<summary>Spawn the prefab ask for from the pool and return the component you want</summary>
        public T UseObjectFromPool<T>(GameObject aPrefab, Vector3 aPosition, Quaternion aRotation)
        {
            GameObject pooledObject = UseObjectFromPool(aPrefab, aPosition, aRotation);
            return pooledObject.GetComponent<T>();
        }

        ///<summary>Spawn the prefab you want from the pool and return it</summary>
        public GameObject UseObjectFromPool(GameObject aPrefab, Vector3 aPosition, Quaternion aRotation)
        {
            GameObject pooledObject = GetObjectFromPool(aPrefab);

            if (pooledObject != null)
            {
                pooledObject.transform.position = aPosition;
                pooledObject.transform.rotation = aRotation;
                pooledObject.SetActive(true);

                m_PoolsActiveObjects[aPrefab].Add(pooledObject);
            }

            return pooledObject;
        }

        ///<summary>Get the first inactive poolObject found</summary>
        private GameObject GetObjectFromPool(GameObject a_Prefab)
        {
            List<GameObject> pooledObjects = new List<GameObject>();
            m_PoolsInactiveObjects.TryGetValue(a_Prefab, out pooledObjects);

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            return NotEnoughPooledObject(a_Prefab);
        }

        //If there's no inactive poolObject -> Catch Action : Get oldest active pooledObject or DynamicSafeGrowPool 
        private GameObject NotEnoughPooledObject(GameObject aPrefab)
        {
            sPool pool = GetPoolStruct(aPrefab);

            if (IsValidPoolObject(pool))
            {
                if (!pool.overlapse)
                {
                    return UseFirstActivePooledObject(pool);
                }
                else
                {
                    return DynamicGrowPool(pool);
                }
            }

            return null;
        }

        ///<summary>Get the pool struct of the ask Prefab</summary>
        private sPool GetPoolStruct(GameObject aPoolPrefab)
        {
            for (int i = 0; i < m_Pools.Count; i++)
            {
                if (m_Pools[i].prefab.Equals(aPoolPrefab))
                {
                    return m_Pools[i];
                }
            }

            return new sPool() { prefab = null };
        }

        ///<summary>Get the oldest pooledobject to re-use it for a new one</summary>
        private GameObject UseFirstActivePooledObject(sPool aPool)
        {
            List<GameObject> activeObjects = new List<GameObject>();
            m_PoolsActiveObjects.TryGetValue(aPool.prefab, out activeObjects);

            if(activeObjects.Count != 0)
            {
                GameObject firstElement = activeObjects[0];
                activeObjects.RemoveAt(0);
                return firstElement;
            }

            return null;
        }

        ///<summary>create other poolobject if the pool doesn't have any free ones.
        ///This is just for safety of the runtime, but will require to change it in editor mode
        ///</summary>
        private GameObject DynamicGrowPool(sPool aPool)
        {
            Debug.LogErrorFormat("Pool Too Small: The Pool {0} is not big enough, Creating {1} more PooledObject! Increments the pool size!", aPool.prefab.name, aPool.amount);

            GrowPool(aPool);
            return GetObjectFromPool(aPool.prefab);
        }

        ///<summary>Manually return a PooledObject.</summary>
        public void ReturnedPooledObject(PooledObjSettings a_PooledObject, GameObject a_PoolOwner)
        {
            sPool pool = GetPoolStruct(a_PooledObject.gameObject);

            if (m_PoolsActiveObjects.ContainsKey(a_PooledObject.PoolOwner))
            {
                List<GameObject> activeObjects = m_PoolsActiveObjects[a_PoolOwner];

                if (activeObjects.Count != 0)
                {
                    activeObjects.Remove(a_PooledObject.gameObject);
                }
            }
        }

        
    }
}

