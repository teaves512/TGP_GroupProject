using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMovementBehaviour : MonoBehaviour
{
	[SerializeField] public bool m_Move;
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private GameObject m_SmokeObject;

    // Update is called once per frame
    void Update()
    {
        if(m_Move)
		{
			m_SmokeObject.transform.position = new Vector3(m_SmokeObject.transform.position.x, m_SmokeObject.transform.position.y -(m_MoveSpeed*Time.deltaTime), m_SmokeObject.transform.position.z );
		}
    }
}
