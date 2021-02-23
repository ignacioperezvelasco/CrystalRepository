using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    #region VARIABLES
    [Header("CAMERA")]
    [SerializeField] Camera mainCamera;
    [SerializeField] int newFOV;

    [Header("TARGET CAMERA")]
    [SerializeField] Transform targetCamera;
    [SerializeField] Vector3 newRotationTarget;

    [SerializeField] float speedTransition;

    FollowCharacter myTargetCamera;

    #endregion

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetCamera.DORotate(newRotationTarget, speedTransition);
            mainCamera.DOFieldOfView(newFOV, speedTransition);
            
        }
    }
}
