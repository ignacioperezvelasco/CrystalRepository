using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //variables
    public float life;
    public float speed;
    //methods
    public void GetDamage(float _damage) { life -= _damage; }
    public void Death() { Destroy(this.gameObject); }
}
