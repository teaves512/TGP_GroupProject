using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIPositionScale : MonoBehaviour
{
    [SerializeField] CanvasScaler m_canvasScaler;
    [SerializeField] Vector2 m_originalPosition;

    Vector2 m_ScreenResolution;
    RectTransform m_rectTransform;
    RectTransform m_canvasRectTransform;

    void Start()
    {
        m_canvasRectTransform = m_canvasScaler.GetComponent<RectTransform>();
        m_rectTransform = GetComponent<RectTransform>();

        OnResolutionChange(m_canvasRectTransform.sizeDelta);
    }

    void Update()
    {
        if (m_canvasRectTransform.sizeDelta != m_ScreenResolution)    //Needed in update, since Unity doesn't have a resolution change callback
            OnResolutionChange(m_canvasRectTransform.sizeDelta);
    }

    void OnResolutionChange(Vector2 newResolution)
    {
        m_ScreenResolution = newResolution;

        m_rectTransform.anchoredPosition = new Vector2(m_originalPosition.x * (m_canvasRectTransform.rect.width / m_canvasScaler.referenceResolution.x), m_rectTransform.anchoredPosition.y);
    }
}
