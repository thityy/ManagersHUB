using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagersHUB;

public class FootSoundCaller : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_GroundLayer = 0;
    [SerializeField]
    private Transform m_GroundCheck_LeftFootPoint = null;
    [SerializeField]
    private Transform m_GroundCheck_RightFootPoint = null;
    [SerializeField]
    private float m_DistanceFromGround = 1.0f;
    private RaycastHit m_Hit;

    public void PlayFootSound_Left(eAudioM_FootSoundType aType)
    {
        ManagersHUB.AudioManager.Instance.FootSound_Play(transform.position, GetGroundType(eAudioM_FootSoundDir.Left), aType, eAudioM_FootSoundDir.Left);
    }

    public void PlayFootSound_Right(eAudioM_FootSoundType aType)
    {
        ManagersHUB.AudioManager.Instance.FootSound_Play(transform.position, GetGroundType(eAudioM_FootSoundDir.Right), aType, eAudioM_FootSoundDir.Right);
    }

    private eAudioM_GroundType GetGroundType(eAudioM_FootSoundDir aDir)
    {
        Vector3 footPos = Vector3.zero;
        switch(aDir)
        {
            case eAudioM_FootSoundDir.Left:
            {
                if(m_GroundCheck_LeftFootPoint != null)
                {
                    footPos = m_GroundCheck_LeftFootPoint.position;
                }
                else
                {
                    Debug.Log("There's no Left foot point to check ground.");
                }
                
                break;
            }
            case eAudioM_FootSoundDir.Right:
            {
                if(m_GroundCheck_RightFootPoint != null)
                {
                    footPos = m_GroundCheck_RightFootPoint.position;
                }
                else
                {
                    Debug.Log("There's no Right foot point to check ground.");
                }
                break;
            }
        }
        if(Physics.Raycast(footPos, Vector3.down, out m_Hit, m_DistanceFromGround, m_GroundLayer))
        {
            GroundSelector ground = m_Hit.transform.GetComponent<GroundSelector>();
            if(ground != null)
            {
                return ground.GetGroundType();
            }
        }

        return eAudioM_GroundType.Grass;
    }
}
