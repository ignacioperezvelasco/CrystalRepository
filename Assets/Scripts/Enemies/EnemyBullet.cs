using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region VARIABLES
    Rigidbody rb;
    public float bulletSpeed = 10;
    public float LifeTime = 10;
    public float damage = 10;
    #endregion


    #region START
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        Invoke("Die", LifeTime);
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {      

        if (other.CompareTag("Player"))
        {
            PlayerLogic player = other.GetComponent<PlayerLogic>();
            player.GetDamage(damage);

            Destroy(this.gameObject);
        }

        

    }
    #endregion

    #region SET VELOCITY
    public void SetVelocity(Vector3 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }
    #endregion

    #region DIE
    void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

}
