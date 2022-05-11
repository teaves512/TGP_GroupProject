using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ThisGameObject;

    private Transform m_TargetPosition;

    [SerializeField]
    private float c_MovementSpeed;

    // -----------------------------------------------------------------

    private void Start()
    {
        m_TargetPosition = m_ThisGameObject.transform;
    }

    public void SetTargetPosition(Transform position)
    {
        m_TargetPosition = position;
    }

    // -----------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection               = (m_TargetPosition.position - m_ThisGameObject.transform.position).normalized;

        m_ThisGameObject.transform.position += moveDirection * Time.deltaTime * c_MovementSpeed;

        m_ThisGameObject.transform.LookAt(new Vector3(m_TargetPosition.position.x, m_ThisGameObject.transform.position.y, m_TargetPosition.position.z), Vector3.up);
    }

    // -----------------------------------------------------------------
}
