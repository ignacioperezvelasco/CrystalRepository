using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoaim : MonoBehaviour
{
    #region VARIABLES
    struct Target
    {
        public GameObject go;
        public int index;
    };

    List<Target> posiblesTargets = new List<Target>();
    Target currentTarget;
    int targetCounter = 0;

    [Header("PLAYER")]
    public Transform player;
    public LayerMask layerMask;
    #endregion    

    #region UPDATE
    void Update()
    {

        CheckNewTarget();        
    }
    #endregion

    #region CHECK NEW TARGET
    void CheckNewTarget()
    {        
        if (posiblesTargets.Count > 0)
        {
            for (int i = 0; i < posiblesTargets.Count; i++)
            {
                float ditanceToNewTarget = Vector3.Distance(player.position, posiblesTargets[i].go.transform.position);

                //Comprobamos que no haya nada de por medio
                RaycastHit hit;
                Debug.DrawRay(player.position, (posiblesTargets[i].go.transform.position - player.position)*1000, Color.white);
                if (Physics.Raycast(player.position, (posiblesTargets[i].go.transform.position - player.position),  out hit , Mathf.Infinity,layerMask))
                {

                    if (hit.collider.CompareTag("Finish"))
                    {
                        Debug.Log("ALGO LO BLOQUEA");
                        break;
                    }
                }

                if (currentTarget.go == null)
                {
                    currentTarget.go = posiblesTargets[i].go;
                    currentTarget.index = posiblesTargets[i].index;

                    currentTarget.go.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                //Miramos si el siguiente target de la lista está más cerca que el actual target
                else if (ditanceToNewTarget < Vector3.Distance(player.position, currentTarget.go.transform.position))
                {
                    

                    UpdateNewTarget(posiblesTargets[i]);
                }
            }
        }
        
    }
    #endregion

    #region UPDATE NEW TARGET
    void UpdateNewTarget(Target _newTarget)
    {
        //el que era target y ya no será le cambiamos el color
        currentTarget.go.GetComponent<MeshRenderer>().material.color = Color.yellow;

        //Actualizamos
        currentTarget.go = _newTarget.go;
        currentTarget.index = _newTarget.index;

        //el que es target le cambiamos el color
        currentTarget.go.GetComponent<MeshRenderer>().material.color = Color.green;
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            AddNewPosibleTarget(other.gameObject);
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            ErasePosibleTarget(other.gameObject);
        }
    }
    #endregion

    #region ADD NEW POSIBLE TARGET
    void AddNewPosibleTarget(GameObject _go)
    {
        _go.GetComponent<MeshRenderer>().material.color = Color.yellow;

        //Creamos el nuevo Target
        Target aux;
        aux.go = _go;
        aux.index = posiblesTargets.Count;
        
        //Añadimos el posible target
        posiblesTargets.Add(aux);

        //buscamos un nuevo Target        
        CheckNewTarget();
        
    }
    #endregion

    #region ERASE POSIBLE TARGET
    void ErasePosibleTarget(GameObject _go)
    {
        _go.GetComponent<MeshRenderer>().material.color = Color.red;

        for (int i = 0; i < posiblesTargets.Count; i++)
        {
            if (posiblesTargets[i].go == _go)
            {
                //Actualizamos el currentTarget
                if (posiblesTargets[i].go == currentTarget.go)
                {
                    currentTarget.go = null;
                }

                //Eliminamos el target que sale
                posiblesTargets.Remove(posiblesTargets[i]);

                //Buscamos un nuevos target
                CheckNewTarget();                
                return;
            }
            
        }        
    }
    #endregion

    #region GET TARGET
    public Vector3 GetCurrentTarget()
    {
        return currentTarget.go.transform.position;
    }
    #endregion
}
