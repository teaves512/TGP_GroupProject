using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private PlayerCharacter m_Player;

    private Animator        m_PlayerAnimator;

    private AnimState       m_CurrentState = AnimState.IDLE;

    // ----------------------------------------------------------------------

    private void Start()
    {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player         = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimState newState = m_Player.GetAnimState();

        if (newState != m_CurrentState)
        {
            switch(newState)
            {
                case AnimState.IDLE:
                    m_PlayerAnimator.SetBool("Walking", false);
                    m_PlayerAnimator.SetBool("Running", false);
                break;

                case AnimState.WALKING:
                    m_PlayerAnimator.SetBool("Walking", true);
                    m_PlayerAnimator.SetBool("Running", false);
                break;

                case AnimState.SPRINTING:
                    m_PlayerAnimator.SetBool("Walking", false);
                    m_PlayerAnimator.SetBool("Running", true);
                break;

                case AnimState.CROUCHING:
                    m_PlayerAnimator.SetBool("Crouching", true);
                    m_PlayerAnimator.SetBool("Running", false);
                break;

                case AnimState.DEAD:
                    m_PlayerAnimator.SetBool("Dead", true);
                break;

                case AnimState.THROW_BOMB:
                break;

                case AnimState.CLIMBING:
                    m_PlayerAnimator.SetBool("Climbing Ladder", true);
                break;

                case AnimState.PLACE_BOMB_FLOOR:
                break;

                case AnimState.PLACE_BOMB_WALL:
                break;

                default:
                return;
            }

            m_CurrentState = newState;
        }
    }

    // ----------------------------------------------------------------------
}
