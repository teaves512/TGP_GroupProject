using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDropBehaviour : MonoBehaviour
{
	[SerializeField] private float m_AmmoDropTime;
	[SerializeField] private GameObject m_AreaBomb;
	private GameObject m_CurrentArea;
	private Transform m_AreaHolder;
	private bool m_IsAreaDropping = false;
	[SerializeField] private GameObject m_FireBomb;
	private GameObject m_CurrentFire;
	private Transform m_FireHolder;
	private bool m_IsFireDropping = false;
	[SerializeField] private GameObject m_WalkBomb;
	private GameObject m_CurrentWalk;
	private Transform m_WalkHolder;
	private bool m_IsWalkDropping = false;

	private void Start()
	{

		m_AreaHolder = m_AreaBomb.GetComponent<AmmoDrops>().m_AmmoHolder;
		m_FireHolder = m_FireBomb.GetComponent<AmmoDrops>().m_AmmoHolder;
		m_WalkHolder = m_WalkBomb.GetComponent<AmmoDrops>().m_AmmoHolder;
	}
	public void InitDrop()
	{
		DropArea();
		DropFire();
		DropWalk();
	}

	private void Update()
	{
		if(m_AreaHolder.transform.childCount<1)
		{
			if(!m_IsAreaDropping)
				StartCoroutine(DropAreaTimer());
			
		}
		if (m_FireHolder.transform.childCount < 1)
		{
			if (!m_IsFireDropping)
				StartCoroutine(DropFireTimer());
		}
		if (m_WalkHolder.transform.childCount < 1)
		{
			if (!m_IsWalkDropping)
				StartCoroutine(DropWalkTimer());
		}
	}
	private IEnumerator DropAreaTimer()
	{
		m_IsAreaDropping = true;
		yield return new WaitForSeconds(m_AmmoDropTime);
		DropArea();
		m_IsAreaDropping = false;
	}
	private IEnumerator DropFireTimer()
	{
		m_IsFireDropping = true;
		yield return new WaitForSeconds(m_AmmoDropTime);
		DropFire();
		m_IsFireDropping = false;
	}
	private IEnumerator DropWalkTimer()
	{
		m_IsWalkDropping = true;
		yield return new WaitForSeconds(m_AmmoDropTime);
		DropWalk();
		m_IsWalkDropping = false;
	}

	private void DropArea()
	{
		m_AreaBomb.GetComponent<AmmoDrops>().GenerateDrop();
	}
	private void DropFire()
	{
		m_FireBomb.GetComponent<AmmoDrops>().GenerateDrop();
	}
	private void DropWalk()
	{
		m_WalkBomb.GetComponent<AmmoDrops>().GenerateDrop();
	}

}
