using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCombat : MonoBehaviour
{
    #region VARIABLES
    ControlWagonAnimation controlWagon;
    [SerializeField] GameObject cinematic;
    [SerializeField] string enemyName;
    [SerializeField] float timeToCallWagon = 3.5f;

    rvMovementPers playerMovement;

    [Header("MAIN CAMERA")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] float timeToDeactivateCamera = 0.25f;
    [SerializeField] float timeToReactivateCamera = 6.75f;
    #endregion

    #region START
    void Start()
    {
        controlWagon = GetComponent<ControlWagonAnimation>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
    }
    #endregion

    #region TRIGGR ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == enemyName)
        {
            playerMovement.StopMovement();

            cinematic.SetActive(true);

            Invoke("DeactivateCamera", timeToDeactivateCamera);
            Invoke("ActivateCamera", timeToReactivateCamera);

            Invoke("CallWagon", timeToCallWagon);
        }
    }
    #endregion

    #region CALL WAGON
    void CallWagon()
    {
        controlWagon.CallWagon();
    }
    #endregion

    #region DEACTIVATEC CAMERA
    void DeactivateCamera()
    {
        mainCamera.SetActive(false);
    }
    #endregion

    #region ACTIVATE CAMERA
    void ActivateCamera()
    {
        mainCamera.SetActive(true);

        playerMovement.ResumeMovement();
    }
    #endregion

}
