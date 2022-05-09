using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private float m_Health = 10.0f;
    [SerializeField] private Texture m_MainTexture;
    private Material m_mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " says ouch");
        m_Health -= damage;

        //transform.GetComponent<Renderer>().material.SetColor("_BaseMap", Color.red);
        m_mat = transform.GetComponent<Renderer>().material;
        m_mat.SetColor("_BaseMap", Color.red);
        transform.GetComponent<Renderer>().material = m_mat;
        if (m_Health<=0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
