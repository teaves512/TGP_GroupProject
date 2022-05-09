using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBombBehaviour : MonoBehaviour
{

    [SerializeField]private GameObject m_BombPrefab;
    [SerializeField] private GameObject m_BombVisual;
    [SerializeField] private ParticleSystem m_ExplosionParticleEffect;
    [SerializeField] private int m_LayerMask;
    [SerializeField][Range(0f,1f)] private float m_DestroyDelay;
    [SerializeField] [Range(0f, 1f)] private float m_SelfDestroyDelay;


    void Start()
    {
    }


    void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        Debug.Log("boom");
        //check if this is destructable
        RaycastHit hit;
        m_ExplosionParticleEffect.Play();
        m_BombVisual.SetActive(false);

        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~m_LayerMask))
        {
            Debug.DrawRay(hit.transform.position, transform.forward, Color.red);
            yield return new WaitForSeconds(m_DestroyDelay);
            Destroy(hit.collider.gameObject);

        }

        yield return new WaitForSeconds(m_SelfDestroyDelay);
        Destroy(m_BombPrefab);
        yield return null;
    }
  
}
