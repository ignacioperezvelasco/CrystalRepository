using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerZone : MonoBehaviour
{
    [SerializeField] BossLogic boss;
    [SerializeField] AreaType myAreaType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetCurrentArea(myAreaType);            
        }
    }
}
