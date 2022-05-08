using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    private int m_GridPosX = 0;
    private int m_GridPosZ = 0;
    [SerializeField] private float m_GridCellSize = 1.0f;
    [SerializeField] private float m_GridMovementAllowance = 0.01f;

    [SerializeField] private float m_MoveSpeed = 10.0f;

    private Direction m_Direction = Direction.UP;

    private Coroutine m_cMove = null;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        m_GridPosX = 0;
        m_GridPosZ = 0;

        m_Direction = Direction.UP;

        m_cMove = null;
    }

    private void Update()
    {
        //check for directional input
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            //determine the direction the player wants to move
            if (Input.GetKey(KeyCode.W))
            { m_Direction = Direction.UP; }
            else if (Input.GetKey(KeyCode.S))
            { m_Direction = Direction.DOWN; }
            else if (Input.GetKey(KeyCode.A))
            { m_Direction = Direction.LEFT; }
            else if (Input.GetKey(KeyCode.D))
            { m_Direction = Direction.RIGHT; }

            //start moving, if not already moving
            if (m_cMove == null)
            { Move(); }
        }
    }

    private void Move()
    {
        //ensure there are no existing instances of the C_Move coroutine
        if (m_cMove != null) { StopCoroutine(m_cMove); }
        //assign a new instance of the C_Move coroutine
        m_cMove = StartCoroutine(C_Move());
    }

    private IEnumerator C_Move()
    {
        //adjust the grid cell position, appropriately
        if (m_Direction == Direction.UP)            { m_GridPosZ++; }
        else if (m_Direction == Direction.DOWN)     { m_GridPosZ--; }
        else if (m_Direction == Direction.LEFT)     { m_GridPosX--; }
        else if (m_Direction == Direction.RIGHT)    { m_GridPosX++; }

        //determine the target position, in world space
        Vector3 targetPosition = new Vector3(m_GridPosX, 0, m_GridPosZ) * m_GridCellSize;

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > m_GridMovementAllowance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            transform.Translate(direction * m_MoveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPosition;

        m_cMove = null;
    }
}
