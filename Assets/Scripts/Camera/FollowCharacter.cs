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
    }
    #endregion

    #region UPDATE
    void Update()
    {
        this.transform.DOMove(player.position, speedCamera);
    }
    #endregion

}
