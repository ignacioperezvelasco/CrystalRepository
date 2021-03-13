using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RailWagonBehaviour : MonoBehaviour
{
    #region VARIABLES
    [Header("WAGON")]
    [SerializeField] GameObject wagon;
    [SerializeField] float wagonSpeed = 10;
    List<GameObject> wagons;

    [Header("SPAWNER")]
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float timeBetweenWagons = 4;
    [SerializeField] float distanceToTargetThreshold = 0.5f;
    float startingDelay;
    float timer = 0;

    bool started = false;
    float timeToStart = 0;
    #endregion


    #region START
    private void Start()
    {
        wagons = new List<GameObject>();

        startingDelay = Random.Range(0, 2);
    }
    #endregion

    #region UPDATE 
    void Update()
    {
        if (started)
        {
            //aumentamos el timer
            timer += Time.deltaTime;

            if (timer >= timeBetweenWagons)
            {
                timer = 0;
                //Instanciamos el nuevo vagón
                SpawnNewWagon();
            }

            //Si hay más de  vagonetas
            if (wagons.Capacity > 0)
            {

                foreach (GameObject item in wagons)
                {
                    float distance = Vector3.Distance(item.transform.position, end.position);

                    if (distance == 0)
                    {
                        //Eliminamos la vagoneta
                        DeleteWagon(item);
                        return;
                    }
                }
            }
        }
        else
        {
            timeToStart += Time.deltaTime;

            if (timeToStart >= startingDelay)
            {
                started = true;
            }
        }
      
        
    }
    #endregion

    #region SPAWN NEW WAGON
    void SpawnNewWagon()
    {
        //Instanciamos el vagón nuevo
        GameObject go = Instantiate(wagon, start.position, start.rotation) as GameObject;

        //Lo añadimos a la lista de vagonetas
        wagons.Add(go);

        //Le damos movimiento a la vagoneta
        go.transform.DOMove(end.position, wagonSpeed);

        //liberamos go
        go = null;
        
    }
    #endregion


    #region DELETE WAGON
    void DeleteWagon(GameObject _wagon)
    {
        //Nos guardamos el GameObject
        GameObject go = _wagon;

        //Eliminamos la vagoneta de la lista
        wagons.Remove(_wagon);

        //Destruimos la vagoneta
        Destroy(go);

    }    
    #endregion
}
