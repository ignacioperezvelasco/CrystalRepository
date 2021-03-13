using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //VARIABLES
    [Header("GameObjects")]
    public GameObject player;

    //REFERENCED SCRIPTS
    private PlayerLogic _player;
    private ShootingScript _shootscript;

    //HUD sliders
    [Header("HUD sliders")]
    public Slider sliderLifePlayer;

    //REVOLVER POSITIVE IMAGE
    [Header("Revolver")]
    public Image revolverSliderPositive;
    public Image revolverSliderNegative;
    public List<Image> bulletImagesEmpty;

    //IMAGES NEGATIVE AND POSITIVE
    [Header("Images positive and negative")]
    public List<Image> bulletImagesPositive;
    public List<Image> bulletImagesNegative;

    //PRIVATE VARIABLES
    private int lifePlayer;
    private float negativeBullets;
    private float positiveBullets;
    private bool isChargingPositive;
    private bool isChargingNegative;
    private int currentBulletPositive;
    private int currentBulletNegative;
    private bool first_charge;
    private bool second_charge;
    private bool third_charge;

    // Start is called before the first frame update
    void Start()
    {
        isChargingPositive = false;
        first_charge = false;
        second_charge = false;
        third_charge = false;

        _player = player.GetComponent<PlayerLogic>();
        _shootscript = player.GetComponent<ShootingScript>();

        currentBulletPositive = 0;
        currentBulletNegative = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //VARIABLES TO USE IN HUD
        lifePlayer = (int)_player.GetLife();

        //FUNCTIONS BULLETS 
        isChargingPositiveBullet();
        isChargingNegativeBullet();

        //SLIDER LIFE PLAYER
        SliderPlayerLife();
    }

    #region SliderPlayerLife
    void SliderPlayerLife()
    {
        sliderLifePlayer.value = lifePlayer;
        //Debug.Log(lifePlayer);
    }
    #endregion

    #region IsChargingPositiveBullet
    void isChargingPositiveBullet()
    {
        if (_shootscript.GetIsChargingPositive())
        {
            positiveBullets = _shootscript.GetShootPositive();

            if ((positiveBullets) >= 0.1)
            {
                //SLIDER AZUL
                if (positiveBullets < 3)
                    revolverSliderPositive.fillAmount = positiveBullets % 1.0f;
                else
                    revolverSliderPositive.fillAmount = 1.0f;

                //PRIMERA CARGA
                if (first_charge == false)
                {
                    if ((positiveBullets) >= 1 && (positiveBullets) <= 1.9)
                    {
                        bulletImagesPositive[0].enabled = true;
                        RestartFillSlider();
                        first_charge = true;
                        IncrementBulletPositive(1);
                    }
                }
                //SEGUNDA CARGA
                else if (second_charge == false)
                {
                    if ((positiveBullets) >= 2 && (positiveBullets) < 2.9)
                    {
                        bulletImagesPositive[1].enabled = true;
                        RestartFillSlider();
                        second_charge = true;
                        IncrementBulletPositive(1);
                    }
                }
                //TERCERA CARGA
                else if (third_charge == false)
                {
                    if ((positiveBullets) >= 2.9)
                    {

                        bulletImagesPositive[2].enabled = true;
                        third_charge = true;
                        IncrementBulletPositive(1);
                    }
                }
                isChargingPositive = true;
            }
        }
        //CUANDO DISPARA
        else if (isChargingPositive == true)
        {

            //ROTATES AND RESTART CHARGES
            RestartRevolverCharges(positiveBullets);

            //RESTART VARIABLES
            RestartFillSlider();
            RestartImagesCharges();

            isChargingPositive = false;
        }
        else if (positiveBullets != 0)
        {
            positiveBullets = 0;
        }
        currentBulletNegative = currentBulletPositive;
    }
    #endregion

    #region IsChargingNegativeBullet
    void isChargingNegativeBullet()
    {
        if (_shootscript.GetIsChargingNegative())
        {
            negativeBullets = _shootscript.GetShootNegative();

            //EN CASO QUE MANTENGAMOS PULSADO EL CLICK
            if (negativeBullets >= 0.1)
            {
                //SLIDER ROJO
                if (negativeBullets < 3)
                    revolverSliderNegative.fillAmount = negativeBullets % 1.0f;
                else
                    revolverSliderNegative.fillAmount = 1.0f;

                //PRIMERA CARGA
                if (first_charge == false)
                {
                    if ((negativeBullets) >= 1 && (negativeBullets) <= 1.9)
                    {
                        bulletImagesNegative[0].enabled = true;
                        RestartFillSlider();
                        first_charge = true;
                        IncrementBulletNegative(1);
                    }
                }
                //SEGUNDA CARGA
                else if (second_charge == false)
                {
                    if ((negativeBullets) >= 2 && (negativeBullets) < 2.9)
                    {
                        bulletImagesNegative[1].enabled = true;
                        RestartFillSlider();
                        second_charge = true;
                        IncrementBulletNegative(1);
                    }
                }
                //TERCERA CARGA
                else if (third_charge == false)
                {
                    if ((negativeBullets) >= 2.9)
                    {
                        bulletImagesNegative[2].enabled = true;
                        third_charge = true;
                        IncrementBulletNegative(1);
                    }
                }

                isChargingNegative = true;
            }

        }
        //CUANDO DISPARA
        else if (isChargingNegative == true)
        {
            //ROTATES AND RESTART CHARGES
            RestartRevolverCharges(negativeBullets);

            //RESTART VARIABLES
            RestartFillSlider();
            RestartImagesCharges();

            isChargingNegative = false;
        }
        else if (negativeBullets != 0)
        {
            negativeBullets = 0;
        }

        currentBulletPositive = currentBulletNegative;
    }
    #endregion

    #region IncrementBulletPositive
    void IncrementBulletPositive(int toInc)
    {
        currentBulletPositive += toInc;
        if (currentBulletPositive >= 3)
        {
            currentBulletPositive = 0;
        }
    }
    #endregion

    #region IncrementBulletNegative
    void IncrementBulletNegative(int toInc)
    {
        currentBulletNegative += toInc;
        if (currentBulletNegative >= 3)
        {
            currentBulletNegative = 0;
        }
    }
    #endregion

    #region RestartFillSlider
    void RestartFillSlider()
    {
        revolverSliderNegative.fillAmount = 0;
        revolverSliderPositive.fillAmount = 0;
    }
    #endregion

    #region RestartImagesCharges
    void RestartImagesCharges()
    {
        for (int i = 0; i < bulletImagesPositive.Count; i++)
        {
            bulletImagesNegative[i].enabled = false;
            bulletImagesPositive[i].enabled = false;
        }
    }
    #endregion

    #region RestartRevolverCharges
    void RestartRevolverCharges(float bullet)
    {
        if (bullet <= 1.9)
        {
            first_charge = false;
        }
        else if (bullet >= 2 && bullet <= 2.9)
        {
            first_charge = false;
            second_charge = false;
        }
        else if (bullet >= 2.9)
        {
            first_charge = false;
            second_charge = false;
            third_charge = false;
        }
    }
    #endregion
}
