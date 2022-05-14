using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : ButtonTemplate
{
    [SerializeField] string m_NextSceneName, m_CurrentSceneName;
    private const string c_PersistentSceneName = "PersistentScene";

    protected override void Awake()
    {
        base.Awake();

        if (!SceneManager.GetSceneByName(c_PersistentSceneName).isLoaded)
            SceneManager.LoadScene(c_PersistentSceneName, LoadSceneMode.Additive);
    }

    protected override void ButtonAction()
    {
        SceneManager.UnloadSceneAsync(m_CurrentSceneName);
        SceneManager.LoadSceneAsync(m_NextSceneName, LoadSceneMode.Additive);
    }
}
