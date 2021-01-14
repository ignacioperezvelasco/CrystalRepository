using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : Agent
{
    [SerializeField] float maxHealth=100;
    bool dead = false;
    public bool canBeDamaged = true;
    public float timeInvencible;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        life = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Comprobamos la invencibilidad
        if (!canBeDamaged)
        {
            timer += Time.deltaTime;

            if (timer >= timeInvencible)
            {
                canBeDamaged = true;
                timer = 0;
            }
        }
    }

    public override void GetDamage(float _damage)
    {
        if (canBeDamaged)
        {
            life -= _damage;
            canBeDamaged = false;
        }

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

    public float GetLife()
    {
        return life;
    }
}