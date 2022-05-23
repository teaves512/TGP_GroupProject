using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockTriggers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ping");
        if (other.gameObject.tag == "Explosive")
        {
            other.gameObject.GetComponent<Destructable>().TakeDamage(5f);
        }
    }
}
