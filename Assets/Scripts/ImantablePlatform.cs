using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImantablePlatform : MonoBehaviour
{
    #region VARIABLES
    public enum PlatformState
    {
        RIGHT_SIDE,
        LEFT_SIDE,
        MOVING
    };

    public enum PlatformType
    {
        PLATFORM,
        RAIL
    };
    PlatformState state;
    public PlatformType type;
    public float damage = 20;

    [SerializeField] float speed;

    [Header("RIGHT DOOR")]
    [SerializeField] ImanBehavior platformIman;
    [SerializeField] ImanBehavior rightIman;
    [SerializeField] ImanBehavior leftIman;

    [Header("POSITIONS")]
    [SerializeField] Transform platformTransform;
    [SerializeField] Transform rightPosition;
    [SerializeField] Transform leftPosition;

    GameObject player;
    bool hitted = false;
    #endregion

    #region START
    void Start()
    {
        //Player
        player = GameObject.FindGameObjectWithTag("Player");

        rightIman.outline.enabled = true;
        leftIman.outline.enabled = true;

        //Ponemos el color que toca a los imanes fijos
        if (rightIman.myPole == iman.NEGATIVE)
        {
            rightIman.outline.OutlineColor = new Color32(0, 0, 255, 255);
        }
        else if (rightIman.myPole == iman.POSITIVE)
        {            
            rightIman.outline.OutlineColor = new Color32(255, 0, 0, 255);
        }

        if (leftIman.myPole == iman.NEGATIVE)
        {
            leftIman.outline.OutlineColor = new Color32(0, 0, 255, 255);
        }
        else if (leftIman.myPole == iman.POSITIVE)
        {
            leftIman.outline.OutlineColor = new Color32(255, 0, 0, 255);
        }
    }
    #endregion

    #region UPDATE
    void Update()
    {
        switch (state)
        {
            case PlatformState.RIGHT_SIDE:
                {
                    if (platformIman.myPole == rightIman.myPole)
                    {
                        //Pasamos el estado a Moving
                        state = PlatformState.MOVING;

                        // Movemos la plataforma al otro lado
                        platformTransform.DOMove(leftPosition.position,speed);

                        //Preparamos que se reestablezca el estado
                        Invoke("ArriveToLeft", speed);
                    }
                    break;
                }
            case PlatformState.LEFT_SIDE:
                {
                    if (platformIman.myPole == leftIman.myPole)
                    {
                        //Pasamos el estado a Moving
                        state = PlatformState.MOVING;

                        // Movemos la plataforma al otro lado
                        platformTransform.DOMove(rightPosition.position, speed);

                        //Preparamos que se reestablezca el estado
                        Invoke("ArriveToRight", speed);
                    }
                    break;
                }
            case PlatformState.MOVING:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    #endregion

    #region ARRIVE TO RIGHT
    void ArriveToRight()
    {
        state = PlatformState.RIGHT_SIDE;

        //Reestablecemos el iman
        platformIman.myPole = iman.NONE;
        platformIman.outline.enabled = false;

        hitted = false;
    }
    #endregion

    #region ARRIVE TO LEFT
    void ArriveToLeft()
    {
        state = PlatformState.LEFT_SIDE;

        //Reestablecemos el iman
        platformIman.myPole = iman.NONE;
        platformIman.outline.enabled = false;

        hitted = false;
    }
    #endregion


    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && type == PlatformType.PLATFORM)
        {
            player.transform.SetParent(platformTransform);
        }

        if (type == PlatformType.RAIL && other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hitted && state == PlatformState.MOVING)
        {            
            hitted = true;

            other.GetComponent<Agent>().GetDamage(damage);
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && type == PlatformType.PLATFORM)
        {
            player.transform.SetParent(null);
        }        
    }
    #endregion

}
