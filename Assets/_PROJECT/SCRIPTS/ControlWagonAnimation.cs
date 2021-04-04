using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWagonAnimation : MonoBehaviour
{
    #region VARIABLE
    public enum CallWagonType
    {
        CALL,
        BACK
    };

    [SerializeField] Animator wagonAnimator;
    [SerializeField] CallWagonType type;
    #endregion

    #region CALL WAGON
    public void CallWagon()
    {
        wagonAnimator.SetBool("CallWagon", true);
    }
    #endregion

    #region BACK WAGON
    void BackWagon()
    {
        wagonAnimator.SetBool("BackWagon", true);
    }
    #endregion


    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    switch (type)
        //    {
        //        case CallWagonType.CALL:
        //            {
        //                CallWagon();
        //                break;
        //            }
        //        case CallWagonType.BACK:
        //            {
        //                BackWagon();
        //                break;
        //            }
        //        default:
        //            break;
        //    }
        //}
    }
    #endregion
    
}
