using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealthPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.transform.tag == "Player")
            EventManager.OnGameOver(true);
        else
            other.gameObject.SetActive(false);
    }
}
