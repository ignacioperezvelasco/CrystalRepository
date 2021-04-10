using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform initialCheckpoint;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (PlayerPrefs.GetInt("CHECKPOINT") == 0)
        {
            player.transform.position = initialCheckpoint.transform.position;
        }
        else
        {
            Vector3 newPosition = new Vector3(PlayerPrefs.GetFloat("POS_X"),
                                                PlayerPrefs.GetFloat("POS_Y"),
                                                PlayerPrefs.GetFloat("POS_Z"));

            player.transform.position = newPosition;
        }
    }
    

}
