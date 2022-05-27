using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBombBehaviour : MonoBehaviour
{

    [Header("Self Info")]
    [SerializeField] protected GameObject m_BombPrefab;
    [SerializeField] protected GameObject m_BombVisual;
    [SerializeField] protected ParticleSystem m_ExplosionParticleEffect;
    [SerializeField] private GameObject m_RedFlash;
    [HideInInspector] private float m_RedFlashRate;
    [SerializeField] private float m_MaxRedFlashRate = 0.8f;
    [HideInInspector] private bool m_RedFlashActive = true;
	[HideInInspector] private bool m_RedFlashExists = false;
    [Header("Stats")]
    [SerializeField] protected int m_LayerMask = 6;
    [SerializeField] [Range(0f, 1f)] protected float m_DestroyDelay=0.3f;
    [SerializeField] [Range(0f, 1f)] protected float m_SelfDestroyDelay=0.6f;
	[Header("Bomb type")]
	[SerializeField] protected bool m_TimerEnabled;
	[SerializeField] protected bool m_ClusterEnabled;
	[Header("Bomb stats")]
    [SerializeField] protected float m_Damage;
    [HideInInspector] protected bool m_Explode;
    [HideInInspector] protected bool m_Exploded;
    [SerializeField] protected float m_Timer;
	[SerializeField] protected float m_ClusterDamage;
	[SerializeField] protected GameObject m_ClusterBomb;
	[SerializeField][Range(0.0f,1.0f)] protected float m_ForceValue;
    [Header("Screen shake")]
    [SerializeField] protected float m_ShakeMagnitude = 0.5f;
    [SerializeField] protected float m_ShakeTime = 0.1f;
    [SerializeField] protected float m_ShakeLength = 0.5f;

	[Header("Bomb Sound Radius")]
	[SerializeField] protected GameObject m_BombSoundRadius;


	// Start is called before the first frame update
	protected virtual void Start()
	{
		m_RedFlashRate = m_MaxRedFlashRate;
		if (m_ClusterEnabled)
		{
			Debug.Log(m_Damage);
			m_Damage = m_ClusterDamage;
			Debug.Log(m_Damage);
		}

		if (m_RedFlash != null)
		{
			m_RedFlashExists = true;
		}
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
		if (m_RedFlashExists)
		{
			m_RedFlashRate -= Time.deltaTime;
			if (m_RedFlashRate <= 0)
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
    }

    protected virtual IEnumerator Explode()
    {
		if(m_BombSoundRadius)
			m_BombSoundRadius.SetActive(true);

		AudioManager.Play("Boom");
		Camera.main.GetComponent<LocalCameraShake>().Shake();

		m_ExplosionParticleEffect.Play();
        m_BombVisual.SetActive(false);
		if(m_ClusterEnabled)
			SpawnCluster();
        Camera.main.GetComponent<CameraShake>()?.Shake(m_ShakeMagnitude, m_ShakeTime, m_ShakeLength);
        yield return new WaitForSeconds(m_SelfDestroyDelay);
        Destroy(m_BombPrefab);
        yield return null;
    }
	private void SpawnCluster()
	{
		for (int i = 0; i < 4; i++)
		{
			//SPAWN CLUSTER
			GameObject Cluster = Instantiate(m_ClusterBomb, transform.position, transform.rotation);
			//APPLY FORCE IN 4 DIRECTIONS
			Cluster.GetComponent<Rigidbody>().AddForce(new Vector3((i%2)*2-1,1,(i/2)*2-1) * m_ForceValue, ForceMode.Impulse);
		}
	}

	private void Timer()
    {
        m_Timer -= Time.deltaTime;
    }
}
