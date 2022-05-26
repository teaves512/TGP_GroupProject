using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] string menuScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }
}
