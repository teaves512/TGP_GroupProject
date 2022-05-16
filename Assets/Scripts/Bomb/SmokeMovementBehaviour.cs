using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMovementBehaviour : MonoBehaviour
{
	[SerializeField] public bool m_Move;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_FireMovementSpeed;
	[SerializeField] private GameObject m_SmokeObject;

    // Update is called once per frame
    void Update()
    {
        if(m_Move)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y -(m_FireMovementSpeed *Time.deltaTime), transform.position.z );
			m_SmokeObject.transform.position = new Vector3(m_SmokeObject.transform.position.x, m_SmokeObject.transform.position.y - (m_MoveSpeed * Time.deltaTime), m_SmokeObject.transform.position.z);
		}
    }
}
