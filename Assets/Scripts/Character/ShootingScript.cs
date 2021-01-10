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
    float timerNegative, timerpositive = 0;
    float positiveCharge = 0;
    float negativeCharge = 0;
    bool isChargingNegative, isChargingPositive = false;

    [SerializeField] float cooldown1, cooldown2, cooldown3, cooldown0 = 0;

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
        if (Input.GetButtonUp("Fire1") && isChargingNegative)
        {
            //Set cooldown
            switch ((int)negativeCharge)
            {
                case 0:
                    timerNegative = cooldown0;
                    break;
                case 1:
                    timerNegative = cooldown1;
                    break;
                case 2:
                    timerNegative = cooldown2;
                    break;
                case 3:
                    timerNegative = cooldown3;
                    break;
                default:
                    break;
            }
            
            //Shoot if have to
            ShootNegative();
            
            //reset
            negativeCharge = 0;
            isChargingNegative = false;
            //Start CD
            canShootNegative = false;
        }
        if (Input.GetButtonDown("Fire2") && canShootPositive)
        {
            isChargingPositive = true;
        }
        if (Input.GetButtonUp("Fire2") && isChargingPositive)
        {
            //Set cooldown
            switch ((int)positiveCharge)
            {
                case 0:
                    timerpositive = cooldown0;
                    break;
                case 1:
                    timerpositive = cooldown1;
                    break;
                case 2:
                    timerpositive = cooldown2;
                    break;
                case 3:
                    timerpositive = cooldown3;
                    break;
                default:
                    break;
            }
            //shoot if u have
            ShootPositive();
            //reset
            positiveCharge = 0;
            isChargingPositive = false;
            //Start CD
            canShootPositive = false;
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
        else if (!canShootNegative)
        {
            timerNegative -= Time.fixedDeltaTime;
            if (timerNegative <= 0)
            {
                timerNegative = 0;
                canShootNegative = true;
            }
        }
        if (isChargingPositive)
        {
            if (positiveCharge <= 3)
                positiveCharge += Time.fixedDeltaTime;
            //Debug.Log(positiveCharge);
        }
        else if (!canShootPositive)
        {
            timerpositive -= Time.fixedDeltaTime;
            if (timerpositive <= 0)
            {
                timerpositive = 0;
                canShootPositive = true;
            }
        }
    }

    void ShootNegative()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, rightPistol.transform.position, rightPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.NEGATIVE);
        //Debug.Log("Negative");
        bulletClone.gameObject.GetComponent<BulletScript>().SetCharge((int)negativeCharge);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }

    void ShootPositive()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(crystal, leftPistol.transform.position, leftPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.POSITIVE);
        //Debug.Log("Positive");
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
