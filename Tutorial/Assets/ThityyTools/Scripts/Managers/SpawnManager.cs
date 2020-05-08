using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        private bool m_IsInit = false;          //If the manager is init.
        private List<SpawnPoint> m_Spawns;

        protected override void Awake()
        {
            base.Awake();
            InitDebuging(eManagers.SpawnManager);
            m_Spawns = new List<SpawnPoint>();
        }

        ///<summary> Return TRUE if the SpawnManager as been correctly init </summary>
        private bool SpawnManager_IsInit(bool aShowWarning)
        {
            if (!m_IsInit && aShowWarning)
            {
                Debug.LogWarning(Debug_Warning("The 'Spawn Manager' isn't initialize."));
            }
            return m_IsInit;
        }

        ///<summary>(USE BY SPAWN POINT) Tell the manager they exist so we can use them to spawn object</summary>
        public void SetNewSpawnPoint(SpawnPoint a_Spawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
        }

        ///<summary>(USE BY SPAWN POINT) Tell the manager they can't be use to spawn object anymore</summary>
        public void RemoveSpawnPoint(SpawnPoint a_Spawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
        }

        ///<suumary>(USE BY THE SCENE MANAGER OF UNITY) Erase all current spawnPoint(s) from the Spawn Manager</summary>
        private void ResetAllSpawnPoint()
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
            m_Spawns.Clear();
        }

        ///<summary>Spawn the object (Player, PNJ, etc.) on a SpawnPoint that is set as Initialisation Spawn. Use to spawn object at a specific position the first time they appear in game.</summary>
        public void Spawn_InitGame(SpawnUser a_UserToSpawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
            eSpawnM_Teams userTeam = a_UserToSpawn.GetUserTeam();
            for(int i = 0; i < m_Spawns.Count; i++)
            {
                if(m_Spawns[i].CanUseForInitSpawn(userTeam))
                {
                    a_UserToSpawn.transform.position = m_Spawns[i].transform.position;
                    a_UserToSpawn.transform.rotation = m_Spawns[i].transform.rotation;
                    a_UserToSpawn.transform.gameObject.SetActive(true);
                }
            }
        }

        ///<summary>Spawn the object (Player, PNJ, etc.) on a SpawnPoint that is currently safe. Close to allies, but far from ennemies (if possible)</summary>
        public void Spawn_Safe(SpawnUser a_UserToSpawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
        }

        ///<summary>Spawn the object (Player, PNJ, etc.) on a SpawnPoint that is currently hostile. Close to ennemies, but far from allies (if possible)</summary>
        public void Spawn_Hostile(SpawnUser a_UserToSpawn)
        {   
            if(!SpawnManager_IsInit(true))
            {
                return;
            }   
        }

        ///<summary>Spawn the object (Player, PNJ, etc.) next to an object of the same team. Spawn for the safest one</summary>
        public void Spawn_OnTeam(SpawnUser a_UserToSpawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
        }

        ///<summary>Spawn the object (Player, PNJ, etc.) next to an object of the same squad. Spawn on the safest one</summary>
        public void Spawn_OnSquad(SpawnUser a_UserToSpawn)
        {
            if(!SpawnManager_IsInit(true))
            {
                return;
            }
        }

    }

}

