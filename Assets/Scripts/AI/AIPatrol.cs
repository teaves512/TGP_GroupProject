using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public virtual void Update(float deltaTime, GameObject player, ref AnimState animationState, Vector3 pointOfInterest) { }

    public virtual void OnEnter(ref AnimState animationState, GameObject thisObject) { }

    public virtual void OnExit(ref AnimState animationState, GameObject thisObject) { }

    public PatrolFSMState m_InternalState;
}

// ----------------------------------------------------------------------

public class PatrolState : FSMBaseState
{
    private float m_DecisionTimer;
    private float m_DecisionTimerMax = 3.0f;
    private bool  m_HeardBomb        = false;

    private GameObject mPlayer;
    private Vector3    m_PlayerEyePosition;

    public PatrolState()
    {
        m_DecisionTimer = Random.Range(0.5f, m_DecisionTimerMax);

        m_InternalState = PatrolFSMState.PATROL;
    }

    public override PatrolFSMState HandleTransition() 
    {
        // Send a ray cast out of the enemy's eyes to see if they can see the player
       // RaycastHit hit;
      //  Physics.Raycast(m_PlayerEyePosition, mPlayer.transform.forward, out hit, 300.0f);

        //if(hit.collider.tag == "Player")
        //{
        //    return PatrolFSMState.ATTACK_PLAYER;
        //}

        // if the player is heard then go into invertigate
        if (m_HeardBomb)
            return PatrolFSMState.INVESTIGATE;

        // Otherwise just stay in this state
        return PatrolFSMState.SAME; 
    }

    public override void Update(float deltaTime, GameObject player, ref AnimState animationState, Vector3 pointOfInterest) 
    {
        if (!mPlayer)
            mPlayer = player;

        // See if we are at the next waypoint position, if so choose a direction to go from here
        Vector3 position          = player.transform.position;
        Vector3 waypointPosition  = player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ThisPosition.position;

        double length = new Vector3(position.x - waypointPosition.x, 0.0f, position.z - waypointPosition.z).magnitude;

        // If hit the waypoint
        if (length < 0.2f)
        {
            // Take time off the delay
            m_DecisionTimer -= Time.deltaTime;

            // If the decision has been made
            if (m_DecisionTimer <= 0.0f)
            {
                // Reset the timer
                m_DecisionTimer = Random.Range(0.1f, m_DecisionTimerMax);

                // Set that they are walking
                animationState = AnimState.WALKING;

                // Choose a new waypoint to go to
                int waypointLength = player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints.Length;

                if (waypointLength == 0)
                {
                    animationState = AnimState.IDLE;
                    return;
                }

                if (waypointLength == 1)
                {
                    player.GetComponent<AIPatrol>().SetPriorWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint());
                    player.GetComponent<AIPatrol>().SetCurrentWaypoint(player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints[0]);
                    player.GetComponent<AIPatrolMovement>().SetTargetPosition(player.GetComponent<AIPatrol>().GetCurrentWaypoint().transform);
                    return;
                }

                int cameFromID = 0;

                // Find the current ID of where we came from
                for (int i = 0; i < waypointLength; i++)
                {
                    if (player.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ConnectedWaypoints[i].transform.position == player.GetComponent<AIPatrol>().GetPriorWaypoint().transform.position)
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
            else
            {
                animationState = AnimState.IDLE;
            }
        }
    }

    public override void OnEnter(ref AnimState animationState, GameObject thisObject) 
    {
        m_HeardBomb     = false;
        m_DecisionTimer = 1.0f;

        if (thisObject)
        {
            mPlayer = thisObject;

            Vector3 pointOfInterest = thisObject.GetComponent<AIPatrol>().GetPointOfInterest();
            if (pointOfInterest != Vector3.zero)
            {
                mPlayer.GetComponent<AIPatrol>().SetCurrentWaypoint((thisObject.GetComponent<AIPatrol>().GetWaypointAtPosition(pointOfInterest)));
            }
            
            Vector3 position = mPlayer.transform.position;
            Vector3 waypointPosition = mPlayer.GetComponent<AIPatrol>().GetCurrentWaypoint().m_ThisPosition.position;

            double length = new Vector3(position.x - waypointPosition.x, 0.0f, position.z - waypointPosition.z).magnitude;

            if (length <= 0.1f)
                animationState = AnimState.IDLE;
            else
            {
                animationState = AnimState.WALKING;
            }
        }
        else
            animationState = AnimState.IDLE;
    }

    public override void OnExit(ref AnimState animationState, GameObject thisObject) 
    {
        m_HeardBomb     = false;
        m_DecisionTimer = 1.0f;

        animationState = AnimState.IDLE;
    }

    public void SetHeardBomb(bool state)
    {
        m_HeardBomb = state;
    }
}

// ----------------------------------------------------------------------

public class AttackPlayerState : FSMBaseState
{
    private Vector3    m_PositionToInvestigate;

    private GameObject mPlayer;
    private Vector3    m_PlayerEyePosition;

    public AttackPlayerState()
    {
        m_InternalState = PatrolFSMState.ATTACK_PLAYER;
    }

    public override PatrolFSMState HandleTransition()
    {
        // Send a ray cast out of the enemy's eyes to see if they can see the player
       // RaycastHit hit;
       // Physics.Raycast(m_PlayerEyePosition, mPlayer.transform.forward, out hit, 300.0f);

       // if (hit.collider.tag == "Player")
       // {
       //     return PatrolFSMState.ATTACK_PLAYER;
        //}

        return PatrolFSMState.SAME;
    }

    public override void Update(float deltaTime, GameObject player, ref AnimState animationState, Vector3 pointOfInterest)
    {
        // Walk towards the position we want to investigate
    }

    public override void OnEnter(ref AnimState animationState, GameObject thisObject)
    {
        animationState = AnimState.SHOOT;
    }

    public override void OnExit(ref AnimState animationState, GameObject thisObject)
    {
        animationState = AnimState.SHOOT;
    }
}

// ----------------------------------------------------------------------

public class InvestigateState : FSMBaseState
{
    private NavMeshAgent m_NavmeshAgent;

    private Vector3 m_PositionToInvestigate;

    private Transform m_AgentTransform;

    private bool m_MovingBackToWaypoint = false;

    public InvestigateState()
    {
        m_InternalState = PatrolFSMState.INVESTIGATE;
    }

    public InvestigateState(Vector3 pointOfInterest)
    {
        m_InternalState = PatrolFSMState.INVESTIGATE;

        m_PositionToInvestigate = pointOfInterest;
    }

    public override PatrolFSMState HandleTransition()
    {
        // Check to see if we can see the player


        // Check to see if we are at the destination point, if so then just go back to the patrol state
        if( m_MovingBackToWaypoint && 
           (m_AgentTransform.position - m_PositionToInvestigate).magnitude < 0.5f)
        {
            return PatrolFSMState.PATROL;
        }

        return PatrolFSMState.SAME;
    }

    public override void Update(float deltaTime, GameObject player, ref AnimState animationState, Vector3 pointOfInterest)
    {
        if (!m_MovingBackToWaypoint && (m_AgentTransform.position - m_PositionToInvestigate).magnitude < 0.5f)
        {
            m_MovingBackToWaypoint = true;

            m_PositionToInvestigate = player.GetComponent<AIPatrol>().GetRandomWaypoint().m_ThisPosition.position;
            player.GetComponent<AIPatrol>().SetPointOfInterest(m_PositionToInvestigate);

            m_NavmeshAgent.destination = m_PositionToInvestigate;
        }
        else
        {
            if (pointOfInterest != Vector3.zero)
            {
                m_PositionToInvestigate = pointOfInterest;
            }
        }
    }

    public override void OnEnter(ref AnimState animationState, GameObject thisObject)
    {
        animationState             = AnimState.WALKING;

        m_MovingBackToWaypoint = false;

        m_NavmeshAgent             = thisObject.GetComponent<NavMeshAgent>();
        m_AgentTransform           = thisObject.GetComponent<Transform>();


        if (m_NavmeshAgent)
        {
            m_NavmeshAgent.enabled     = true;
            m_NavmeshAgent.destination = m_PositionToInvestigate;
        }

        thisObject.GetComponent<AIPatrolMovement>().enabled = false;
    }

    public override void OnExit(ref AnimState animationState, GameObject thisObject)
    {
        animationState = AnimState.IDLE;

        m_MovingBackToWaypoint = false;

        if (m_NavmeshAgent)
        {
            m_NavmeshAgent.enabled = false;
        }

        thisObject.GetComponent<AIPatrolMovement>().enabled = true;
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

    [SerializeField]
    private PatrolWaypoint[] m_AllWaypoints;

    private PatrolWaypoint m_CurrentWaypoint;
    private PatrolWaypoint m_PriorWaypoint;

    [SerializeField]
    private GameObject m_ThisObject;

    private PatrolFSM m_PatrolFSM;

    private AnimState m_CurrentState = AnimState.IDLE;

    public AnimState GetCurrentAnimationState() { return m_CurrentState; }

    private Vector3 m_PointOfInterest = Vector3.zero;

    // ----------------------------------------------------------------------------

    public PatrolWaypoint GetCurrentWaypoint()                           { return m_CurrentWaypoint; }
    public void           SetCurrentWaypoint(PatrolWaypoint newWaypoint) { m_CurrentWaypoint = newWaypoint; }

    public PatrolWaypoint GetPriorWaypoint()                 { return m_PriorWaypoint; }
    public void           SetPriorWaypoint(PatrolWaypoint newWaypoint) { m_PriorWaypoint = newWaypoint; }

    // ----------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentState = AnimState.IDLE;

        m_CurrentWaypoint  = m_StartWaypoint;
        m_PriorWaypoint    = m_CurrentWaypoint;

        transform.position = m_StartWaypoint.m_ThisPosition.position;

        m_PatrolFSM        = new PatrolFSM();

        m_PatrolFSM.m_FSMStack.Push(new PatrolState());

        m_PatrolFSM.m_FSMStack.Peek().OnEnter(ref m_CurrentState, this.gameObject);
    }

    // ----------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        m_PatrolFSM.m_FSMStack.Peek().Update(Time.deltaTime, this.gameObject, ref m_CurrentState, m_PointOfInterest);

        switch ((m_PatrolFSM.m_FSMStack.Peek().HandleTransition()))
        {
            case PatrolFSMState.PATROL:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit(ref m_CurrentState, gameObject);

                    m_PatrolFSM.m_FSMStack.Push(new PatrolState());

                m_PatrolFSM.m_FSMStack.Peek().OnEnter(ref m_CurrentState, gameObject);
            }
            break;

            case PatrolFSMState.INVESTIGATE:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit(ref m_CurrentState, gameObject);

                    m_PatrolFSM.m_FSMStack.Push(new InvestigateState(m_PointOfInterest));

                m_PatrolFSM.m_FSMStack.Peek().OnEnter(ref m_CurrentState, gameObject);
            }
            break;

            case PatrolFSMState.ATTACK_PLAYER:
            {
                m_CurrentState = AnimState.SHOOT;

                m_PatrolFSM.m_FSMStack.Peek().OnExit(ref m_CurrentState, gameObject);

                    m_PatrolFSM.m_FSMStack.Push(new AttackPlayerState());

                m_PatrolFSM.m_FSMStack.Peek().OnEnter(ref m_CurrentState, gameObject);
            }
            break;

            case PatrolFSMState.Exit:
            {
                m_PatrolFSM.m_FSMStack.Peek().OnExit(ref m_CurrentState, gameObject);

                    m_PatrolFSM.m_FSMStack.Pop();

                m_PatrolFSM.m_FSMStack.Peek().OnEnter(ref m_CurrentState, gameObject);
            }
            break;

            default:
                m_PointOfInterest = Vector3.zero;
            return;
        }
    }

    // ---------------------------------------------------------------------------

    public void SetHeardBomb(Vector3 position)
    {
        // In here if a bomb goes off in range of the enemy
        if(m_PatrolFSM.m_FSMStack.Peek().m_InternalState == PatrolFSMState.PATROL)
        {
            PatrolState state = (PatrolState)m_PatrolFSM.m_FSMStack.Peek();
            state.SetHeardBomb(true);
        }

        m_PointOfInterest = position;
    }

    // ---------------------------------------------------------------------------

    public PatrolWaypoint GetClosestWaypoint()
    {
        float          closestDistance  = 40000000.0f;
        PatrolWaypoint closestWaypoint  = m_AllWaypoints[0];

        foreach (PatrolWaypoint waypoint in m_AllWaypoints)
        {
            Vector3 offset = waypoint.m_ThisPosition.position - waypoint.m_ThisPosition.position;

            if (offset.magnitude < closestDistance)
            {
                closestWaypoint = waypoint;
                closestDistance = offset.magnitude;
                continue;
            }
        }

        return closestWaypoint;
    }

    // ---------------------------------------------------------------------------

    public void SetPointOfInterest(Vector3 position)
    {
        m_PointOfInterest = position;
    }

    public Vector3 GetPointOfInterest()
    {
        return m_PointOfInterest;
    }

    public PatrolWaypoint GetWaypointAtPosition(Vector3 position)
    {
        foreach(PatrolWaypoint waypoint in m_AllWaypoints)
        {
            if (waypoint.m_ThisPosition.position == position)
                return waypoint;
        }

        return m_AllWaypoints[0];
    }
}

// ----------------------------------------------------------------------