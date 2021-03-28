using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAmimator : MonoBehaviour
{

    #region VARIABLES
    public Animator elementalAnimator;
    [SerializeField]GameObject explosionVFX;
    [SerializeField]Enemy enemyScript;
    #endregion

    #region UPDATE
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            IdleAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WalkAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AreaAttackAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RangeAttackAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HittedAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DeathAnimation();
        }
    }*/
    #endregion

    #region IDLE ANIMATION
    public void IdleAnimation()
    {
        elementalAnimator.SetBool("isWalking", false);
    }
    #endregion

    #region WALK ANIMATION
    public void WalkAnimation()
    {
        elementalAnimator.SetBool("isWalking", true);
    }
    #endregion

    #region AREA ATTACK ANIMATION
    public void AreaAttackAnimation()
    {
        elementalAnimator.SetTrigger("AreaAttack");
    }
    #endregion

    #region RANGE ATTACK ANIMATION
    public void RangeAttackAnimation()
    {
        elementalAnimator.SetTrigger("RangeAttack");
    }
    #endregion

    #region HITTED ANIMATION
    public void HittedAnimation()
    {
        elementalAnimator.SetTrigger("Hitted");
    }
    #endregion

    #region DEATH ANIMATION
    public void DeathAnimation()
    {
        elementalAnimator.SetTrigger("Death");
    }
    #endregion

    void ExplosionAreaAttack()
    {
        enemyScript.ExplosionAreaAttack();        
    }
}
