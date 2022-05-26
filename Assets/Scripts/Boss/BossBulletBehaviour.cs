using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletBehaviour : AreaBombBehaviour
{
    [SerializeField] public float m_DamagePass;
	protected override void Start()
	{
		m_Damage = m_DamagePass;
		base.Start();
	}


	private void OnCollisionEnter(Collision collision)
    {
		GetComponent<Rigidbody>().velocity =new Vector3(0,0,0);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(m_Damage);
        }
        StartCoroutine(base.Explode());
    }
}
