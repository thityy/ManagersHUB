using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagersHUB
{
    public class GroundSelector : MonoBehaviour
    {
        [SerializeField]
        private eAudioM_GroundType m_GroundType = eAudioM_GroundType.Grass;

        public eAudioM_GroundType GetGroundType()
        {
            return m_GroundType;
        }
    }
}


