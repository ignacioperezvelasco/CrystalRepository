using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    Vector3 initialPosition;
    public float distanceCanReach = 40;
    int numCharge = 1;
    private void Start()
    {
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        Vector3 distanceV = (this.transform.position - initialPosition);
        if (distanceV.magnitude >= distanceCanReach)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanBeHitted")
        {
            //other.GetComponent<ImanBehavior>().AddCharge(iman.POSITIVE, numCharge);
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
