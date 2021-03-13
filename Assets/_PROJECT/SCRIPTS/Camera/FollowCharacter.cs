using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowCharacter : MonoBehaviour
{
    #region VARIABLES 
    Transform player;
    [SerializeField] float speedCamera;
    #endregion

    #region START
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        this.transform.position = player.position;
    }
    #endregion

    #region UPDATE
    void FixedUpdate()
    {
        this.transform.DOMove(player.position, speedCamera);
    }
    #endregion

}
