using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MainBombBehaviour
{
	//[SerializeField]private float m_MoveBack;
	[SerializeField] private Collider[] m_HitColliders;
	protected override void Update()
    {
        if (Input.GetKeyDown("l")&&!m_Exploded)
        {
            StartCoroutine(Explode());
        }
        if(m_Timer<0 && !m_Exploded)
        { 
            StartCoroutine(Explode());
        }
        
        base.Update();
    }

    protected override IEnumerator Explode()
    {
        m_Exploded = true;

		m_HitColliders = Physics.OverlapBox(transform.position, new Vector3(gameObject.GetComponent<Collider>().transform.localScale.x/4, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z/4), transform.localRotation);

		foreach (Collider nearbyOject in m_HitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			HealthComponent enemyHealthScript = nearbyOject.GetComponent<HealthComponent>();
			BossBehaviour bossBehaviourScript = nearbyOject.GetComponent<BossBehaviour>();
			if (destructableScript != null)
			{
				destructableScript?.TakeDamage(m_Damage,true);
				nearbyOject.tag = "Placeable";
			}
			else if (enemyHealthScript != null)
			{
				enemyHealthScript.TakeDamage(m_Damage);
			}
			else if (bossBehaviourScript)
			{
				bossBehaviourScript.TakeDamage();
				nearbyOject.tag = "Placeable";
			}
		}

		StartCoroutine(base.Explode());
		yield return null;
		//yield return null;
		////check if this is destructable
		//RaycastHit hit;
		//      if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z+m_MoveBack), transform.forward, out hit,6))
		//      {
		//          Debug.DrawRay(hit.transform.position, transform.forward, Color.red);
		//          yield return new WaitForSeconds(m_DestroyDelay);
		//	Debug.Log(hit.collider.gameObject.layer);
		//	if (hit.collider.gameObject.layer == 6)
		//	{
		//		hit.collider.gameObject.GetComponent<Destructable>().TakeDamage(m_Damage);
		//	}
		//          hit.collider.tag = "Placeable";

		//      }
		//      StartCoroutine(base.Explode());
	}
  
}
