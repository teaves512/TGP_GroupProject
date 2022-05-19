using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private LadderConnect m_TopConnect;
    [SerializeField] private LadderConnect m_BotConnect;

    private Vector3 m_ClimbDirection;

    private Coroutine m_cDettach;

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

    public void DettachFromLadder()
    {
        if (m_cDettach != null) { StopCoroutine(m_cDettach); }
        m_cDettach = StartCoroutine(C_Dettach());
    }

    private IEnumerator C_Dettach()
    {
        m_TopConnect.GetComponent<Collider>().enabled = false;
        m_BotConnect.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        m_TopConnect.GetComponent<Collider>().enabled = true;
        m_BotConnect.GetComponent<Collider>().enabled = true;
    }
}
