using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLogic : Agent
{
    [SerializeField] float maxHealth=100;
    bool dead = false;
    public bool canBeDamaged = true;
    public float timeInvencible;
    float timer = 0;
    [SerializeField]Animator characterAnimator;
    public bool oneHand = false;
    [SerializeField] rvMovementPers myMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        life = maxHealth;
        characterAnimator.SetBool("oneHand", oneHand);
    }

    // Update is called once per frame
    void Update()
    {
        //Comprobamos la invencibilidad
        if (!canBeDamaged)
        {
            timer += Time.deltaTime;

            if (timer >= timeInvencible)
            {
                canBeDamaged = true;
                timer = 0;
            }
        }

        AnimationHandler();
    }

    public override void GetDamage(float _damage)
    {
        if (canBeDamaged)
        {
            life -= _damage;
            canBeDamaged = false;
        }

        Debug.Log(life);
        if (life <= 0)
            Die();
        else
        {
            characterAnimator.SetBool("damaged", true);
        }
    }

    public override void GetDamage(float _damage, Vector3 pushPosition, float force)
    {

        if (canBeDamaged)
        {
            life -= _damage;

            //Calculamos la direccion del empujón
            Vector3 directionPush = -pushPosition - this.transform.position;
            directionPush.y = 0;
            directionPush = directionPush.normalized;

            //Calculamos la posición del empujón
            Vector3 positionTOJump = this.transform.position + (directionPush * force);

            this.transform.DOJump(positionTOJump, 0.75f, 1, 0.25f);

            canBeDamaged = false;
        }

        Debug.Log(life);
        if (life <= 0)
            Die();
        else
        {
            characterAnimator.SetBool("damaged", true);
        }
    }

    public bool IsAlive()
    {
        return !dead;
    }

    public void Heal(float healUnit)
    {
        Debug.Log(healUnit);
        life += healUnit;
    }

    void AnimationHandler()
    {
        //Velocity
        characterAnimator.SetFloat("Velocity", this.GetComponent<Rigidbody>().velocity.magnitude);

        //Dash
        if (myMovementScript.isDashing)
        {
            characterAnimator.SetBool("Dash", true);
        }
        else 
        {
            if(characterAnimator.GetBool("Dash"))
                characterAnimator.SetBool("Dash", false);
        }

        //Direction
        float angle = 0;

        if (myMovementScript.desiredVelocity != Vector3.zero)
        {
            angle = Vector3.SignedAngle(this.transform.forward, myMovementScript.desiredVelocity, Vector3.up);
            characterAnimator.SetFloat("Horizontal", Mathf.Sin(angle * Mathf.Deg2Rad));
            characterAnimator.SetFloat("Forward", Mathf.Cos(angle * Mathf.Deg2Rad));
        }
        else
        {
            characterAnimator.SetFloat("Horizontal", 0);
            characterAnimator.SetFloat("Forward", 0);
        }
           
       
    }

    void Die()
    {
        dead = true;
        //Activater animate
        characterAnimator.SetBool("die", true);
    }

    public float GetLife()
    {
        return life;
    }
}