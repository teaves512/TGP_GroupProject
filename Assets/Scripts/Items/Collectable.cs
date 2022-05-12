using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject m_collectable;
    [SerializeField] private Text m_text;
    [SerializeField] private const string c_level1Bomb = "Level 1 Bomb";
    [SerializeField] private const string c_level2Bomb = "Level 2 Bomb";
    // Start is called before the first frame update
    void Start()
    {
        m_collectable = gameObject;
        m_text.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch(m_collectable.tag)
            {
                case c_level1Bomb:
                    m_text.text = "Level 1 Bomb Collected!";
                    m_collectable.transform.position = new Vector3(10000, 0, 10000);
                    yield return new WaitForSeconds(0.75f);
                    m_text.text = "";                    
                    break;
                case c_level2Bomb:
                    m_text.text = "Level 2 Bomb Collected!";
                    m_collectable.transform.position = new Vector3(10000, 0, 10000);
                    yield return new WaitForSeconds(0.75f);
                    m_text.text = "";
                    break;
                default:
                    break;
            }
        }
    }
}
