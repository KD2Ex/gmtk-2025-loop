using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneNameToLoad;

    public void OnLoad()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
