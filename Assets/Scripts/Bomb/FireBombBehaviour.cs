using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombBehaviour : AreaBombBehaviour
{
    [HideInInspector] private Collider[] m_FirehitColliders;
    [SerializeField] private GameObject m_FireParticleObject;
	[SerializeField] private GameObject m_FireVisual;
    [Header("FireRelatedStats")]
    [SerializeField] private float m_FireDamage;
    [SerializeField] private float m_TimeOnFire ;


    protected override void Start()
    {
        m_Damage = m_FireDamage;
    }


    protected override void Update()
    {
        base.Update();
        
    }

    protected override IEnumerator Explode()
    {
        m_Exploded = true;
		m_FireVisual.SetActive(true);

		m_FirehitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y * 2, gameObject.GetComponent<Collider>().transform.localScale.z * 2), transform.localRotation);
        foreach (Collider nearbyOject in m_FirehitColliders)
        {
			if (nearbyOject.GetComponent<HealthComponent>())
			{
				Vector3 firePos = new Vector3(nearbyOject.transform.position.x, transform.position.y, nearbyOject.transform.position.z);
				GameObject fireParticle = Instantiate(m_FireParticleObject, firePos, nearbyOject.transform.rotation, nearbyOject.transform);
				fireParticle.GetComponent<ParticleSystem>().Play();
				fireParticle.GetComponent<FireTickBehaviour>().m_FireDamage = m_FireDamage;
				fireParticle.GetComponent<FireTickBehaviour>().m_MaxTimeOnFire = m_TimeOnFire;
			}

        }
		m_FireVisual.transform.eulerAngles = new Vector3(0f, m_FireVisual.transform.eulerAngles.y, 0f);
		m_FireVisual.transform.position = new Vector3(m_FireVisual.transform.position.x, 0f, m_FireVisual.transform.position.z);
		m_FireVisual.GetComponent<FireTickBehaviour>().m_FireDamage = m_FireDamage;
		m_FireVisual.GetComponent<FireTickBehaviour>().m_MaxTimeOnFire = m_TimeOnFire;
		m_FireVisual.transform.parent = null;
        return base.Explode();
    }

}
