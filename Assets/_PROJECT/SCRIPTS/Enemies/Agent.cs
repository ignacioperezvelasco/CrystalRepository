using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Agent : MonoBehaviour
{
    //variables
    public float life;
    public float speed;
    //methods
    public virtual void GetDamage(float _damage) { life -= _damage; }
    public virtual void GetDamage(float _damage, Vector3 pushPosition, float force)
    {
        life -= _damage;

        //Calculamos la direccion del empujón
        Vector3 directionPush = this.transform.position - pushPosition;
        directionPush.y = 0;
        directionPush = directionPush.normalized;

        //Calculamos la posición del empujón
        Vector3 positionTOJump = this.transform.position + (directionPush * force);

        this.transform.DOJump(positionTOJump, 0.75f, 1, 0.25f);
        
    }
    public virtual void Death() { Destroy(this.gameObject); }
}
