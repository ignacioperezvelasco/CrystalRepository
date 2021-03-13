using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImantableDoor : MonoBehaviour
{
    #region VARIABLES
    public enum StateDoor
    {
        OPENED,
        CLOSED
    };

    [SerializeField] float speed;

    [Header("RIGHT DOOR")]
    [SerializeField] ImanBehavior rightIman;
    [SerializeField] Transform rightDoor;
    [SerializeField] Transform rightClose;
    [SerializeField] Transform rightOpen;
    StateDoor stateRight = StateDoor.CLOSED;

    [Header("LEFT DOOR")]
    [SerializeField] ImanBehavior leftIman;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform leftClose;
    [SerializeField] Transform leftOpen;
    StateDoor stateLeft = StateDoor.CLOSED;
    #endregion

    #region UPDATE
    void Update()
    {
        //comprobamos si están cerradas las puertas
        if (stateLeft == StateDoor.CLOSED && stateRight == StateDoor.CLOSED)
        {
            //Si tienen el mismo polo pero este polo es diferente a NONE entonces abrimos las puertas
            if (leftIman.myPole == rightIman.myPole && leftIman.myPole != iman.NONE && rightIman.myPole != iman.NONE)
            {
                //Abrimos las puertas
                OpenDoor();
            }
        }
        else
        {
            //Si tienen diferente polo pero este polo es diferente a NONE entonces cerramos las puertas
            if (leftIman.myPole != rightIman.myPole && leftIman.myPole != iman.NONE && rightIman.myPole != iman.NONE)
            {
                //Cerramos las puertas
                CloseDoor();
            }
        }
    }
    #endregion

    #region OPEN DOOR
    void OpenDoor()
    {
        //Cambiamos el estado a abiertas
        stateRight = StateDoor.OPENED;
        stateLeft = StateDoor.OPENED;

        //Ponemos la polaridad a NONE para que no haya problemas
        rightIman.myPole = iman.NONE;
        leftIman.myPole = iman.NONE;

        //Animamos las puertas para que se abran
        rightDoor.DOMove(rightOpen.position, speed);
        leftDoor.DOMove(leftOpen.position, speed);

        //Desactivamos el outline en un tiempo determinado
        Invoke("DeactivateOutline", speed * 2);
    }
    #endregion

    #region CLOSE DOOR
    void CloseDoor()
    {
        //Cambiamos el estado a abiertas
        stateRight = StateDoor.CLOSED;
        stateLeft = StateDoor.CLOSED;

        //Ponemos la polaridad a NONE para que no haya problemas
        rightIman.myPole = iman.NONE;
        leftIman.myPole = iman.NONE;

        //Animamos las puertas para que se abran
        rightDoor.DOMove(rightClose.position, speed);
        leftDoor.DOMove(leftClose.position, speed);

        //Desactivamos el outline en un tiempo determinado
        Invoke("DeactivateOutline", speed*2);
    }
    #endregion

    #region DEACTIVATE OUTLINE
    void DeactivateOutline()
    {
        leftIman.outline.enabled = false;
        rightIman.outline.enabled = false;
    }
    #endregion
}
