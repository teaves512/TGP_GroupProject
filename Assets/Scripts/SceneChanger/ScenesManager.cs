using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : SingletonPersistence<ScenesManager>
{
    private ScenesManager scenesManager;

    public void ChangeScene(string _nextSceneName, string _sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(_sceneToUnload);
        SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
    }
    
}
