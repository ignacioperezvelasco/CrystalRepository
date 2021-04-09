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
        RAIL,
        ELEVATOR
    };
    PlatformState state;
    public PlatformType type;
    public float damage = 20;
    public float forceToPush = 10f;

    [SerializeField] float speed;
    [SerializeField] AudioSource movement_SFX;

    [Header("IMANS")]
    public ImanBehavior platformIman;
    [SerializeField] ImanBehavior rightIman;
    [SerializeField] ImanBehavior leftIman;

    [Header("TRASNFORMS")]
    [SerializeField] Transform platformTransform;
    [SerializeField] Transform rightPosition;
    [SerializeField] Transform leftPosition;

    GameObject player;
    bool hitted = false;

    [Header("ELEVATOR")]
    [SerializeField] float elevatorDuration = 4;
    float elevatorTimer = 0;
    bool elevatorActivated = false;    
    #endregion

    #region START
    void Start()
    {
        movement_SFX = GetComponent<AudioSource>();

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
        switch (type)
        {
            case PlatformType.PLATFORM:
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
                                    platformTransform.DOMove(leftPosition.position, speed);

                                                                       

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

                                    //Activamos el sonido
                                    movement_SFX.Play();
                                    Invoke("StopSound", speed);

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
                    break;
                }
            case PlatformType.RAIL:
                {
                    break;
                }
            case PlatformType.ELEVATOR:
                {
                    if (platformIman.myPole == rightIman.myPole)
                    {
                        if (!elevatorActivated)
                        {
                            //Ponemos a true
                            elevatorActivated = true;

                           // Subimos el ascensor
                            platformTransform.DOMove(leftPosition.position, speed);
                        }
                        else
                        {
                            elevatorTimer += Time.deltaTime;

                            if (elevatorTimer >= elevatorDuration)
                            {
                                elevatorTimer = 0;

                                //Reseteamos el polo del ascensor
                                platformIman.myPole = iman.NONE;
                                platformIman.outline.OutlineColor = Color.white; 

                            }
                        }
                    }
                    else
                    {
                        if (elevatorActivated)
                        {
                            //Desactivamos el ascensor
                            elevatorActivated = false;

                            // Bajamos el ascensor
                            platformTransform.DOMove(rightPosition.position, speed/2);
                        }
                    }
                    break;
                }
            default:
                break;
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
        else if (other.CompareTag("CanBeHitted") && type == PlatformType.PLATFORM)
        {
            other.gameObject.transform.SetParent(platformTransform);
        }

        if (type == PlatformType.RAIL && other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hitted && state == PlatformState.MOVING)
        {            
            hitted = true;

            other.GetComponent<Agent>().GetDamage(damage, this.transform.position, 4);
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
        else if (other.CompareTag("CanBeHitted") && type == PlatformType.PLATFORM)
        {
            other.gameObject.transform.SetParent(null);
        }
    }
    #endregion

    #region STOP SOUND
    void StopSound()
    {
        movement_SFX.Stop();
    }
    #endregion

}
