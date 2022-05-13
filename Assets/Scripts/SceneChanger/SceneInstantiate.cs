using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{

    [SerializeField]
    private string persitentScene;
    
    void Start()
    {
        SceneManager.LoadScene(persitentScene, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
