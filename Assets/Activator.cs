using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Activator : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] Room1PrototipeManager manager;
    [SerializeField] ImanBehavior imanActivator;
    [SerializeField] GameObject cinematic;
    [SerializeField] GameObject switchDoor;
    [SerializeField] float cinematicTime = 3;
    [SerializeField] Transform toRotate;
    [SerializeField] float speedRotation = 1;
    bool isActivated;
    #endregion

    #region UPDATE
    void Update()
    {
        if (imanActivator.myPole == iman.POSITIVE && !isActivated)
        {            
            RotationPart1();
            isActivated = true;
            Invoke("StartAnimation", 2);
        }
    }
    #endregion

    #region ROTATION PART1
    void RotationPart1()
    {
        toRotate.DORotate(new Vector3(0, -180, 0), speedRotation);
        Invoke("RotationPart2", speedRotation);
    }
    #endregion

    #region ROTATION PART 2
    void RotationPart2()
    {
        toRotate.DORotate(new Vector3(0, -360, 0), speedRotation);
        Invoke("RotationPart1", speedRotation);
    }
    #endregion

    #region START ANIMATION
    void StartAnimation()
    {
        cinematic.SetActive(true);
        Invoke("DeactivateSwitch", cinematicTime/2);
        Invoke("DeactivateCamera", cinematicTime);
    }
    #endregion

    #region DEACTIVATE SWITCH
    void DeactivateSwitch()
    {
        manager.AddButton();
        switchDoor.SetActive(false);
    }
    #endregion

    #region DEACTIVATE CAMERA
    void DeactivateCamera()
    {
        cinematic.SetActive(false);
    }
    #endregion

}
