using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rvMovementPers : MonoBehaviour
{
    //movement
    public bool dead = false;
    [Header("movement")]
    public Rigidbody myRb;
    public float speed=2;
    public float maxSpeed=5;
    public float jumpForce = 20;
    public float airControl=1;
    Vector3 desiredVelocity;
    float horizontal;
    float vertical;
    private bool _isGrounded = true;
    public Transform _groundChecker;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    public LayerMask slowerGround;
    //public cameraScript myCam;
    private GameObject myPlayer;

    //La camara para el movimiento en funcion a ella
    Camera viewCamera;
    Vector3 camForward;
    Vector3 camRight;

    [Header("DASH")]
    [SerializeField] GameObject myTrail;
    bool isDashing=false;
    float dashTimer = 0f;
    public float dashvelocity = 30;
    private Vector3 dashV;
    bool doubleJumped = false;
    public float dashTime = 0.15f;
    bool isSlowed=false;
    [SerializeField] float timeToNextDash=2;
    bool canDash = true;
    float dashCounter;
    //other
    [Header("other")]
    private int currentHealth = 100;
    private int maxHealth = 100;
    bool onLiquidSlower = false;

    private void Start()
    {
        dashCounter = timeToNextDash;
        currentHealth = maxHealth;
        myPlayer = GameObject.FindGameObjectWithTag("Player");

        //Seteamos la camara
        viewCamera = Camera.main;
        
    }

    void Update()
    {
        
        //myPlayer.transform.LookAt(new Vector3(myCam.myMouse.x, this.transform.position.y, myCam.myMouse.z));

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


        if (_isGrounded && ((myRb.drag != 7)|| (myRb.drag != 14)))
        {
            if(isSlowed)
                myRb.drag = 25;
            else
                myRb.drag = 7;
        }      
        else if(!_isGrounded)
            myRb.drag = 0;


        //Comprobamos la dirección del movimiento respecto a la camara
        CheckMovementRelativeToCamera();

        //desiredVelocity = new Vector3(horizontal,0f,vertical);
        desiredVelocity = camForward * vertical + camRight * horizontal;
        desiredVelocity.Normalize();
        //move respect camera

        float angle = Vector3.Angle(desiredVelocity, myRb.velocity.normalized);

        if (!_isGrounded)
        {
            desiredVelocity *= airControl;
            Vector3 aux = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
            if ((angle < 80) && (aux.magnitude > maxSpeed))
            {
                desiredVelocity = new Vector3(0, 0, 0);
            }
        }
        else if (doubleJumped)
            doubleJumped = false;

        if (Input.GetButtonDown("Jump"))
        {
            
        }

        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }


    private void FixedUpdate()
    {
        if (!isDashing)
            myRb.AddForce((desiredVelocity * speed ), ForceMode.Acceleration);

        if ((myRb.velocity.magnitude > maxSpeed) && _isGrounded)
        {
            myRb.velocity = myRb.velocity.normalized * maxSpeed;
        }
        if (isDashing)
        {
            myRb.MovePosition(myRb.position + dashV * dashvelocity * 2 * Time.fixedDeltaTime);
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
       
        if (!canDash)
        {
            dashCounter -= Time.fixedDeltaTime;
            if (dashCounter <= 0)
            {
                dashCounter = timeToNextDash;
                canDash = true;
            }

        }
    }  

    void Dash()
    {
        if(canDash)
        {
            myTrail.SetActive(true);
            dashV = desiredVelocity.normalized;
            dashTimer = dashTime;
            isDashing = true;
            canDash = false;
            Invoke("DeactivateTrail", 0.5f);
        }      

    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //pause game with score and gfgo main menu
        dead = true;
    }

    public int GetLife()
    {
        return currentHealth;
    }

    void CheckMovementRelativeToCamera()
    {
        camForward = viewCamera.transform.forward;
        camRight = viewCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void DeactivateTrail()
    {

        myTrail.SetActive(false);
    }
}
