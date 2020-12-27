using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : Agent
{
    #region VARIABLES
    public enum StateEnemy
    {
        NONE,
        PATROL,
        SEEK,
        FLEE
    };
    public enum AttackType
    {
        NONE,
        MELEE_LIGHT,
        MELEE_HEAVY,
        CHARGE,
        AREA
    };

    /////////ENEMY
    [Header("ENEMY")]
    [SerializeField] float lifeThreshold = 30f;
    [SerializeField] float distanceToRun;
    [SerializeField] float chargeForce = 10f;
    [SerializeField] float chargeSpeed =1f;
    [Header("ENEMY DAMAGE")]
    [SerializeField] int attackDamage= 10;
    [SerializeField] int chargeDamage = 20;

    NavMeshAgent agent;
    StateEnemy currentState;
    float maxLife;
    float currentPercentLife;

    /////////PATROL ELEMENTS
    [Header("PATROL ELEMENTS")]
    public Transform[] patrolPoints;
    public float minDistanceToPoint = 1.5f;
    int currentPoint;

    /////////PLAYER
    [Header("PLAYER")]
    public float maxDistanceToPlayer = 20f;
    public float minDistanceToPlayer = 1.5f;
    Rigidbody playerRB;
    Transform playerTransform;

    /////////ATTACK
    [Header("ATTACK")]
    public GameObject bullet;
    public GameObject areaAttack;
    public float areaAtkLife = 0.3f;
    public float timeToAttack = 5f;
    float countDownToAttack = 5f;
    AttackType currentAttack;

    bool isCharging = false;

    /////////KNOCBACK   
    private NavMeshAgent nma = null;
    private Rigidbody rb;
    [Header("KNOCBACK")]
    public float knockbackForce = 5.0f;
    public Vector3 knockbackDirection;
    public bool applyKnockBack;
    #endregion

    #region START
    void Start()
    {
        ///Set Agent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        maxLife = life;

        ////Set Patrol
        currentState = StateEnemy.PATROL;
        UpdatePatrolPoint();

        ////Set Player
        GameObject auxPlayer = GameObject.FindGameObjectWithTag("Player");
        if (auxPlayer != null)
        {
            playerTransform = auxPlayer.transform;
            playerRB = auxPlayer.GetComponent<Rigidbody>();
        }

        ////NAVMESH AGENT KNOCKBACK
        nma = GetComponentInParent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        countDownToAttack = timeToAttack;
    }
    #endregion

    #region UPDATE
    void Update()
    {
        distanceToRun = maxDistanceToPlayer * 0.5f;
        CheckChangeState();

        switch (currentState)
        {
            case StateEnemy.NONE:
                {
                    break;
                }
            case StateEnemy.PATROL:
                {
                    if (Vector3.Distance(this.transform.position, patrolPoints[currentPoint].position) <= minDistanceToPoint)
                    {
                        UpdatePatrolPoint();
                    }
                    break;
                }
            case StateEnemy.SEEK:
                {
                    //Controlamos el ataque
                    HandleAttack();

                    //Miramos si no acercamos demasiado
                    if (Vector3.Distance(this.transform.position, playerTransform.position) >= minDistanceToPoint * 3f)
                    {
                        agent.SetDestination(playerTransform.position);
                    }
                    break;
                }
            case StateEnemy.FLEE:
                {
                    //Controlamos el ataque
                    HandleAttack();

                    //Miramos si no acercamos demasiado
                    if (Vector3.Distance(this.transform.position, playerTransform.position) <= distanceToRun)
                    {
                        //Sacamos la dirección al player
                        Vector3 dirToplayer = this.transform.position - playerTransform.position;
                        Vector3 newPosFlee = this.transform.position + dirToplayer;

                        agent.SetDestination(newPosFlee);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        if (applyKnockBack)
        {
            StartCoroutine(KnockBackCo());
            applyKnockBack = false;
        }
    }
    #endregion


    #region PATROL METHODS
        #region  CHECK CHANGE STATE
    void CheckChangeState()
    {
        switch (currentState)
        {
            case StateEnemy.NONE:
                {
                    break;
                }
            case StateEnemy.PATROL:
                {
                    if (Vector3.Distance(this.transform.position, playerTransform.position) <= maxDistanceToPlayer)
                    {

                        //Current % of Life
                        currentPercentLife = (life / maxLife) * 100;

                        if (currentPercentLife >= lifeThreshold)
                        {
                            countDownToAttack = timeToAttack;
                            //Debug.Log("EL ESTADO ACTUAL ES SEEK");
                            currentState = StateEnemy.SEEK;
                            agent.speed = speed * 2;
                        }
                        else
                        {
                            countDownToAttack = timeToAttack;
                            //Debug.Log("EL ESTADO ACTUAL ES FLEE");
                            currentState = StateEnemy.FLEE;
                            agent.speed = speed * 2;
                        }


                    }
                    break;
                }
            case StateEnemy.SEEK:
                {
                    //Distance to Player
                    if (Vector3.Distance(this.transform.position, playerTransform.position) >= maxDistanceToPlayer)
                    {
                        countDownToAttack = timeToAttack;

                        //Debug.Log("EL ESTADO ACTUAL ES PATROL");
                        currentState = StateEnemy.PATROL;
                        agent.speed = speed;
                        UpdatePatrolPoint();
                    }

                    //Current % of Life
                    currentPercentLife = (life / maxLife) * 100;

                    if (currentPercentLife <= lifeThreshold)
                    {
                        countDownToAttack = timeToAttack;

                        //Debug.Log("EL ESTADO ACTUAL ES FLEE");
                        currentState = StateEnemy.FLEE;
                    }

                    break;
                }
            case StateEnemy.FLEE:
                {
                    if (Vector3.Distance(this.transform.position, playerTransform.position) >= maxDistanceToPlayer)
                    {
                        countDownToAttack = timeToAttack;

                        //Debug.Log("EL ESTADO ACTUAL ES PATROL");
                        currentState = StateEnemy.PATROL;
                        agent.speed = speed;
                        UpdatePatrolPoint();
                    }

                    //Current % of Life
                    currentPercentLife = (life / maxLife) * 100;

                    if (currentPercentLife >= lifeThreshold)
                    {
                        countDownToAttack = timeToAttack;

                        //Debug.Log("EL ESTADO ACTUAL ES SEEK");
                        currentState = StateEnemy.SEEK;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    #endregion

        #region UPDATE PATROL POINT
    void UpdatePatrolPoint()
    {
        //buscar un siguiente punto
        currentPoint = Random.Range(0, patrolPoints.Length - 1);

        agent.SetDestination(patrolPoints[currentPoint].position);
    }
    #endregion
    #endregion

    #region ATTACK METHODS

        #region HANDLE ATTACK
        void HandleAttack()
        {
            countDownToAttack -= Time.deltaTime;
            if (countDownToAttack <= 0f)
            {
                switch (currentState)
                {
                    case StateEnemy.SEEK:
                        {
                            //hacemos un random entre los dos tipos de ataque
                            int rand = Random.Range(0, 100);
                            if (rand > lifeThreshold)
                            {
                                Debug.Log("RANDOM: " + rand + "  LIGHT ATTACK");
                                Attack();
                                break;
                            }
                            else
                            {
                                Debug.Log("RANDOM: " + rand + " CHARGE ATTACK");
                                AreaAttack();
                                //ChargeAttack();
                                break;
                            }

                        }
                    case StateEnemy.FLEE:
                        {
                            int rand = Random.Range(0, 100);
                            //hacemos un random entre los dos tipos de ataque
                            if (rand < lifeThreshold)
                            {
                                Debug.Log("RANDOM: " + rand + "  LIGHT ATTACK");
                                Attack();
                                break;
                            }
                            else
                            {
                                Debug.Log("RANDOM: " + rand + " FRONT ATTACK");
                                AreaAttack();
                                //ChargeAttack();
                            break;
                            }
                        }
                    default:
                        break;
                }

                countDownToAttack = timeToAttack;
            }
        }
        #endregion

        #region ATTACK
        void Attack()
        {
            Instantiate(bullet, this.transform.position, this.transform.rotation);

        //Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, rightPistol.transform.position, rightPistol.transform.rotation);
        //bulletClone.velocity = transform.forward * bulletSpeed;


        /*if (Vector3.Distance(this.transform.position, playerTransform.position) <= minDistanceToPoint * 3f)
        {
            Debug.Log("LIGHT ATTACK!!");

            PushPlayer();
            CauseDamage(attackDamage);
        }*/
        }
    #endregion

        #region AREA ATTACK
        void AreaAttack()
        {
            areaAttack.SetActive(true);
            Invoke("DeactivateAreaAttack", areaAtkLife);
        }
        #endregion

        #region DEACTIVATE AREA ATTACK
        void DeactivateAreaAttack()
        {
            areaAttack.SetActive(false);
        }
        #endregion

        #region CHARGE ATTACK
        void ChargeAttack()
        {
            if (Vector3.Distance(this.transform.position, playerTransform.position) <= minDistanceToPoint * 10f)
            {
                Vector3 pushDirection = Vector3.Normalize(playerTransform.position - this.transform.position);

                this.transform.DOMove(playerTransform.position + pushDirection * chargeForce, chargeSpeed);
                isCharging = true;

                Invoke("RestartChargeAttack", chargeSpeed);
            }
        }
        #endregion

        #region RESTART CHARGE ATTACK
        void RestartChargeAttack()
        {
            isCharging = false;
        }
        #endregion

        #region PUSH PLAYER
        void PushPlayer()
        {
            Vector3 pushDirection = playerTransform.position - this.transform.position;
            pushDirection.y += 2f;
            playerTransform.DOMove(playerTransform.position + pushDirection, 0.3f);
        }
        #endregion    

        #region CAUSE DAMAGE
        public void CauseDamage(int _damage)
        {
        }
        #endregion

        #region GET HURT
        public void GetHurt(int _damage)
        {
            this.life -= _damage;
        }
        #endregion

    #endregion



    #region GET LIFE
    public int GetLife()
    {
        return this.life;
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isCharging)
        {
            PushPlayer();

            CauseDamage(chargeDamage);
        }
    }
    #endregion

    #region ON DRAW GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, maxDistanceToPlayer);
        Gizmos.DrawWireSphere(this.transform.position, distanceToRun);
    }
    #endregion

    #region ONCOLLISIONENTER
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Spikes")
        {
            GetHurt(10);
            knockbackDirection = (agent.transform.position - col.transform.position).normalized;
            StartCoroutine(KnockBackCo());
        }
    }
    #endregion

    #region COURUTINE KNOCBACK
    IEnumerator KnockBackCo()
    {
        nma.enabled = false;
        rb.isKinematic = false;

        knockbackDirection.y = 0.5f;

        //agent.velocity = knockbackDirection * knockbackForce;
        rb.velocity = knockbackDirection * knockbackForce;

        yield return new WaitForSeconds(0.5f);

        nma.enabled = true;
        rb.isKinematic = true;
    }
    #endregion

}
