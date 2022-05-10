using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBombBehaviour : MonoBehaviour
{

    [Header("Self Info")]
    [SerializeField] protected GameObject m_BombPrefab;
    [SerializeField] protected GameObject m_BombVisual;
    [SerializeField] protected ParticleSystem m_ExplosionParticleEffect;
    [Header("Stats")]
    [SerializeField] protected int m_LayerMask;
    [SerializeField] [Range(0f, 1f)] protected float m_DestroyDelay;
    [SerializeField] [Range(0f, 1f)] protected float m_SelfDestroyDelay;
    [Header("Bomb stats")]
    [SerializeField] protected float m_Damage;
    [SerializeField] protected bool m_TimerEnabled;
    [HideInInspector] protected bool m_Exploded;
    [SerializeField] protected float m_Timer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(m_TimerEnabled)
        {
            Timer();
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
