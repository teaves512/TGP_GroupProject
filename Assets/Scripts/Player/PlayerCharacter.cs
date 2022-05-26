using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ------------------------------------------------------------------ 

public enum AnimState
{
    IDLE,
    WALKING,
    SPRINTING,
    CROUCHING,
    CLIMBING,

    PLACE_BOMB_FLOOR,
    PLACE_BOMB_WALL,
    THROW_BOMB,

    SHOOT,
    DEAD,

    MAX
}

// ------------------------------------------------------------------ 

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour
{
    // ------------------------------------------------------------------ 

    private AnimState m_AnimState;

    private GunControl m_PlayerWeapons;

    private float     m_CurrentSpeed;

    [SerializeField]
    private Vector3 m_PlayerHandPositionOffset;

    // ------------------------------------------------------------------ 

    [SerializeField] public float m_WalkSpeed   = 10.0f;
    [SerializeField] private float m_SprintSpeed = 15.0f;
    [SerializeField] private float m_CrouchSpeed = 5.0f;
    [SerializeField] private float m_ClimbSpeed  = 5.0f;
    [SerializeField] private float m_Dampening   = 10.0f;
    [SerializeField] private float m_Gravity     = 10.0f;

    [SerializeField] private PlayerAnimationTriggers m_AnimationTriggers;
    [SerializeField]private GameObject m_GameOverScreen;

    // ------------------------------------------------------------------ 

    private Vector3 m_ClimbDirection;
    private Ladder m_Ladder;

    private Vector2   m_AxisInput;

    private Rigidbody m_RB;
    private Collider m_Coll;
    

    // ------------------------------------------------------------------ 

    private bool      m_bWalking;
    private bool      m_bSprinting;

    private bool      m_bCrouching;

    [SerializeField] private CapsuleCollider m_CapsuleCollider;

    private bool      m_bClimbing;
    private bool      m_bShooting;

    private bool      m_bPlacingBombOnWall;
    private bool      m_bPlacingBombOnFloor;
    private bool      m_bThrowingBomb;

    private bool      m_bDead;

    private int       m_MovementKeysPressedConcurrently;
    // ------------------------------------------------------------------ 

    private void Start()
    {
        Init();
    }

    // ------------------------------------------------------------------ 

    private void Init()
    {
        m_RB         = GetComponent<Rigidbody>();

        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        m_Coll = GetComponent<Collider>();

        m_AnimState  = AnimState.IDLE;

        m_bSprinting = false;
        m_bCrouching = false;
        m_bClimbing  = false;
        m_bShooting  = false;
        m_bWalking   = false;
        m_bDead      = false;
        m_bPlacingBombOnFloor = false;
        m_bPlacingBombOnWall  = false;
        m_bThrowingBomb       = false;

        m_CurrentSpeed        = 0.0f;
        m_MovementKeysPressedConcurrently = 0;

        m_PlayerWeapons = GetComponent<GunControl>();
        EventManager.GameOver += GameOver;
    }

    // ------------------------------------------------------------------

    private void FixedUpdate()
    {
        Movement();
        Rotate();

        m_CapsuleCollider.height = (m_bCrouching) ? 0.8f : 2.0f;
        m_CapsuleCollider.center = (m_bCrouching)
            ? new Vector3(0.0f, 0.4f, 0.0f)
            : new Vector3(0.0f, 1.0f, 0.0f);
    }

    // ------------------------------------------------------------------ 

    private void Movement()
    {
        Vector3 velocity = Vector3.zero;

        if (m_bClimbing)
        {
            velocity = m_ClimbDirection * m_AxisInput.y * m_ClimbSpeed;

            float height = transform.position.y;

            if (height > m_Ladder.GetTopPos().y)
            {
                if (m_AxisInput.y > 0) { AttachToLadder(m_Ladder); }
            }
            if (height < m_Ladder.GetBotPos().y)
            {
                if (m_AxisInput.y < 0) { AttachToLadder(m_Ladder); }
            }
        }
        else
        {
            //generate target velocity, based off direction player wants to move and current speed
            Vector3 direction = new Vector3(m_AxisInput.x, 0, m_AxisInput.y).normalized;
            velocity = direction * m_CurrentSpeed;
            velocity.y = m_RB.velocity.y - m_Gravity * Time.fixedDeltaTime;
        }

        //interpolate towards the target velocity, from the current velocity
        m_RB.velocity = Vector3.Lerp(m_RB.velocity, velocity, m_Dampening * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (Vector2.SqrMagnitude(m_AxisInput) > 0.0f)
        {
            Vector3    direction = new Vector3(m_AxisInput.x, 0, m_AxisInput.y);
            Quaternion rotation  = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation   = Quaternion.Lerp(transform.rotation, rotation, m_Dampening * Time.fixedDeltaTime);
        }
    }

    // ------------------------------------------------------------------ 

    public void SwapBombs(InputAction.CallbackContext context)
    {
        if(context.control.name == "1")
        {
            gameObject.GetComponent<BombPlacement>().SetCurrentBomb("Basic Bomb");
        }
        if(context.control.name == "2")
        {
            gameObject.GetComponent<BombPlacement>().SetCurrentBomb("Fire Bomb");
        }
        if(context.control.name == "3")
        {
            gameObject.GetComponent<BombPlacement>().SetCurrentBomb("Area Bomb");
        }
        if(context.control.name == "4")
        {
            gameObject.GetComponent<BombPlacement>().SetCurrentBomb("Walking Bomb");
        }
    }

    // ------------------------------------------------------------------

    public void Move(InputAction.CallbackContext context)
    {
        if(m_bShooting)
        {
            m_PlayerWeapons.StopSpawningBullets();
        }

        switch (context.phase)
        {
            case InputActionPhase.Started:
                if(m_bCrouching)
                {
                    if(m_AnimationTriggers)
                        m_AnimationTriggers.SetCrouchWalking(true);

                    m_bWalking = true;
                }

                m_MovementKeysPressedConcurrently++;
            break;

            //ONLY if the analogue stick is being used, get its input
            case InputActionPhase.Performed:

                if (m_MovementKeysPressedConcurrently > 1)
                    return;

                m_AxisInput = context.ReadValue<Vector2>();

                if (m_bClimbing) 
                { 
                    m_AnimState    = AnimState.CLIMBING;
                    m_CurrentSpeed = m_ClimbSpeed;
                }
                else if (m_bCrouching) 
                { 
                    m_AnimState    = AnimState.CROUCHING;
                    m_CurrentSpeed = m_CrouchSpeed;
                    m_bWalking     = true;
                }
                else if (m_bSprinting) 
                { 
                    m_AnimState    = AnimState.SPRINTING;
                    m_CurrentSpeed = m_SprintSpeed;
                }
                else 
                { 
                    m_AnimState    = AnimState.WALKING;
                    m_CurrentSpeed = m_WalkSpeed;
                    m_bWalking     = true;
                }

            break;

            //in any other state, reset to idle
            case InputActionPhase.Canceled:
                m_AxisInput    = Vector2.zero;

                m_bWalking     = false;
                m_bSprinting   = false;

                if (!m_bCrouching)
                    m_AnimState = AnimState.IDLE;
                else
                {
                    if (m_AnimationTriggers)
                        m_AnimationTriggers.SetCrouchWalking(false);

                    m_AnimState = AnimState.CROUCHING;
                }

                m_CurrentSpeed = 0.0f;

                m_MovementKeysPressedConcurrently--;
            break;
        }
    }

    // ------------------------------------------------------------------ 

    public void Shoot(InputAction.CallbackContext context)
    {
        if (m_bClimbing || m_bWalking || m_bSprinting || m_bCrouching) 
            return; 

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Vector3 m_offset = new Vector3();
                m_offset.y = 1.34f;
                m_offset.x = 0.82f * transform.forward.x;
                m_offset.z = 0.82f * transform.forward.z;
                m_PlayerWeapons.FireBullet(this.transform.position + m_offset, transform.forward);
            break;

            case InputActionPhase.Performed:

                if (m_bSprinting)
                    return;

                m_bShooting    = true;
                m_bWalking     = false;
                m_bCrouching   = false;

                m_AnimState = AnimState.SHOOT;

                m_CurrentSpeed = 0.0f;
            break;

            case InputActionPhase.Canceled:
                m_bShooting = false;

                if (m_bSprinting)
                {
                    m_CurrentSpeed = m_SprintSpeed;
                    m_AnimState    = AnimState.SPRINTING;
                }
                else if (m_bWalking)
                {
                    m_CurrentSpeed = m_WalkSpeed;
                    m_AnimState    = AnimState.WALKING;
                }
                else
                {
                    m_CurrentSpeed = 0.0f;
                    m_AnimState    = AnimState.IDLE;
                }

                m_PlayerWeapons.StopSpawningBullets();
            break;
        }
    }

    // ------------------------------------------------------------------ 

    public void Sprint(InputAction.CallbackContext context)
    {
        if (m_bShooting)
        {
            m_PlayerWeapons.StopSpawningBullets();
        }

        if (m_bClimbing) 
            return; 

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (m_bWalking || m_bCrouching)
                {
                    m_bSprinting   = true;
                    m_CurrentSpeed = m_SprintSpeed;

                    m_AnimState    = AnimState.SPRINTING;
                }
            break;

            case InputActionPhase.Canceled:
                m_bSprinting = false;

                if (m_bWalking)
                {
                    m_CurrentSpeed = m_WalkSpeed;
                    m_AnimState    = AnimState.WALKING;
                }
                else
                {
                    m_AnimState    = AnimState.IDLE;
                    m_CurrentSpeed = 0.0f;
                }
            break;
        }
    }

    // ------------------------------------------------------------------ 

    public void Crouch(InputAction.CallbackContext context)
    {
        if (m_bShooting)
        {
            m_PlayerWeapons.StopSpawningBullets();
        }

        if (m_bClimbing) 
            return; 

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                m_bCrouching   = true;
                m_AnimState    = AnimState.CROUCHING;
                m_CurrentSpeed = m_CrouchSpeed;
            break;

            case InputActionPhase.Canceled:
                m_bCrouching = false;

                if (m_bSprinting)
                {
                    m_CurrentSpeed = m_SprintSpeed;
                    m_AnimState    = AnimState.SPRINTING;
                }
                else if (m_bWalking)
                {
                    m_CurrentSpeed = m_WalkSpeed;
                    m_AnimState    = AnimState.WALKING;
                }
                else
                {
                    m_CurrentSpeed = 0.0f;
                    m_AnimState    = AnimState.IDLE;
                }
                break;
        }
    }

    public void AttachToLadder(Ladder ladder)
    {
        if (m_bShooting)
        {
            m_PlayerWeapons.StopSpawningBullets();
        }

        m_Ladder         = ladder;
        m_ClimbDirection = m_Ladder.GetClimbDirection();
        m_RB.velocity    = Vector3.zero;
        m_bClimbing      = !m_bClimbing;
        m_Coll.enabled   = !m_bClimbing;

        if (m_bClimbing == false) 
        { 
            m_Ladder.DettachFromLadder(); 
        }
        else 
        {
            m_AnimState = AnimState.CLIMBING;
        }

        Debug.Log(m_bClimbing ? "Attached." : "Detached.");
    }

    // ------------------------------------------------------------------ 

    public AnimState GetAnimState() { return m_AnimState; }
    public bool      GetShooting()  { return m_bShooting; }

    // ------------------------------------------------------------------ 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Destination")) 
        {
            EventManager.OnGameOver(true);
        }else if (collision.collider.CompareTag("DeathFloor"))
        {
            EventManager.OnGameOver(false);
        }
    }

    void GameOver(bool victory)
    {
        m_GameOverScreen.gameObject.SetActive(true);
    }
    
}
