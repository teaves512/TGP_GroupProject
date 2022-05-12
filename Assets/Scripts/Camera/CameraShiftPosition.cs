using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShiftPosition : MonoBehaviour
{
    // -------------------------------------------------------

    [SerializeField]
    private BoxCollider m_BoxTriggerArea;

    [SerializeField]
    public Transform m_PositionToMoveCameraTo;

    [SerializeField]
    private CameraShifting m_CameraShifter;

    // -------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // -------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        
    }

    // -------------------------------------------------------

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(m_CameraShifter)
            {
                m_CameraShifter.MoveCameraTo(name);
            }
        }
    }

    // -------------------------------------------------------
}
