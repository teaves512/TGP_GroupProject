using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LadderConnect : MonoBehaviour
{
    private Ladder m_Ladder;

    public void SetLadder(Ladder ladder) { m_Ladder = ladder; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerCharacter>().AttachToLadder(m_Ladder);
            other.transform.position = transform.position;
        }
    }
}
