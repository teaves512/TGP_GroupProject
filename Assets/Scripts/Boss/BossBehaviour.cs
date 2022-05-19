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
    [Header("Boss Self Stats")]
    [SerializeField] private GameObject m_MainBody;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private Vector3 m_TargetDirection;
    [SerializeField] private Quaternion m_LookRot;
    [Header("Bullet Stats")]
    [SerializeField] private Transform m_BulletSpawn;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private float m_BulletDamage;
    [SerializeField] private float m_BulletForce;
    [Header("State")]
    [SerializeField] private State m_CurrentState;
    [SerializeField] private bool m_Rotating = false;
    [Header("Game Knowledge")]
    [HideInInspector] private GameObject m_Player;
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
                CheckForPlayer();
                break;
            }
            case State.FIRING:
            {
                Attack();
                break;
            }
       }
    }
    private void SearchForPlayer()
    {
        //picking random angle
        if (!m_Rotating)
        {
            m_TargetDirection = (new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) ;
            m_LookRot = Quaternion.LookRotation( m_TargetDirection);
        }
        m_Rotating = true;
        m_Turret.transform.rotation = Quaternion.RotateTowards(m_Turret.transform.rotation, m_LookRot,m_RotationSpeed * Time.deltaTime);
        //if(Quaternion.Angle( m_Turret.transform.rotation , m_LookRot) < 0.01f)
        //{
        //    m_Rotating = false;
        //    m_CurrentState = State.PLAYER_IN_SIGHT;
        //}

    }
    private void CheckForPlayer()
    {

        if (Vector3.Distance(transform.position, m_Player.transform.position) < m_ViewDistance)
        {
            //check direction and angle 
            Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
            if (Vector3.Angle(m_AimDirection, playerDirection) < m_FOV / 2) // Vector3.Angle uses the middle so FOV/2 each side
            {
                // is view of player blocked?
                RaycastHit hit;
                if (Physics.Raycast(transform.position, playerDirection, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == m_Player)
                    {
                        Debug.DrawRay(transform.position, playerDirection, Color.green);
                        m_CurrentState = State.FIRING;
                    }
                    else
                    {
                        Debug.Log("Player not visable");
                        Debug.DrawRay(transform.position, playerDirection, Color.red);
                        m_CurrentState = State.SEARCHING;
                    }
                }

            }

        }
    }
    private void Attack()
    {
        Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
        Debug.Log("BOOM");
        GameObject bullet = Instantiate(m_Bullet, m_BulletSpawn.position, m_BulletSpawn.rotation);
        bullet.GetComponent<BossBulletBehaviour>().m_Damage = m_BulletDamage;
        bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletForce, ForceMode.Impulse);
        m_CurrentState = State.SEARCHING;
    }
}
