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
        AGRESSIVE
    };

    StateEnemy currentState = StateEnemy.PATROL;
    Transform player;
    NavMeshAgent agentNavMesh;
    ImanBehavior myImanBehaviorScript;
    Rigidbody myRB;

    [Header("ENEMY")]
    public int startingLife = 100;
    public int lifeThershold = 30;

    [Header("PATROL")]
    public List<Transform> patrolPoints;
    public float distanceToPoint = 1;
    public float timeToNextPoint = 1f;
    public float distanceAlert = 20;
    int currentPatrolPoint = -1;
    bool onPoint = false;

    [Header("SPEEDS")]
    public float patrolSpeed;
    public float seekSpeed;
    public float agressiveSpeed;

    [Header("ATTACK")]
    public float timeBetweenAttacks = 3;
    public float distanceToDoAreaAtttack = 3;
    public GameObject attackArea;
    float timer;

    [Header("TELEGRAPHING")]
    public LineRenderer line;
    public Transform baseTelegraph;
    public Transform targetTelegraph;
    public float chargeDistance = 7;
    public float speedTelegraph = 1.5f;
    bool isAttacking = false;
    float timerAttack = 0;
    [Header("CHARGE")]
    public float speedCharging = 0.7f;
    #endregion

    #region START
    void Start()
    {
        //Buscamos al Player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Buscamos el agente Nev
        agentNavMesh = GetComponent<NavMeshAgent>();

        //Asignamos la velocidad
        agentNavMesh.speed = patrolSpeed;

        //Añadimos el primer destino
        SetNewDestination();

        //Ponemos los puntos del telegraph en su sitio
        line.SetPosition(0, baseTelegraph.position);
        line.SetPosition(1, baseTelegraph.position);
        line.enabled = false;

        //Referenciamos nuestro ImanBehavior
        myImanBehaviorScript = this.GetComponent<ImanBehavior>();

        //Referenciamos RigidBody
        myRB = this.GetComponent<Rigidbody>();
    }
    #endregion

    #region UPDATE
    void Update()
    {
        //Controlamos si debe haber cambio de estado
        CheckState();

        //Controlamos el estado actual
        StateBehaviour(); 
    }
    #endregion

    #region CHECK STATE
    void CheckState()
    {
        switch (currentState)
        {
            case StateEnemy.PATROL:
                {
                    if (Vector3.Distance(this.transform.position, player.position) < distanceAlert)
                    {
                        if (life < lifeThershold)
                        {
                            currentState = StateEnemy.AGRESSIVE;

                            //Asignamos la velocidad
                            agentNavMesh.speed = agressiveSpeed;
                        }
                        else
                        {
                            currentState = StateEnemy.SEEK;

                            //Asignamos la velocidad
                            agentNavMesh.speed = seekSpeed;
                        }
                    }
                    break;
                }
            case StateEnemy.SEEK:
                {
                    if (Vector3.Distance(this.transform.position, player.position) > distanceAlert)
                    {
                        currentState = StateEnemy.PATROL;

                        //Asignamos la velocidad
                        agentNavMesh.speed = patrolSpeed;
                    }

                    if (life < lifeThershold)
                    {
                        currentState = StateEnemy.AGRESSIVE;

                        //Asignamos la velocidad
                        agentNavMesh.speed = agressiveSpeed;
                    }

                    break;
                }
            case StateEnemy.AGRESSIVE:
                {
                    if (life > lifeThershold)
                    {
                        currentState = StateEnemy.SEEK;

                        //Asignamos la velocidad
                        agentNavMesh.speed = seekSpeed;
                    }
                    if (Vector3.Distance(this.transform.position, player.position) > distanceAlert)
                    {
                        currentState = StateEnemy.PATROL;

                        //Asignamos la velocidad
                        agentNavMesh.speed = patrolSpeed;
                    }
                    break;
                }
            default:
                {
                    //Debug.Log("NO HAY NINGUN ESTADO");
                    break;
                }
        }
    }
    #endregion

    #region STATE BEHAVIOUR
    void StateBehaviour()
    {
        switch (currentState)
        {
            case StateEnemy.PATROL:
                {
                    FollowPatrol();
                    break;
                }
            case StateEnemy.SEEK:
                {
                    AttackBehaviour();
                    break;
                }
            case StateEnemy.AGRESSIVE:
                {
                    AttackBehaviour();
                    break;
                }
            default:
                {
                    //Debug.Log("NO HAY NINGUN ESTADO");
                    break;
                }
        }
    }
    #endregion

    #region PATROL METHODS

        #region FOLLOW PATROL
        void FollowPatrol()
        {
            if (Vector3.Distance(this.transform.position, patrolPoints[currentPatrolPoint].position) < distanceToPoint && !onPoint)
            {
                onPoint = true;
                //Mirar al siguiente punto y dirigirnos

                Invoke("LookAtNewDestination", timeToNextPoint / 2);
                Invoke("SetNewDestination", timeToNextPoint);
                //SetNewDestination();
            }
        }
        #endregion

        #region SET NEW DESTINATION
        void SetNewDestination()
        {
            onPoint = false;
            //Aumentamos la posicion del array
            currentPatrolPoint++;
            if (currentPatrolPoint == patrolPoints.Count)
            {
                currentPatrolPoint = 0;
            }

            //Seleccionamos el siguiente destino
            agentNavMesh.SetDestination(patrolPoints[currentPatrolPoint].position);

        }
        #endregion

        #region LOOK AT NEW DESTINATION
        void LookAtNewDestination()
        {
            int aux = currentPatrolPoint;
            aux++;

            if (aux == patrolPoints.Count)
            {
                aux = 0;
            }

            //Mirar al siguiente punto
            this.transform.DOLookAt(patrolPoints[aux].position, timeToNextPoint / 2);
        }
    #endregion

    #endregion

    #region ATTACK METHODS

    #region ATTACK BEHAVIOUR
    void AttackBehaviour()
    {
        line.SetPosition(0, baseTelegraph.position);
        line.SetPosition(1, targetTelegraph.position);

        if (!myImanBehaviorScript.GetApplyForce())
            {

            if (!isAttacking)
            {
                if (Vector3.Distance(player.position, this.transform.position) > 3)
                {
                    agentNavMesh.SetDestination(player.position);
                }

                timer += Time.deltaTime;
                if (timer > timeBetweenAttacks)
                {
                    timer = 0;

                    float distanceToplayer = Vector3.Distance(this.transform.position, player.position);

                    //SI ESTA CERCA DEL ENEMIGO,ATAQUE EN AREA
                    if (distanceToplayer <= distanceToDoAreaAtttack)
                    {
                        //Paramos al enemigo
                        agentNavMesh.isStopped = true;
                        //Hacemos el Ataque en area
                        attackArea.SetActive(true);
                        Invoke("DeactivateAreaAttack", 0.5f);
                    }
                    //SI ESTA LEJOS HACEMOS ATAQUE CARGA
                    else
                    {
                        line.enabled = true;
                        isAttacking = true;
                        //Paramos al enemigo
                        agentNavMesh.isStopped = true;
                        //Hacemos el ataque embestida
                        ChargeAttack();
                        //Debug.Log("ATAQUE CARGADO");
                    }


                }
            }
            else
            {
                //line.SetPosition(0, baseTelegraph.position);
                //line.SetPosition(1, targetTelegraph.position);

                timerAttack += Time.deltaTime;
                if (timerAttack >= speedTelegraph)
                {
                    timerAttack = 0;

                    JumpToTarget();
                }
            }
        }
    }
    #endregion

    #region DEACTIVATE AREA ATTACK
    void DeactivateAreaAttack()
    {
        attackArea.SetActive(false);
        agentNavMesh.isStopped = false;
    }
    #endregion

    #region CHARGE ATTACK
    void ChargeAttack()
    {
        //line.SetPosition(0, baseTelegraph.position);
        //line.SetPosition(1, baseTelegraph.position);


        //Vector3 targetPosition = baseTelegraph.position + (this.transform.forward * chargeDistance);

        //targetTelegraph.DOMove(targetPosition, speedTelegraph);        
    }
    #endregion

    #region JUMP TO TARGET
    void JumpToTarget()
    {
        //Desactivamos el lineRenderer
        line.enabled = false;

        //Hacemos el movimiento
        this.transform.DOMove(targetTelegraph.position, speedCharging);

        Invoke("StopAttacking", speedCharging);
    }
    #endregion

    #region STOP ATTACKING
    void StopAttacking()
    {

        isAttacking = false;
        if (!myImanBehaviorScript.GetApplyForce())
            agentNavMesh.isStopped = false;
    }
    #endregion

    #endregion

    #region ON DRAW GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, distanceToDoAreaAtttack);
        Gizmos.DrawWireSphere(this.transform.position, distanceAlert);
    }
    #endregion

    #region DAMAGE
    private void OnCollisionEnter(Collision collision)
    {
        //En caso de estar imantado y que choque con un objeto imantado añadimos daño
        if ((collision.gameObject.tag == "CanBeHitted") && (myImanBehaviorScript.GetApplyForce()))
        {
            float goVelocityMagnitude = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //En caso que la fuerza sea mayor a 4 hacemos daño
            if ((goVelocityMagnitude + myRB.velocity.magnitude ) > 3)
            {
                GetDamage(goVelocityMagnitude + myRB.velocity.magnitude);
            }
        }
    }

    public void GetDamage(float damage)
    {
        life -= (int)damage;
    }

    #endregion

}
