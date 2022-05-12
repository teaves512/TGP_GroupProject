using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
	private enum Direction
	{
		FORWARD,
		BACKWARD,
		LEFT,
		RIGHT
	};

	private struct GridPosition
	{
		public int x;
		public int y;
		public int z;

		public GridPosition
			(int _x = 0, int _y = 0, int _z = 0)
		{
			x = _x;
			y = _y;
			z = _z;
		}
	}

	[Header("Grid")]
	[SerializeField] private float m_GridCellSize = 1.0f;
	[SerializeField] private float m_GridMovementAllowance = 0.01f;
	private GridPosition m_GridPos;

	[Header("Movement")]
	[SerializeField] private float m_MoveSpeed = 10.0f;
	[SerializeField] private bool m_bAllowMovementQueueing = true;

	[Header("Rotation")]
	[SerializeField] private Vector3 m_RotationForward;
	[SerializeField] private Vector3 m_RotationBackward;
	[SerializeField] private Vector3 m_RotationLeft;
	[SerializeField] private Vector3 m_RotationRight;

	[Header("Components")]
	[SerializeField] private Transform m_Model;

	private Direction m_Direction = Direction.FORWARD;

	private Coroutine m_cMove = null;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		m_GridPos = new GridPosition();
		m_Direction = Direction.FORWARD;
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
			{ m_Direction = Direction.FORWARD; }
			else if (Input.GetKey(KeyCode.S))
			{ m_Direction = Direction.BACKWARD; }
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
		if (m_Model)
		{
			//set the character model's rotation appropriately
			if (m_Direction == Direction.FORWARD) { m_Model.rotation = Quaternion.Euler(m_RotationForward); }
			else if (m_Direction == Direction.BACKWARD) { m_Model.rotation = Quaternion.Euler(m_RotationBackward); }
			else if (m_Direction == Direction.LEFT) { m_Model.rotation = Quaternion.Euler(m_RotationLeft); }
			else if (m_Direction == Direction.RIGHT) { m_Model.rotation = Quaternion.Euler(m_RotationRight); }
		}

		Direction initialDirection = m_Direction;

		//adjust the grid cell position, appropriately
		if (m_Direction == Direction.FORWARD) { m_GridPos.z++; }
		else if (m_Direction == Direction.BACKWARD) { m_GridPos.z--; }
		else if (m_Direction == Direction.LEFT) { m_GridPos.x--; }
		else if (m_Direction == Direction.RIGHT) { m_GridPos.x++; }

		//determine the target position, in world space
		Vector3 targetPosition = new Vector3(m_GridPos.x, 0, m_GridPos.z) * m_GridCellSize;

		//translate linearlly towards the target position, until within the specified allowance
		while (Vector3.SqrMagnitude(transform.position - targetPosition) > m_GridMovementAllowance)
		{
			Vector3 direction = (targetPosition - transform.position).normalized;

			transform.Translate(direction * m_MoveSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		//set position to the exact world-space position
		transform.position = targetPosition;

		//nullify the coroutine, ready to be used again
		m_cMove = null;

		//queue the next movement, if the player made a directional input during this movement
		if (m_bAllowMovementQueueing && m_Direction != initialDirection) { Move(); }
	}

}
