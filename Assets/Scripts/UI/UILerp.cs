using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILerp : MonoBehaviour
{
    [SerializeField] float m_WidthMultiplier;
    [SerializeField] float m_HeightMultiplier;
    [SerializeField] float m_time;
    [SerializeField] RectTransform m_UIToMove;
    [SerializeField] UnityEvent m_OnFinishMove;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] float m_AmountToMove;

    Coroutine m_coroutine;

    public void Move()
    {
        TryCoroutine(Lerp(m_WidthMultiplier, 0));
    }

    public void MoveBack()
    {
        TryCoroutine(Lerp(-m_WidthMultiplier, 0));
    }

    public void MoveUp()
    {
        TryCoroutine(Lerp(0, m_HeightMultiplier, false));
    }

    public void MoveDown()
    {
        TryCoroutine(Lerp(0, -m_HeightMultiplier, false));
    }

    void TryCoroutine(IEnumerator routine)
    {
        if (m_coroutine == null)
            m_coroutine = StartCoroutine(routine);
    }

    IEnumerator Lerp(float widthMultiplier, float heightMultiplier, bool isHorizontal = true)
    {
        RectTransform canvasTransform = m_Canvas.GetComponent<RectTransform>();
        float startTime = Time.time;
        float currentTime = 0;

        Vector2 targetPosition = new Vector2();
        if (isHorizontal)
            targetPosition.x = m_UIToMove.anchoredPosition.x + widthMultiplier * m_AmountToMove * canvasTransform.rect.width;
        else
            targetPosition.y = m_UIToMove.anchoredPosition.y + heightMultiplier * m_AmountToMove * canvasTransform.rect.height;

        while (currentTime < m_time)
        {
            currentTime = Time.time - startTime;

            if(isHorizontal)
                m_UIToMove.anchoredPosition = new Vector2(Mathf.Lerp(m_UIToMove.anchoredPosition.x, targetPosition.x, 10.0f * Time.deltaTime), m_UIToMove.anchoredPosition.y);
            else
                m_UIToMove.anchoredPosition = new Vector2(m_UIToMove.anchoredPosition.x, Mathf.Lerp(m_UIToMove.anchoredPosition.y, targetPosition.y, 10.0f * Time.deltaTime));

            yield return null;
        }

        m_OnFinishMove.Invoke();
        m_coroutine = null;
    }
}
