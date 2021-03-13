using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFallEnemy : MonoBehaviour
{
    #region VARIABLES
    rvMovementPers playerMovement;
    [Header("ENEMY")]
    [SerializeField] string enemyName;
    [SerializeField] LayerMask layer;

    [Header("ANIMATION")]
    [SerializeField] Animator animator;
    [SerializeField] float duration = 4.5f;
    #endregion

    #region START
    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartAnimation();
        }
    }
    #endregion

    #region START ANIMATION
    private void StartAnimation()
    {
        playerMovement.StopMovement();
        animator.SetBool("Activate", true);

        Invoke("DeactivateAnimation", duration);
    }
    #endregion


    #region DEACTIVATE ANIMATION
    void DeactivateAnimation ()
    {
    }
    #endregion
}
