using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILerp : MonoBehaviour
{
    [SerializeField] float m_WidthMultiplier;
    [SerializeField] float m_time;
    [SerializeField] RectTransform m_UIToMove;
    [SerializeField] UnityEvent m_OnFinishMove;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] float m_AmountToMove;

    Coroutine m_coroutine;

    public void Move()
    {
        TryCoroutine(Lerp(m_WidthMultiplier));
    }

    public void MoveBack()
    {
        TryCoroutine(Lerp(-m_WidthMultiplier));
    }

    void TryCoroutine(IEnumerator routine)
    {
        if (m_coroutine == null)
            m_coroutine = StartCoroutine(routine);
    }

    IEnumerator Lerp(float widthMultiplier)
    {
        RectTransform canvasTransform = m_Canvas.GetComponent<RectTransform>();
        float initialX = m_UIToMove.anchoredPosition.x;
        float targetX = initialX + widthMultiplier * m_AmountToMove * canvasTransform.rect.width;
        float startTime = Time.time;
        float currentTime = 0;

        while (currentTime < m_time)
        {
            currentTime = Time.time - startTime;

            m_UIToMove.anchoredPosition = new Vector2(Mathf.Lerp(initialX, targetX, currentTime / m_time), m_UIToMove.anchoredPosition.y);
            yield return null;
        }

        m_OnFinishMove.Invoke();
        m_coroutine = null;
    }
}
