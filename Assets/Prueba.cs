using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //if (animator.GetBool("forward"))
       //    Debug.Log("Entro Forward");
       //else if (animator.GetBool("backward"))
       //    Debug.Log("Entro backward");
       //else if(animator.GetBool("right"))
       //    Debug.Log("Entro right");
       //else if(animator.GetBool("left"))
       //    Debug.Log("Entro left");
       if(animator.GetBool("damaged"))
           animator.SetBool("damaged", false);
    }

     //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // if (animator.GetBool("forward"))
      //     Debug.Log("Salgo Forward");
      // if (animator.GetBool("backward"))
      //     Debug.Log("Salgo backward");
      // if (animator.GetBool("right"))
      //     Debug.Log("Salgo right");
      // if (animator.GetBool("left"))
      //     Debug.Log("Salgo left");
    }

    //OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
