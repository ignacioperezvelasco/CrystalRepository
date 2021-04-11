using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AreaType
{
    AREA_1,
    AREA_2,
    AREA_3,
    AREA_4,
    AREA_5,
    AREA_6,
    AREA_7,
    AREA_8,

};


public class BossLogic : MonoBehaviour
{

    #region VARIABLES
    public enum AttackType
    {
        NONE,
        CHARGE_ATTACK,
        ROCK_ATTACK,
        AREA_ATTACK
    };

    AreaType playerCurrentArea;
    AreaType bossCurrentArea;

    Transform player;
    PlayerLogic playerLogic;

    [HideInInspector] public bool isKilled = false;
    [HideInInspector] public bool isActive = false;

    public AttackType currentAttack;

    [Header("AREAS")]
    [SerializeField] Transform[] areasPosition;

    [Header("CHARGE ATTACK")]
    [SerializeField] float timePreparingChargeAttack = 1.5f;
    [SerializeField] float speedAttack = 0.5f;
    [SerializeField] float timeBetweenAttacks = 5;
    [SerializeField] float chargeDamage = 30;

    [Header("TELEGRAPHING")]
    [SerializeField] Transform startTelegraphing;
    [SerializeField] Transform endTelegraphing;
    LineRenderer line;

    [Header("ROCK ATTACK")]
    [SerializeField] GameObject rock;
    [SerializeField] Transform rockSpawner;
    [SerializeField] float rockSpeed = 0.75f;
    [SerializeField] float heightRock = 5;

    [Header("AREA ATTACK")]
    [SerializeField] float areaAttackTime = 1.5f;
    [SerializeField] float areaDamage = 35;
    [SerializeField] float pushForce = 25;
    [SerializeField] Transform VFX_Spawner;
    [SerializeField] GameObject area_VFX;
    SphereCollider areaCollider;
    bool playerHitted = false;

    //[SerializeField] float rotationSpeed = 1.5f;
    //[SerializeField] float rotationMagnitud = 400;
    Animator bossAnimator;
    [Header("BOSS ANIMATIONS")]
    [SerializeField] Animator modelAnimator;
    [SerializeField] GameObject particlesLaunchRock;
    [SerializeField] float startParticles = 0.25f;
    [SerializeField] float endParticles = 2.05f;
    float delayLaunchRock = 1.25f;

    float timerAttack = 0;
    #endregion

    #region START
    private void Start()
    {
        //Buscamos al player
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        player = go.GetComponent<Transform>();
        playerLogic = go.GetComponent<PlayerLogic>();

        //Buscamos el LineRenderer
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        //Buscamos al Animator
        bossAnimator = GetComponent<Animator>();

        //Buscamos el SphereCollider
        areaCollider = GetComponent<SphereCollider>();
    }
    #endregion

    #region UPDATE
    void Update()
    {
        if (!isKilled && isActive)
        {
            //Seeteamos las posiciones del telgraphing
            line.SetPosition(0, startTelegraphing.position);
            line.SetPosition(1, endTelegraphing.position);

            timerAttack += Time.deltaTime;
            if (timerAttack >= timeBetweenAttacks)
            {
                timerAttack = 0;

                //Controlameos el ataque
                AttackBehaviourHandler();
            }
        }
        
    }
    #endregion

    #region ATTACK BEHAVIOUR HANDLER
    void AttackBehaviourHandler()
    {
        //Escogemos el ataque según la situación
        if (bossCurrentArea == playerCurrentArea)
        {
            currentAttack = AttackType.AREA_ATTACK;
        }
        else
        {
            int random = Random.Range(0, 100);

            if (random % 2 == 0)
            {
                currentAttack = AttackType.CHARGE_ATTACK;
            }
            else if (random % 2 == 1)
            {
                currentAttack = AttackType.ROCK_ATTACK;
            }
        }


        switch (currentAttack)
        {
            case AttackType.NONE:
                {
                    break;
                }
            case AttackType.CHARGE_ATTACK:
                {
                    //Activamos el telegraphing
                    float distanceToArea = Vector3.Distance(this.transform.position, areasPosition[(int)playerCurrentArea].position);
                    Vector3 newEndTelepgraphing = new Vector3(startTelegraphing.position.x + distanceToArea, startTelegraphing.position.y, startTelegraphing.position.z);

                    endTelegraphing.position = newEndTelepgraphing;



                    //Miramos hacia el siguiente area
                    this.transform.DOLookAt(areasPosition[(int)playerCurrentArea].position, timePreparingChargeAttack);

                    //Seteamos la siguiente area
                    bossCurrentArea = playerCurrentArea;

                    //Activamos el telegraphing
                    Invoke("ActiveTelegraphing", timePreparingChargeAttack);

                    //Llamamos a la funcion de atacar
                    Invoke("ChargeAttack", timePreparingChargeAttack + 0.5f);
                    break;
                }
            case AttackType.ROCK_ATTACK:
                {
                    //Miramos al jugador
                    Vector3 toLookAt = new Vector3(player.position.x, this.transform.position.y, player.position.z);
                    this.transform.DOLookAt(toLookAt, timePreparingChargeAttack);

                    //PREPARAMOS EL ATAQUE
                    Invoke("PreparingRockAttack", timePreparingChargeAttack);
                    

                    break;
                }
            case AttackType.AREA_ATTACK:
                {
                    //Area attack
                    AreaAttack();

                    //Activamos el collider
                    ActivateCollider();

                    //VFX 
                    GameObject go = Instantiate(area_VFX, VFX_Spawner.position, VFX_Spawner.rotation);
                    Destroy(go, areaAttackTime);

                    //Invocamos para el desactivar el collider
                    Invoke("DeactivateCollider", areaAttackTime);

                    break;
                }
            default:
                break;
        }
    }
    #endregion

    #region CHARGE ATTACK
    void ChargeAttack()
    {
        //miramos la distancia hasta el siguiente punto
        float distance = Vector3.Distance(this.transform.position, areasPosition[(int)bossCurrentArea].position);

        if (distance < 25)
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack / 3);
        }
        else if (distance > 25 && distance < 45)
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack / 2);
        }
        else
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack);
        }

        DeactiveTelegraphing();

        //Activamos el collider
        ActivateCollider();

        //Preparamos la desactivación del collider
        Invoke("DeactivateCollider", speedAttack);



    }
    #endregion

    #region ACTIVE TELEGRAPHING
    void ActiveTelegraphing()
    {
        line.enabled = true;
    }
    #endregion

    #region DEACTIVE TELEGRAPHING
    void DeactiveTelegraphing()
    {
        line.enabled = false;
    }
    #endregion

    #region PREPARING ROCK ATTACK
    void PreparingRockAttack()
    {
        //Activamos la animacion
        modelAnimator.SetTrigger("RockAttack");

        //Preparamos las particulas
        Invoke("StartParticles", startParticles);
        Invoke("EndParticles", endParticles);

        //Preparamos el lanzamiento
        Invoke("RockAttack", delayLaunchRock);
    }
    #endregion

    #region ROCK ATTACK
    void RockAttack()
    {
        GameObject go = Instantiate(rock, rockSpawner.position, rockSpawner.rotation) as GameObject;

        go.transform.DOJump(player.position, heightRock, 1, rockSpeed);
        Destroy(go, rockSpeed);
    }
    #endregion

    #region CHARGE ATTACK
    void AreaAttack()
    {
        //Vector3 newRotation = new Vector3(0, rotationMagnitud, 0 );
        //this.transform.DORotate(newRotation, rotationSpeed);

        //Activamos la animación
        bossAnimator.SetTrigger("AreaAttack");
    }
    #endregion

    #region SET CURRENT AREA
    public void SetCurrentArea(AreaType newArea)
    {
        playerCurrentArea = newArea;
    }
    #endregion

    #region ACTIVATE COLLIDER
    void ActivateCollider()
    {
        //Desactivamos el sphereCollider
        areaCollider.enabled = true;

        //Reseteamos el Hitted
        playerHitted = false;
    }
    #endregion

    #region DEACTIVATE COLLIDER
    void DeactivateCollider()
    {
        //Desactivamos el sphereCollider
        areaCollider.enabled = false;

        //Reseteamos el Hitted
        playerHitted = false;
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerHitted && (currentAttack == AttackType.AREA_ATTACK || currentAttack == AttackType.CHARGE_ATTACK))
            {
                //Seteamos a true el hitted para no darle muchas veces, solo una por ataque
                playerHitted = true;

                if (currentAttack == AttackType.AREA_ATTACK)
                {
                    playerLogic.GetDamage(areaDamage, this.transform.position, pushForce);
                }
                else if (currentAttack == AttackType.CHARGE_ATTACK)
                {
                    playerLogic.GetDamage(chargeDamage, this.transform.position, pushForce * 2);
                }
            }
        }
    }
    #endregion

    #region START PARTICLES
    void StartParticles()
    {
        particlesLaunchRock.SetActive(true);
    }
    #endregion

    #region END PARTICLES
    void EndParticles()
    {
        particlesLaunchRock.SetActive(false);
    }
    #endregion

    #region KILL BOSS
    public void KillBoss()
    {
        isKilled = true;
    }
    #endregion


}
