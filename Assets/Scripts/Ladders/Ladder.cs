using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Vector3 m_TopPos;
    [SerializeField] private Vector3 m_BotPos;

    [SerializeField] private LadderConnect m_TopConnect;
    [SerializeField] private LadderConnect m_BotConnect;

    private Vector3 m_ClimbDirection;

    private void Start()
    {
        m_TopConnect.transform.position = transform.position + m_TopPos;
        m_BotConnect.transform.position = transform.position + m_BotPos;

        m_ClimbDirection = (m_TopPos - m_BotPos).normalized;

        m_TopConnect.SetLadder(this);
        m_BotConnect.SetLadder(this);
    }

    public Vector3 GetClimbDirection() { return m_ClimbDirection; }

    public Vector3 GetTopPos() { return m_TopPos + transform.position; }
    public Vector3 GetBotPos() { return m_BotPos + transform.position; }

    private void OnDrawGizmosSelected()
    {
        Vector3 topPos = transform.position + m_TopPos;
        Vector3 botPos = transform.position + m_BotPos;

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(topPos, 0.1f);
        Gizmos.DrawSphere(botPos, 0.1f);
    }
}
