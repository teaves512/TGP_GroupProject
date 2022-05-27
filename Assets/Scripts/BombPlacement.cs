using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombPlacement : MonoBehaviour
{
    //Variables
    [SerializeField] [Range(0.0f, 10.0f)] private float m_rayDistance = 1.25f;
    [SerializeField] [Range(-1.0f, 1.0f)] private float m_bombOffset = 0.02f;
    [SerializeField] private string m_bombKey;
    [SerializeField] private LayerMask m_layerMask;

    [SerializeField] private GameObject m_basicBomb;
    [SerializeField] private GameObject m_fireBomb;
    [SerializeField] private GameObject m_areaBomb;
    [SerializeField] private GameObject m_walkingBomb;

    [SerializeField] private string m_currentBomb;
    [SerializeField] private GameObject m_canvas;
    [SerializeField] private Text m_pointer;
    private Inventory m_inventory;
    [SerializeField]private UserManager m_userManager;

    public void SetCurrentBomb(string m_newBomb)
    {
        m_currentBomb = m_newBomb;
    }

    private void Start()
    {
        if(GetComponent<Inventory>())
            m_inventory = gameObject.GetComponent<Inventory>();

        m_currentBomb = "Basic Bomb";

        if(m_canvas && m_canvas.transform.Find("Bomb Pointer").GetComponent<Text>())
            m_pointer = m_canvas.transform.Find("Bomb Pointer").GetComponent<Text>();
        m_userManager = FindObjectOfType<UserManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Create raycast
        RaycastHit m_hit = new RaycastHit();

        //move the bomb pointer to point at the equipped bomb
        switch(m_currentBomb)
        {
            case "Basic Bomb":
                m_pointer.text = "-->" + "\n" + "\n" + "\n";
                break;
            case "Fire Bomb":
                m_pointer.text = "\n" + "-->" + "\n" + "\n";
                break;
            case "Area Bomb":
                m_pointer.text = "\n" + "\n" + "-->" + "\n";
                break;
            case "Walking Bomb":
                m_pointer.text = "\n" + "\n" + "\n" + "-->";
                break;

            default:
                return;
        }

        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(new Vector3( transform.position.x, transform.position.y+0.3f, transform.position.z), transform.forward, out m_hit, m_rayDistance, ~m_layerMask) && Input.GetKeyDown(m_bombKey))
        {
            if (!m_inventory)
                return;

            switch (m_currentBomb)
            {
                case "Basic Bomb":
                    //Limited Basic Bombs
                    /*
                    if (m_inventory.GetBasicBombCount() > 0)
                    {
                        if (m_hit.collider.gameObject.tag == "Placeable")
                        {
                            PlaceBomb(m_hit, m_basicBomb);
                            m_inventory.ReduceBasicBombCount();
                            Debug.Log("Bomb Placed on Wall");
                        }
                        else if (m_hit.collider.gameObject.tag == "Placed")
                        {
                            Debug.Log("Bomb already placed");
                        }
                    }
                    else
                    {
                        Debug.Log("Out of Basic Bombs");
                    }
                    */
                    //Unlimited Basic Bombs
                    if (m_hit.collider.gameObject.tag == "Placeable")
                    {
                        PlaceBomb(m_hit, m_basicBomb);
                        m_inventory.ReduceBasicBombCount();
                        Debug.Log("Bomb Placed on Wall");
                    }
                    else if (m_hit.collider.gameObject.tag == "Placed")
                    {
                        Debug.Log("Bomb already placed");
                    }
                    break;
                case "Fire Bomb":
                    if (m_inventory.GetFireBombCount() > 0)
                    {
                        if (m_hit.collider.gameObject.tag == "Placeable")
                        {
                            PlaceBomb(m_hit, m_fireBomb);
                            m_inventory.ReduceFireBombCount();
                            Debug.Log("Bomb Placed on Wall");
                        }
                        else if (m_hit.collider.gameObject.tag == "Placed")
                        {
                            Debug.Log("Bomb already placed");
                        }
                    }
                    else
                    {
                        Debug.Log("Out of Fire Bombs");
                    }
                    break;
                case "Area Bomb":
                    if (m_inventory.GetAreaBombCount() > 0)
                    {
                        if (m_hit.collider.gameObject.tag == "Placeable")
                        {
                            PlaceBomb(m_hit, m_areaBomb);
                            EventManager.OnPlayerDroppedBomb();
                            m_inventory.ReduceAreaBombCount();
                            Debug.Log("Bomb Placed on Wall");
                        }
                        else if (m_hit.collider.gameObject.tag == "Placed")
                        {
                            Debug.Log("Bomb already placed");
                        }
                    }
                    else
                    {
                        Debug.Log("Out of Area Bombs");
                    }
                    break;
                case "Walking Bomb":
                    if (m_inventory.GetWalkingBombCount() > 0)
                    {
                        if (m_hit.collider.gameObject.tag == "Placeable")
                        {
                            PlaceBomb(m_hit, m_walkingBomb);
                            m_inventory.ReduceWalkingBombCount();
                            Debug.Log("Bomb Placed on Wall");
                        }
                        else if (m_hit.collider.gameObject.tag == "Placed")
                        {
                            Debug.Log("Bomb already placed");
                        }
                    }
                    else
                    {
                        Debug.Log("Out of Walking Bombs");
                    }
                    break;


                default:
                    break;
            }    
        }
        else if(Input.GetKeyDown(m_bombKey))
        {
            if (!m_inventory)
                return;

            //Place bomb at the players feet
            switch (m_currentBomb)
            {
                case "Basic Bomb":
                    //Limited basic bombs
                    /*
                    if (m_inventory.GetBasicBombCount() > 0)
                    {
                        PlaceBomb(gameObject.transform, m_basicBomb);
                        m_inventory.ReduceBasicBombCount();
                    }
                    */
                    //Unlimited Basic Bombs
                    PlaceBomb(gameObject.transform, m_basicBomb);
                    m_inventory.ReduceBasicBombCount();
                    break;
                case "Fire Bomb":
                    if (m_inventory.GetFireBombCount() > 0)
                    {
                        PlaceBomb(gameObject.transform, m_fireBomb);
                        m_inventory.ReduceFireBombCount();
                    }
                    break;
                case "Area Bomb":
                    if (m_inventory.GetAreaBombCount() > 0)
                    {
                        PlaceBomb(gameObject.transform, m_areaBomb);
                        m_inventory.ReduceAreaBombCount();
                    }
                    break;
                case "Walking Bomb":
                    if (m_inventory.GetWalkingBombCount() > 0)
                    {
                        PlaceBomb(gameObject.transform, m_walkingBomb);
                        m_inventory.ReduceWalkingBombCount();
                    }
                    break;

                default:
                    break;
            }
        }

        if (m_inventory.GetBasicBombCount() < 0)
            m_inventory.ZeroBasicBomb();
        if (m_inventory.GetFireBombCount() < 0)
            m_inventory.ZeroFireBomb();
        if (m_inventory.GetAreaBombCount() < 0)
            m_inventory.ZeroAreaBomb();
        if (m_inventory.GetWalkingBombCount() < 0)
            m_inventory.ZeroWalkingBomb();
    }

    private void PlaceBomb(Transform m_pos, GameObject m_bomb)
    {
        Vector3 newPos = new Vector3();
        newPos.x = transform.forward.x * 0.4f + transform.position.x;
        newPos.z = transform.forward.z * 0.4f + transform.position.z;
        newPos.y = transform.position.y;
        Object.Instantiate(m_bomb, newPos, Quaternion.Euler(new Vector3(90.0f, transform.eulerAngles.y, 0.0f)));
        if (m_userManager)
        {
            m_userManager.m_User.PlayersAchievements.AddBombsDropped();
            m_userManager.Save();
        }
    }

    private void PlaceBomb(RaycastHit hit, GameObject m_bomb)
    {
        //Set tag to "placed" to stop multiple bombs being placed
        Debug.Log("Collided with wall");
        hit.collider.gameObject.tag = "Placed";

        //Place bomb
        //Calc new pos for bomb
        Vector3 m_newPos = new Vector3();
        m_newPos.y = hit.point.y + 1.0f;

        if (transform.forward.x == -1)
            m_newPos.x = hit.point.x + m_bombOffset;
        else if (transform.forward.x == 1)
            m_newPos.x = hit.point.x - m_bombOffset;
        else
            m_newPos.x = hit.point.x;

        if (transform.forward.z == -1)
            m_newPos.z = hit.point.z + m_bombOffset;
        else if (transform.forward.z == 1)
            m_newPos.z = hit.point.z - m_bombOffset;
        else
            m_newPos.z = hit.point.z;

        Object.Instantiate(m_bomb, m_newPos, transform.rotation);

        if (m_userManager)
        {
            m_userManager.m_User.PlayersAchievements.AddBombsDropped();
            m_userManager.Save();
        }
    }
}