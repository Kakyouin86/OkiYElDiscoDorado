using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangebtwScenes : MonoBehaviour
{

    public string sceneName;




    void ChangeScene()

    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}

