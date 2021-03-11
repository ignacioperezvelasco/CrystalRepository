using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerChild : MonoBehaviour
{
    bool isPlayerInside = false;
    Transform player;

    #region UPDATE
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.SetParent(null);
            player = null;
        }
    }    
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            other.transform.SetParent(this.transform);
            player = other.transform;
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            other.transform.SetParent(null);

            player = null;
        }
    }
    #endregion
}
