using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBombBehaviour : MonoBehaviour
{

    [Header("Self Info")]
    [SerializeField] protected GameObject m_BombPrefab;
    [SerializeField] protected GameObject m_BombVisual;
    [SerializeField] protected ParticleSystem m_ExplosionParticleEffect;
    [SerializeField] private Light m_RedFlash;
    [HideInInspector] private float m_RedFlashRate;
    [SerializeField] private float m_MaxRedFlashRate;
    [HideInInspector] private bool m_RedFlashActive = true;
    [Header("Stats")]
    [SerializeField] protected int m_LayerMask;
    [SerializeField] [Range(0f, 1f)] protected float m_DestroyDelay;
    [SerializeField] [Range(0f, 1f)] protected float m_SelfDestroyDelay;
    [Header("Bomb stats")]
    [SerializeField] protected float m_Damage;
    [HideInInspector] protected bool m_Explode;
    [HideInInspector] protected bool m_Exploded;
    [SerializeField] protected bool m_TimerEnabled;
    [SerializeField] protected float m_Timer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_RedFlashRate= m_MaxRedFlashRate;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(m_TimerEnabled)
        {
            if(m_Timer<=0)
            {
                m_Explode = true;
            }
            Timer();
        }
        m_RedFlashRate -= Time.deltaTime;
        if(m_RedFlashRate<=0)
        {
            if (m_RedFlashActive)
            {
                m_RedFlashActive = false;
            }
            else
            {
                m_RedFlashActive = true;
            }
            m_RedFlash.gameObject.SetActive(m_RedFlashActive);
            m_RedFlashRate = m_MaxRedFlashRate;
        }
    }

    protected virtual IEnumerator Explode()
    {
        m_ExplosionParticleEffect.Play();
        m_BombVisual.SetActive(false);

        yield return new WaitForSeconds(m_SelfDestroyDelay);
        Destroy(m_BombPrefab);
        yield return null;
    }
    
    private void Timer()
    {
        m_Timer -= Time.deltaTime;
    }
}
