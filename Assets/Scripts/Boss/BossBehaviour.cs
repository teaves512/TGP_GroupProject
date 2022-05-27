using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    IDLE,
    SEARCHING,
    PLAYER_IN_SIGHT,
    FIRING,
	VULNERABLE,
	NORMAL
}
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss Self Stats")]
    [SerializeField] private GameObject m_MainBody;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private GameObject m_TurretBarrel;
    [SerializeField] private Transform m_BombDropPos;

    [HideInInspector] private float m_TurretT = 0;
    [HideInInspector] private float m_SentryT = 0;
    [HideInInspector] private bool m_CanSee;
    [HideInInspector] private BombDropBehaviour m_BombDropBehaviourScript;
	[SerializeField] private GameObject m_DeadSelf;

    [Header("State")]
    [SerializeField] private State m_CurrentState;
    [HideInInspector] public bool m_FiringSentry = false;

    [Header("Game Knowledge")]
    [HideInInspector] private GameObject m_Player;
    [HideInInspector] private Vector3 m_PlayerLastKnownPos;
    [HideInInspector] private Rigidbody m_PlayerRB;
    [SerializeField] private float m_FOV = 50.0f;
    [SerializeField] private float m_ViewDistance = 20.0f;
    [HideInInspector] private Vector3 m_AimDirectionTurret;
    [HideInInspector] private Vector3 m_AimDirectionGun;
    [SerializeField] private LayerMask m_IgnoreLayer;

	[Header("Health")]
	[SerializeField] private int m_Health = 3;
    [HideInInspector] private bool m_CanTakeDamage = false;
	[SerializeField] private float m_ImmunityInterval = 10.0f;
	[SerializeField] private float m_ExposedInterval = 3.0f;
	[SerializeField] private GameObject m_HeartBarrel;
	[SerializeField] private GameObject m_CoverBarrel;

    [Header("Main Gun")]
    [SerializeField] private int m_NumOfBullets;
    [SerializeField] private Transform m_BulletSpawn;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private float m_TurretRotationSpeed;
    [HideInInspector] private Vector3 m_TargetDirection;
    [HideInInspector] private Quaternion m_TurretLookRot;
    [SerializeField] private float m_TurretInterval;
    [HideInInspector] private bool m_ReadyToFireTurret = true;
    [SerializeField] private float m_BulletDamage;
    [SerializeField] private float m_BulletForce;

    [Header("Sentry Gun")]
    [SerializeField] private GameObject m_SentryGun;
    [SerializeField] private GameObject m_SentryBullet;
    [SerializeField] private Transform m_SentryBulletSpawn;
	[SerializeField] private GameObject m_AmmoSpew;
    [SerializeField] private float m_SentryRotationSpeed;
    [SerializeField] private GameObject m_RedLight;
    [SerializeField] private GameObject m_GreenLight;
    [HideInInspector] private Vector3 m_SentryTargetDirection;
    [HideInInspector] private Quaternion m_SentryLookRot;
    [SerializeField] private float m_SentryInterval;
    [SerializeField] private int m_NumOfSentryBullets;
    [SerializeField] private float m_SentryBulletForce;
    [SerializeField] [Range(1f, 5f)]private float m_SentryOffsetFactor;
    [SerializeField] private float m_RBVelocityDeadzone;
    [SerializeField] private float m_SpreadAngle = 10.0f;

    [Header("Bomb Drop")]
    [SerializeField] private int m_NumOfBombs;
    [SerializeField] private float m_DropInterval = 10.0f;
    [SerializeField] private float m_ShockAttackInterval = 5.0f;
    [HideInInspector] public bool m_CycleComplete = false;

    [Header("Shock Waves")]
    [SerializeField] private GameObject m_FullShock;
    [SerializeField] private GameObject m_HalfShock;
    [SerializeField] private float m_ShockwaveInterval = 3.0f;
    private bool m_bFullShock = true;

    private Coroutine m_cShockwaves = null;
    private Coroutine m_cBombDrop = null;
    private Coroutine m_cFireGun = null;
    private Coroutine m_cFireTurret = null;
	private Coroutine m_cInvulnerableTimer = null;

    [Header("Level End")]
    [SerializeField] private Animator m_CrateAnimator;
    [SerializeField] private string m_CrateAnimatorTriggerName;

	private void ChangeState(State wantedState)
	{
		m_CurrentState = wantedState;
	}
	// Start is called before the first frame update
	void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_BombDropBehaviourScript = GetComponent<BombDropBehaviour>();
		m_cShockwaves = null;
        m_cFireTurret = null;
        m_cFireGun = null;
		m_cBombDrop = null;
		m_cInvulnerableTimer = null;
		m_PlayerRB = m_Player.GetComponent<Rigidbody>();
		//StartShockwaves();
		EventManager.GameOver += BossGameOver;
		ChangeState(State.IDLE);
    }
	public void Init()
	{
		StartBombDrop();
		StartInvulnerableTimer();
		ChangeState(State.SEARCHING);
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

            yield return new WaitForSeconds(m_DropInterval); //timer between attack
	
			BombDropAttack();
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
	private void StartInvulnerableTimer()
	{
		if (m_cInvulnerableTimer != null) { StopCoroutine(m_cInvulnerableTimer); }
		m_cInvulnerableTimer = StartCoroutine(C_InvulnerableTimer());
	}
	private IEnumerator C_InvulnerableTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(m_ImmunityInterval); //timer between attacks
			ChangeState(State.VULNERABLE);
			yield return new WaitForSeconds(m_ExposedInterval); //timer between attacks
			ChangeState(State.NORMAL);

		}
	}
	public void TakeDamage()
    {
        if(m_CanTakeDamage)
		{
			StartCoroutine(ApplyDamage());
		}
    }
	private IEnumerator ApplyDamage()
	{
		m_CanTakeDamage = false;
		m_Health -= 1;
		m_HeartBarrel.GetComponentInChildren<ParticleSystem>().Play();
		// cut  vulernable period short
		ChangeState(State.NORMAL);
		//StopCoroutine(m_cInvulnerableTimer);
		if(m_Health <1)
		{
            m_CrateAnimator.SetTrigger(m_CrateAnimatorTriggerName);
            //Death;
            //EventManager.OnGameOver(true);
            Debug.Log("Death");
			m_DeadSelf.SetActive(true);
			gameObject.SetActive(false);
		}

		// change barrel
		yield return null;
	}
	private void UncoverHeart()
	{
		//play shrink anim
		m_CoverBarrel.SetActive(false);
		m_CanTakeDamage = true;
		
	}
	private void CoverHeart()
	{
		//play unshrink anim
		m_CoverBarrel.SetActive(true);
		m_CanTakeDamage = false;

	}
	void Update()
    {

		m_CanSee = RaycastCheck();
        m_AimDirectionGun = m_SentryGun.transform.forward;
        m_AimDirectionTurret = m_Turret.transform.forward;
        SwitchLights(m_FiringSentry);

        switch (m_CurrentState) 
       {
			case State.VULNERABLE:
			{
					UncoverHeart();
				break;
			}
			case State.NORMAL:
			{
				CoverHeart();
				
					ChangeState(State.SEARCHING);
					break;
			}
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
                    if (m_CanSee)
                    {
                        StartGunFire();
                    }
                    if (m_ReadyToFireTurret)
                    {
                        StartFireTurret();
                    }
                break;
            }
       }
    }
    private void SearchForPlayer()
    {
        //main gun
        m_TargetDirection = (m_Player.transform.position - m_Turret.transform.position).normalized;
        Vector3 turretAngle = Quaternion.LookRotation(m_TargetDirection).eulerAngles;
        turretAngle.x = 0;
        m_TurretLookRot = Quaternion.Euler(turretAngle);
        m_TurretT += Time.deltaTime;
        m_Turret.transform.rotation = Quaternion.Lerp(m_Turret.transform.rotation, m_TurretLookRot, m_TurretT * m_TurretRotationSpeed);

        //sentry
        if (m_PlayerRB.velocity.magnitude > m_RBVelocityDeadzone)
            m_SentryTargetDirection = (m_PlayerLastKnownPos + (m_Player.transform.forward * m_SentryOffsetFactor) - m_SentryGun.transform.position).normalized;
        else
            m_SentryTargetDirection = (m_PlayerLastKnownPos - m_SentryGun.transform.position).normalized;

		Vector3 sentryAngle = Quaternion.LookRotation(m_SentryTargetDirection).eulerAngles;
		//angle.x = 0;
		m_SentryLookRot = Quaternion.Euler(sentryAngle);
        m_TurretT += Time.deltaTime;
        m_SentryGun.transform.rotation = Quaternion.Lerp(m_SentryGun.transform.rotation, m_SentryLookRot, m_TurretT * m_SentryRotationSpeed);

        if (m_TurretT > 1.0f || m_SentryT > 1.0f)
		{
			ChangeState(State.PLAYER_IN_SIGHT);
			m_TurretT = 0;
            m_SentryT = 0;
		}
	}
    private void IsPlayerInSight()
    {
		ChangeState(State.SEARCHING);
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
						ChangeState(State.FIRING);
				}
                else
                {
                    //Debug.Log("Player not visable");
                    Debug.DrawRay(transform.position, playerDirection, Color.red);
                }
            }
            if (Vector3.Angle(m_AimDirectionTurret, playerDirection) < m_FOV / 2)
            {
				ChangeState(State.FIRING);
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
		m_AmmoSpew.SetActive(true);
		ChangeState(State.SEARCHING);
		m_FiringSentry = true;
        for (int i = 0; i < m_NumOfSentryBullets; i++)
        {
            if(!m_FiringSentry){break;}

            Vector3 rotOffset = Vector3.zero;
            rotOffset.y = Random.Range(-1.0f, 1.0f) * m_SpreadAngle;
            Vector3 bulletRot = m_SentryBulletSpawn.rotation.eulerAngles + rotOffset;

            int rand = Random.Range(1, 2);
            switch (rand)
            {
                case 1:
                    AudioManager.Play("TankSentry2");
                    break;
                case 2:
                    AudioManager.Play("TankSentry3");
                    break;
            }

            GameObject bullet = Instantiate(m_SentryBullet, m_SentryBulletSpawn.position, Quaternion.Euler(bulletRot));

            //Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
            //float distance = Vector3.Distance( m_Player.transform.position , bullet.transform.position);
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1);
            //float bulletSpeed = bulletRB.velocity.magnitude;
            //float timeToReach = (distance / bulletSpeed);
            //Vector3 futureLocation = m_Player.transform.forward * m_Player.GetComponent<PlayerCharacter>().m_WalkSpeed;
            //Vector3 multipliedVector = futureLocation * timeToReach;
            //Vector3 predictedPos = m_Player.transform.position + multipliedVector;

            bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.transform.forward * (m_SentryBulletForce* Vector3.Distance(transform.position, m_Player.transform.position))) ;
            yield return new WaitForSeconds(m_SentryInterval);
        }
        m_FiringSentry = false;
		m_AmmoSpew.SetActive(false);

	}
    private void SwitchLights(bool active)
    {
        m_RedLight.SetActive(!active);
        m_GreenLight.SetActive(active);
    }

    private void StartFireTurret()
    {
        if (m_cFireTurret != null) { StopCoroutine(m_cFireTurret); }
        m_cFireTurret = StartCoroutine(FireTurret());
    }
    private IEnumerator FireTurret()
    {
        m_ReadyToFireTurret = false;
        AudioManager.Play("TankTurret");
        GameObject bullet = Instantiate(m_Bullet, m_BulletSpawn.position, m_BulletSpawn.rotation);
        bullet.GetComponent<BossBulletBehaviour>().m_DamagePass = m_BulletDamage;
        bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletForce, ForceMode.Impulse);
        yield return new WaitForSeconds(m_TurretInterval);
        m_ReadyToFireTurret = true;
		ChangeState(State.SEARCHING);
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
		m_BombDropBehaviourScript.GenerateRandomLocations(m_NumOfBombs);
	}


	private bool RaycastCheck()
	{
		bool canSee = false;
		Vector3 playerDirection = (m_Player.transform.position - transform.position).normalized;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, playerDirection, out hit, Mathf.Infinity, ~m_IgnoreLayer))
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

	void BossGameOver(bool victory)
	{
    }

}
