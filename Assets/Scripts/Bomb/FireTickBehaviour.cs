using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTickBehaviour : MonoBehaviour
{
	private Transform m_AreaElevator;
    [Header("Componenent to damage")]
    [HideInInspector] private Destructable m_DestructableScript;
    [HideInInspector] private HealthComponent m_HealthCompScript;
	[SerializeField] private bool m_AreaSpread;
    [Header("Stats from bomb")]
    [SerializeField] public float m_FireDamage;
	[SerializeField] private float m_FleshDamage;
    [SerializeField] public float m_MaxTimeOnFire;
    [HideInInspector] private float m_TimeOnFire;
	[SerializeField] private GameObject m_FireObject;
	[SerializeField] private float m_SmokeTime;
	[SerializeField] private GameObject m_SmokeObject;
	[Header("Self stats")]
    [SerializeField] private float m_FleshMultiplier;
    // Start is called before the first frame update
    void Start()
    {
		m_AreaElevator = transform.Find("Area elevator");
		if (!m_AreaSpread)
		{
			if (m_HealthCompScript == null && m_DestructableScript == null)
			{
				DestroySelf();
			}
		}
        m_TimeOnFire = m_MaxTimeOnFire;
		m_SmokeTime = m_MaxTimeOnFire/2;
		m_FleshDamage = m_FireDamage * m_FleshMultiplier;
	}

    // Update is called once per frame
    void Update()
    {
        if (m_TimeOnFire >= 0)
        {
            if(m_DestructableScript)
                m_DestructableScript.TakeDamage(m_FireDamage);
            if(m_HealthCompScript)
                m_HealthCompScript.TakeDamage(m_FleshDamage);
            m_TimeOnFire -= 1 * Time.deltaTime;

			//move smoke down
			if (m_SmokeObject)
			{
				m_AreaElevator.GetComponent<SmokeMovementBehaviour>().m_Move = true;
			}
		}
        else if(m_FireObject && m_SmokeObject)
        {
			// disable fire object
			if (m_FireObject.activeInHierarchy)
			{
				m_FireObject.SetActive(false);
			}
			//destroy
			if(m_SmokeTime>=0)
			{
				m_SmokeTime -= 1 * Time.deltaTime;
			}
			else
			{
				//move down
				DestroySelf();
			}
		}
		else
		{
			DestroySelf();
		}
	}

    private void OnTriggerStay(Collider other)
    {
		if (m_TimeOnFire>1f )
		{
			if(other.tag == "Enemy" || other.tag == "Player")
				other.gameObject.GetComponent<HealthComponent>().TakeDamage(m_FleshDamage);
			else if(other.gameObject.layer == 6)
			{
				m_DestructableScript=other.gameObject.GetComponent<Destructable>();
			}
		}
	}

	private void DestroySelf()
    {
        transform.parent = null;
        Destroy(gameObject);
    }
}
