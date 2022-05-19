using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // ------------------------------------------------------------------

    private float m_CurrentMovementSpeed;

    [SerializeField]
    private float m_WalkMovementSpeed;

    [SerializeField]
    private float m_RunMovementSpeed;

    [SerializeField]
    private float m_CrouchMovementSpeed;

    [SerializeField]
    private float m_ClimbSpeed;

    private Vector2 movementDirection;

    private Rigidbody m_Rigidbody;

    // ------------------------------------------------------------------

    // States for the animation system to use
    private bool m_InIdle;
    private bool m_Walking;
    private bool m_Running;
    private bool m_Crouching;
    private bool m_ClimbingLadder;

    private bool m_PlacingBombOnFloor;
    private bool m_PlacingBombOnWall;
    private bool m_ThrowingBomb;

    private bool m_ShootingGun;
    private bool m_Dead;

    // ------------------------------------------------------------------

    // Getters for the animation system
    public bool GetIsInIdle() { return m_InIdle; }
    public bool GetIsWalking() { return m_Walking; }
    public bool GetIsRunning() { return m_Running; }
    public bool GetIsCrouching() { return m_Crouching; }

    public bool GetIsClimbingLadder() { return m_ClimbingLadder; }

    public bool GetIsPlacingBombOnFloor() { return m_PlacingBombOnFloor; }
    public bool GetIsPlacingBombOnWall() { return m_PlacingBombOnWall; }
    public bool GetIsThrowingBomb() { return m_ThrowingBomb; }

    public bool GetIsShootingGun() { return m_ShootingGun; }
    public bool GetIsDead() { return m_Dead; }

    // ------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentMovementSpeed = 0.0f;
        m_Rigidbody            = GetComponent<Rigidbody>();

        m_InIdle             = true;
        m_Walking            = false;
        m_Running            = false;
        m_Dead               = false;
        m_PlacingBombOnFloor = false;
        m_PlacingBombOnWall  = false;
        m_ShootingGun        = false;
        m_ThrowingBomb       = false;
        m_ClimbingLadder     = false;
    }

    // ------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // See if we are going to be climbing a ladder
        if(m_ClimbingLadder)
        {

        }
        else
        {
            if (m_Rigidbody)
            {
                Vector3 dir = new Vector3(movementDirection.x, 0, movementDirection.y);
                Vector3 velocity = dir * m_CurrentMovementSpeed;
                velocity.y = m_Rigidbody.velocity.y;
                //gameObject.m_Rigidbody.AddForce(new Vector3(movementForce.x, 0.0f, movementForce.y), ForceMode.Force);

                m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, velocity, 20.0f * Time.deltaTime);

                //Debug.Log(movementDirection);
            }
        }

    }

    // ------------------------------------------------------------------

    public void Move(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            movementDirection = context.ReadValue<Vector2>();

            if (m_InIdle)
            {
                m_Walking              = true;
                m_CurrentMovementSpeed = m_WalkMovementSpeed;
            }
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            // Going back into idle
            if (m_Walking)
            {
                m_CurrentMovementSpeed = 0.0f;
                m_InIdle               = true;
            }

            movementDirection -= context.ReadValue<Vector2>();
        }
    }

    // ------------------------------------------------------------------

    // If active is true then we are shooting the gun, if false then we are going from shooting to not shooting
    public void ShootGun(InputAction.CallbackContext context)
    {
        if (m_ClimbingLadder)
            return;

        if(context.phase == InputActionPhase.Performed)
            m_ShootingGun = true;
        else if(context.phase == InputActionPhase.Canceled)
            m_ShootingGun = false; 
    }

    // ------------------------------------------------------------------

    public void SetRunning(InputAction.CallbackContext context)
    {
        // Ignore if climbing ladder
        if (m_ClimbingLadder)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            m_Running              = true;
            m_Crouching            = false;
            m_CurrentMovementSpeed = m_RunMovementSpeed;
            m_InIdle               = false;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            if (m_Walking || m_Running)
            {
                m_CurrentMovementSpeed = m_WalkMovementSpeed;
                m_Running              = false;
            }
            else
            {
                m_InIdle               = true;
                m_CurrentMovementSpeed = 0.0f;
            }
        }
    }

    // ------------------------------------------------------------------ 

    public void SetCrouching(InputAction.CallbackContext context)
    {
        if (m_ClimbingLadder)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            m_Crouching = true;
            m_CurrentMovementSpeed = m_CrouchMovementSpeed;
            m_Running = false;
            m_Walking = false;
            m_InIdle = false;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            m_Crouching = false;
            m_InIdle    = true;
        }
    }

    // ------------------------------------------------------------------
}
