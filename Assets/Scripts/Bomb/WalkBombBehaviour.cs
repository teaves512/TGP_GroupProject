using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBombBehaviour : MainBombBehaviour
{
	[Header("Walk stats")]
	[SerializeField] private float m_WalkSpeed;
	[SerializeField] private float m_Dampening = 10.0f;
	[SerializeField] private Collider[] hitColliders;
	[SerializeField] private GameObject m_Player;
	[HideInInspector] private Vector3 m_PlayerForward;
	[HideInInspector] private Rigidbody m_RB;

	protected override void Start()
	{
		m_RB = GetComponent<Rigidbody>();
		m_Player = GameObject.FindGameObjectWithTag("Player");
		transform.eulerAngles = new Vector3 (transform.rotation.eulerAngles.x, m_Player.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		m_PlayerForward = m_Player.transform.forward;

		transform.position = new Vector3(transform.position.x, transform.position.y+0.4f, transform.position.z)+m_PlayerForward;
	}

	private void FixedUpdate()
	{
		Walk();
	}
	// Update is called once per framed
	protected override void Update()
	{
		if (Input.GetKeyDown("l") && !m_Exploded)
		{
			StartCoroutine(Explode());
		}
		if (m_Timer < 0 && !m_Exploded)
		{
			StartCoroutine(Explode());
		}

		base.Update();
	}
	private void Walk()
	{
		Vector3 direction = m_PlayerForward.normalized;
		Vector3 velocity = direction * m_WalkSpeed;
		velocity.y = m_RB.velocity.y;

		//interpolate towards the target velocity, from the current velocity
		m_RB.velocity = Vector3.Lerp(m_RB.velocity, velocity, m_Dampening * Time.fixedDeltaTime);
	}
	protected override IEnumerator Explode()
	{
		m_Exploded = true;
		hitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y * 2, gameObject.GetComponent<Collider>().transform.localScale.z * 2), transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent enemyHealthScript = nearbyOject.GetComponent<HealthComponent>();
			BossBehaviour bossBehaviourScript = nearbyOject.GetComponent<BossBehaviour>();
			if (destructableScript != null)
			{
				destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)), true);
			}
			else if (enemyHealthScript != null)
			{
				enemyHealthScript.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			}
			else if (bossBehaviourScript)
			{
				bossBehaviourScript.TakeDamage();
				nearbyOject.tag = "Placeable";
			}
		}
		//yield return new WaitForSeconds(m_DestroyDelay);
		StartCoroutine(base.Explode());

		yield return null;
	}
	private float GetDamage(float distanceTo)
	{
		return m_Damage / distanceTo;
	}
	private void OnTriggerEnter(Collider other)
	{
		
	}
}
