using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor : MonoBehaviour
{
    //VARIABLES
    public GameObject pivotDoor1;
    public GameObject pivotDoor2;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //SI ES IMANTADO
        if(Input.GetKeyDown(KeyCode.R))
        {
            //pivotDoor1.LeanRotate(new Vector3(0, -90, 0), time);
            //pivotDoor2.LeanRotate(new Vector3(0, 90, 0), time);
        }
    }
}
