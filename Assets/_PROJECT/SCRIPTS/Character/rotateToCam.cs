using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToCam : MonoBehaviour
{
    Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(new Vector3(this.transform.position.x, camera.position.y, camera.transform.position.z));
    }
}
