using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUP : MonoBehaviour
{
    #region VARIABLES
    public float speedAtraction = 0.5f;
    Transform objectTransform;
    string objectName;
    [SerializeField]
    bool isObjectInside = false;

    #endregion

    #region UPDATE
    void Update()
    {
        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && isObjectInside)
        {
            objectTransform.DOMove(this.transform.position, speedAtraction);
        }

    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanBeHitted") && !isObjectInside)
        {
            Debug.Log("ENTRA UN OBJETO");

            isObjectInside = true;

            objectName = other.gameObject.name;
            objectTransform = other.transform;

        }
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanBeHitted") && other.gameObject.name == objectName )
        {
            Debug.Log("ENTRA UN OBJETO");

            isObjectInside = false;

            objectName = null;
            objectTransform = null;

        }
    }
    #endregion
}
