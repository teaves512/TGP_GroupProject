using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] Level m_MainMenu;
    float m_OldTimeScale = 1;

   public void Retry()
    {
        Time.timeScale = m_OldTimeScale;
        SceneManager.LoadScene(GetCurrentSceneName());
    }

    public void Exit()
    {
        Time.timeScale = m_OldTimeScale;
        SceneManager.LoadScene(m_MainMenu.scene);
    }

    private void Awake()
    {
        //This gameobject is disabled by default, so awake only gets called on game over
        m_OldTimeScale = Time.timeScale;
        Time.timeScale = 0.1f;
    }

    private string GetCurrentSceneName()
    {
        int sceneCount = SceneManager.sceneCount;
        string name = "";

        for(int i = 0; i < sceneCount; i++)
        {
            string currentSceneName = SceneManager.GetSceneAt(i).name;
            if (currentSceneName != "PersistentScene")
                return currentSceneName;
        }

        return name;
    }
}
