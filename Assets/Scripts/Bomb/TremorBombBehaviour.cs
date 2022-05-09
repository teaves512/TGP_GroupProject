

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorBombBehaviour : MonoBehaviour
{
	[Header("Damage")]
	[SerializeField] private float m_Damage;
	[SerializeField] private float m_DamageDropOff;
	[SerializeField] private Collider[] hitColliders ;
	[SerializeField] private int m_LayerMask;
	private bool m_Started;
	// Start is called before the first frame update
	void Start()
    {
		m_Started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("l"))
		{
			Explode();
		}

    }

    void Explode()
	{
		hitColliders = Physics.OverlapBox(transform.position,new Vector3 (gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z) , transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			
		}
	}

	private float GetDamage(float distanceTo)
	{
		return m_Damage / distanceTo;
	}

    private void OnDrawGizmos()
    {
		if (m_Started)
		{
			Gizmos.DrawWireCube(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x * 4, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z));
		}
    }
}
