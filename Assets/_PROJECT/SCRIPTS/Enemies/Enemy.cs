using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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
    List<GameObject> gameObjectsHittedMe;
    public LayerMask Ground;
    public Transform _groundChecker;
    public bool _isGrounded=false;
    private float GroundDistance = 0.2f;

    PlayerLogic playerLogic;

    [Header("ENEMY")]
    [SerializeField] float startingLife = 100;
    [SerializeField] int lifeThershold = 30;
    [SerializeField] ElementalAmimator animator;

    [Header("HEALTH BAR")]
    [SerializeField] Image healthBar;
    [SerializeField] GameObject healthTransform;
    bool isDead = false;

    [Header("DAMAGE")]
    [SerializeField] float areaDamage = 10;
    [SerializeField] float areaPushingForce = 4;
    [SerializeField] float chargeDamage = 20;
    [SerializeField] float chargePushingForce = 3;

    [Header("PATROL")]
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float distanceToPoint = 1;
    [SerializeField] float timeToNextPoint = 1f;
    [SerializeField] float distanceAlert = 20;
    int currentPatrolPoint = -1;
    bool onPoint = false;

    [Header("SPEEDS")]
    [SerializeField] float patrolSpeed;
    [SerializeField] float seekSpeed;
    [SerializeField] float agressiveSpeed;

    [Header("ATTACK")]
    [SerializeField] float timeBetweenAttacks = 3;
    [SerializeField] float distanceToDoAreaAtttack = 3;
    [SerializeField] float forceExplosion = 10000;
    [SerializeField] MeshRenderer attackArea;
    [SerializeField] AreaAttack areaAttackLogic;
    [SerializeField] AreaAttack rangeAttackLogic;
    float timer;
    bool isCharging = false;
    GameObject auxiliar;
    [SerializeField] GameObject explosionVFX;

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

    [Header("Shaders")]
    [SerializeField]Renderer rend;
    bool shaderActive = false;
    iman shaderState = iman.NONE;
    #endregion

    #region START
    void Start()
    {
        gameObjectsHittedMe = new List<GameObject>();
        life = startingLife;
        //Buscamos al Player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Buscar el player Logic
        playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();

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
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        //HandleKinematic
        HandleGround(_isGrounded);

        //Comprobamos si está muerto
        if (life <= 0 && !isDead)
        {
            //Ponemos que esta muerto
            isDead = true;
            //Paramos al navMeshAgent
            //agentNavMesh.isStopped = true;

            //Desactivamos la barra de vida
            healthTransform.SetActive(false);
            //Ponemos la animación de morir
            animator.DeathAnimation();
        }
        
        if (!isDead)
        {
            //Actualizamos la barra de vide
            healthBar.fillAmount = life / startingLife;
            healthTransform.transform.LookAt(Camera.main.transform);

            //Controlamos si debe haber cambio de estado
            CheckState();

            //Controlamos el estado actual
            StateBehaviour();
        }

        //CheckGOHittedme
        if (!myImanBehaviorScript.GetApplyForce() && (gameObjectsHittedMe != null))
        {
            gameObjectsHittedMe.Clear();
        }

        //ShaderHandle
        HandleShader();
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

            animator.IdleAnimation();

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

        animator.WalkAnimation();

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
        //Miramos si esta haciendo el ataque en carga y choca a el player 
        if (isCharging && rangeAttackLogic.GetIsPlayer())
        {
            playerLogic.GetDamage(chargeDamage, this.transform.position, chargePushingForce);
        }

        line.SetPosition(0, baseTelegraph.position);
        line.SetPosition(1, targetTelegraph.position);

        if (!myImanBehaviorScript.GetApplyForce() && _isGrounded)
        {

            if (!isAttacking)
            {
                //Miramos al player
                this.transform.DOLookAt(new Vector3(player.position.x, this.transform.position.y,player.position.z),0.5f);

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
                        
                       
                        Invoke("DeactivateAreaAttack", 0.5f);

                        //Ponemos la animación de ataque en area
                        animator.AreaAttackAnimation();
                    }
                    //SI ESTA LEJOS HACEMOS ATAQUE CARGA
                    else if(Physics.CheckSphere(targetTelegraph.position, 1, Ground, QueryTriggerInteraction.Ignore))
                    {
                        line.enabled = true;
                        isAttacking = true;

                        isCharging = true;
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
        else
        {
            //Ponemos la animación de idle
            animator.IdleAnimation();
        }
    }
    #endregion

    #region DEACTIVATE AREA ATTACK
    void DeactivateAreaAttack()
    {
        attackArea.enabled = false;
        agentNavMesh.isStopped = false;

        //Ponemos la animación de caminar
        animator.WalkAnimation();
    }
    #endregion

    #region DEACTIVATE RANGE ATTACK
    void DeactivaterRangeAttack()
    {
        isCharging = false;
    }
    #endregion

    #region EXPLOSION
    public void ExplosionAreaAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 7);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;

                rb.AddExplosionForce(forceExplosion, this.transform.position, forceExplosion, 2, ForceMode.Force);
                if (rb.gameObject.tag == "Player")
                {
                    rb.gameObject.GetComponent<PlayerLogic>().GetDamage(areaDamage);
                }
            }
        }
        auxiliar = Instantiate(explosionVFX, this.transform.position, Quaternion.identity);
        auxiliar.transform.localScale = new Vector3(7, 7, 7);

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
        //Ponemos la animación de ataque  embestida
        animator.RangeAttackAnimation();

        //Desactivamos el lineRenderer
        line.enabled = false;

        //Hacemos el movimiento
        this.transform.DOMove(targetTelegraph.position, speedCharging);

        Invoke("StopAttacking", speedCharging);

        Invoke("DeactivaterRangeAttack", speedCharging);        
    }
    #endregion

    #region STOP ATTACKING
    void StopAttacking()
    {
        //Ponemos la animación de caminar
        animator.WalkAnimation();

        isAttacking = false;
        if (!myImanBehaviorScript.GetApplyForce() && _isGrounded)
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

    #region KINEMATIC
    void HandleGround(bool isGrounded)
    {
        //No esta aplicando fuerza
        if (!myImanBehaviorScript.GetApplyForce())
        {
            //Esta grounded
            if (isGrounded)
            {
                if (myRB.isKinematic == false)
                    myRB.isKinematic = true;
            }
            //no esta grounded
            else if (myRB.isKinematic == true)
            {
                myRB.isKinematic = false;
            }
        }
        else
        {
            if (isGrounded)
            {
                
            }
        }
    }
    #endregion

    #region DAMAGE
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void GetDamage(float damage)
    {
        life -= (int)damage;
    }

    #endregion

    #region SHADER
    void HandleShader()
    {
        if (myImanBehaviorScript.myPole == iman.NONE && shaderState != iman.NONE)
        {
            //Scale a 0
            rend.materials[1].SetFloat("Vector1_9B88008B", 0f);
            shaderState = iman.NONE;
        }
        else if (myImanBehaviorScript.myPole == iman.NEGATIVE && shaderState != iman.NEGATIVE)
        {

            //Scale
            rend.materials[1].SetFloat("Vector1_9B88008B", 20f);
            //blue 
            rend.materials[1].SetColor("Color_77181350", new Color(0.1f, 0.1f, 255, 1));
            //FlagBool
            shaderState = iman.NEGATIVE;
        }
        else if (myImanBehaviorScript.myPole == iman.POSITIVE && shaderState != iman.POSITIVE)
        {
            //Scale
            rend.materials[1].SetFloat("Vector1_9B88008B", 20f);
            //red
            rend.materials[1].SetColor("Color_77181350", new Color(255, 0, 0, 1));
            //FlagBool
            shaderState = iman.POSITIVE;
        }
    }
    #endregion

}
