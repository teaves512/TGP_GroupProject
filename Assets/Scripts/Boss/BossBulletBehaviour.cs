using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletBehaviour : MonoBehaviour
{
    [SerializeField] public float m_Damage;
    // Start is called before the first frame update
    void Start()
    {
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //deal damage
        }

        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<Destructable>().TakeDamage(m_Damage);
        }
    }
}
