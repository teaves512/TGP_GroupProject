using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSoundsTrigger : MonoBehaviour
{
    // ---------------------------------------------------------------

    public void OnTriggerEnter(Collider other)
    {
        AIPatrol patrol = GetComponent<AIPatrol>();

        if (!patrol)
            return;

        if(other.tag == "BombAudio" && Mathf.Abs(this.gameObject.transform.position.y - other.transform.position.y) < 3.5f)
        {
            patrol.SetHeardBomb(other.GetComponentInParent<Transform>().position);
        }
    }

    // ---------------------------------------------------------------
}
