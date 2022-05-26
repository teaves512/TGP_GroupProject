using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_BombPara;
	[SerializeField] private List<GameObject> m_BombDropLocations = new List<GameObject>();
    [SerializeField] private int m_LastBombIndex;
    [SerializeField] private Transform m_BombHolder;

    private void Start()
    {
        m_LastBombIndex = 0;
    }
    public void GenerateRandomLocations(int numOfBombs)
    {
        int UpperIndexOfBombs = numOfBombs + m_LastBombIndex;
        for (int i = m_LastBombIndex; i< UpperIndexOfBombs; i++)
        {
            //int rand = Random.Range(0, m_CopyBombDropLocations.Count);
			if (i >= m_BombDropLocations.Count) { break; }
            Instantiate(m_BombPara, m_BombDropLocations[i].transform.position, m_BombDropLocations[i].transform.rotation, m_BombHolder);
        }
        m_LastBombIndex += numOfBombs;
		if (UpperIndexOfBombs >= m_BombDropLocations.Count)
		{
			m_LastBombIndex = 0;
			// once cycle is complete fire full ring
			GetComponent<BossBehaviour>().m_CycleComplete = true;
		}
	}


}
