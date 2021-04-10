using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    #region VARIABLES
    Camera viewCamera;
    public Transform groundChecker;
    bool isPaused = false;

    [Header("NEGATIVE LASER")]
    public LineRenderer negativeLaser;
    public Transform initialNegativeLaser;
    public Transform finalNegativeLaser;
    Vector3 naturalPositionNegative;

    [Header("POSITIVE LASER")]
    public LineRenderer positiveLaser;
    public Transform initialPoitiveLaser;
    public Transform finalPositiveLaser;
    Vector3 naturalPositionPositive;

    [Header("AUTOAIM")]
    [SerializeField] Autoaim autoaim;
    public bool hasTarget = false;

    Vector3 currentTarget;

    bool isStoped;

    #endregion

    #region START
    void Start()
    {
        //Asignamos nuestra camara
        viewCamera = Camera.main;

        //Guardamos la posicion inical de los laseres
        naturalPositionPositive = finalPositiveLaser.position;
        naturalPositionNegative = finalPositiveLaser.position;
    }
    #endregion


    #region UPDATE
    void Update()
    {
        if (!isPaused && !isStoped)
        {
            //Creamos un rayo de la cámara a la posición del raton en pantalla
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

            //Generamos el plano a los pies de la posicion del personaje
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundChecker.position.y, 0));
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);

                //Comprobamos el target actual
                currentTarget = autoaim.GetCurrentTarget();

                //miramos si hay target
                if (currentTarget != new Vector3(1000, 1000, 1000))
                {

                    currentTarget.y = this.transform.position.y;
                    hasTarget = true;
                }
                else
                {
                    hasTarget = false;
                }
                
                LookAtMouse(point);                        
            }
        }

        if (hasTarget)
        {
            //Seteamos el Laser Negativo
            negativeLaser.SetPosition(0, initialNegativeLaser.position);
            negativeLaser.SetPosition(1, currentTarget);

            //Seteamos el Laser Positive
            positiveLaser.SetPosition(0, initialPoitiveLaser.position);
            positiveLaser.SetPosition(1, currentTarget);
        }
        else
        {
            //Seteamos el Laser Negativo
            negativeLaser.SetPosition(0, initialNegativeLaser.position);
            negativeLaser.SetPosition(1, finalNegativeLaser.position);

            //Seteamos el Laser Positive
            positiveLaser.SetPosition(0, initialPoitiveLaser.position);
            positiveLaser.SetPosition(1, finalPositiveLaser.position);
        }


        
    }
    #endregion


    #region LOOK AT
    void LookAtMouse(Vector3 _point)
    {
        Vector3 correctedPoint = new Vector3(_point.x, this.transform.position.y, _point.z);
        //Hacemos que mire donde queremos
        this.transform.LookAt(correctedPoint);
    }
    #endregion

    #region STOP PLAYER
    public void StopPlayer()
    {
        isPaused = true;
    }
    #endregion

    #region RESUME PLAYER
    public void ResumePlayer()
    {
        isPaused = false;
    }
    #endregion

    #region GETTER HAS TARGET
    public bool HasTargert()
    {
        return hasTarget;
    }
    #endregion

    #region GETTER SHOOT DIRECTION
    public Vector3 GetShootDirection()
    {
        Vector3 targetDirection;        

        targetDirection = autoaim.GetCurrentTarget() - this.transform.position;
       // targetDirection.y = 0;        
        targetDirection = targetDirection.normalized;

        return targetDirection;
    }
    #endregion

    #region STOP
    public void Stop()
    {
        isStoped = true;
    }
    #endregion

    #region RESUME
    public void Resume()
    {
        isStoped = false;
    }
    #endregion
}
