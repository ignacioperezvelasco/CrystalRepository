using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AreaType
{
    AREA_1,
    AREA_2,
    AREA_3,
    AREA_4,
    AREA_5,
    AREA_6,
    AREA_7,
    AREA_8,

};


public class BossLogic : MonoBehaviour
{

    #region VARIABLES
    AreaType playerCurrentArea;
    AreaType bossCurrentArea;
    [Header("AREAS")]
    [SerializeField] Transform[] areasPosition;
    [Header("ATTACK VALUES")]
    [SerializeField] float timePreparingChargeAttack = 1.5f;
    [SerializeField] float speedAttack = 0.5f;
    [SerializeField] float timeBetweenAttacks = 5;
    float timerAttack = 0;
    #endregion

    #region UPDATE
    void Update()
    {
        timerAttack += Time.deltaTime;
        if (timerAttack >= timeBetweenAttacks)
        {
            timerAttack = 0;

            //Miramos hacia el siguiente area
            this.transform.DOLookAt(areasPosition[(int)playerCurrentArea].position, timePreparingChargeAttack);

            //Seteamos la siguiente area
            bossCurrentArea = playerCurrentArea;

            //Llamamos a la funcion de atacar
            Invoke("ChargeAttack", timePreparingChargeAttack);
        }
    }
    #endregion

    #region CHARGE ATTACK
    void ChargeAttack()
    {
        //miramos la distancia hasta el siguiente punto
        float distance = Vector3.Distance(this.transform.position, areasPosition[(int)bossCurrentArea].position);

        if (distance < 25)
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack/3);
        }
        else if (distance > 25  && distance < 45)
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack / 2);
        }
        else
        {
            this.transform.DOMove(areasPosition[(int)bossCurrentArea].position, speedAttack);
        }
        
    }
    #endregion

    #region SET CURRENT AREA
    public void SetCurrentArea(AreaType newArea)
    {
        playerCurrentArea = newArea;
    }
    #endregion

}
