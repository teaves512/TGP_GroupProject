using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnviro : MonoBehaviour
{
	private LightFlash m_LightFlashScript;
	private Destructable m_DestructableScript;
	[Header("Explosive")]
	[HideInInspector] public bool m_Exploded = false;
	[SerializeField] private bool m_ProximityEnabled;
	[SerializeField] private float m_Damage;
	[SerializeField] private int m_ColliderScale;

	void Start()
	{
		if(m_ProximityEnabled)
			m_LightFlashScript = GetComponentInChildren<LightFlash>();
		m_DestructableScript = GetComponent<Destructable>();

	}
	public void Explode() //same as area effect
	{
		m_Exploded = true;
		Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3
			(gameObject.GetComponent<Collider>().transform.localScale.x * m_ColliderScale, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z * m_ColliderScale),
			transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent healthScript = nearbyOject.GetComponent<HealthComponent>();
			if (destructableScript != null)
			{
				destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
				nearbyOject.tag = "Placeable";
			}
			else if (healthScript != null)
			{

				healthScript.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
				Debug.Log("Hit for = " + GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			}
		}
	}
	private float GetDamage(float distanceTo)
	{
		return m_Damage / distanceTo;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			m_LightFlashScript.Detonate();
			m_DestructableScript.TakeDamage(300.0f);
		}
	}
}
