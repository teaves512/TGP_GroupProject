using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
	[SerializeField] private float m_MaxHealth = 10.0f;
	[SerializeField] private float m_Health;
    [SerializeField] private Material m_Material;
    [SerializeField] private ParticleSystem m_DeathEffect;
	[SerializeField] private float m_DamageTime;

	//check here later -> https://thomasmountainborn.com/2016/05/25/materialpropertyblocks/

	private void Start()
	{
		m_Health = m_MaxHealth;
		m_Material = GetComponent<Renderer>()?.material;
	}

	public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " says ouch");

		m_Health -= damage;
		StartCoroutine(ApplyDamageToMat(damage));

		if (m_Health<=0)
        {
            StartCoroutine(Death());
        }
    }

	private IEnumerator ApplyDamageToMat(float damage)
	{

		m_Material?.SetFloat("DamageRatio", 1-(m_Health / m_MaxHealth));
		
		yield return null;
	}
    private IEnumerator Death()
    {
        if(m_DeathEffect!=null)
            m_DeathEffect.Play();
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        yield return null;
    }


}
