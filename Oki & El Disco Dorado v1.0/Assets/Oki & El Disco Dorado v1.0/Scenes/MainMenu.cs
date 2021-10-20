using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene;
    public string loadScene;
    public string creditsScene;

    void Update()
    {
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
        PlayerPrefs.DeleteAll();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(loadScene);
        PlayerPrefs.DeleteAll();
    }

    public void Credits()
    {
        SceneManager.LoadScene(creditsScene);
        PlayerPrefs.DeleteAll();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
