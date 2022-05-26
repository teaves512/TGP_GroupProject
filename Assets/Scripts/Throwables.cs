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
    [SerializeField]private UserManager m_userManager;

    private bool m_thrown;

    // Start is called before the first frame update
    void Start()
    {
        m_player = gameObject;
        m_userManager = FindObjectOfType<UserManager>();
    }

    void Update()
    {
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
            m_userManager.m_User.PlayersAchievements.AddBombsDropped();
            m_userManager.Save();
        }
        
    }
}
