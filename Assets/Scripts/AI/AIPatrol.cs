using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------

public enum PatrolFSMState
{
    PATROL        = 0,
    ATTACK_PLAYER = 1,
    INVESTIGATE   = 2,

    SAME,
    Exit
}

// ----------------------------------------------------------------------

public class FSMBaseState
{
    public virtual PatrolFSMState HandleTransition() { return PatrolFSMState.SAME; }

    public virtual void Update(float deltaTime, GameObject player) { }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public PatrolFSMState m_InternalState;
}

// ----------------------------------------------------------------------

public class PatrolState : FSMBaseState
{
    private float m_DecisionTimer = 0.0f;

    private float m_MinimumWaitTimeAtWaypoint;
    private float m_MaximumWaitTimeAtWaypoint;

    public PatrolState(float minimumWaitTime, float maximumWaitTime)
    {
        m_InternalState = PatrolFSMState.PATROL;

        m_MinimumWaitTimeAtWaypoint = minimumWaitTime;
        m_MaximumWaitTimeAtWaypoint = maximumWaitTime;
    }

    public override PatrolFSMState HandleTransition() 
    {
        // See if the player is in sight
        // If so transition to attacking the player
        //if()
          //  return PatrolFSMState.ATTACK_PLAYER;

        // if the player is heard then go into invertigate
        //if ()
          //  return PatrolFSMState.INVESTIGATE;

        // Otherwise just stay in this state
        return PatrolFSMState.SAME; 
    }

    public override void Update(float deltaTime, GameObject player) 
    {
        // See if we are at the next waypoint position, if so choose a direction to go from here
        if ((player.transform.position - player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ThisPosition.position).magnitude < 0.5f)
        {
            m_DecisionTimer -= Time.deltaTime;

            if (m_DecisionTimer <= 0.0f)
            {
                // Choose a random time to wait for the next waypoint
                m_DecisionTimer = Random.Range(m_MinimumWaitTimeAtWaypoint, m_MaximumWaitTimeAtWaypoint);
            }
            else
            {
                // Currently waiting
                player.GetComponent<AIPatrolMovement>().SetTargetPosition(player.transform);
                return;
            }

            // Choose a new waypoint to go to
            int waypointLength = player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints.Length;

            if (waypointLength == 0)
                return;

            if(waypointLength == 1)
            {
                player.GetComponent<AIPatrol>().SetPriorWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint());
                player.GetComponent<AIPatrol>().SetCurrentWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints[0]);
                player.GetComponent<AIPatrolMovement>().SetTargetPosition(player.GetComponent<AIPatrol>().GetCurrentWaypoint().transform);
                return;
            }
            
            int cameFromID = 0;

            // Find the current ID of where we came from
            for(int i = 0; i < waypointLength; i++)
            {
                if(player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints[i].transform.position == player.GetComponent<AIPatrol>().GetPriorWaypoint().transform.position)
                {
                    cameFromID = i;
                    break;
                }
            }

            // Choose a random ID that does not match the one we came from
            int chosenID = Random.Range(0, waypointLength);
            while (chosenID == cameFromID)
            {
                chosenID = Random.Range(0, waypointLength);
            }

            player.GetComponent<AIPatrol>().SetPriorWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint());
            player.GetComponent<AIPatrol>().SetCurrentWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints[chosenID]);

            player.GetComponent<AIPatrolMovement>().SetTargetPosition(player.GetComponent<AIPatrol>().GetCurrentWaypoint().transform);
        }
    }

    public override void OnEnter() 
    {
    
    }

    public override void OnExit() 
    {
    
    }
}

// ----------------------------------------------------------------------

public class AttackPlayerState : FSMBaseState
{
    public AttackPlayerState()
    {
        m_InternalState = PatrolFSMState.ATTACK_PLAYER;
    }

    public override PatrolFSMState HandleTransition()
    {
        return PatrolFSMState.SAME;
    }

    public override void Update(float deltaTime, GameObject player)
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }
}

// ----------------------------------------------------------------------

public class InvestigateState : FSMBaseState
{
    public InvestigateState()
    {
        m_InternalState = PatrolFSMState.INVESTIGATE;
    }

    public override PatrolFSMState HandleTransition()
    {
        return PatrolFSMState.SAME;
    }

    public override void Update(float deltaTime, GameObject player)
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }
}

// ----------------------------------------------------------------------

public class PatrolFSM
{
    public PatrolFSM()
    {
        m_FSMStack = new Stack<FSMBaseState>();
    }

    public Stack<FSMBaseState> m_FSMStack; 
}

// ----------------------------------------------------------------------

public class AIPatrol : MonoBehaviour
{
    // ----------------------------------------------------------------------------

    [SerializeField]
    private PatrolWaypoint m_StartWaypoint;

    private PatrolWaypoint m_CurrentWaypoint;
    private PatrolWaypoint m_PriorWaypoint;

    [SerializeField]
    private GameObject m_ThisObject;

    private PatrolFSM m_PatrolFSM;

    [SerializeField]
    private float m_MinimumWaitTimeAtWaypoint = 0.0f;

    [SerializeField]
    private float m_MaximumWaitTimeAtWaypoint = 1.0f;

    // ----------------------------------------------------------------------------

    public PatrolWaypoint GetCurrentWaypoint()                    { return m_CurrentWaypoint; }
    public void           SetCurrentWaypoint(PatrolWaypoint newWaypoint) { m_CurrentWaypoint = newWaypoint; }

    public PatrolWaypoint GetPriorWaypoint() { return m_PriorWaypoint; }
    public void SetPriorWaypoint(PatrolWaypoint newWaypoint) { m_PriorWaypoint = newWaypoint; }

    // ----------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentWaypoint  = m_StartWaypoint;
        m_PriorWaypoint    = m_CurrentWaypoint;

        transform.position = m_StartWaypoint.m_ThisPosition.position;

        m_PatrolFSM = new PatrolFSM();

        m_PatrolFSM.m_FSMStack.Push(new PatrolState(m_MinimumWaitTimeAtWaypoint, m_MaximumWaitTimeAtWaypoint));

        m_PatrolFSM.m_FSMStack.Peek().OnEnter();
    }

    // ----------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        m_PatrolFSM.m_FSMStack.Peek().Update(Time.deltaTime, this.gameObject);

        switch ((m_PatrolFSM.m_FSMStack.Peek().HandleTransition()))
        {
            case PatrolFSMState.PATROL:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit();

                    m_PatrolFSM.m_FSMStack.Push(new PatrolState(m_MinimumWaitTimeAtWaypoint, m_MaximumWaitTimeAtWaypoint));

                m_PatrolFSM.m_FSMStack.Peek().OnEnter();
            }
            break;

            case PatrolFSMState.INVESTIGATE:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit();

                    m_PatrolFSM.m_FSMStack.Push(new InvestigateState());

                m_PatrolFSM.m_FSMStack.Peek().OnEnter();
            }
            break;

            case PatrolFSMState.ATTACK_PLAYER:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit();

                    m_PatrolFSM.m_FSMStack.Push(new AttackPlayerState());

                m_PatrolFSM.m_FSMStack.Peek().OnEnter();
            }
            break;

            case PatrolFSMState.Exit:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit();

                    m_PatrolFSM.m_FSMStack.Pop();

                m_PatrolFSM.m_FSMStack.Peek().OnEnter();
            }
            break;

            default:
            return;
        }
    }

    // ----------------------------------------------------------------------------
}

// ----------------------------------------------------------------------