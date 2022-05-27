using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraShake : MonoBehaviour
{
    [SerializeField] private float m_MaxMagnitude = 1.0f;
    [SerializeField] private float m_ReductionSpeed = 1.0f;
    private float m_Magnitude;

    private Coroutine m_cShake = null;

    private void Start()
    {
        m_cShake = null;
    }

    public void Shake(float _amount = 0.5f)
    {
        if (m_cShake != null) { StopCoroutine(m_cShake); }
        m_cShake = StartCoroutine(C_Shake(_amount));
    }

    private IEnumerator C_Shake(float _amount)
    {
        m_Magnitude += _amount;
        if (m_Magnitude > m_MaxMagnitude) { m_Magnitude = m_MaxMagnitude; }

        while (m_Magnitude > 0)
        {
            Vector3 pos = Vector3.zero;
            pos += new Vector3(
                Random.Range(-0.1f, 1.0f),
                Random.Range(-0.1f, 1.0f),
                Random.Range(-0.1f, 1.0f)).normalized
                * m_Magnitude;

            transform.localPosition = pos;

            m_Magnitude -= m_ReductionSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_Magnitude = 0;

        m_cShake = null;
    }
}
