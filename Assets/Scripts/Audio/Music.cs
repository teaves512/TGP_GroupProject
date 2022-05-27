using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene[] openScenes = SceneManager.GetAllScenes();

        foreach (Scene scene in openScenes)
        {
            switch (scene.name)
            {
                case "Tutorial2":
                    AudioManager.SetMusic("Tutorial");
                    break;
                case "Level Mockup":
                    AudioManager.SetMusic("Level 1");
                    break;
                case "Level 2 Mock":
                    AudioManager.SetMusic("Level 2");
                    break;
                case "Level 3 Mock":
                    AudioManager.SetMusic("Level 3");
                    break;
                case "Level 5":
                    AudioManager.SetMusic("Level 4");
                    break;
                case "TomL1":
                    AudioManager.SetMusic("Level 5");
                    break;
                case "Boss Fight":
                    AudioManager.SetMusic("Boss");
                    break;
                default:
                    continue;
            }

            AudioManager.PlayMusic();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        AudioManager.PlayMusic(false);
    }
}
