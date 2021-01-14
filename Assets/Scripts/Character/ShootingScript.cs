using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //Gun
    [Header("GUN")]
    [SerializeField] GameObject negativePS;
    [SerializeField] GameObject positivePS;
    public float damage = 10f;
    public float bulletSpeed = 100;
    public Rigidbody bullet;
    public Rigidbody crystal;
    public Transform rightPistol;
    public Transform leftPistol;
    bool canShootPositive = true;
    bool canShootNegative = true;
    bool wantToShotNegative, wantToShotPositive = false;
    float timerNegative, timerpositive = 0;
    float positiveCharge = 0;
    float negativeCharge = 0;
    bool isChargingNegative, isChargingPositive = false;
    bool tryingShootPositive = false, tryingShootNegative = false;

    [SerializeField] float cooldown1, cooldown2, cooldown3, cooldown0 = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NegativeShootHandler(Input.GetButton("Fire2"), Input.GetButtonUp("Fire2"));

        PositiveShootHandler(Input.GetButton("Fire1"), Input.GetButtonUp("Fire1"));
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

    #region SHOOTING FUNCTIONS
    //Controling -
    void NegativeShootHandler(bool shotButtonDown, bool shotButtonUp)
    {
        //Get Input
        if ((shotButtonDown && !wantToShotNegative) && !wantToShotPositive)
            wantToShotNegative = true;

        //Can shot?
        if (wantToShotNegative && canShootNegative)
        {
            if (!isChargingPositive && !isChargingNegative)
            {
                isChargingNegative = true;
                negativePS.SetActive(true);
            }
        }
        else if (wantToShotNegative && !canShootNegative)
        {
            if (!isChargingPositive)
                tryingShootNegative = true;
        }
        //Shoot
        if (shotButtonUp && isChargingNegative)
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
            wantToShotNegative = false;
            //Start CD
            canShootNegative = false;
            //PS
            negativePS.SetActive(false);

        }
        else if (shotButtonUp)
        {
            wantToShotNegative = false;
        }
    }
    //Controling +
    void PositiveShootHandler(bool shotButtonDown, bool shotButtonUp)
    {
        if ((shotButtonDown && !wantToShotPositive) && !wantToShotNegative)
            wantToShotPositive = true;

        if (wantToShotPositive && canShootPositive)
        {
            if (!isChargingPositive && !isChargingNegative)
            {
                isChargingPositive = true;
                positivePS.SetActive(true);
            }
        }
        else if (wantToShotPositive && !canShootPositive)
        {
            if (!isChargingNegative)
                tryingShootPositive = true;
        }

        if (shotButtonUp && isChargingPositive)
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
            wantToShotPositive = false;
            //Start CD
            canShootPositive = false;
            //PS
            positivePS.SetActive(false);
        }
        else if (shotButtonUp)
        {
            wantToShotPositive = false;
        }
    }
    //Shoot-
    void ShootNegative()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, rightPistol.transform.position, rightPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.NEGATIVE);
        //Debug.Log("Negative");
        bulletClone.gameObject.GetComponent<BulletScript>().SetCharge((int)negativeCharge);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }
    //Shoot+
    void ShootPositive()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(crystal, leftPistol.transform.position, leftPistol.transform.rotation);
        bulletClone.gameObject.GetComponent<BulletScript>().SetPole(iman.POSITIVE);
        //Debug.Log("Positive");
        bulletClone.gameObject.GetComponent<BulletScript>().SetCharge((int)positiveCharge);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }

    #endregion

    #region Getter/Setter
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

    public bool GetCooldownPositive()
    {
        return canShootPositive;
    }

    public bool GetCooldownNegative()
    {
        return canShootNegative;
    }

    public bool GetTryingShootPositive()
    {
        return tryingShootPositive;
    }

    public void SetTryingShootPositive(bool _tryingShootPositive)
    {
        tryingShootPositive = _tryingShootPositive;
    }

    public bool GetTryingShootNegative()
    {
        return tryingShootNegative;
    }

    public void SetTryingShootNegative(bool _tryingShootNegative)
    {
        tryingShootNegative = _tryingShootNegative;
    }
    #endregion

}
