using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    [SerializeField] int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("POS_X", this.transform.position.x);
            PlayerPrefs.SetFloat("POS_Y", this.transform.position.y);
            PlayerPrefs.SetFloat("POS_Z", this.transform.position.z);

            PlayerPrefs.SetInt("CHECKPOINT", index);
        }
    }
}
