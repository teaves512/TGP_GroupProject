

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorBombBehaviour : MainBombBehaviour
{
	[Header("Damage")]
	[SerializeField] private Collider[] hitColliders ;
	private bool m_Started;
	// Start is called before the first frame update
	protected override void Start()
    {
		m_Started = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(Input.GetKeyDown("l"))
		{
			StartCoroutine(Explode());
		}

    }

    protected override IEnumerator Explode()
	{
		hitColliders = Physics.OverlapBox(transform.position,new Vector3 (gameObject.GetComponent<Collider>().transform.localScale.x * 2, gameObject.GetComponent<Collider>().transform.localScale.y, gameObject.GetComponent<Collider>().transform.localScale.z) , transform.localRotation);

		foreach (Collider nearbyOject in hitColliders)
		{
			Destructable destructableScript = nearbyOject.GetComponent<Destructable>();
			destructableScript?.TakeDamage(GetDamage(Vector3.Distance(nearbyOject.transform.position, transform.position)));
			nearbyOject.tag = "Placeable";
		}
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
