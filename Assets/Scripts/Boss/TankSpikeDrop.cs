using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpikeDrop : MonoBehaviour
{
	[Header("Asset breakdown")]
	[SerializeField] private GameObject m_Prefab;
	[SerializeField] private GameObject m_Parachute;
	[SerializeField] private GameObject m_Spike;

	[SerializeField] private float m_DistanceToGround;
    // Start is called before the first frame update
    void Start()
    {
		m_DistanceToGround = transform.GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckGround())
		{
			PlayParachute();
		}
    }
	private bool CheckGround()
	{
		return Physics.Raycast(transform.position, -Vector3.up, m_DistanceToGround);
	}	

	private void PlayParachute()
	{
		Debug.Log("Flop");
	}
}
