using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class HealthComponent : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [Header("Health Info")]
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_Health;
    [SerializeField] [Range(0, 1)] private float m_RegenRate;
    [SerializeField] private GameObject m_HealthUI;
    [SerializeField] private Slider m_HealthSlider;
    [SerializeField] private bool m_HealthIsActive;
    [SerializeField] private float m_MaxRegenTimer = 5.0f;
    [SerializeField] private float m_RegenTimer;
    [Header("Health non dietetic ui")]
    [SerializeField] private UserManager m_userManager;

    [SerializeField] private bool isRegening;
    // Start is called before the first frame update

    private void Start()
    {
        m_Health = m_MaxHealth;
        isRegening = false;
        EventManager.GameOver += GameOver;
        m_RegenTimer = m_MaxRegenTimer;
        m_userManager = FindObjectOfType<UserManager>();
    }

    private void OnDestroy()
    {
        EventManager.GameOver -= GameOver;
    }

    void Update()
    {
        if(m_HealthSlider)
            m_HealthSlider.value = m_Health / m_MaxHealth;

        //m_HealthUIImage.fillAmount = m_Health / m_MaxHealth;

        if (m_Health < m_MaxHealth)
        {
            if (m_RegenTimer > 0)
                m_RegenTimer -= 1 * Time.deltaTime;
            if (m_RegenTimer <= 0)
            {
                Regen();
            }
        }

        if (m_Health >= m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
    }



    public void ActivateHealth()
    {
        if (m_HealthIsActive)
        {
            return;
        }
        if(m_HealthUI)
            m_HealthUI.SetActive(true);

        m_HealthIsActive = true;
        //print("health activated");
        StartCoroutine(StopShowingHealth());
    }

    IEnumerator StopShowingHealth()
    {
        yield return new WaitForSeconds(3);
        //print("health Deactivated");

        if(m_HealthUI)
            m_HealthUI.SetActive(false);

        m_HealthIsActive = false;
    }

    public void TakeDamage(float damage)
    {
        m_RegenTimer = m_MaxRegenTimer;
        m_Health -= damage;
        //StopCoroutine(RegenHealth());
        //if (isPlayer)
        //{
        //    StartCoroutine(RegenHealth());
        //}
        if (m_Health <= 0 && isPlayer)
        {
            GetComponent<PlayerAnimationTriggers>().SetDead();

            EventManager.OnGameOver(false);
        }
        else if (m_Health <= 0 && !isPlayer)
        {
            if(GetComponent<EnemyAnimationTriggers>())
            {
                GetComponent<EnemyAnimationTriggers>().SetDead();

                GetComponent<AIPatrol>().enabled         = false;
                GetComponent<AIPatrolMovement>().enabled = false;
            }

            AudioManager.Play("Death");
            //gameObject.SetActive(false);

            if (m_userManager)
            {
                m_userManager.m_User.PlayersAchievements.AddEnemiesSpliffed();
                m_userManager.Save();
            }
        }
        ActivateHealth();
    }

    void GameOver(bool victory)
    {
        if(!isPlayer)
            gameObject.SetActive(gameObject);
        else 
        {
            GetComponent<PlayerAnimationTriggers>().enabled = false;
        }
    }
    private void Regen()
    {
        //Debug.Log("heal");
        m_Health += m_RegenRate;
    }
    private IEnumerator RegenHealth()
    {
        //print("coroutine entered");
        yield return new WaitForSeconds(2f);
        //isRegening = true;
        while (m_Health < m_MaxHealth)
        {
            m_Health += m_RegenRate;
            yield return new WaitForSeconds(1f);
            Debug.Log("Heal");
        }
        //print("coroutine exited");
    }
}
