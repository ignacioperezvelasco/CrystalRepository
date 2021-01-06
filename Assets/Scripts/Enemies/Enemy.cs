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
                    Debug.Log("NO HAY NINGUN ESTADO");
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
                    AttackAgressiveBehaviour();
                    break;
                }
            default:
                {
                    Debug.Log("NO HAY NINGUN ESTADO");
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
            if (Vector3.Distance(player.position, this.transform.position) > 3)
            {
                agentNavMesh.SetDestination(player.position);
            }

            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                timer = 0;

                float distanceToplayer = Vector3.Distance(this.transform.position, player.position);

                if (distanceToplayer <= distanceToDoAreaAtttack)
                {
                    //Hacemos el Ataque en area
                    attackArea.SetActive(true);
                    Invoke("DeactivateAreaAttack", 0.5f);
                }
                else
                {
                    //Hacemos el ataque embestida
                    ChargeAttack();
                    Debug.Log("ATAQUE CARGADO");
                }


            }
        }
        #endregion

        #region DEACTIVATE AREA ATTACK
        void DeactivateAreaAttack()
        {
            attackArea.SetActive(false);
        }
    #endregion

        //TO DO
        #region CHARGE ATTACK
        void ChargeAttack()
        {

        }
        #endregion

        //TO DO
        #region ATTACK AGRESSIVE BEHAVIOUR
        void AttackAgressiveBehaviour()
        {

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
}
