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
	[SerializeField] private float m_T = 0;
	[HideInInspector] private bool m_CanSee;
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

	private Coroutine m_cShockwaves = null;

	// Start is called before the first frame update
	void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");

		m_cShockwaves = null;
		StartShockwaves();
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

    // Update is called once per frame
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
       }
    }
    private void SearchForPlayer()
    {
		//picking random angle
		//if (!m_Rotating)
		//{
		//	m_TargetDirection = (new Vector3(0.0f, Random.Range(-180.0f, 180.0f), 0.0f));
		//}
		//m_Rotating = true;
		//m_Turret.transform.Rotate(Vector3.up, m_RotationSpeed * Time.deltaTime, Space.Self);

		//picking random angle
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
