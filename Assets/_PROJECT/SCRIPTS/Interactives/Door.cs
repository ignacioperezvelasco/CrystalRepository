using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region VARIABLES
    public enum DoorType
    {
        NONE,
        PROXIMITY,
        BUTTON,
        KEY
    };

    public DoorType doorType = DoorType.NONE;
    public Animator doorAnimator;

    bool isPlayerInside = false;
    bool isDoorOpened = false;

    public bool haveTheKey = false;
    #endregion


    #region UPDATE
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside)
        {
            switch (doorType)
            {
                case DoorType.BUTTON:
                    if (!isDoorOpened)
                    {
                        OpenDoor();
                    }
                    else
                    {
                        CloseDoor();
                    }

                    break;
                case DoorType.KEY:
                    {
                        if (haveTheKey)
                        {
                            OpenDoor();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            switch (doorType)
            {
                case DoorType.NONE:
                    {
                        break;
                    } 
                case DoorType.PROXIMITY:
                    {
                        OpenDoor();
                        break;
                    }
                case DoorType.BUTTON:
                    {
                        break;
                    }
                case DoorType.KEY:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            switch (doorType)
            {
                case DoorType.NONE:
                    {
                        break;
                    }
                case DoorType.PROXIMITY:
                    {
                        CloseDoor();
                        break;
                    }
                case DoorType.BUTTON:
                    {
                        break;
                    }
                case DoorType.KEY:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
    #endregion

    #region OPEN DOOR
    void OpenDoor()
    {
        doorAnimator.SetBool("Open", true);
        doorAnimator.SetBool("Close", false);

        isDoorOpened = true;
    }
    #endregion

    #region CLOSE DOOR
    void CloseDoor()
    {
        doorAnimator.SetBool("Open", false);
        doorAnimator.SetBool("Close", true);

        isDoorOpened = false;
    }
    #endregion
}
