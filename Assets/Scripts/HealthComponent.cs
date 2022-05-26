using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HealthComponent : MonoBehaviour
{
    public bool isPlayer;
    [Header("Health Info")]
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_Health;
    [SerializeField][Range(0,1)] private float m_RegenRate;
    [SerializeField] private GameObject m_HealthUI;
    [SerializeField] private Slider m_HealthSlider;
    [SerializeField] private bool m_HealthIsActive;
	[SerializeField] private float m_MaxRegenTimer = 5.0f;
	[SerializeField] private float m_RegenTimer;
	[Header("Health non dietetic ui")] 
    [SerializeField] private Image m_HealthUIImage;

    
    [SerializeField]private bool isRegening;
    // Start is called before the first frame update
    
    private void Start()
    {
        m_Health = m_MaxHealth;
        //m_HealthUIImage.type = Image.Type.Filled;
        isRegening = false;
        EventManager.GameOver += GameOver;
		m_RegenTimer = m_MaxRegenTimer;
	}
    
    
    
    void Update()
    {
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
        m_HealthUI.SetActive(true);
        m_HealthIsActive = true;
        //print("health activated");
        StartCoroutine(StopShowingHealth());
    }
    
    IEnumerator StopShowingHealth()
    {
        yield return new WaitForSeconds(3);
        //print("health Deactivated");
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
        if (m_Health <= 0)
        {
            EventManager.OnGameOver(false);
        }
        ActivateHealth();
    }

    void GameOver(bool victory)
    {
        gameObject.SetActive(gameObject);
    }
	private void Regen()
	{
		Debug.Log("heal");
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
