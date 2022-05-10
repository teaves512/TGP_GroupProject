using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWaypoint : MonoBehaviour
{
    [SerializeField]
    public Transform m_ThisPosition;

    [SerializeField]
    public PatrolWaypoint[] m_ConnectedWaypoints;
}
