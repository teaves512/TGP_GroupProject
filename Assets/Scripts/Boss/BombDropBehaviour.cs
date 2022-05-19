using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropBehaviour : MonoBehaviour
{
	[SerializeField] private List<GameObject> m_BombDropLocations = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t"))
		{

		}
    }


}
