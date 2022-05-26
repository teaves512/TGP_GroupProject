

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBombBehaviour : MainBombBehaviour
{
	[Header("Damage")]
	[SerializeField] private Collider[] m_HitColliders ;
	private bool m_Started;
	// Start is called before the first frame update
	protected override void Start()
    {
		m_Started = true;
		base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(Input.GetKeyDown("l") && !m_Exploded)
		{
			StartCoroutine(Explode());
		}
		if (m_Timer < 0 && !m_Exploded)
		{
			StartCoroutine(Explode());
		}

		base.Update();
	}

    protected override IEnumerator Explode()
	{
		m_Exploded = true;
		m_HitColliders = Physics.OverlapBox(transform.position,new Vector3 (gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z) , transform.localRotation);

		foreach (Collider nearbyOject in m_HitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent healthScript = nearbyOject.GetComponent<HealthComponent>();
			BossBehaviour bossBehaviourScript = nearbyOject.GetComponentInParent<BossBehaviour>();

			if (destructableScript!=null)
            {
				destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
				nearbyOject.tag = "Placeable";
			}
			else if(healthScript!=null)
			{
				
				healthScript.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
				//Debug.Log("Hit for = "+ GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
            }
			else if(bossBehaviourScript)
            {
				bossBehaviourScript.TakeDamage();
            }
		}
		//yield return new WaitForSeconds(m_DestroyDelay);
		StartCoroutine(base.Explode());

		yield return null;
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
