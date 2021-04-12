using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CinematicCamera : MonoBehaviour
{
    #region VARIABLES
    [Header("TIMES")]
    [SerializeField] float pausedTime;
    [SerializeField] float cameraSpeed;

    [Header("POSITION CAMERA")]
    [SerializeField] Transform positionCinematicCamera;
    [SerializeField] int FOV;


    //Transform de la camara
    Transform mainCamera;
    //Posiciones iniciales de la camara
    Vector3 initialPosition;
    Vector3 initialRotation;
    float initialFOV;
    rvMovementPers player;
    bool isActivated = false;

    #endregion

    #region START
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
        mainCamera = Camera.main.transform;
    }
    #endregion


    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {

            //Paramos el movimiento del player
            player.StopMovement();

            Invoke("StartCinematic", 1.5f);          

        }
    }
    #endregion

    public void StartCinematic()
    {
        isActivated = true;

        //Guardamos la posicion
        initialPosition = mainCamera.position;
        initialRotation = mainCamera.eulerAngles;
        initialFOV = Camera.main.fieldOfView;

        

        //Movemos la camara
        mainCamera.transform.DOMove(positionCinematicCamera.position, cameraSpeed);
        mainCamera.transform.DORotate(positionCinematicCamera.eulerAngles, cameraSpeed);
        Camera.main.DOFieldOfView(FOV, cameraSpeed);

        //LLamamos a la funcion de acabar la cinematica
        Invoke("StartFinishCinematic", cameraSpeed);
    }


    #region FINISH CINEMATIC
    void StartFinishCinematic()
    {
        Invoke("ReturnCamera", pausedTime);
    }
    #endregion

    #region RETURN CAMERA
    void ReturnCamera()
    {
        //Movemos la camara
        mainCamera.transform.DOMove(initialPosition, cameraSpeed);
        mainCamera.transform.DORotate(initialRotation, cameraSpeed);
        Camera.main.DOFieldOfView(initialFOV, cameraSpeed);

        Invoke("FinishCamera", cameraSpeed);
    }
    #endregion

    #region FINISH CAMERA
    void FinishCamera()
    {
        player.ResumeMovement();
    }
    #endregion
}
