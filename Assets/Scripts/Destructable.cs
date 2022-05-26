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
	[Header("Regenable")]
	[SerializeField] private bool m_CanRegen = false;
	[SerializeField] private float m_DeathTime = 10.0f;
	[SerializeField] private float m_WallMoveSpeed;
	[SerializeField] private GameObject m_RiseParticle;
	[HideInInspector] private Vector3 m_OriginalPos;
	[HideInInspector] private Vector3 m_DeadPos;
	[HideInInspector] private bool m_MoveUp = false;
	[HideInInspector] private float m_T;
	[SerializeField]private UserManager m_userManager;
	


	private void Start()
	{
		m_OriginalPos = transform.position;
		m_DeadPos = new Vector3(transform.position.x, transform.position.y -2.2f, transform.position.z);
		m_Health = m_MaxHealth;
		m_RegenTimer = m_MaxRegenTimer;
		m_Material = GetComponent<Renderer>()?.material;
		if(GetComponent<ExplosiveEnviro>())
		{
			m_Explosive = true;
			m_ExplosiveScript = GetComponent<ExplosiveEnviro>();
		}
		m_userManager = FindObjectOfType<UserManager>();
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
		if(m_MoveUp)
        {
			m_T += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, m_OriginalPos, m_T * m_WallMoveSpeed);
			if (m_T > 1.0f)
			{
				m_T = 0;
				m_MoveUp = false;
				m_RiseParticle.SetActive(m_MoveUp);
			}
		}
    }

    public void TakeDamage(float damage, bool _isPlayer = default)
    {
	    bool isPlayer = _isPlayer;
		m_Health -= damage;
		StartCoroutine(ApplyDamageToMat());
		if (m_Health<=0)
        {
	        if (isPlayer)
	        {
				if (m_userManager)
				{
					m_userManager.m_User.PlayersAchievements.AddObjectsDestroyed();
					m_userManager.Save();
				}
	        }
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
		if (gameObject.name == "SM_Veh_Truck_01" || gameObject.name == "SM_Veh_Tank_Russia_01")
			gameObject.SetActive(false);

		if (m_DeathEffect != null)
			m_DeathEffect.Play();
		yield return new WaitForSeconds(0.3f);
		if (!m_CanRegen)
		{
			Destroy(gameObject);
		}
		else
		{
			m_Health = 0.0f;
			transform.position = m_DeadPos;
			yield return new WaitForSeconds(m_DeathTime);
			m_MoveUp = true;
			m_RiseParticle.SetActive(m_MoveUp);
			//m_RiseParticle.Play();
		}
        yield return null;
    }



}
