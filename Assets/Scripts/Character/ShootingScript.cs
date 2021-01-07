using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //Gun
    [Header("GUN")]
    public float damage = 10f;
    public float bulletSpeed = 100;
    public Rigidbody bullet;
    public Rigidbody crystal;
    public Transform rightPistol;
    public Transform leftPistol;
    bool canShootPositive = true;
    bool canShootNegative = true;
    float positiveCharge = 0;
    float negativeCharge = 0;
    bool isChargingNegative, isChargingPositive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShootNegative)
        {
            isChargingNegative = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (negativeCharge >= 1)
                ShootNegative();
            negativeCharge = 0;
            isChargingNegative = false;
        }
        if (Input.GetButtonDown("Fire2") && canShootPositive)
        {
            isChargingPositive = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if(positiveCharge>=1)
                ShootPositive();
            positiveCharge = 0;
            isChargingPositive = false;
        }
    }

    private void FixedUpdate()
    {
        if (isChargingNegative)
        {
            if (negativeCharge <= 3)
                negativeCharge += Time.fixedDeltaTime;
           // Debug.Log(negativeCharge);
        }
        if (isChargingPositive)
        {
            if (positiveCharge <= 3)
                positiveCharge += Time.fixedDeltaTime;
            //Debug.Log(positiveCharge);
        }
    }

    void ShootNegative()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, rightPistol.transform.position, rightPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.NEGATIVE);
        Debug.Log("Negative");
        bulletClone.gameObject.GetComponent<BulletScript>().SetCharge((int)negativeCharge);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }

    void ShootPositive()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(crystal, leftPistol.transform.position, leftPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.POSITIVE);
        Debug.Log("Positive");
        bulletClone.gameObject.GetComponent<BulletScript>().SetCharge((int)positiveCharge);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }

    public float GetShootPositive()
    {
        return positiveCharge;
    }

    public float GetShootNegative()
    {
        return negativeCharge;
    }

    public bool GetIsChargingPositive()
    {
        return isChargingPositive;
    }

    public bool GetIsChargingNegative()
    {
        return isChargingNegative;
    }
}
