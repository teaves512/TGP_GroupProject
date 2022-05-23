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

        if(other.tag == "BombAudio")
        {
            patrol.SetHeardBomb(other.GetComponentInParent<Transform>().position);
        }
    }

    // ---------------------------------------------------------------
}
