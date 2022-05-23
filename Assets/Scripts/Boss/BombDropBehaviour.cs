using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_BombPara;
	[SerializeField] private List<GameObject> m_TotalBombDropLocations = new List<GameObject>();
    [HideInInspector] private List<GameObject> m_CopyBombDropLocations;
    [SerializeField] private int m_LastBombIndex;

    private void Start()
    {
        m_LastBombIndex = 0;
    }
    public void GenerateRandomLocations(int numOfBombs)
    {
        m_CopyBombDropLocations = new List<GameObject>(m_TotalBombDropLocations);

        int UpperIndexOfBombs = numOfBombs + m_LastBombIndex;
        if(UpperIndexOfBombs >= m_CopyBombDropLocations.Count)
        {
            m_LastBombIndex = 0;
            // once cycle is complete fire full ring
            GetComponent<BossBehaviour>().m_CycleComplete = true;
        }
        for (int i = m_LastBombIndex; i< numOfBombs+m_LastBombIndex; i++)
        {
            //int rand = Random.Range(0, m_CopyBombDropLocations.Count);
            Instantiate(m_BombPara, m_CopyBombDropLocations[i].transform.position, m_CopyBombDropLocations[i].transform.rotation);
            m_CopyBombDropLocations.RemoveAt(i);
        }
        m_LastBombIndex += numOfBombs;
    }


}
