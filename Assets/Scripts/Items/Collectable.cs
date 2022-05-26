using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Text m_inventoryText;
    [SerializeField] private Text m_bombPointer;
    [SerializeField] private const float  c_textDelay   = 0.75f;
    [SerializeField] private const string c_basicBomb   = "Basic Bomb";
    [SerializeField] private const string c_fireBomb    = "Fire Bomb";
    [SerializeField] private const string c_areaBomb    = "Area Bomb";
    [SerializeField] private const string c_walkingBomb = "Walking Bomb";
    // Start is called before the first frame update
    void Start()
    {
        m_text.alignment = TextAnchor.MiddleCenter;
        m_inventoryText.alignment = TextAnchor.LowerRight;

        m_text.text = "";
        m_inventoryText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Inventory m_inventory = gameObject.GetComponent<Inventory>();

        m_inventoryText.text = c_basicBomb + ": " + "∞" 
                      + "\n" + c_fireBomb  + ": " + m_inventory.GetFireBombCount().ToString()  + "/" + m_inventory.GetFireBombMax().ToString()
                      + "\n" + c_areaBomb + ": "  + m_inventory.GetAreaBombCount().ToString()  + "/" + m_inventory.GetAreaBombMax().ToString()
                      + "\n" + c_walkingBomb + ": " + m_inventory.GetWalkingBombCount().ToString() + "/" + m_inventory.GetWalkingBombMax().ToString();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case c_basicBomb:
                m_text.text = c_basicBomb + " Collected";
                gameObject.GetComponent<Inventory>().BasicBombPickup();
                other.transform.position = new Vector3(10000, 0, 10000);
                yield return new WaitForSeconds(c_textDelay);
                m_text.text = "";
                break;
            case c_fireBomb:
                m_text.text = c_fireBomb + " Collected";
                gameObject.GetComponent<Inventory>().FireBombPickup();
                other.transform.position = new Vector3(10000, 0, 10000);
				Destroy(other.gameObject);
				yield return new WaitForSeconds(c_textDelay);
                m_text.text = "";
                break;
            case c_areaBomb:
                m_text.text = c_areaBomb + " Collected";
                gameObject.GetComponent<Inventory>().AreaBombPickup();
				//other.transform.position = new Vector3(10000, 0, 10000);
				Destroy(other.gameObject);
                yield return new WaitForSeconds(c_textDelay);
                m_text.text = "";
                break;
            case c_walkingBomb:
                m_text.text = c_walkingBomb + " Collected";
                gameObject.GetComponent<Inventory>().WalkingBombPickup();
                other.transform.position = new Vector3(10000, 0, 10000);
				Destroy(other.gameObject);
				yield return new WaitForSeconds(c_textDelay);
                m_text.text = "";
                break;
            default:
                break;
        }
    }
}
