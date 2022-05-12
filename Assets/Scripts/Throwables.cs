using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject m_throwable;
    [SerializeField] private GameObject m_player;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_maxForce;

    [SerializeField] private float m_thrust = 0.0f;

    private bool m_thrown;

    // Start is called before the first frame update
    void Start()
    {
        m_player = gameObject;
    }

    void Update()
    {
        //m_thrown = Input.GetKeyDown("g");
        //if (m_thrown)
        //{
        //    Debug.Log("Throwing Bomb");
        //    //m_thrust = 100.0f;
        //    Vector3 m_startPos = new Vector3(transform.position.x + (transform.forward.x * 0.5f), transform.position.y + 1.0f, transform.position.z + (transform.forward.z * 0.5f));
            
        //    GameObject newObject = Object.Instantiate(m_throwable, m_startPos, transform.rotation);
        //    newObject.transform.forward = transform.forward;
        //    newObject.m_Rigidbody.AddForce(transform.forward * m_thrust, ForceMode.Impulse);
        //}

        if(Input.GetMouseButton(1))
        {
            if (m_thrust < m_maxForce)
                m_thrust += Time.deltaTime;
            Debug.Log("Accumulating force");
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Debug.Log("Throwing Bomb");
            Vector3 m_startPos = new Vector3(transform.position.x + (transform.forward.x * 0.5f), transform.position.y + 1.0f, transform.position.z + (transform.forward.z * 0.5f));

            GameObject newObject = Object.Instantiate(m_throwable, m_startPos, transform.rotation);
            newObject.transform.forward = transform.forward;
            newObject.GetComponent<Rigidbody>().AddForce(transform.forward * m_thrust, ForceMode.Impulse);

            m_thrust = 0.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ////Increase force while mouse button is held
        //if(Input.GetMouseButton(1))
        //{
        //    if (m_thrust < m_maxForce)
        //        m_thrust++;
        //    Debug.Log("Increasing force");
        //}
        //else if (Input.GetMouseButtonUp(1))
        //{
        //    Debug.Log("Throwing Bomb");
        //    //Create Starting pos for bomb
        //    Vector3 m_startPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        //    Object.Instantiate(m_throwable, m_startPos, transform.rotation);

        //    m_throwable.m_Rigidbody.AddForce(transform.forward * m_thrust);
        //    m_thrust = 0.0f;
        //}
    }
}
