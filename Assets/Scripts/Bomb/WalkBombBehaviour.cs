using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBombBehaviour : MainBombBehaviour
{
	[Header("Walk stats")]
	[SerializeField] private float m_WalkSpeed;
	[SerializeField] private Collider[] hitColliders;


	// Update is called once per frame
	protected override void Update()
	{
		if (Input.GetKeyDown("l"))
		{
			StartCoroutine(Explode());
		}
		transform.position += Vector3.forward * Time.deltaTime * m_WalkSpeed;
		if (m_Timer < 0 && !m_Exploded)
		{
			StartCoroutine(Explode());
		}

		base.Update();
	}

	protected override IEnumerator Explode()
	{
		m_Exploded = true;
		hitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z), transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent enemyHealthScript = nearbyOject.GetComponent<HealthComponent>();
			if (destructableScript != null)
			{
				destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			}
			else if (enemyHealthScript != null)
			{

				enemyHealthScript.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
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
}
