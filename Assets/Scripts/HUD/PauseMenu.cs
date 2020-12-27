using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    //VARIABLEs
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private string MainMenu = "Menu Screen Enviro";

    //REFERENCE MENUS
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject inventoryMenuPanel;
    [SerializeField] private GameObject questMenuPanel;

    //WHERE IS (PANEL)
    enum whereIs { Quest, System, Inventory};
    whereIs actualPanel;

    private void Start()
    {
        //PANEL PRINCIPAL
        actualPanel = whereIs.System;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        //GAMEISPAUSED
        if(GameIsPaused)
        {
            ChangePanel();
        }
    }

    #region RESUME
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.volume = 1.0f;
    }
    #endregion

    #region PAUSE
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.volume = 0.2f;
    }
    #endregion

    #region LOADMENU
    private void LoadMenu()
    {
        Time.timeScale = 1f;
        //Debug.Log("Loading Menu...");
        SceneManager.LoadScene(MainMenu);
        AudioListener.volume = 1.0f;
    }
    #endregion

    #region QUITGAME
    private void QuitGame()
    {
        //Debug.Log("QUIT GAME PAUSE MENU");
        Application.Quit();
    }
    #endregion

    #region CHAMGEPANEL
    private void ChangePanel()
    {
        switch (actualPanel)
        {
            case whereIs.Quest:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log("Estoy en Quest, y debo ir a System");
                    actualPanel = whereIs.System;

                    //LEANTWEEN
                    questMenuPanel.SetActive(false);
                    pauseMenuPanel.SetActive(true);
                }
                break;
            case whereIs.System:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log("Estoy en System, y debo ir a Inventory");
                    actualPanel = whereIs.Inventory;

                    //LEANTWEEN
                    pauseMenuPanel.SetActive(false);
                    inventoryMenuPanel.SetActive(true);
                    //LeanTween.moveX(pauseMenuPanel, 0.0f, 0.3f).setIgnoreTimeScale(true);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("Estoy en System, y debo ir a Quest");
                    actualPanel = whereIs.Quest;

                    //LEANTWEEN
                    pauseMenuPanel.SetActive(false);
                    questMenuPanel.SetActive(true);
                }
                break;
            case whereIs.Inventory:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("Estoy en Inventory, y debo ir a System");
                    actualPanel = whereIs.System;

                    //LEANTWEEN
                    inventoryMenuPanel.SetActive(false);
                    pauseMenuPanel.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
    #endregion
}
