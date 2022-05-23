using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
	[SerializeField] private Light m_Light;
	[SerializeField] private float m_FlashInterval = 0.5f;
	private float m_CurrentFlashInterval = 0.0f;

	private Coroutine m_cLightFlash = null;

	private void Start()
	{
		m_CurrentFlashInterval = m_FlashInterval;
		StartLightFlash();
	}

	private void StartLightFlash()
	{
		if (m_cLightFlash != null) { StopCoroutine(m_cLightFlash); }
		m_cLightFlash = StartCoroutine(C_LightFlash());
	}

	public void Detonate()
	{
		m_CurrentFlashInterval /= 5.0f;
		StartLightFlash();
	}

	private IEnumerator C_LightFlash()
	{
		while (true)
		{
			yield return new WaitForSeconds(m_CurrentFlashInterval);
			m_Light.enabled = !m_Light.enabled;
		}
		//m_cLightFlash = null;
	}
}
