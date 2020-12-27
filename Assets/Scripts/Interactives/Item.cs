using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    #region VARIABLES
    public enum ItemType
    {
        NONE,
        CRYSTAL_DUST,
        CRYSTAL,
        LIFE_POTION,
        ATTACK_POTION,
        DEFENSE_POTION,
        SPEED_POTION

    };

    public enum ItemBehaviour
    {
        NONE,
        PICK_UP,
        ATTRACK
    };

    [Header("ITEM TYPE")]
    public ItemType item;
    public ItemBehaviour behaviour;
    public Animator animator;

    [Header("ATTRACK")]
    public float timeToAttrack;
    public float speedAttractionItem;
    float timer;

    Transform playerTransform;

    #endregion

    #region START
    void Start()
    {
        if (behaviour == ItemBehaviour.ATTRACK)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }
    #endregion

    #region UPDATE
    void Update()
    {
        if (timer >= timeToAttrack)
        {
            animator.SetBool("Idle", true);

            Debug.Log("IR PARA EL JUGADOR");
            this.transform.DOMove(new Vector3(  playerTransform.position.x, 
                                                playerTransform.position.y, 
                                                playerTransform.position.z), 
                                                speedAttractionItem);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        AddItem();

        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region ADD ITEM
    public void AddItem()
    {
        switch (item)
        {
            case ItemType.NONE:
                {
                    break;
                }
            case ItemType.CRYSTAL_DUST:
                {
                    Debug.Log("CRYSTAL DUST");
                    break;
                }
            case ItemType.CRYSTAL:
                {
                    Debug.Log("CRYSTAL");
                    break;
                }
            case ItemType.LIFE_POTION:
                {
                    Debug.Log("LIFE POTION");
                    break;
                }
            case ItemType.ATTACK_POTION:
                {
                    Debug.Log("ATTACK POTION");
                    break;
                }
            case ItemType.DEFENSE_POTION:
                {
                    Debug.Log("DEFENSE POTION");
                    break;
                }
            case ItemType.SPEED_POTION:
                {
                    Debug.Log("SPEED POTION");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    #endregion
}
