using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] Animator bridgeAnimator;
    CinematicCamera cinematic;

    int numButtonsPressed = 0;



    private void Start()
    {
        cinematic = GetComponent<CinematicCamera>();
    }
    public void PressButton()
    {

        if (numButtonsPressed < 2)
        {
            numButtonsPressed++;
            if (numButtonsPressed == 2)
            {
                cinematic.StartCinematic();
                bridgeAnimator.SetBool("Active", true);
            }
        }

    }
}
