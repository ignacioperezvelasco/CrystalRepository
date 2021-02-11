using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //VARIABLES
    [Header("Scene to Open")]
    public string sceneToOpen;

    [Header("Red points")]
    public List<Image> RedList;

    [Header("Buttons list")]
    public List<Button> ButtonList;

    private void Start()
    {
        //DESACTIVAR TODOS LOS REDPOINTS DE INICIO
        for (int i = 0; i < RedList.Count; i++)
        {
            RedList[i].enabled = false;
        }
    }


    #region PlayGame
    public void PlayGame()
    {
        //HAY QUE AÑADIR LA ESCENA AL BUILDEAR
        Debug.Log("Empecemos");
        //SceneManager.LoadScene(sceneToOpen);

        Invoke("LoadNextScene", 0.5f);
    }
    #endregion

    #region ExitGame
    public void ExitGame()
    {
        Debug.Log("Debo cerrarme");
        Application.Quit();
    }
    #endregion

    #region LoadNextScene
    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToOpen);
    }
    #endregion

    #region Hovers
    public void Hover(int numRedPoint)
    {
        //HABILITAMOS REDPOINT
        RedList[numRedPoint].enabled = true;
    }
    #endregion

    #region UnHovers
    public void UnHover(int numRedPoint)
    {
        //DESHABILITAMOS REDPOINT
        RedList[numRedPoint].enabled = false;
    }
    #endregion

    #region ClickOnButton
    public void ClickOnButton(int numButton)
    {
        /*float widthButton = ButtonList[numButton].gameObject.GetComponent<RectTransform>().rect.width;
        float widthRedPoint = RedList[numButton].gameObject.GetComponent<RectTransform>().rect.width;

        //DONDE EMPIEZA Y DONDE ACABA
        float moveTo = (RedList[numButton].transform.position.x - (widthRedPoint / 2));
        float startIn = (ButtonList[numButton].transform.position.x + (widthButton / 2));
        float distanceToMove = moveTo - startIn;*/

        float startIn = ButtonList[numButton].gameObject.GetComponent<RectTransform>().offsetMax.x;
        float moveTo = RedList[numButton].gameObject.GetComponent<RectTransform>().offsetMin.x;

        Debug.Log(moveTo);

        //MOVE TO X
        LeanTween.moveX(ButtonList[numButton].gameObject, moveTo, 1f);
    }
    #endregion
}
