using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string sceneToOpen;
    
    public void PlayGame()
    {
        PlayerPrefs.SetInt("CHECKPOINT", 0);
        //HAY QUE AÑADIR LA ESCENA AL BUILDEAR
        Debug.Log("Empecemos");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Debo cerrarme");
        Application.Quit();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToOpen);
    }
}
