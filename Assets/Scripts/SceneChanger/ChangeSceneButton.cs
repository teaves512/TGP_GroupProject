using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : ButtonTemplate
{
    private const string c_PersistentSceneName = "PersistentScene";

    [SerializeField] Text text;
    [SerializeField] Image image;
    [SerializeField] Level level;

    void Start()
    {
        if (!SceneLoaded(c_PersistentSceneName))
            SceneManager.LoadScene(c_PersistentSceneName, LoadSceneMode.Additive);

        if (text != null && image != null)
        {
            text.text = level.levelName;
            image.sprite = level.previewImage;
        }
    }

    protected override void ButtonAction()
    {
        LoadScene(level.scene);
    }

    protected void LoadScene(string newSceneName)
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
    }

    bool SceneLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(c_PersistentSceneName);

        int count = SceneManager.sceneCount;
        for(int i = 0; i < count; i++)
        {
            if (SceneManager.GetSceneAt(i) == scene)
                return true;
        }

        return false;
    }
}
