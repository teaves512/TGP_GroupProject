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
        if (!SceneManager.GetSceneByName(c_PersistentSceneName).isLoaded)
            SceneManager.LoadScene(c_PersistentSceneName, LoadSceneMode.Additive);
    }

    protected override void ButtonAction()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadSceneAsync(level.scene, LoadSceneMode.Additive);
    }

    public void Initialise(Level level)
    {
        this.level = level;

        text.text = level.levelName;
        image.sprite = level.previewImage;
    }
}
