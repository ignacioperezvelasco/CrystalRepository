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
    bool isGrabed = false;
    [SerializeField] Collider pickedCollider;
    Rigidbody objectRb;
    #endregion    

    #region UPDATE
    void Update()
    {
        if (isObjectInside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isGrabed)
                {
                    isGrabed = false;
                    pickedCollider.enabled = false;
                    collider.enabled = true;
                    objectRb.isKinematic = false;
                    objectTransform.parent = null;
                }
                else
                {
                    isGrabed = true;
                    collider.enabled = false;
                    objectRb.isKinematic = true;
                    pickedCollider.enabled = true;
                    objectTransform.parent = this.transform;
                    objectTransform.rotation = this.transform.rotation;

                }
            }
        }
        
        if (isObjectInside)
        {
            if (isGrabed)
            {
                //objectTransform.DOMove(this.transform.position, speedAtraction);
            }
            else
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
            objectRb = other.GetComponent<Rigidbody>();
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanBeHitted") && other.gameObject.name == objectName )
        {

            collider.enabled = true;

            isObjectInside = false;

            objectName = null;
            objectTransform = null;

        }
    }
    #endregion
}
