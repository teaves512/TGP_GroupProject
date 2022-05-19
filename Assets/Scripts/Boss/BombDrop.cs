using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour
{
	[SerializeField] private GameObject m_DropMarker;

	// Start is called before the first frame update
	void Start()
    {
		m_DropMarker.transform.position = new Vector3( transform.position.x,0.0f, transform.position.z);
		m_DropMarker.transform.parent = null;
	}

    // Update is called once per frame
    void Update()
    {
        if(transform.GetComponent<TankSpikeDrop>().m_IsGrounded)
		{
			Destroy(m_DropMarker);
		}
    }
}
