using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeSceneButton : ButtonTemplate
{
    private const string c_PersistentSceneName = "PersistentScene";

    [SerializeField] TMP_Text text;
    [SerializeField] Image image;
    [SerializeField] Level level;

    protected override void Awake()
    {
        CheckPersistentScene();
        base.Awake();
        
    }

    protected void CheckPersistentScene()
    {
        if (!SceneLoaded(c_PersistentSceneName))
            SceneManager.LoadScene(c_PersistentSceneName, LoadSceneMode.Additive);
    }

    void Start()
    {
        if (text != null && image != null && level != null)
        {
            text.text    = level.levelName; 
            image.sprite = level.previewImage;
        }
    }

    protected override void ButtonAction()
    {
        LoadScene(level.scene);
    }

    public void LoadScene(string newSceneName)
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
