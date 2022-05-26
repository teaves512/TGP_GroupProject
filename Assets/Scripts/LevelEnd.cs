using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] string menuScene;
    [SerializeField] float m_FadeTime;
    [SerializeField] Image m_Image;

    Coroutine m_Coroutine;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (m_Coroutine == null)
                m_Coroutine = StartCoroutine(EndLevel());
        }
            
    }

    private IEnumerator EndLevel()
    {
        float startTime = Time.time;
        float currentTime = Time.time - startTime;
        while(currentTime < m_FadeTime)
        {
            currentTime = Time.time - startTime;

            float t = currentTime / m_FadeTime;

            m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, t);

            yield return null;
        }

        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }
}
