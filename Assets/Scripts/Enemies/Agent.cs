using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //variables
    public float life;
    public float speed;
    //methods
    public virtual void GetDamage(float _damage) { life -= _damage; }
    public virtual void Death() { Destroy(this.gameObject); }
}
