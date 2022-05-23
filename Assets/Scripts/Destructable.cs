using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
	[SerializeField] private float m_MaxHealth = 10.0f;
	[SerializeField] private float m_Health;
	[SerializeField] private float m_MaxRegenTimer = 5.0f;
	[SerializeField] private float m_RegenTimer;
    [SerializeField] private float m_RegenRate = 0.01f;
    [SerializeField] private Material m_Material;
    [SerializeField] private ParticleSystem m_DeathEffect;
	[Header("Explosive")]
    [SerializeField] private bool m_Explosive;
	[HideInInspector] private bool m_Exploded = false;
	[SerializeField] private float m_Damage;
	[SerializeField] private int m_ColliderScale;

	//check here later -> https://thomasmountainborn.com/2016/05/25/materialpropertyblocks/

	private void Start()
	{
		m_Health = m_MaxHealth;
		m_RegenTimer = m_MaxRegenTimer;
		m_Material = GetComponent<Renderer>()?.material;
	}
    private void Update()
    {
        if (m_Health < m_MaxHealth)
        {
            if(m_RegenTimer>0)
                m_RegenTimer -= 1* Time.deltaTime;
            if (m_RegenTimer <= 0)
            {
                HealDamageToMat();
            }
        }
    }

    public void TakeDamage(float damage)
    {
		m_Health -= damage;
		StartCoroutine(ApplyDamageToMat());
		if (m_Health<=0)
        {
            StartCoroutine(Death());
        }
	
    }

	private IEnumerator ApplyDamageToMat()
	{
        m_RegenTimer = m_MaxRegenTimer;
		m_Material?.SetFloat("DamageRatio", 1-(m_Health / m_MaxHealth));
		yield return null;
	}
	private void HealDamageToMat()
    {
        m_Health += m_RegenRate;
		m_Material?.SetFloat("DamageRatio", 1 - (m_Health / m_MaxHealth));
		
    }
    private IEnumerator Death()
    {
		if(m_Explosive)
        {
			if(!m_Exploded)
				Explode();
			if (gameObject.name == "SM_Veh_Truck_01" || gameObject.name == "SM_Veh_Tank_Russia_01")
				gameObject.SetActive(false);
        }
		if (m_DeathEffect != null)
			m_DeathEffect.Play();
		yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        yield return null;
    }

	private void Explode() //same as area effect
	{
		m_Exploded = true;
		Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * m_ColliderScale, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z * m_ColliderScale), transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent enemyHealthScript = nearbyOject.GetComponent<HealthComponent>();
			if (destructableScript != null)
			{
				destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
				nearbyOject.tag = "Placeable";
			}
			else if (enemyHealthScript != null)
			{

				enemyHealthScript.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			}
		}
	}
	private float GetDamage(float distanceTo)
	{
		return m_Damage / distanceTo;
	}

}
