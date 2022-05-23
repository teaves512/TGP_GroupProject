using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private string m_TriggerName;
	[SerializeField] private GameObject m_ChuteHolder;

	private void OnTriggerEnter(Collider other)
	{
		if (m_Animator == null) { Debug.LogWarning("No animator found"); return; }

		if (other.tag == "Player")
		{
			m_Animator.SetTrigger(m_TriggerName);
			m_ChuteHolder.SetActive(true);
		}
	}
}
