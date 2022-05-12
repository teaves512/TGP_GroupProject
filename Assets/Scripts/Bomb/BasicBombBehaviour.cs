using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MainBombBehaviour
{
    protected override void Update()
    {
        if (Input.GetKeyDown("l"))
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
        Debug.Log("boom");
        //check if this is destructable
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(hit.transform.position, transform.forward, Color.red);
            yield return new WaitForSeconds(m_DestroyDelay);
            if(hit.collider.gameObject.layer==m_LayerMask)
                hit.collider.gameObject.GetComponent<Destructable>().TakeDamage(m_Damage);
            hit.collider.tag = "Placeable";

        }
        StartCoroutine(base.Explode());
    }
  
}
