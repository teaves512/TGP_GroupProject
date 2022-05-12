using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour
{
    // -------------------------------------------------------------------

    [SerializeField]
    private CameraShiftPosition[] m_CameraShiftPositionTriggers;

    [SerializeField]
    private Transform m_CameraTransform;

    private bool m_MovingCamera;
    private int  m_PositionIndexToMoveTo;

    [SerializeField]
    private float m_CameraShiftSpeed = 1.0f;

    // -------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // If moving the camera to the new position
        if(m_MovingCamera)
        {
            if ((m_CameraShiftPositionTriggers[m_PositionIndexToMoveTo].m_PositionToMoveCameraTo.position - m_CameraTransform.position).magnitude < 1.0f)
            {
                m_MovingCamera          = false;
                m_PositionIndexToMoveTo = 0;
                return;
            }

            float            direction = m_CameraShiftPositionTriggers[m_PositionIndexToMoveTo].m_PositionToMoveCameraTo.position.x - m_CameraTransform.position.x;

            m_CameraTransform.position = new Vector3(m_CameraTransform.position.x + direction * Time.deltaTime * m_CameraShiftSpeed, m_CameraTransform.position.y, m_CameraTransform.position.z);
        }
    }

    // -------------------------------------------------------------------

    public void MoveCameraTo(string name)
    {
        // Find the collider we are going to move to
        for(int i = 0; i < m_CameraShiftPositionTriggers.Length; i++)
        {
            if(m_CameraShiftPositionTriggers[i].name == name)
            {
                m_MovingCamera          = true;
                m_PositionIndexToMoveTo = i;
                return;
            }
        }
    }

    // -------------------------------------------------------------------
}
