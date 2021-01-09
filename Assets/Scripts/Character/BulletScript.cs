using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    int numCharge = 1;
    public float distanceCanReach = 40;
    Vector3 initialPosition;
    iman myPole;
    Rigidbody myRB;

    private void Start()
    {
        initialPosition = this.transform.position;
        myRB = this.GetComponent<Rigidbody>();       
    }

    private void Update()
    {
        Vector3 distanceV = (this.transform.position - initialPosition);
        if (distanceV.magnitude >= distanceCanReach)
        {
            Die();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detecta ostia");
        if (other.tag == "CanBeHitted")
        {
            other.GetComponent<ImanBehavior>().AddCharge(myPole, numCharge, myRB);
            Die();
        }
    }

    public void SetPole(iman pole)
    {
        myPole = pole;
    }

    public void SetCharge(int charge)
    {
        numCharge = charge;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
