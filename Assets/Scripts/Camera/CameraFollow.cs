using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float m_Dampening = 5.0f;
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_MinXPos = 30.0f;
    [SerializeField] private float m_MaxXPos = 100.0f;
    private Vector3 m_InitialPosition;

    private void Start()
    {
        m_InitialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (m_Target) { Follow(); }
        else { Debug.LogWarning("Camera target does not exist."); }
    }

    private void Follow()
    {
        Vector3 pos = m_InitialPosition;
        pos.x = Mathf.Clamp(m_Target.position.x, m_MinXPos, m_MaxXPos);

        transform.position = Vector3.Lerp(transform.position, pos, m_Dampening * Time.fixedDeltaTime);
    }
}
