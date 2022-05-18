using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    IDLE,
    SEARCHING,
    PLAYER_IN_SIGHT,
    FIRING,

    REPAIRING
}
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss Body")]
    [SerializeField] private GameObject m_MainBody;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private Vector3 m_RaycastOriginOffset;
    [Header("State")]
    [SerializeField] private State m_CurrentState;
    [Header("Game Knowledge")]
    [SerializeField] private GameObject m_Player;
    [SerializeField] private float m_FOV = 50.0f;
    [SerializeField] private float m_ViewDistance = 20.0f;
    [HideInInspector] private Vector3 m_AimDirection;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        m_AimDirection = m_Turret.transform.forward;
       switch (m_CurrentState)
       {
            case State.IDLE:
            {
                break;
            }
            case State.SEARCHING:
            {
                SearchForPlayer();
                break;
            }
            case State.PLAYER_IN_SIGHT:
            {
                break;
            }
            case State.FIRING:
            {
                break;
            }
       }
    }
    private void SearchForPlayer()
    {
        if(Vector3.Distance(transform.position, m_Player.transform.position) < m_ViewDistance)
        {
            //check direction and angle 
            Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
            if(Vector3.Angle(m_AimDirection,playerDirection) < m_FOV/2) // Vector3.Angle uses the middle so FOV/2 each side
            {
                // is view of player blocked?
                RaycastHit hit;
                if(Physics.Raycast(transform.position+ m_RaycastOriginOffset, playerDirection, out hit,Mathf.Infinity))
                {
                    Debug.Log("Player cone");
                    if(hit.collider.gameObject == m_Player)
                    {
                        Debug.DrawRay(transform.position + m_RaycastOriginOffset, playerDirection, Color.green);
                        Attack();
                    }
                    else
                    {
                        Debug.Log("Player not visable");
                        Debug.DrawRay(transform.position + m_RaycastOriginOffset, playerDirection, Color.red);
                    }
                }

            }

        }
    }
    private void Attack()
    {
        Debug.Log("BOOM");
    }
}
