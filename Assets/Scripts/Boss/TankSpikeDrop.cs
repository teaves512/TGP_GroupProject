using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpikeDrop : MonoBehaviour
{
	[Header("Asset breakdown")]
	[SerializeField] private GameObject m_Prefab;
	[SerializeField] private GameObject m_Parachute;

	[SerializeField] private float m_DestroyDelay = 2.0f;
	[SerializeField] private float m_DistanceToGround;
	public bool m_IsGrounded = false;

	private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
		m_DistanceToGround = transform.GetComponent<Collider>().bounds.extents.y;

		m_Animator = GetComponent<Animator>();

		RandomiseRotation();
    }

	private void RandomiseRotation()
	{
		Vector3 rot = new Vector3(0.0f,
			Random.Range(-360.0f, 360.0f), 0.0f);
		transform.rotation = Quaternion.Euler(rot);
	}

    // Update is called once per frame
    void Update()
    {
		CheckGround();
        if(m_IsGrounded)
		{
			PlayParachute();
		}
    }
	public void CheckGround()
	{
		m_IsGrounded = Physics.Raycast(transform.position, -Vector3.up, m_DistanceToGround);
	}	

	private void PlayParachute()
	{
		GetComponent<Rigidbody>().mass = 100.0f;
		//Debug.Log("Flop");
		m_Animator.SetTrigger("Flop");
		Invoke("DestroyChute", m_DestroyDelay);
	}

	private void DestroyChute()
	{
		if (m_Parachute) { m_Parachute.SetActive(false); }
	}
}
