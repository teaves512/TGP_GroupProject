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

    private Direction m_Direction;

    private Coroutine m_cMove;

    private void Update()
    {
        //check for directional input
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.W))
            { m_Direction = Direction.UP; }
            else if (Input.GetKeyDown(KeyCode.S))
            { m_Direction = Direction.DOWN; }
            else if (Input.GetKeyDown(KeyCode.A))
            { m_Direction = Direction.LEFT; }
            else if (Input.GetKeyDown(KeyCode.D))
            { m_Direction = Direction.RIGHT; }


            if (m_cMove == null)
            {
                //start moving in direction
            }
        }
    }

    private IEnumerator C_Move()
    {
        yield return new WaitForEndOfFrame();

        m_cMove = null;
    }
}
