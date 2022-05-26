using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private BossBehaviour m_BossBehaviourScript;
    [SerializeField] private Transform m_BulletSpawn;
    [SerializeField] private float m_BulletInterval;
    // Start is called before the first frame update
    void Start()
    {
        m_BossBehaviourScript = GetComponentInParent<BossBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator FireGun(int numOfBullets)
    {
        for (int i = 0; i < numOfBullets; i++)
        {
            int rand = Random.Range(1, 3);
            switch(rand)
            {
                case 1:
                    AudioManager.Play("TankSentry");
                    break;
                case 2:
                    AudioManager.Play("TankSentry2");
                    break;
                case 3:
                    AudioManager.Play("TankSentry3");
                    break;
            }
            GameObject bullet = Instantiate(m_Bullet, m_BulletSpawn.position, m_BulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * 20, ForceMode.Impulse);
            yield return new WaitForSeconds(m_BulletInterval);
        }
        //m_BossBehaviourScript.m_Firing = false;
    }
}
