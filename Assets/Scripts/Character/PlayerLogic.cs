using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : Agent
{
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        life = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetDamage(float _damage)
    {
        life -= _damage;

        if (life <= 0)
            Die();

    }

    public bool IsAlive()
    {
        return !dead;
    }

    public void Heal(float healUnit)
    {
        Debug.Log(healUnit);
        life += healUnit;
    }

    void Die()
    {
        dead = true;
        //Activater animate

    }
}
