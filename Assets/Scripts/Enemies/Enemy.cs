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
    public Transform[] patrolPoints;
    public float distanceAlert = 20;
    int currentPatrolPoint = 0;
    [Header("SPEEDS")]
    public float patrolSpeed;
    public float seekSpeed;
    public float agressiveSpeed;
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
                            Debug.Log("NOS VOLVEMOS AGRESIVOS");
                            currentState = StateEnemy.AGRESSIVE;

                            //Asignamos la velocidad
                            agentNavMesh.speed = agressiveSpeed;
                        }
                        else
                        {
                            Debug.Log("VAMOS A PERSEGUIR AL JUGADOR");
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
                        Debug.Log("VAMOS A PATRULLAR");
                        currentState = StateEnemy.PATROL;

                        //Asignamos la velocidad
                        agentNavMesh.speed = patrolSpeed;
                    }

                    if (life < lifeThershold)
                    {
                        Debug.Log("NOS VOLVEMOS AGRESIVOS");
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
                        Debug.Log("VAMOS A PERSEGUIR AL JUGADOR");
                        currentState = StateEnemy.SEEK;

                        //Asignamos la velocidad
                        agentNavMesh.speed = seekSpeed;
                    }
                    if (Vector3.Distance(this.transform.position, player.position) > distanceAlert)
                    {
                        Debug.Log("VAMOS A PATRULLAR");
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

      //TO DO
    #region FOLLOW PATROL
    void FollowPatrol()
    {
    }
    #endregion

        //TO DO
    #region ATTACK BEHAVIOUR
    void AttackBehaviour()
    {
    }
    #endregion

        //TO DO
    #region ATTACK AGRESSIVE BEHAVIOUR
    void AttackAgressiveBehaviour()
    {
    }
    #endregion
}
