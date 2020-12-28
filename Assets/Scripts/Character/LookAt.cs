using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    #region VARIABLES
    Camera viewCamera;
    public Transform groundChecker;
    #endregion

    #region START
    void Start()
    {
        //Asignamos nuestra camara
        viewCamera = Camera.main;
    }
    #endregion


    #region UPDATE
    void Update()
    {
        //Creamos un rayo de la cámara a la posición del raton en pantalla
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        //Generamos el plano a los pies de la posicion del personaje
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundChecker.position.y,0));
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);

            LookAtMouse(point);
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

}
