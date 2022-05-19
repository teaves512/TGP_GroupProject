using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MainBombBehaviour
{
	[SerializeField]private float m_MoveBack;
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

        //check if this is destructable
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z+m_MoveBack), transform.forward, out hit))
        {
            Debug.DrawRay(hit.transform.position, transform.forward, Color.red);
            yield return new WaitForSeconds(m_DestroyDelay);
			Debug.Log(hit.collider.gameObject.layer);
			if (hit.collider.gameObject.layer == 6)
			{
				hit.collider.gameObject.GetComponent<Destructable>().TakeDamage(m_Damage);
			}
            hit.collider.tag = "Placeable";

        }
        StartCoroutine(base.Explode());
    }
  
}
