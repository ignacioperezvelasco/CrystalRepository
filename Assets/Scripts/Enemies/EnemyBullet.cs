using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region VARIABLES
    Rigidbody rb;
    public float bulletSpeed = 10;
    float LifeTime = 10;
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
        Debug.Log("CHOCA CON ALGO");
        if (other.CompareTag("Finish"))
        {
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("CHOCA CON PLAYER");
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
