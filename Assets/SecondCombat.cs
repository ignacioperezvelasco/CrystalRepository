using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCombat : MonoBehaviour
{

    //rvMovementPers playerMovement;
    [SerializeField] string enemyName;
    [SerializeField] ImantablePlatform imanPlatform;

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
            imanPlatform.platformIman.myPole = iman.POSITIVE;   
        }
    }
    #endregion
}
