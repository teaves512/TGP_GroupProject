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
	[SerializeField] private GameObject m_TurretBarrel;

	[SerializeField] private Transform m_BombDropPos;
	[SerializeField] private float m_RotationSpeed;
    [HideInInspector] private Vector3 m_TargetDirection;
    [HideInInspector] private Quaternion m_LookRot;
    [HideInInspector] private float m_T = 0;
	[HideInInspector] private float m_BombT = 0;
	[SerializeField] private bool m_CanSee;
    [HideInInspector] private BombDropBehaviour m_BombDropBehaviourScript;

    [Header("State")]
    [SerializeField] private State m_CurrentState;
    [SerializeField] public bool m_FiringSentry = false;

    [Header("Game Knowledge")]
    [HideInInspector] private GameObject m_Player;
	[HideInInspector] private Vector3 m_PlayerLastKnownPos;
    [SerializeField] private float m_FOV = 50.0f;
    [SerializeField] private float m_ViewDistance = 20.0f;
    [HideInInspector] private Vector3 m_AimDirectionTurret;
    [HideInInspector] private Vector3 m_AimDirectionGun;
    [SerializeField] private LayerMask m_IgnoreLayer;

	[Header("Health")]
	[SerializeField] private int m_Health;
	[SerializeField] private float m_ImmunityInterval;
	[SerializeField] private GameObject m_ActiveBarrel;
	[SerializeField] private GameObject m_ImmuneBarrel;
    [Header("Main Gun")]
    [SerializeField] private int m_NumOfBullets;
    [SerializeField] private Transform m_BulletSpawn;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private float m_BulletDamage;
    [SerializeField] private float m_BulletForce;
    [Header("Sentry Gun")]
    [SerializeField] private GameObject m_SentryGun;
    [SerializeField] private GameObject m_SentryBullet;
    [SerializeField] private Transform m_SentryBulletSpawn;
    [SerializeField] private float m_BulletInterval;
    [SerializeField] private int m_NumOfSentryBullets;
    [SerializeField] private float m_SentryBulletForce;
    [SerializeField] private float m_SentryHeightOffset;

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
    private Coroutine m_cFireGun = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_BombDropBehaviourScript = GetComponent<BombDropBehaviour>();
		m_cShockwaves = null;
		//StartShockwaves();
        StartBombDrop();
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

		//m_cShockwaves = null;
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
    }
    private IEnumerator C_ShockwaveAttack()
    {
        yield return new WaitForSeconds(m_ShockwaveInterval);

        Vector3 rot = (m_Player.transform.position - transform.position).normalized;
        Quaternion qRot = Quaternion.LookRotation(rot, Vector3.up);

        Instantiate((m_CycleComplete)?m_FullShock: m_HalfShock, transform.position, qRot);
        m_CycleComplete = false;
    }

	private IEnumerator TakeDamage()
	{

		// apply damage
		m_Health -= 1;
		if (m_Health <= 0)
		{
			// do the death
		}
		// change barrel
		ChangeBarrelVisability(true);
		yield return new WaitForSeconds(m_ImmunityInterval);
		// change barrel back
		ChangeBarrelVisability(false);
		yield return null;
	}
	private void ChangeBarrelVisability(bool active)
	{
		//regen bomb

		m_ActiveBarrel.SetActive(!active);
		m_ImmuneBarrel.SetActive(active);
	}
	void Update()
    {
		if(Input.GetKeyDown("space"))
		{
			StartCoroutine(TakeDamage());
		}
		m_CanSee = RaycastCheck();
        m_AimDirectionGun = m_SentryGun.transform.forward;

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
                IsPlayerInSight();
                break;
            }
            case State.FIRING:
            {

                StartGunFire();

                m_CurrentState = State.SEARCHING;
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
			m_TargetDirection = (m_PlayerLastKnownPos - new Vector3(m_SentryGun.transform.position.x, m_SentryGun.transform.position.y+ m_SentryHeightOffset, m_SentryGun.transform.position.z)).normalized;
			Vector3 angle = Quaternion.LookRotation(m_TargetDirection).eulerAngles;
			//angle.x = 0;
			m_LookRot = Quaternion.Euler(angle);
			m_T += Time.deltaTime;
            m_SentryGun.transform.rotation = Quaternion.Lerp(m_SentryGun.transform.rotation, m_LookRot, m_T * m_RotationSpeed);

			if (m_T > 1.0f)
			{
				m_CurrentState = State.PLAYER_IN_SIGHT;
				m_T = 0;
			}
	}
    private void IsPlayerInSight()
    {
        m_CurrentState = State.SEARCHING;
        if (Vector3.Distance(transform.position, m_Player.transform.position) <= m_ViewDistance)
        {
            //check direction and angle 
            Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
            if (Vector3.Angle(m_AimDirectionGun, playerDirection) < m_FOV / 2) // Vector3.Angle uses the middle so FOV/2 each side
            {

                if (m_CanSee)
                {
                    Debug.DrawRay(transform.position, playerDirection, Color.green);
                    if(!m_FiringSentry)
                        m_CurrentState = State.FIRING;
                }
                else
                {
                    //Debug.Log("Player not visable");
                    Debug.DrawRay(transform.position, playerDirection, Color.red);
                }
            }
        }

    }

    private void StartGunFire()
    {
        if (m_cFireGun != null) { StopCoroutine(m_cFireGun); }
        m_cFireGun = StartCoroutine(FireGun());
    }
    private IEnumerator FireGun()
    {
        m_CurrentState = State.SEARCHING;
        m_FiringSentry = true;
        for (int i = 0; i < m_NumOfSentryBullets; i++)
        {
            if(!m_FiringSentry){break;}    
            GameObject bullet = Instantiate(m_SentryBullet, m_SentryBulletSpawn.position, m_SentryBulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(m_SentryBulletSpawn.forward * m_SentryBulletForce, ForceMode.Impulse) ;
            yield return new WaitForSeconds(m_BulletInterval);
        }
        m_FiringSentry = false;

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
		// fire upwards
		//m_TargetDirection = ((m_BombDropPos.position - m_Turret.transform.position)).normalized;
		//Vector3 angle = Quaternion.LookRotation(m_TargetDirection).eulerAngles;
		//m_LookRot = Quaternion.Euler(angle);
		//m_BombT += Time.deltaTime;
		//m_TurretBarrel.transform.rotation = Quaternion.Lerp(m_TurretBarrel.transform.rotation, m_LookRot, m_BombT * m_RotationSpeed);

		//if (m_BombT > 1.0f)
		//{
		//	m_BombDropBehaviourScript.GenerateRandomLocations(m_NumOfBombs);
		//	m_CurrentState = State.SEARCHING;
		//}
		//m_BombDropBehaviourScript.GenerateRandomLocations(m_NumOfBombs);
		m_CurrentState = State.SEARCHING;
	}


	private bool RaycastCheck()
	{
		bool canSee = false;
		Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, playerDirection, out hit, Mathf.Infinity,~m_IgnoreLayer))
		{
			if (hit.collider.gameObject == m_Player)
			{
				canSee = true;
				m_PlayerLastKnownPos = m_Player.transform.position;
			}
			else
			{
				canSee = false;
                //m_FiringSentry = false;
			}
		}
		return canSee;
	}

}
