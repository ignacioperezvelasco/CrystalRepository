using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WheelsLogic : MonoBehaviour
{

    #region VARIABLES
    BossLogic boss;

    [SerializeField] float wheelDisplacement = 0.2f;
    [SerializeField] Transform praphicsTransform;

    [Header("PARTICLES EFECT")]
    [SerializeField] GameObject leftFrontParticles;
    [SerializeField] GameObject leftBackParticles;
    [SerializeField] GameObject rightFrontParticles;
    [SerializeField] GameObject rightBackParticles;

    [Header("WHEEL IMANS")]
    [SerializeField] ImanBehavior leftFrontIman;
    [SerializeField] ImanBehavior leftBackIman;
    [SerializeField] ImanBehavior rightFrontIman;
    [SerializeField] ImanBehavior rightBackIman;

    [Header("WHEEL ")]
    [SerializeField] Transform leftFrontWheel_1;
    [SerializeField] Transform leftFrontWheel_2;
    [SerializeField] Transform leftBackWheel_1;
    [SerializeField] Transform leftBackWheel_2;
    [SerializeField] Transform rightFrontWheel_1;
    [SerializeField] Transform rightFrontWheel_2;
    [SerializeField] Transform rightBackWheel_1;
    [SerializeField] Transform rightBackWheel_2;

    bool leftFrontDisabled = false;
    bool leftBackDisabled = false;
    bool rightFrontDisabled = false;
    bool rightBackDisabled = false;

    bool finished = false;
    #endregion


    #region START
    void Start()
    {
        boss = GetComponent<BossLogic>();
    }
    #endregion


    #region UPDATE
    void Update()
    {

        if (leftFrontDisabled && leftBackDisabled && rightFrontDisabled && rightBackDisabled && !finished)
        {
            finished = true;
            boss.KillBoss();
        }

        if (!finished)
        {
            CheckWheels();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DeactivateLeftFront();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DeactivateLeftBack();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DeactivateRightFront();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DeactivateRightBack();
            }
        }
    }
    #endregion


    #region CHECK WHEELS
    void CheckWheels()
    {
        /// -- CHECK LEFT FRONT
        if (!leftFrontDisabled)
        {
            if (leftFrontIman.myPole == iman.POSITIVE)
            {
                //Desactivamos la rueda
                DeactivateLeftFront();
            }
        }

        /// -- CHECK LEFT BACK
        if (!leftBackDisabled)
        {
            if (leftBackIman.myPole == iman.POSITIVE)
            {
                //Desactivamos la rueda
                DeactivateLeftBack();
            }
        }

        /// -- CHECK RIGHT FRONT
        if (!rightFrontDisabled)
        {
            if (rightFrontIman.myPole == iman.POSITIVE)
            {
                //Desactivamos la rueda
                DeactivateRightFront();
            }
        }

        /// -- CHECK RIGHT BACK
        if (!rightBackDisabled)
        {
            if (rightBackIman.myPole == iman.POSITIVE)
            {
                //Desactivamos la rueda
                DeactivateRightBack();
            }
        }

    }
    #endregion

    #region DEACTIVATE LEFT FRON WHEEL
    void DeactivateLeftFront()
    {
        leftFrontDisabled = true;

        //Activamos el sistema de particulas
        leftFrontParticles.SetActive(true);

        //Desplazamos la rueda un poco
        //leftFrontWheel_1.DOMoveX((leftFrontWheel_1.position.x - 0.2f), 0.5f);

        //Quitamos las ruedas de ser hijas de la animación
        leftFrontWheel_1.SetParent(praphicsTransform);
        leftFrontWheel_2.SetParent(praphicsTransform);
    }
    #endregion

    #region DEACTIVATE LEFT BACK WHEEL
    void DeactivateLeftBack()
    {

        leftBackDisabled = true;

        //Activamos el sistema de particulas
        leftBackParticles.SetActive(true);

        //Desplazamos la rueda un poco
        //leftBackWheel_1.DOMoveX((leftBackWheel_1.position.x + 0.2f), 0.5f);

        //Quitamos las ruedas de ser hijas de la animación
        leftBackWheel_1.SetParent(praphicsTransform);
        leftBackWheel_2.SetParent(praphicsTransform);

    }
    #endregion

    #region DEACTIVATE RIGHT FRONT WHEEL
    void DeactivateRightFront()
    {
        rightFrontDisabled = true;

        //Activamos el sistema de particulas
        rightFrontParticles.SetActive(true);

        //Desplazamos la rueda un poco
        //rightFrontWheel_1.DOMoveX((rightFrontWheel_1.position.x - 0.2f), 0.5f);

        //Quitamos las ruedas de ser hijas de la animación
        rightFrontWheel_1.SetParent(praphicsTransform);
        rightFrontWheel_2.SetParent(praphicsTransform);
    }
    #endregion

    #region DEACTIVATE RIGHT BACK WHEEL
    void DeactivateRightBack()
    {
        rightBackDisabled = true;

        //Activamos el sistema de particulas
        rightBackParticles.SetActive(true);

        //Desplazamos la rueda un poco
        //rightBackWheel_1.DOMoveX((rightBackWheel_1.position.x + 0.2f), 0.5f);

        //Quitamos las ruedas de ser hijas de la animación
        rightBackWheel_1.SetParent(praphicsTransform);
        rightBackWheel_2.SetParent(praphicsTransform);
    }
    #endregion
}
