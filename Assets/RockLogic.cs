using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLogic : MonoBehaviour
{

    [SerializeField] float damage = 25;
    [SerializeField] float pushForce = 20;

    [SerializeField] GameObject rockVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Buscamos al boss para calcular el empuje
            Transform boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();


            //Cogemos el player logic
            PlayerLogic player = other.GetComponent<PlayerLogic>(); 
            
            //Aplicamos el daño al player
            player.GetDamage(damage, boss.position, pushForce);


            Instantiate(rockVFX, this.transform.position, this.transform.rotation);
            //Desruimos la roca
            Destroy(this.gameObject);

        }

    }
    
}
