using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        m_InIdle  = true;
        m_Walking = false;
        m_Running = false;
        m_Dead    = false;
        m_PlacingBombOnFloor = false;
        m_PlacingBombOnWall  = false;
        m_ShootingGun        = false;
        m_ThrowingBomb       = false;
    }

    // ------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        
    }

    // ------------------------------------------------------------------

    public void Move(Vector2 direction)
    {
        // Check which direction we are wanting to go and apply a force in that direction
        if(m_ClimbingLadder)
        {
            // If climbing the ladder then check to see if the direction being pressed aligns with the ladder's position

        }
        else
        {
            if(gameObject.GetComponent<Rigidbody>())
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, 0.0f, direction.y) * Time.deltaTime * m_CurrentMovementSpeed, ForceMode.Force);
        }
    }

    // ------------------------------------------------------------------

    // If active is true then we are shooting the gun, if false then we are going from shooting to not shooting
    public void ShootGun(bool active)
    {
        if (m_ClimbingLadder)
            return;

        m_ShootingGun = active;
    }

    // ------------------------------------------------------------------

    public void SetRunning(bool active)
    {
        if (m_ClimbingLadder)
            return;

        if(active)
        {
            m_Running              = true;
            m_Crouching            = false;
            m_CurrentMovementSpeed = m_RunMovementSpeed;
            m_InIdle               = false;
        }
        else
        {
            m_CurrentMovementSpeed = m_WalkMovementSpeed;
            m_Running              = false;
        }
    }

    // ------------------------------------------------------------------

    public void SetCrouching(bool active)
    {
        if (m_ClimbingLadder)
            return;

        m_Crouching = active;

        m_CurrentMovementSpeed = m_CrouchMovementSpeed;

        m_Running = false;
        m_Walking = false;
        m_InIdle  = false;
    }

    // ------------------------------------------------------------------
}
