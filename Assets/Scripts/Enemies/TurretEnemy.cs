using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretEnemy : MonoBehaviour
{
    #region VARIABLES
    public enum TurretType
    {
        PROJECTILE,
        LASER
    };

    Transform player;
    PlayerLogic playerLogic;

    public TurretType turretType;

    [Header("HEAD")]
    public bool headActivate = true;
    public Transform head;
    public Transform eyeTurret;
    public Outline headOutline;

    [Header("CROSSHAIR")]
    public Transform crosshair;
    public float followSpeed;
    bool isInside = false;

    [Header("LASER")]
    public GameObject laserEffect;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float damage;

    [Header("SHOOTING")]
    public float fireRate = 3;
    public Transform shootSpawner;
    public GameObject projectile;

    [Header("PARTICLES EFFECT")]
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
    public bool isDead = false;
    #endregion

    #region START
    void Start()
    {
        //Buscar el player Logic
        playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();

        line = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Activamos los outlines
        frontIman.outline.enabled = true;
        backIman.outline.enabled = true;
        leftIman.outline.enabled = true;
        rightIman.outline.enabled = true;

        switch (turretType)
        {
            case TurretType.PROJECTILE:
                {
                    //Seteamos las ImanStones
                    frontIman.myPole = iman.POSITIVE;
                    backIman.myPole = iman.POSITIVE;
                    leftIman.myPole = iman.POSITIVE;
                    rightIman.myPole = iman.POSITIVE;
                    
                    //Seteamos los Outlines
                    frontIman.outline.OutlineColor = Color.blue;
                    backIman.outline.OutlineColor = Color.blue;
                    leftIman.outline.OutlineColor = Color.blue;
                    rightIman.outline.OutlineColor = Color.blue;


                    headOutline.OutlineColor = Color.red;
                    break;
                } 
            case TurretType.LASER:
                {
                    //Seteamos las ImanStones
                    frontIman.myPole = iman.NEGATIVE;
                    backIman.myPole = iman.NEGATIVE;
                    leftIman.myPole = iman.NEGATIVE;
                    rightIman.myPole = iman.NEGATIVE;

                    //Seteamos los Outlines
                    frontIman.outline.OutlineColor = Color.red;
                    backIman.outline.OutlineColor = Color.red;
                    leftIman.outline.OutlineColor = Color.red;
                    rightIman.outline.OutlineColor = Color.red;
                    
                    headOutline.OutlineColor = Color.blue;
                    break;
                }
            default:
                break;
        }
    }
    #endregion

    #region UPDATE
    void Update()
    {
        if (!isDead)
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
                    head.DOLookAt(new Vector3(player.position.x, crosshair.position.y + 1.5f, player.position.z), followSpeed);

                    switch (turretType)
                    {
                        case TurretType.PROJECTILE:
                            {
                                //Dispaamos el proyectil solo si es de tipo proyectil
                                if (turretType == TurretType.PROJECTILE)
                                {
                                    shootTimer += Time.deltaTime;
                                    if (shootTimer >= fireRate)
                                    {
                                        shootTimer = 0;
                                        Shoot();
                                    }
                                }
                                break;
                            }
                        case TurretType.LASER:
                            {
                                RaycastHit hit; ;
                                Debug.DrawRay(eyeTurret.position, (crosshair.position - eyeTurret.position) * 1000, Color.white);

                                if (Physics.Raycast(eyeTurret.position, (crosshair.position - eyeTurret.position), out hit, Mathf.Infinity, layerMask))
                                {
                                    Debug.Log("CHOCA CON ALGO");

                                    if (hit.collider.CompareTag("Player"))
                                    {
                                        playerLogic.GetDamage(damage);
                                    }
                                }
                                break;
                            }
                        default:
                            break;
                    }
                    

                }

                switch (turretType)
                {
                    case TurretType.PROJECTILE:
                        {
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
                                //Paramos las particulas
                                chargeParticles.SetActive(false);

                                DeactivateHead();
                                headActivate = false;

                                line.enabled = false;

                                Debug.Log("LA TORRETA SE MUERE");
                                isDead = true;
                            }
                            break;
                        }
                    case TurretType.LASER:
                        {
                            if (frontIman.myPole == iman.POSITIVE)
                            {
                                DeactivateFrontStone();
                            }
                            if (backIman.myPole == iman.POSITIVE)
                            {
                                DeactivateBackStone();
                            }
                            if (leftIman.myPole == iman.POSITIVE)
                            {
                                DeactivateLeftStone();
                            }
                            if (rightIman.myPole == iman.POSITIVE)
                            {
                                DeactivateRightStone();
                            }

                            if (frontIman.myPole == iman.POSITIVE && backIman.myPole == iman.POSITIVE &&
                                leftIman.myPole == iman.POSITIVE && rightIman.myPole == iman.POSITIVE)
                            {
                                DeactivateHead();
                                headActivate = false;

                                //Paramos las particulas
                                chargeParticles.SetActive(false);

                                line.enabled = false;

                                //Desactivamos el sistema de particulas

                                laserEffect.SetActive(false);

                                isDead = true;
                            }
                            break;
                        }
                    default:
                        break;
                }

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
            if (!isDead)
            {
                line.enabled = true;

                //Si la torreta es laser activamos el sistema de particulas
                if (turretType == TurretType.LASER)
                {
                    laserEffect.SetActive(true);
                }

                //encendemos las particulas
                chargeParticles.SetActive(true);

                isInside = true;

                //Animamos la torreta para que se active
                ActivateTurretAnimation();
            }                      
        }
    }
    #endregion
    
    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            line.enabled = false;

            //Si la torreta es laser activamos el sistema de particulas
            if (turretType == TurretType.LASER)
            {
                laserEffect.SetActive(false);
            }

            isInside = false;

            DeactivateTurretAnimation();
        }
    }
    #endregion

    #region ACTIVATE TURRET ANIMATION
    void ActivateTurretAnimation()
    {
        if (!isDead)
        {
            //Cambiamos el outline
            switch (turretType)
            {
                case TurretType.PROJECTILE:
                    {
                        frontIman.outline.OutlineColor = Color.red;
                        backIman.outline.OutlineColor = Color.red;
                        leftIman.outline.OutlineColor = Color.red;
                        rightIman.outline.OutlineColor = Color.red;
                        break;
                    }
                case TurretType.LASER:
                    {
                        frontIman.outline.OutlineColor = Color.blue;
                        backIman.outline.OutlineColor = Color.blue;
                        leftIman.outline.OutlineColor = Color.blue;
                        rightIman.outline.OutlineColor = Color.blue;
                        break;
                    }
                default:
                    break;
            }



            //Elevamos la cabeza
            head.DOMove(upHead.position, speedAnimation);

            //Separamos las piedras imantables
            frontStone.DOMove(frontFar.position, speedAnimation);
            backStone.DOMove(backFar.position, speedAnimation);
            leftStone.DOMove(leftFar.position, speedAnimation);
            rightStone.DOMove(rightFar.position, speedAnimation);
        }
        

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
            switch (turretType)
            {
                case TurretType.PROJECTILE:
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
                        if (rightIman.myPole == iman.POSITIVE)
                        {
                            DeactivateRightStone();
                        }

                        DeactivateHead();

                        //Reestablecemos
                        frontIman.myPole = iman.POSITIVE;
                        backIman.myPole = iman.POSITIVE;
                        leftIman.myPole = iman.POSITIVE;
                        rightIman.myPole = iman.POSITIVE;

                        //Seteamos los Outlines
                        frontIman.outline.OutlineColor = Color.blue;
                        backIman.outline.OutlineColor = Color.blue;
                        leftIman.outline.OutlineColor = Color.blue;
                        rightIman.outline.OutlineColor = Color.blue;
                        break;
                    }
                case TurretType.LASER:
                    {
                        //Comprobamos la frontal 
                        if (frontIman.myPole == iman.NEGATIVE)
                        {
                            DeactivateFrontStone();
                        }
                        //Comprobamos la trasera 
                        if (backIman.myPole == iman.NEGATIVE)
                        {
                            DeactivateBackStone();
                        }
                        //Comprobamos la izquierda 
                        if (leftIman.myPole == iman.NEGATIVE)
                        {
                            DeactivateLeftStone();
                        }
                        //Comprobamos la frontal 
                        if (rightIman.myPole == iman.NEGATIVE)
                        {
                            DeactivateRightStone();
                        }

                        DeactivateHead();

                        //Reestablecemos
                        frontIman.myPole = iman.NEGATIVE;
                        backIman.myPole = iman.NEGATIVE;
                        leftIman.myPole = iman.NEGATIVE;
                        rightIman.myPole = iman.NEGATIVE;

                        //Seteamos los Outlines
                        frontIman.outline.OutlineColor = Color.red;
                        backIman.outline.OutlineColor = Color.red;
                        leftIman.outline.OutlineColor = Color.red;
                        rightIman.outline.OutlineColor = Color.red;
                        break;
                    }
                default:
                    break;
            }         

        }        

        headActivate = true;
    }
    #endregion

    #region DEACTIVATE HEAD
    void DeactivateHead()
    {
        //isDead = true;
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
