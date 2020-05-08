using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class SpawnUser : MonoBehaviour
    {
        [SerializeField]
        private eSpawnM_Teams m_UserTeam = eSpawnM_Teams.Team01;
        [SerializeField]
        private eSpawnM_Squads m_UserSquad = eSpawnM_Squads.SquadAlpha;
        [SerializeField]
        [Range(0.1f, 100f)]
        private float m_RadiusNeededToSpawn = 1.0f;
        [SerializeField]
        private bool m_AutoSpawn;
        [SerializeField]
        private eSpawnM_Types m_AutoSpawnType;

        private void OnEnable()
        {
            if (m_AutoSpawn)
            {
                AutoSpawnWhenEnable();
            }
        }

        ///<summary>Spawn when the object became enable, useful when use the pool manager</summary>
        private void AutoSpawnWhenEnable()
        {
            switch (m_AutoSpawnType)
            {
                case eSpawnM_Types.Spawn_OnTeam:
                    {
                        SpawnManager.Instance.Spawn_OnTeam(this);
                        break;
                    }
                case eSpawnM_Types.Spawn_OnSquad:
                    {
                        SpawnManager.Instance.Spawn_OnSquad(this);
                        break;
                    }
                case eSpawnM_Types.Spawn_Safe:
                    {
                        SpawnManager.Instance.Spawn_Safe(this);
                        break;
                    }
                case eSpawnM_Types.Spawn_Hostile:
                    {
                        SpawnManager.Instance.Spawn_Hostile(this);
                        break;
                    }
            }
        }

        public eSpawnM_Teams GetUserTeam()
        {
            return m_UserTeam;
        }

        public eSpawnM_Squads GetUserSquad()
        {
            return m_UserSquad;
        }

        public void ChangeTeam(eSpawnM_Teams a_NewTeam)
        {
            m_UserTeam = a_NewTeam;
        }

        public void ChangeSquad(eSpawnM_Squads a_NewSquad)
        {
            m_UserSquad = a_NewSquad;
        }

        public void ChangeRadiusOfSpawn(float a_NewRadius)
        {
            if (a_NewRadius < 0.1f)
            {
                a_NewRadius = 0.1f;
            }

            m_RadiusNeededToSpawn = a_NewRadius;
        }
    }
}

