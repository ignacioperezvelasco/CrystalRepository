using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform character;
    public Vector3 myMouse;
    
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    
    public float smoothing = 5f;
    Vector3 offset;

    Camera mainCamera;

    [SerializeField] Transform targetCamera;

    private void Start()
    {
        offset = transform.position - character.position;
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.LookAt(targetCamera.position);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.LookAt(character);
        
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000f))
        {
            myMouse = hit.point;
        }

        Vector3 targetCamPos = character.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
                
    }
}
