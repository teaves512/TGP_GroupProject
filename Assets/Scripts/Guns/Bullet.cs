using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// --------------------------------------------------------------------

public class Bullet : MonoBehaviour
{
    private float     m_TimeLeftAlive = 5.0f;

    private Rigidbody m_ThisRigidbody;

    [SerializeField] private float m_MovementSpeed = 10.0f;
    [SerializeField] private float m_damage = 100;
    [SerializeField] private bool m_DoThisCode = true;


    // -----------

    // Start is called before the first frame update
    void Start()
    {
        m_ThisRigidbody          = GetComponent<Rigidbody>();

        m_ThisRigidbody.velocity = transform.forward * m_MovementSpeed;
    }

    // -----------

    // Update is called once per frame
    void Update()
    {
        m_TimeLeftAlive -= Time.deltaTime;

        if(m_TimeLeftAlive <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // -----------

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthComponent>())
        {
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(m_damage);
            Debug.Log("Hit");
        }


        if (m_DoThisCode)
        {
            if (collision.gameObject.GetComponent<PlayerInput>())
            {
                collision.gameObject.transform.forward = -transform.forward;
            }
        }




        Destroy(this.gameObject);
    }

    // -----------
}

// --------------------------------------------------------------------
