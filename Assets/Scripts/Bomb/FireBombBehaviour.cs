using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombBehaviour : AreaBombBehaviour
{
    [SerializeField] private Collider[] m_FirehitColliders;
    [SerializeField] private GameObject m_FireParticleObject;
    [Header("FireRelatedStats")]
    [SerializeField] private float m_FireDamage;
    [SerializeField] private float m_TimeOnFire;


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
        m_FirehitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z), transform.localRotation);

        foreach (Collider nearbyOject in m_FirehitColliders)
        {
            Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
            HealthComponent enemyHealthScript = nearbyOject.GetComponent<HealthComponent>();
            GameObject fireParticle = Instantiate(m_FireParticleObject, nearbyOject.transform);
            fireParticle.GetComponent<ParticleSystem>().Play();
            fireParticle.GetComponent<FireTickBehaviour>().m_FireDamage = m_FireDamage;
            fireParticle.GetComponent<FireTickBehaviour>().m_MaxTimeOnFire = m_TimeOnFire;

        }
        return base.Explode();
    }
}
