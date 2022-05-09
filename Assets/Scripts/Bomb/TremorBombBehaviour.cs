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
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("l"))
		{
			Explode();
		}
    }
	private void OnTriggerStay(Collider other)
	{
		//if(!m_Colliders.Contains(other))
		//{
		//	m_Colliders.Add(other);
		//}
	}
	
	void Explode()
	{
		hitColliders = Physics.OverlapBox(transform.position, transform.localScale , transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{

			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			
		}
	}

	private float GetDamage(float distanceTo)
	{
		return 1 / distanceTo;
	}
}
