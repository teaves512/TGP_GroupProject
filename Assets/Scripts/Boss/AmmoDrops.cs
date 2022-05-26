using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrops : MonoBehaviour
{
	[SerializeField] private GameObject m_AmmoPara;
	[SerializeField] private List<GameObject> m_AmmoDropLocations = new List<GameObject>();
	[SerializeField] private int m_LastAmmoIndex;
	[SerializeField] public Transform m_AmmoHolder;


	private void Update()
	{

	}

	public GameObject GenerateDrop()
	{
		GameObject owner = null;
		int UpperIndexOfBombs = 1 + m_LastAmmoIndex;
		for (int i = m_LastAmmoIndex; i < UpperIndexOfBombs; i++)
		{
			//int rand = Random.Range(0, m_CopyBombDropLocations.Count);
			if (i >= m_AmmoDropLocations.Count) { break; }
			owner = Instantiate(m_AmmoPara, m_AmmoDropLocations[i].transform.position, m_AmmoDropLocations[i].transform.rotation, m_AmmoHolder);
		}
		m_LastAmmoIndex += 1;
		if (UpperIndexOfBombs >= m_AmmoDropLocations.Count)
		{
			m_LastAmmoIndex = 0;
			// once cycle is complete fire full ring
		}

		return owner;
	}
}
