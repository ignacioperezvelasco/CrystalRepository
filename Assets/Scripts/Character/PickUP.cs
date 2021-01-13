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
    BoxCollider collider;

    #endregion

    #region UPDATE
    void Update()
    {
        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && isObjectInside)
        {
            objectTransform.DOMove(this.transform.position, speedAtraction);
            collider.enabled = false;
        }
        else
        {
            if (isObjectInside)
            {
                collider.enabled = true;
            }
        }

    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanBeHitted") && !isObjectInside)
        {
            isObjectInside = true;

            objectName = other.gameObject.name;
            objectTransform = other.transform;

            collider = other.GetComponent<BoxCollider>();
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanBeHitted") && other.gameObject.name == objectName )
        {
            Debug.Log("ENTRA UN OBJETO");

            collider.enabled = true;

            isObjectInside = false;

            objectName = null;
            objectTransform = null;

        }
    }
    #endregion
}
