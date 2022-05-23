using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisableTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            // Now do a raycast to see if we can actually see them
            RaycastHit data;

            if(Physics.Raycast(transform.position, other.transform.position - transform.position, out data))
            {
                GetComponentInParent<AIPatrol>().SetCanSeePlayer(true, other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<AIPatrol>().SetCanSeePlayer(false, null);
        }
    }
}
