using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] Animator bridgeAnimator;

    int numButtonsPressed = 0;

   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PressButton()
    {

        if (numButtonsPressed < 2)
        {
            numButtonsPressed++;
            if (numButtonsPressed == 2)
            {
                bridgeAnimator.SetBool("Active", true);
            }
        }

    }
}
