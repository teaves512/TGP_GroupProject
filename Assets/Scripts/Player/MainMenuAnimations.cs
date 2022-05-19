using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    // ------------------------------------------------------------

    [SerializeField]
    private Animator m_AnimationSystem;

    [SerializeField]
    private RuntimeAnimatorController m_StandingController;

    [SerializeField]
    private RuntimeAnimatorController m_SittingController;

    private Quaternion m_StartRotation;
    private Quaternion m_EndRotation;
    private float      m_Time;

    private bool       m_StandingUp  = false;
    private bool       m_SittingDown = false;

    private Vector3    m_SittingLocation;

    private bool       m_Rotating = false;

    private Vector3    m_StandingOffscreenPosition;
     
    private float      m_StandingWalkDelay = 1.7f;

    // ------------------------------------------------------------

    private void Start()
    {
        m_StartRotation   = transform.rotation;
        m_EndRotation     = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));

        m_SittingLocation = transform.position;

        m_StandingOffscreenPosition = m_SittingLocation + Vector3.right * 5.0f;
    }

    // ------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        // ------------------------------------------------

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetWalkingOff(true);
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            SetWalkingOff(false);
        }

        // ------------------------------------------------

        // Walking to the positions
        if(m_StandingUp && transform.rotation == m_EndRotation)
        {
            m_StandingWalkDelay -= Time.deltaTime;

            if(m_StandingWalkDelay <= 0.0f)
                transform.position += Vector3.right * Time.deltaTime * 1.3f;
        }
        else if (m_SittingDown && (transform.position - m_SittingLocation).magnitude > 0.1f)
        {
            transform.position -= Vector3.right * Time.deltaTime * 1.3f;
        }

        // ------------------------------------------------

        if(m_Rotating)
        {
            m_Time += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(m_StartRotation, m_EndRotation, m_Time);

            if(m_Time >= 1.0f)
            {
                m_Rotating = false;
            }
        }

        // ------------------------------------------------

        // Check to see if we should be rotating
        if ((m_StandingUp  &&  transform.rotation != m_EndRotation)                             || 
            (m_SittingDown && (transform.position  - m_SittingLocation).magnitude < 0.1f))
        {
            m_Rotating = true;
        }
        else
        {
            m_Rotating = false;
        }
    }

    // ------------------------------------------------------------

    public void SetWalkingOff(bool value)
    {
        if (value)
        {
            // Set the position
            transform.position = m_SittingLocation;

            m_StandingWalkDelay = 1.7f;

            m_Rotating    = true;
            m_StandingUp  = true;
            m_SittingDown = false;

            m_Time        = 0.0f;

            m_AnimationSystem.runtimeAnimatorController = m_StandingController;
            m_AnimationSystem.SetTrigger("Stand");

            m_StartRotation = transform.rotation;
            m_EndRotation   = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
        }
        else
        {
            // Set the position
            transform.position = m_StandingOffscreenPosition;

            m_Rotating    = false;
            m_StandingUp  = false;
            m_SittingDown = true;

            m_Time        = 0.0f;

            m_AnimationSystem.runtimeAnimatorController = m_SittingController;
            m_AnimationSystem.SetTrigger("Sit");

            transform.rotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
            m_StartRotation    = transform.rotation;
            m_EndRotation      = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        }
    }

    // ------------------------------------------------------------
}
