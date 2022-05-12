using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    DEAD
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour
{
    private AnimState m_AnimState;

    [SerializeField] private float m_WalkSpeed = 10.0f;
    [SerializeField] private float m_SprintSpeed = 15.0f;
    [SerializeField] private float m_CrouchSpeed = 5.0f;
    [SerializeField] private float m_ClimbSpeed = 5.0f;
    [SerializeField] private float m_Gravity = 100.0f;
    [SerializeField] private float m_Dampening = 10.0f;

    private Vector3 m_ClimbDirection;

    private Vector2 m_AxisInput;

    private Collider m_Coll;
    private Rigidbody m_RB;

    private bool m_bSprinting;
    private bool m_bCrouching;
    private bool m_bClimbing;
    private bool m_bShooting;

    private Ladder m_Ladder;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        m_Coll = GetComponent<Collider>();
        m_RB = GetComponent<Rigidbody>();

        m_AnimState = AnimState.IDLE;

        m_bSprinting = false;
        m_bCrouching = false;
        m_bClimbing = false;
        m_bShooting = false;
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
    }

    private void Movement()
    {
        //set default speed
        float speed = m_WalkSpeed;

        //adjust speed, if necessary
        if (m_bClimbing) { speed = m_ClimbSpeed; }
        else if (m_bCrouching) { speed = m_CrouchSpeed; }
        else if (m_bSprinting) { speed = m_SprintSpeed; }

        Vector3 velocity = Vector3.zero;

        if (m_bClimbing)
        {
            velocity = m_ClimbDirection * m_AxisInput.y * speed;

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
            velocity = direction * speed;
            velocity.y = m_RB.velocity.y - m_Gravity * Time.fixedDeltaTime;
        }

        //interpolate towards the target velocity, from the current velocity
        m_RB.velocity = Vector3.Lerp(m_RB.velocity, velocity, m_Dampening * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (Vector2.SqrMagnitude(m_AxisInput) > 0.0f)
        {
            Vector3 direction = new Vector3(m_AxisInput.x, 0, m_AxisInput.y);
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, m_Dampening * Time.fixedDeltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            //ONLY if the analogue stick is being used, get its input
            case InputActionPhase.Performed:
                m_AxisInput = context.ReadValue<Vector2>();

                if (m_bClimbing) { m_AnimState = AnimState.CLIMBING; }
                else if (m_bCrouching) { m_AnimState = AnimState.CROUCHING; }
                else if (m_bSprinting) { m_AnimState = AnimState.SPRINTING; }

                break;

            //in any other state, reset to idle
            default:
                m_AxisInput = Vector2.zero;

                m_AnimState = AnimState.IDLE;
                break;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (m_bClimbing) { return; }

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                m_bShooting = true;

                m_AnimState = AnimState.SHOOT;
                break;
            default:
                m_bShooting = false;

                m_AnimState = AnimState.IDLE;
                break;
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (m_bClimbing) { return; }

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                m_bSprinting = true;
                break;
            default:
                m_bSprinting = false;
                break;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (m_bClimbing) { return; }

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                m_bCrouching = true;
                break;
            default:
                m_bCrouching = false;
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
    }

    public AnimState GetAnimState() { return m_AnimState; }
    public bool GetShooting() { return m_bShooting; }
}
