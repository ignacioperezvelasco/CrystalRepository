using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    [SerializeField] BossLogic boss;

    [SerializeField] GameObject HUD;
    [SerializeField] GameObject lastMessage;

    [Header("TIMES")]
    [SerializeField] float timeToShowMessage = 5;
    [SerializeField] float timeDisplayingMessage = 15;

    
    void Update()
    {
        if (boss.isKilled)
        {
            Invoke("DisplayMessage", timeToShowMessage);
        }    
    }

    void DisplayMessage()
    {
        HUD.SetActive(false);
        lastMessage.SetActive(true);

        Invoke("GoToMainMenu", timeDisplayingMessage);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
