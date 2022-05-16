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

    [SerializeField] private float m_WalkSpeed   = 10.0f;
    [SerializeField] private float m_SprintSpeed = 15.0f;
    [SerializeField] private float m_CrouchSpeed = 5.0f;
    [SerializeField] private float m_ClimbSpeed  = 5.0f;
    [SerializeField] private float m_Dampening   = 10.0f;
    [SerializeField] private float m_Gravity     = 10.0f;

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
    }

    // ------------------------------------------------------------------ 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Play("Boom");
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
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
        switch (context.phase)
        {
            case InputActionPhase.Started:
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

                m_AnimState    = AnimState.IDLE;

                m_CurrentSpeed = 0.0f;

                m_MovementKeysPressedConcurrently--;
            break;
        }
    }

    // ------------------------------------------------------------------ 

    public void Shoot(InputAction.CallbackContext context)
    {
        if (m_bClimbing) 
            return; 

        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_PlayerWeapons.FireBullet(this.transform.position + m_PlayerHandPositionOffset, transform.forward);
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
        if (m_bClimbing) 
            return; 

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                m_bSprinting   = true;
                m_CurrentSpeed = m_SprintSpeed;

                m_AnimState    = AnimState.SPRINTING;
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
        m_Ladder = ladder;
        m_ClimbDirection = m_Ladder.GetClimbDirection();
        m_RB.velocity = Vector3.zero;
        m_bClimbing = !m_bClimbing;
        m_Coll.enabled = !m_bClimbing;

        if (m_bClimbing == false) { m_Ladder.DettachFromLadder(); }

        Debug.Log(m_bClimbing ? "Attached." : "Detached.");
    }

    // ------------------------------------------------------------------ 

    public AnimState GetAnimState() { return m_AnimState; }
    public bool      GetShooting()  { return m_bShooting; }

    // ------------------------------------------------------------------ 
}
