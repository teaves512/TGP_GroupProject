using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -----------------------------------------------------------

public class EnemyAnimationTriggers : MonoBehaviour
{
    // ----------

    private AIPatrol  m_AIPatrol;

    private AnimState m_CurrentState = AnimState.MAX;

    [SerializeField]
    private Animator  m_EnemyAnimator;

    // ----------

    // Start is called before the first frame update
    void Start()
    {
        m_AIPatrol      = GetComponent<AIPatrol>();
    }

    // ----------

    // Update is called once per frame
    void Update()
    {
        // Determine the state of the AI
        AnimState newState = m_AIPatrol.GetCurrentAnimationState();

        if (newState != m_CurrentState)
        {
            SwapToState(newState);
        }
    }

    // ----------

    private void SwapToState(AnimState newState)
    {
        switch (newState)
        {
            case AnimState.IDLE:
                m_EnemyAnimator.SetBool("Walking",         false);
                m_EnemyAnimator.SetBool("Running",         false);
                m_EnemyAnimator.SetBool("Crouching",       false);
                m_EnemyAnimator.SetBool("Shooting",        false);
            break;

            case AnimState.WALKING:
                m_EnemyAnimator.SetBool("Walking",         true);
                m_EnemyAnimator.SetBool("Running",         false);
                m_EnemyAnimator.SetBool("Crouching",       false);
                m_EnemyAnimator.SetBool("Shooting",        false);
            break;

            case AnimState.DEAD:
                m_EnemyAnimator.SetBool("Walking",         false);
                m_EnemyAnimator.SetBool("Running",         false);
                m_EnemyAnimator.SetBool("Crouching",       false);
                m_EnemyAnimator.SetBool("Dead",             true);
                m_EnemyAnimator.SetBool("Shooting",        false);
            break;


            case AnimState.SHOOT:
                m_EnemyAnimator.SetBool("Shooting",  true);
                m_EnemyAnimator.SetBool("Walking",   false);
                m_EnemyAnimator.SetBool("Running",   false);
                m_EnemyAnimator.SetBool("Crouching", false);
            break;

            default:
            return;
        }

            m_CurrentState = newState;
    }

    // ----------

    public void SetDead()
    {
        m_EnemyAnimator.SetBool("Walking", false);
        m_EnemyAnimator.SetBool("Running", false);
        m_EnemyAnimator.SetBool("Crouching", false);
        m_EnemyAnimator.SetBool("Dead", true);
        m_EnemyAnimator.SetBool("Shooting", false);
    }

    // ----------
}

// -----------------------------------------------------------