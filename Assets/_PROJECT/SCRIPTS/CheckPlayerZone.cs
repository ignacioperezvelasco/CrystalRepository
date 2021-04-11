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
            if (!boss.isActive)
            {
                boss.isActive = true;
            }

            boss.SetCurrentArea(myAreaType);            
        }
    }
}
