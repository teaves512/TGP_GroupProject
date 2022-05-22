using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomName : ButtonTemplate
{
    public string[] m_RandomNames;
    [SerializeField] private TMP_InputField m_InputFieldText;


    protected override void ButtonAction()
    {
        m_InputFieldText.text = GetRandomName();
    }

    string GetRandomName()
    {
        int rand = Random.Range(0, m_RandomNames.Length);
        return m_RandomNames[rand];
    }
}
