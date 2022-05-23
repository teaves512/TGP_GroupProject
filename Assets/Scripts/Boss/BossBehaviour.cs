using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    IDLE,
    SEARCHING,
    PLAYER_IN_SIGHT,
    FIRING,
    BOMBDROPATTACK,

    REPAIRING
}
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss Self Stats")]
    [SerializeField] private GameObject m_MainBody;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private float m_RotationSpeed;
    [HideInInspector] private Vector3 m_TargetDirection;
    [HideInInspector] private Quaternion m_LookRot;
    [HideInInspector] private float m_T = 0;
    [HideInInspector] private bool m_CanSee;
    [HideInInspector] private BombDropBehaviour m_BombDropBehaviourScript;
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
    [Header("Shock Waves")]
    [SerializeField] private GameObject m_FullShock;
    [SerializeField] private GameObject m_HalfShock;
    [SerializeField] private float m_ShockwaveInterval = 3.0f;
    private bool m_bFullShock = true;
    [Header("Bomb Drop")]
    [SerializeField] private int m_NumOfBombs;
    [SerializeField] private float m_DropInterval = 10.0f;
    [SerializeField] private float m_ShockAttackInterval = 5.0f;
    [SerializeField] public bool m_CycleComplete = false;

    private Coroutine m_cShockwaves = null;
    private Coroutine m_cBombDrop = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_BombDropBehaviourScript = GetComponent<BombDropBehaviour>();
		m_cShockwaves = null;
		//StartShockwaves();
        StartBombDrop();
    }

	private void StartShockwaves()
	{
		if (m_cShockwaves != null) { StopCoroutine(m_cShockwaves); }
		m_cShockwaves = StartCoroutine(C_Shockwaves());
	}
	private IEnumerator C_Shockwaves()
	{
		m_bFullShock = true;
		while (true)
		{
			yield return new WaitForSeconds(m_ShockwaveInterval);

			Vector3 rot = (m_Player.transform.position - transform.position).normalized;
			Quaternion qRot = Quaternion.LookRotation(rot, Vector3.up);

			Instantiate((m_bFullShock) ? m_FullShock : m_HalfShock, transform.position, qRot);
			m_bFullShock = !m_bFullShock;
		}

		m_cShockwaves = null;
	}
    private void StartBombDrop()
    {
        if (m_cBombDrop != null) { StopCoroutine(m_cBombDrop); }
        m_cBombDrop = StartCoroutine(C_BombDrop());
    }
    private IEnumerator C_BombDrop()
    {

        while (true)
        {
            yield return new WaitForSeconds(m_DropInterval); //timer between attacks
            m_CurrentState = State.BOMBDROPATTACK;

            yield return new WaitForSeconds(m_ShockAttackInterval);
            StartCoroutine(C_ShockwaveAttack());
        }

        m_cBombDrop = null;
    }
    private IEnumerator C_ShockwaveAttack()
    {
        yield return new WaitForSeconds(m_ShockwaveInterval);

        Vector3 rot = (m_Player.transform.position - transform.position).normalized;
        Quaternion qRot = Quaternion.LookRotation(rot, Vector3.up);

        Instantiate((m_CycleComplete)?m_FullShock: m_HalfShock, transform.position, qRot);
        m_CycleComplete = false;
    }

    void Update()
    {
		m_CanSee = RaycastCheck();
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
            case State.BOMBDROPATTACK:
            {
                    BombDropAttack();
                    break;
            }
       }
    }
    private void SearchForPlayer()
    {
		if(m_CanSee)
		{
			m_TargetDirection = (m_Player.transform.position - m_Turret.transform.position).normalized;
			Vector3 angle = Quaternion.LookRotation(m_TargetDirection).eulerAngles;
			angle.x = 0;
			m_LookRot = Quaternion.Euler(angle);
			m_T += Time.deltaTime;
			m_Turret.transform.rotation = Quaternion.Lerp(m_Turret.transform.rotation, m_LookRot, m_T * m_RotationSpeed);

			if (m_T > 1.0f)
			{
				//m_Rotating = false;
				m_CurrentState = State.PLAYER_IN_SIGHT;
				m_T = 0;
			}
		}


	}
    private void CheckForPlayer()
    {

        if (Vector3.Distance(transform.position, m_Player.transform.position) < m_ViewDistance)
        {
            //check direction and angle 
            Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
            if (Vector3.Angle(m_AimDirection, playerDirection) < m_FOV / 2) // Vector3.Angle uses the middle so FOV/2 each side
            {

                if (m_CanSee)
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
    private void Attack()
    {
        GameObject bullet = Instantiate(m_Bullet, m_BulletSpawn.position, m_BulletSpawn.rotation);
        bullet.GetComponent<BossBulletBehaviour>().m_DamagePass = m_BulletDamage;
        bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletForce, ForceMode.Impulse);
        m_CurrentState = State.SEARCHING;
    }

    private void BombDropAttack()
    {
        m_BombDropBehaviourScript.GenerateRandomLocations(m_NumOfBombs);
        m_CurrentState = State.SEARCHING;
    }
	private bool RaycastCheck()
	{
		bool canSee = false;
		Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, playerDirection, out hit, Mathf.Infinity))
		{
			if (hit.collider.gameObject == m_Player)
			{
				canSee = true;
			}
			else
			{
				canSee = false;
			}
		}
		return canSee;
	}
}
