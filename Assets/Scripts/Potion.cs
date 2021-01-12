using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] float amountToHeal = 25f;
    // Start is called before the first frame update

    void Start()
    {
        
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
