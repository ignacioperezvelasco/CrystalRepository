using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1PrototipeManager : MonoBehaviour
{
    #region VARIABLE
    public Animator animator;
    public int buttons;
    int buttonsCounter = 0;
    bool isOpened;
    #endregion

    #region ADD BUTTON
    public void AddButton()
    {
        if (!isOpened)
        {
            buttonsCounter++;
            if (buttonsCounter == buttons)
            {
                isOpened = true;
                animator.SetBool("Active", true);
            }
        }
    }
    #endregion
}
