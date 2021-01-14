using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    PlayerLogic player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLogic>();
    }

    private void Update()
    {
        if (player.GetLife() <= 0)
        {
            Reload();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Reload();
        }
    }

    void Reload()
    {
        SceneManager.LoadScene("PrototipeMap");
    }
}
