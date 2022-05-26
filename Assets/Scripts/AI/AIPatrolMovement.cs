using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ThisGameObject;

    private Vector3 m_TargetPosition;

    [SerializeField]
    private float c_MovementSpeed;

    private float m_DistanceCap = 0.5f;

    // -----------------------------------------------------------------

    private void Start()
    {
        m_TargetPosition = m_ThisGameObject.transform.position;
    }

    public void SetTargetPosition(Vector3 position)
    {
        m_TargetPosition = position;
    }

    // -----------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        float distance = (m_TargetPosition - m_ThisGameObject.transform.position).magnitude;

        if (distance < m_DistanceCap)
            return;

        Vector3 moveDirection               = (m_TargetPosition - m_ThisGameObject.transform.position).normalized;

        m_ThisGameObject.transform.position += moveDirection * Time.deltaTime * c_MovementSpeed;

        m_ThisGameObject.transform.LookAt(new Vector3(m_TargetPosition.x, m_ThisGameObject.transform.position.y, m_TargetPosition.z), Vector3.up);
    }

    // -----------------------------------------------------------------
}
