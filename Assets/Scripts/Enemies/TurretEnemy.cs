using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretEnemy : MonoBehaviour
{
    #region VARIABLES
     Transform player;
    [Header("HEAD")]
    public bool headActivate = true;
    public Transform head;
    public Transform eyeTurret;

    [Header("CROSSHAIR")]
    public Transform crosshair;
    public float followSpeed;
    bool isInside = false;

    [Header("Shooting")]
    public float fireRate = 3;
    public Transform shootSpawner;
    public GameObject projectile;
    [Header("Particles Efect")]
    public GameObject chargeParticles;
    public float restartParticles = 0.3f;

    [Header("ANIMATION")]
    public Transform downHead;
    public Transform upHead;
    public float speedAnimation;

    [Header("FRONT STONE")]
    public ImanBehavior frontIman;
    public Transform frontStone;
    public Transform frontNear;
    public Transform frontFar;

    [Header("BACK STONE")]
    public ImanBehavior backIman;
    public Transform backStone;
    public Transform backNear;
    public Transform backFar;

    [Header("LEFT STONE")]
    public ImanBehavior leftIman;
    public Transform leftStone;
    public Transform leftNear;
    public Transform leftFar;

    [Header("RIGHT STONE")]
    public ImanBehavior rightIman;
    public Transform rightStone;
    public Transform rightNear;
    public Transform rightFar;


    LineRenderer line;
    float shootTimer;
    #endregion

    #region START
    void Start()
    {
        line = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion

    #region UPDATE
    void Update()
    {
        //Pintamos la linea laser
        line.SetPosition(0, eyeTurret.position);
        line.SetPosition(1, crosshair.position);
        
        if (isInside)
        {
            if (headActivate)
            {
                //Actualizamos la posicion de la cabeza y crosshair
                crosshair.DOMove(new Vector3(player.position.x, crosshair.position.y, player.position.z), followSpeed);
                head.DOLookAt(new Vector3(player.position.x, crosshair.position.y - 1.5f, player.position.z), followSpeed);

                shootTimer += Time.deltaTime;
                if (shootTimer >= fireRate)
                {
                    shootTimer = 0;
                    Shoot();
                }
            }            

            

            if (frontIman.myPole == iman.NEGATIVE)
            {
                DeactivateFrontStone();
            }
            if (backIman.myPole == iman.NEGATIVE)
            {
                DeactivateBackStone();
            }
            if (leftIman.myPole == iman.NEGATIVE)
            {
                DeactivateLeftStone();
            }
            if (rightIman.myPole == iman.NEGATIVE)
            {
                DeactivateRightStone();
            }

            if (frontIman.myPole == iman.NEGATIVE && backIman.myPole == iman.NEGATIVE &&
                leftIman.myPole == iman.NEGATIVE && rightIman.myPole == iman.NEGATIVE)
            {
                DeactivateHead();
                headActivate = false;

                //Paramos las particulas
                chargeParticles.SetActive(false);
            }

        }
    }
    #endregion

    #region SHOOT
    void Shoot()
    {
        chargeParticles.SetActive(false);
        Invoke("RestartParticles", restartParticles);

        //Instanciamos el proyectil
        GameObject go = Instantiate(projectile, shootSpawner.position, shootSpawner.rotation) as GameObject;
        //Guardamos el bullet script
        EnemyBullet bullet = go.GetComponent<EnemyBullet>();

        //Creamos la direccion del disparo
        Vector3 direction = crosshair.position - eyeTurret.position;


        //bullet.SetVelocity(direction.normalized);
    }
    #endregion

    #region RESTART PARTICLES
    void RestartParticles()
    {
        chargeParticles.SetActive(true);
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //encendemos las particulas
            chargeParticles.SetActive(true);

            isInside = true;

            //Animamos la torreta para que se active
            ActivateTurretAnimation();
        }
    }
    #endregion
    
    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateTurretAnimation();
        }
    }
    #endregion

    #region ACTIVATE TURRET ANIMATION
    void ActivateTurretAnimation()
    {
        //Elevamos la cabeza
        head.DOMove(upHead.position, speedAnimation);

        //Separamos las piedras imantables
        frontStone.DOMove(frontFar.position ,speedAnimation);
        backStone.DOMove(backFar.position, speedAnimation);
        leftStone.DOMove(leftFar.position, speedAnimation);
        rightStone.DOMove(rightFar.position, speedAnimation);

    }
    #endregion

    #region DEACTIVATE TURRET ANIMATION
    void DeactivateTurretAnimation()
    {
        //Paramos las particulas
        chargeParticles.SetActive(false);

        //reiniciamos las cosas
        shootTimer = 0;
        isInside = false;

        //Animamos la torreta para que se desactive
        if (headActivate)
        {
            //Comprobamos la frontal 
            if (frontIman.myPole == iman.POSITIVE)
            {
                DeactivateFrontStone();
            }
            //Comprobamos la trasera 
            if (backIman.myPole == iman.POSITIVE)
            {
                DeactivateBackStone();
            }
            //Comprobamos la izquierda 
            if (leftIman.myPole == iman.POSITIVE)
            {
                DeactivateLeftStone();
            }
            //Comprobamos la frontal 
            if (rightIman.myPole == iman.NEGATIVE)
            {
                DeactivateRightStone();
            }


        }

        //Reestablecemos
        frontIman.myPole = iman.POSITIVE;
        backIman.myPole = iman.POSITIVE;
        leftIman.myPole = iman.POSITIVE;
        rightIman.myPole = iman.POSITIVE;

        headActivate = true;
    }
    #endregion

    #region DEACTIVATE HEAD
    void DeactivateHead()
    {
        head.DOMove(downHead.position, speedAnimation);
    }
    #endregion

    #region DEACTIVATE FRONT STONE
    void DeactivateFrontStone()
    {
        //juntamos la piedra delantera imantable
        frontStone.DOMove(frontNear.position, speedAnimation);
    }
    #endregion

    #region DEACTIVATE BACK STONE
    void DeactivateBackStone()
    {
        //Separamos la piedra trasera imantable
        backStone.DOMove(backNear.position, speedAnimation);
    }
    #endregion

    #region DEACTIVATE LEFT STONE
    void DeactivateLeftStone()
    {
        //Separamos la piedra izquierda imantable
        leftStone.DOMove(leftNear.position, speedAnimation);
    }
    #endregion

    #region DEACTIVATE RIGHT STONE
    void DeactivateRightStone()
    {
        //Separamos la piedra derecha imantable
        rightStone.DOMove(rightNear.position, speedAnimation);
    }
    #endregion

}
