using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCombat : MonoBehaviour
{

    //rvMovementPers playerMovement;
    [SerializeField] string enemyName;
    [SerializeField] ImantablePlatform imanPlatform;

    [SerializeField] CinematicCamera cinematic;

    // Start is called before the first frame update
    void Start()
    {
        //playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
    }

    #region TRIGGR ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == enemyName)
        {
            cinematic.StartCinematic();

            Invoke("ActivateWagon", 1.5f);              

        }
    }
    #endregion

    void ActivateWagon()
    {
        imanPlatform.platformIman.myPole = iman.POSITIVE;
    }
}
