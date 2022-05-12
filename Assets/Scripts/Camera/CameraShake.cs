using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Coroutine m_Coroutine;

    public void Shake(float magnitude, float time, float length)
    {
        if(m_Coroutine == null)
            m_Coroutine = StartCoroutine(ScreenShake(magnitude, time, length));
    }

    private IEnumerator ScreenShake(float magnitude, float time, float length)
    {
        float startTime = Time.time;
        float currentTime = Time.time - startTime;
        while (currentTime < length)
        {
            currentTime = Time.time - startTime;
            Vector3 offset = new Vector3(0, 0, 2.0f * magnitude * Mathf.Ceil(Mathf.Sin(-Mathf.PI / time * currentTime)) - 0.5f);    //Alternates between +- magnitude every time seconds for length
            gameObject.transform.position += offset;

            yield return new WaitForSeconds(time);
            gameObject.transform.position -= offset;
        }

        m_Coroutine = null;
        yield return null;
    }
}
