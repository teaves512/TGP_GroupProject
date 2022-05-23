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
    [HideInInspector] private bool m_Explosive;
	[HideInInspector] private ExplosiveEnviro m_ExplosiveScript;


	private void Start()
	{
		m_Health = m_MaxHealth;
		m_RegenTimer = m_MaxRegenTimer;
		m_Material = GetComponent<Renderer>()?.material;
		if(GetComponent<ExplosiveEnviro>())
		{
			m_Explosive = true;
			m_ExplosiveScript = GetComponent<ExplosiveEnviro>();
		}

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
			if(!m_ExplosiveScript.m_Exploded)
				m_ExplosiveScript.Explode();
        }
		if (m_DeathEffect != null)
			m_DeathEffect.Play();
		yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        yield return null;
    }



}
