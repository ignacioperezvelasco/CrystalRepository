using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLogic : Agent
{
    enum direction {FORWARD, FORWARDLEFT,FORWARDRIGHT,RIGHT,LEFT,BACKWARD, BACKWARDLEFT, BACKWARDRIGHT, IDLE }
    direction currentDirection = direction.IDLE;
    [SerializeField] float maxHealth=100;
    bool dead = false;
    public bool canBeDamaged = true;
    public float timeInvencible;
    float timer = 0;
    [SerializeField]Animator characterAnimator;
    rvMovementPers myMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        life = maxHealth;
        myMovementScript = this.GetComponent<rvMovementPers>();
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
        float angle=0;
        if (myMovementScript.desiredVelocity == Vector3.zero)
            currentDirection = direction.IDLE;
        else
        {
            angle = Vector3.SignedAngle(this.transform.forward, myMovementScript.desiredVelocity, Vector3.up);

            //forward
            if ((angle < 22.5) && (angle > -22.5))
                currentDirection = direction.FORWARD;
            //forwardRight
            else if ((angle > 22.5) && (angle < 67.5))
                currentDirection = direction.FORWARDRIGHT;
            //Right
            else if ((angle > 67.5) && (angle < 112.5))
                currentDirection = direction.RIGHT;
            //Back-Right
            else if ((angle > 112.5) && (angle < 157.5))
                currentDirection = direction.BACKWARDRIGHT;
            //Back
            else if ((angle > 157.5) || (angle < -157.5))
                currentDirection = direction.BACKWARD;
            //Forward-left
            else if ((angle < -22.5) && (angle > -67.5))
                currentDirection = direction.FORWARDLEFT;
            //Left
            else if ((angle < -67.5) && (angle > -112.5))
                currentDirection = direction.LEFT;
            //Back-left
            else if ((angle < -112.5) && (angle > -157.5))
                currentDirection = direction.BACKWARDLEFT;
        }
        SetBool(currentDirection);
    }

    void SetBool(direction current)
    {
        //All false
        characterAnimator.SetBool("forward", false);
        characterAnimator.SetBool("forwardRight", false);
        characterAnimator.SetBool("forwardLeft", false);
        characterAnimator.SetBool("right", false);
        characterAnimator.SetBool("left", false);
        characterAnimator.SetBool("backward", false);
        characterAnimator.SetBool("backwardLeft", false);
        characterAnimator.SetBool("backwardRight", false);
        characterAnimator.SetBool("idle", false);


        //True
        switch (currentDirection)
        {
            case direction.FORWARD:
                characterAnimator.SetBool("forward", true);
                break;
            case direction.FORWARDLEFT:
                characterAnimator.SetBool("forwardLeft", true);
                break;
            case direction.FORWARDRIGHT:
                characterAnimator.SetBool("forwardRight", true);
                break;
            case direction.RIGHT:
                characterAnimator.SetBool("right", true);
                break;
            case direction.LEFT:
                characterAnimator.SetBool("left", true);
                break;
            case direction.BACKWARD:
                characterAnimator.SetBool("backward", true);
                break;
            case direction.BACKWARDLEFT:
                characterAnimator.SetBool("backwardLeft", true);
                break;
            case direction.BACKWARDRIGHT:
                characterAnimator.SetBool("backwardRight", true);
                break;
            case direction.IDLE:
                characterAnimator.SetBool("idle", true);
                break;
            default:
                break;
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