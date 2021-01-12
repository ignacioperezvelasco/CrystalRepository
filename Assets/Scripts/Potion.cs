using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            //Debug.Log("Do something here");
            Destroy(this.gameObject);
            
            //CURAR AL JUGADOR
        }

    }
}
