using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{

    #region VARIABLE
    [SerializeField] float speed = 1f;
    [Header("ELEVATOR")]
    [SerializeField] Transform elevator;
    [SerializeField] Transform elevatorUp;
    [SerializeField] Transform elevatorDown;
    [Header("COUNTERWEIGHT")]
    [SerializeField] Transform counterWeight;
    [SerializeField] Transform counterWeightUp;
    [SerializeField] Transform counterWeightDown;

    GameObject weight;
    #endregion

    #region UPDATE
    void Update()
    {

    }
    #endregion


    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanBeHitted") )
        {
            weight = other.gameObject;
            weight.transform.SetParent(this.transform);

            //elevator
            elevator.DOMove(elevatorUp.position, speed);

            //Contrapeso
            counterWeight.DOMove(counterWeightDown.position, speed);
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanBeHitted"))
        {
            //weight.transform.SetParent(null);
            //weight = null;

            ////elevator
            //elevator.DOMove(elevatorDown.position, speed);

            ////Contrapeso
            //counterWeight.DOMove(counterWeightUp.position, speed);
        }
    }
    #endregion
}
