using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfShock : MonoBehaviour
{
	// NOTE : OnTriggerEnter is on the shockTriggers script not here

	[SerializeField] private float m_InitialScale = 1.0f;
	[SerializeField] private float m_FinalScale = 3.0f;
	private float m_CurrentScale = 0.0f;
	[SerializeField] private float m_TimeToFinalScale = 1.0f;
	private float m_ScaleInterval = 0.0f;
	[SerializeField] private float m_MoveSpeed = 3.0f;

	private Coroutine m_cScale = null;

	private void Start()
	{
		m_ScaleInterval = (m_FinalScale - m_InitialScale) / m_TimeToFinalScale;

		StartScale();
	}

	private void StartScale()
	{
		if (m_cScale != null) { StopCoroutine(m_cScale); }
		m_cScale = StartCoroutine(C_Scale());
	}

	private IEnumerator C_Scale()
	{
		m_CurrentScale = m_InitialScale;
		while (m_CurrentScale < m_FinalScale)
		{
			SetScale(m_CurrentScale);
			m_CurrentScale += m_ScaleInterval * Time.deltaTime;

			transform.Translate(Vector3.forward * m_MoveSpeed * Time.deltaTime);

			yield return new WaitForEndOfFrame();
		}
		SetScale(m_FinalScale);

		gameObject.SetActive(false);
		Destroy(gameObject);
		m_cScale = null;
	}

	private void SetScale(float _scale)
	{
		Vector3 scale = transform.localScale;
		scale.x = _scale;
		scale.z = _scale;
		transform.localScale = scale;
	}

}
