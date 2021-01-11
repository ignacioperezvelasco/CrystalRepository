using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    //VARIABLEs
    [Header("Player")]
    public GameObject player;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private string MainMenu = "Menu Screen TITLE";

    //REFERENCED SCRIPTS
    private LookAt _lookat;

    void Start()
    {
        _lookat = player.GetComponent<LookAt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                _lookat.ResumePlayer();
            }
            else
            {
                Pause();
                _lookat.StopPlayer();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //AudioListener.volume = 1.0f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //AudioListener.volume = 0.2f;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        //Debug.Log("Loading Menu...");
        SceneManager.LoadScene(MainMenu);
        //AudioListener.volume = 1.0f;
    }

    public void QuitGame()
    {
        //Debug.Log("QUIT GAME PAUSE MENU");
        Application.Quit();
    }
}
