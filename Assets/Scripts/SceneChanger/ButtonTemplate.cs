using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonTemplate : MonoBehaviour
{
    protected abstract void ButtonAction();
    private Button m_ButtonAction;

    protected virtual void Awake()
    {
        m_ButtonAction = GetComponent<Button>();       
        m_ButtonAction.onClick.AddListener(ButtonAction);
    }
}
