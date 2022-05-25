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
            RaycastHit hit;

            Vector3 offset = other.transform.position - transform.position;

            Vector3 handPosition = new Vector3();
            handPosition.y = 1.34f;
            handPosition.x = 0.82f * transform.forward.x;
            handPosition.z = 0.82f * transform.forward.z;

            if (Physics.Raycast(transform.position + handPosition, offset.normalized, out hit, offset.magnitude, ~8))
            {
                if (hit.collider.tag == "Player")
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
