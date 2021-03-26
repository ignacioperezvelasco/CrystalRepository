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

    public AttackType currentAttack;

    [Header("AREAS")]
    [SerializeField] Transform[] areasPosition;

    [Header("CHARGE ATTACK")]
    [SerializeField] float timePreparingChargeAttack = 1.5f;
    [SerializeField] float speedAttack = 0.5f;
    [SerializeField] float timeBetweenAttacks = 5;

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
    [SerializeField] SphereCollider areaCollider;
    [SerializeField] float rotationSpeed = 1.5f;
    [SerializeField] float rotationMagnitud = 400;
    Animator bossAnimator;

    float timerAttack = 0;
    #endregion


    #region START
    private void Start()
    {
        //Buscamos al player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Buscamos el LineRenderer
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        //Buscamos al Animator
        bossAnimator = GetComponent<Animator>();
    }
    #endregion

    #region UPDATE
    void Update()
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
    #endregion

    #region ATTACK BEHAVIOUR HANDLER
    void AttackBehaviourHandler()
    {
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
                    Vector3 newEndTelepgraphing = new Vector3(startTelegraphing.position.x + distanceToArea, startTelegraphing.position.y,  startTelegraphing.position.z);

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
                    this.transform.LookAt(toLookAt);

                    RockAttack();
                    break;
                }
            case AttackType.AREA_ATTACK:
                {
                    //Area attack
                    AreaAttack();
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
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack/3);
        }
        else if (distance > 25  && distance < 45)
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack / 2);
        }
        else
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack);
        }

        DeactiveTelegraphing();
        
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

    #region ROCK ATTACK
    void RockAttack()
    {
        GameObject go = Instantiate( rock, rockSpawner.position, rockSpawner.rotation) as GameObject;

        go.transform.DOJump(player.position, heightRock, 1,rockSpeed);
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

}
