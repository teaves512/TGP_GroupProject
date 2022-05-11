using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Info")]
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_Health;
    // Start is called before the first frame update
    void Start()
    {
        m_Health = m_MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
        // Health bar appear above head if damage is taken?
        // -> should they regen health?

    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;

        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
