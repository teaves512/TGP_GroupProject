using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    //Section to store currently held bomb count
    [Header("Inventory")]
    [SerializeField] private int m_basicBombHeld;
    [SerializeField] private int m_fireBombHeld;
    [SerializeField] private int m_areaBombHeld;
    [SerializeField] private int m_walkingBombHeld;

    //Section for the max amount of bombs per type
    [Header("Bomb Max")]
    [SerializeField] private int m_basicBombMax;
    [SerializeField] private int m_fireBombMax;
    [SerializeField] private int m_areaBombMax;
    [SerializeField] private int m_walkingBombMax;

    //Section for the amount collected when a pickup is acquired
    [Header("Bomb Pickup Amount")]
    [SerializeField] private int m_basicBombPickup;
    [SerializeField] private int m_fireBombPickup;
    [SerializeField] private int m_areaBombPickup;
    [SerializeField] private int m_walkingBombPickup;

    //Get Methods
    public int GetBasicBombCount() { return m_basicBombHeld; }
    public int GetFireBombCount() { return m_fireBombHeld; }
    public int GetAreaBombCount() { return m_areaBombHeld; }
    public int GetWalkingBombCount() { return m_walkingBombHeld; }

    public int GetBasicBombMax()   { return m_basicBombMax; }
    public int GetFireBombMax()    { return m_fireBombMax; }
    public int GetAreaBombMax()    { return m_areaBombMax; }
    public int GetWalkingBombMax() { return m_walkingBombMax; }

    //Set Methods
    public void ReduceBasicBombCount()   { m_basicBombHeld--; }
    public void ReduceFireBombCount()    { m_fireBombHeld--; }
    public void ReduceAreaBombCount()    { m_areaBombHeld--; }
    public void ReduceWalkingBombCount() { m_walkingBombHeld--; }
    public void ZeroBasicBomb() { m_basicBombHeld = 0; }
    public void ZeroFireBomb() { m_fireBombHeld = 0; }
    public void ZeroAreaBomb() { m_areaBombHeld = 0; }
    public void ZeroWalkingBomb() { m_walkingBombHeld = 0; }

    // Start is called before the first frame update
    private void Start()
    {
        m_basicBombHeld   = 0;
        m_fireBombHeld    = 0;
        m_areaBombHeld    = 0;
        m_walkingBombHeld = 0;
    }

    public void BasicBombPickup()
    {
        //Increase basic bomb count
        m_basicBombHeld += m_basicBombPickup;

        if (m_basicBombHeld > m_basicBombMax)
            m_basicBombHeld = m_basicBombMax;
    }

    public void FireBombPickup()
    {
        //Increase fire bomb count
        m_fireBombHeld += m_fireBombPickup;

        if (m_fireBombHeld > m_fireBombMax)
            m_fireBombHeld = m_fireBombMax;
    }

    public void AreaBombPickup()
    {
        //Increase area bomb count
        m_areaBombHeld += m_areaBombPickup;

        if (m_areaBombHeld > m_areaBombMax)
            m_areaBombHeld = m_areaBombMax;
    }

    public void WalkingBombPickup()
    {
        //Increase walking bomb count
        m_walkingBombHeld += m_walkingBombPickup;

        if (m_walkingBombHeld > m_walkingBombMax)
            m_walkingBombHeld = m_walkingBombMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("Data Saved", "true");
        PlayerPrefs.SetInt("Basic Bomb Held", m_basicBombHeld);
        PlayerPrefs.SetInt("Fire Bomb Held", m_fireBombHeld);
        PlayerPrefs.SetInt("Area Bomb Held", m_areaBombHeld);
        PlayerPrefs.SetInt("Walking Bomb Held", m_walkingBombHeld);
    }
}
