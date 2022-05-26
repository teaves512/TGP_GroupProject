using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------

public class GunControl : MonoBehaviour
{
    // -------------

    [SerializeField]
    private GameObject m_BulletPrefab;

    private List<GameObject> m_Bullets;

    [SerializeField]
    private float m_TimeDelayForBulletSpawn_FirstPress;

    [SerializeField]
    private GameObject m_BulletParent;

    [SerializeField]
    private float m_TimeDelayForBulletSpawn_HeldDown;

    private float m_TimeDelayRemainingForBulletSpawn;

    private bool m_SpawningBullet = false;
    private bool m_HeldDown       = false;

    private Vector3 m_PositionToSpawn;
    private Vector3 m_DirectionToSpawn;

    // -------------

    // Start is called before the first frame update
    void Start()
    {
        m_Bullets = new List<GameObject>();

        m_TimeDelayRemainingForBulletSpawn = m_TimeDelayForBulletSpawn_FirstPress;
    }

    // -------------

    // Update is called once per frame
    void Update()
    {
        if (m_SpawningBullet)
        {
            m_TimeDelayRemainingForBulletSpawn -= Time.deltaTime;

            if(m_TimeDelayRemainingForBulletSpawn <= 0.0f)
            {
                SpawnBullet();

                m_HeldDown = true;

                m_TimeDelayRemainingForBulletSpawn = m_TimeDelayForBulletSpawn_HeldDown;
            }
        }
    }

    // -------------

    public void FireBullet(Vector3 position, Vector3 direction)
    {
        m_SpawningBullet   = true;

        m_PositionToSpawn  = position;
        m_DirectionToSpawn = direction;
    }

    public void StopSpawningBullets()
    {
        m_SpawningBullet = false;
        m_HeldDown       = false;

        m_TimeDelayRemainingForBulletSpawn = m_TimeDelayForBulletSpawn_FirstPress;
    }

    // -------------

    private void SpawnBullet()
    {
        if (!m_BulletParent || !m_BulletPrefab)
            return;

        //AudioManager.Play("Pew");
        int rand = (int)Random.Range(1, 3);
        switch(rand)
        {
            case 1:
                AudioManager.Play("Pew");
                break;
            case 2:
                AudioManager.Play("Pew2");
                break;
            case 3:
                AudioManager.Play("Pew3");
                break;
        }

        GameObject newBullet = Instantiate(m_BulletPrefab, m_PositionToSpawn, Quaternion.Euler(m_DirectionToSpawn), m_BulletParent.transform);
        newBullet.transform.forward = gameObject.transform.forward;

        m_Bullets.Add(newBullet);
    }

    // -------------
}

// ----------------------------------------------------------------