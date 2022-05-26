using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private float m_Dampening = 10.0f;

    private Coroutine m_cShow = null;

    private void Start()
    {
        Vector3 rot = new Vector3(70.0f, 0.0f, 0.0f);
        transform.rotation = Quaternion.Euler(rot);
    }

    private void Show()
    {
        if (m_cShow != null) { StopCoroutine(m_cShow); }
        m_cShow = StartCoroutine(C_Show());
    }

    private IEnumerator C_Show()
    {
        while (transform.rotation.eulerAngles.x > 0.05f)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, Vector3.zero, m_Dampening * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Show();
        }
    }
}
