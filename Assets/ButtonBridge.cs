using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBridge : MonoBehaviour
{
    [SerializeField] BridgeManager bridgeManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanBeHitted"))
        {
            bridgeManager.PressButton();
        }
    }
}
