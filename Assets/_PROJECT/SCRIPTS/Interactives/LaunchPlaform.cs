using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlaform : MonoBehaviour
{
    //VARIABLES
    [Header("Platform path")]
    public GameObject startPoint;
    public GameObject finalPoint;

    //TIMERS
    private float timerMovePlatform;
    private float timerCountMovePlatform;
    private bool haveToLaunch;

    //PARABOLA
    protected float animation;

    //WHAT I HAVE TO LAUNCH
    [Header("Script Parabola")]
    public GameObject parabola;
    private ParabolaController _pb;


    // Start is called before the first frame update
    void Start()
    {
        timerMovePlatform = 1.0f;
        timerCountMovePlatform = 0.0f;
        haveToLaunch = false;
        _pb = parabola.GetComponent<ParabolaController>();
    }

    // Update is called once per frame
    void Update()
    {
        timerCountMovePlatform += Time.deltaTime;
        LeanTween.move(startPoint, finalPoint.transform, timerMovePlatform);

        if(timerCountMovePlatform >= timerMovePlatform)
        {
            haveToLaunch = true;
            timerCountMovePlatform = 0;
        }

        //SHOOT THE OBJECT
        LaunchObject(haveToLaunch);

    }


    #region LAUNCHOBJECT
    private void LaunchObject(bool haveToLaunch)
    {
       if(haveToLaunch)
       {
            /*animation += Time.deltaTime;
            animation = animation % 5f;
            launchObject.transform.position = MathParabola.Parabola(Vector3.zero, Vector3.forward * 10f, 5f, animation / 5f);*/

            _pb.enabled = true;
       }
    }
    #endregion
}
