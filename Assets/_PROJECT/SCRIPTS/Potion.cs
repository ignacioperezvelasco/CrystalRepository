using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float amountToHeal;
    void Start()
    {
        amountToHeal = 25;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console

            other.gameObject.GetComponent<PlayerLogic>().Heal(amountToHeal);

            Destroy(this.gameObject);

            //CURAR AL JUGADOR
        }
    }

  }
