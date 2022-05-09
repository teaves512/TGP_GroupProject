using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject m_BombPrefab;
    [SerializeField] private ParticleSystem m_ExplosionParticleEffect;


    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            Explode();
        }
    }

    private void Explode()
    {

        //check if this is destructable
        RaycastHit hit;
        if(Physics.Raycast(transform.position,-transform.forward,out hit, 2.0f,6))
        {
            Debug.DrawRay(hit.transform.position, -transform.forward,Color.red);
            Destroy(hit.collider.gameObject);
        }
        //play default explosion particle effect
        m_ExplosionParticleEffect.Play();
    }
}
