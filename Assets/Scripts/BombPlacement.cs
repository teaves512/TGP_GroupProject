using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour
{
    //Variables
    [SerializeField] [Range(0.0f, 10.0f)] private float m_rayDistance = 1.25f;
    [SerializeField] [Range(-1.0f, 1.0f)] private float m_bombOffset = 0.02f;
    [SerializeField] private GameObject m_bomb;
    [SerializeField] private string m_bombKey;
    [SerializeField] private LayerMask m_layerMask;
    private GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        m_player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Create raycast
        RaycastHit m_hit = new RaycastHit();

        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out m_hit, m_rayDistance, m_layerMask) && Input.GetKeyDown(m_bombKey))
        {
            //Check to see if the object can have bombs placed on it
            if (m_hit.collider.gameObject.tag == "Placeable")
            {
                //Set tag to "placed" to stop multiple bombs being placed
                Debug.Log("Collided with wall");
                m_hit.collider.gameObject.tag = "Placed";

                //Place bomb
                //Calc new pos for bomb
                Vector3 m_newPos = new Vector3();
                m_newPos.y = m_hit.point.y + 1.0f;

                if (transform.forward.x == -1)
                    m_newPos.x = m_hit.point.x + m_bombOffset;
                else if (transform.forward.x == 1)
                    m_newPos.x = m_hit.point.x - m_bombOffset;
                else
                    m_newPos.x = m_hit.point.x;

                if (transform.forward.z == -1)
                    m_newPos.z = m_hit.point.z + m_bombOffset;
                else if (transform.forward.z == 1)
                    m_newPos.z = m_hit.point.z - m_bombOffset;
                else
                    m_newPos.z = m_hit.point.z;

                Object.Instantiate(m_bomb, m_newPos, transform.rotation);
            }
            else if (m_hit.collider.gameObject.tag == "Placed")
                //If bomb is already placed on the wall, do nothing
                Debug.Log("Bomb already placed");
        }
        else if(Input.GetKeyDown(m_bombKey))
        {
            //Place bomb at the players feet
            Object.Instantiate(m_bomb, transform.position, Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
        }
    }
}