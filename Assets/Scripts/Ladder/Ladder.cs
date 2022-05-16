using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private LadderConnect m_TopConnect;
    [SerializeField] private LadderConnect m_BotConnect;

    private Vector3 m_ClimbDirection;

    private void Start()
    {
        m_ClimbDirection = (m_TopConnect.transform.position - m_BotConnect.transform.position).normalized;

        m_TopConnect.SetLadder(this);
        m_BotConnect.SetLadder(this);
    }

    public Vector3 GetClimbDirection() { return m_ClimbDirection; }

    public Vector3 GetTopPos() { return m_TopConnect.transform.position; }
    public Vector3 GetBotPos() { return m_BotConnect.transform.position; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(m_TopConnect.transform.position, 0.1f);
        Gizmos.DrawSphere(m_BotConnect.transform.position, 0.1f);
    }
}
