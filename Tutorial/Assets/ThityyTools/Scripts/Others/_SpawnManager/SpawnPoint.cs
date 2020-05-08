using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    [RequireComponent(typeof(SphereCollider))]
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private bool m_IsUseForInit = false;            //If the spawn is use to spawn the player the first time he arrived in game.
        [SerializeField]
        private eSpawnM_Teams m_InitTeam = eSpawnM_Teams.Team01;       //The team that can use this spawn for initialisation.

        [SerializeField]
        private bool m_UseSquadInit = false;    //If the init spawn want to use squad specific option.
        [SerializeField]
        private eSpawnM_Squads m_InitSquad = eSpawnM_Squads.SquadAlpha;//The squad that can use this spawn for initialisation.
        [SerializeField]
        private bool m_BecameNormalSpawn;       //If the spawn can be use normaly after the first time spawn. (NEED IsUseForInit).

        private bool m_IsEmpty;                 //If empty, thats mean it's not safe or hostile, there's just nobody in it.
        private eSpawnM_Teams m_SafeForTeam;    //The team that have the most player around this spawn.

        private SphereCollider m_HostileZone;
        [SerializeField]
        private float m_HostileSize;


        private void Awake()
        {
            SpawnManager.Instance.SetNewSpawnPoint(this);
        }

        public bool IsAvailable()
        {
            //Regarde si l'objet peut apparaitre sans entrer en collision avec un autre joueur, si le spawn est disponible tout court, si oui il le fait apparaitre, sinon il retourne FALSE
            return false;
        }

        public bool CanUseForInitSpawn(eSpawnM_Teams a_UserTeam)
        {
            if (m_IsUseForInit && m_InitTeam == a_UserTeam && IsAvailable())
            {
                return true;
            }
            return false;
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (UnityEditor.EditorPrefs.GetBool("SpawnPoint_DebugHostileZone", true) && (m_BecameNormalSpawn || !m_IsUseForInit))
            {
                Color colorHostile = Color.red;
                colorHostile.a = 0.1f;
                Gizmos.color = colorHostile;
                Gizmos.DrawSphere(transform.position, m_HostileSize);
            }

            if (UnityEditor.EditorPrefs.GetBool("SpawnPoint_DebugPoint", true))
            {
                if (m_IsUseForInit)
                {
                    Gizmos.color = ColorManager.GetSpawnColor(m_InitTeam);
                    Gizmos.DrawSphere(transform.position, 0.500f);
                    if (m_UseSquadInit)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(transform.position, 0.125f);
                        Gizmos.color = ColorManager.GetSpawnColor(m_InitSquad);
                        Gizmos.DrawSphere(transform.position, 0.100f);
                    }
                }
                else
                {
                    Gizmos.color = ColorManager.GetColor(eManagers.SpawnManager, false);
                    Gizmos.DrawSphere(transform.position, 0.500f);
                }
            }
        }

    }

#endif

}

