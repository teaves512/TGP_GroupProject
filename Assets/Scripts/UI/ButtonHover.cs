using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private float m_MaxScaleIncrease = 0.1f;
    [SerializeField] private float m_PulseSpeed = 20.0f;

    private Coroutine m_cHover = null;

    public void OnHoverEnter()
    {
        if (m_cHover != null) { StopCoroutine(m_cHover); }
        m_cHover = StartCoroutine(C_Hover());
    }

    public void OnHoverExit()
    {
        if (m_cHover != null) { StopCoroutine(m_cHover); }
        transform.localScale = Vector3.one;
    }

    private IEnumerator C_Hover()
    {
        while (true)
        {
            Vector3 scale = transform.localScale;
            float scaleMultiplier = 1 + m_MaxScaleIncrease * ((Mathf.Sin(Time.time * m_PulseSpeed) + 1) / 2);
            scale.x = scaleMultiplier;
            scale.y = scaleMultiplier;

            transform.localScale = scale;

            yield return new WaitForEndOfFrame();
        }
        m_cHover = null;
    }
}
