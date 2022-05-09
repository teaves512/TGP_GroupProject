using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MainBombBehaviour
{
    protected override void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            StartCoroutine(Explode());
        }
    }

    protected override IEnumerator Explode()
    {
        Debug.Log("boom");
        //check if this is destructable
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~m_LayerMask))
        {
            Debug.DrawRay(hit.transform.position, transform.forward, Color.red);
            yield return new WaitForSeconds(m_DestroyDelay);
            hit.collider.gameObject.GetComponent<Destructable>().TakeDamage(m_Damage);
            hit.collider.tag = "Placeable";

        }
        StartCoroutine(base.Explode());
    }
  
}
