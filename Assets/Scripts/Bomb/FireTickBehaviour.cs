using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTickBehaviour : MonoBehaviour
{
    [Header("Componenent to damage")]
    [SerializeField] private Destructable m_DestructableScript;
    [SerializeField] private HealthComponent m_HealthCompScript;
    [Header("Stats from bomb")]
    [SerializeField] public float m_FireDamage;
    [SerializeField] public float m_MaxTimeOnFire;
    [SerializeField] private float m_TimeOnFire;
    [Header("Self stats")]
    [SerializeField] private float m_FleshMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        m_DestructableScript = GetComponentInParent<Destructable>();
        m_HealthCompScript = GetComponentInParent<HealthComponent>();

        if(m_HealthCompScript== null && m_DestructableScript == null)
        {
            DestroySelf();
        }
        if(m_HealthCompScript)
        {
            m_FireDamage = m_FireDamage * m_FleshMultiplier;
        }
        m_TimeOnFire = m_MaxTimeOnFire;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimeOnFire >= 0)
        {
            if(m_DestructableScript)
                m_DestructableScript.TakeDamage(m_FireDamage);
            if(m_HealthCompScript)
                m_HealthCompScript.TakeDamage(m_FireDamage);
            m_TimeOnFire -= 1 * Time.deltaTime;
        }
        else
        {
            DestroySelf();
        }
    }

    private void OnTriggerStay(Collider other)
    {

        Vector3 direction = (transform.position - other.transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit,Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, -direction, Color.red);
            if (other.tag == "Enemy")
            {
                Debug.Log(other.name);
                other.gameObject.GetComponent<HealthComponent>().TakeDamage(m_FireDamage);
            }

        }
    }
    private void DestroySelf()
    {
        transform.parent = null;
        Destroy(gameObject);
    }
}
