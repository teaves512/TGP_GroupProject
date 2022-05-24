using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonTemplate : MonoBehaviour
{
    protected abstract void ButtonAction();
    private Button m_ButtonAction;

    protected virtual void Awake()
    {
        m_ButtonAction = GetComponent<Button>();
        if (m_ButtonAction == null)
            m_ButtonAction = GetComponentInChildren<Button>();

        if (m_ButtonAction == null)
            Debug.LogError("No button found");
        else
            m_ButtonAction.onClick.AddListener(ButtonAction);
    }
    
    
}
