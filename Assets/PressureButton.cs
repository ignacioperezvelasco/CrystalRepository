using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public Room1PrototipeManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanBeHitted"))
        {
            Debug.Log("Colocamos un objeto");
            manager.AddButton();
        }
    }


}
