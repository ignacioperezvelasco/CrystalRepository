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
    private rvMovementPers _player;
    private ShootingScript _shootscript; 

    //HUD sliders
    [Header("HUD sliders")]
    public Slider sliderLifePlayer;

    //REVOLVER POSITIVE IMAGE
    [Header("Revolver")]
    public GameObject revolverPositiveGameobject;
    public Image revolverSlider;
    public List<Image> bulletImagesPositive;

    //PRIVATE VARIABLES
    private int lifePlayer;
    public float negativeBullets;
    public float positiveBullets;
    private bool isChargingPositive;
    private bool isCharginNegative;
    private int currentBulletPositive;
    private bool first_charge;
    private bool second_charge;

    // Start is called before the first frame update
    void Start()
    {
        isChargingPositive = false;
        second_charge = false;
        first_charge = false;

        _player = player.GetComponent<rvMovementPers>();
        _shootscript = player.GetComponent<ShootingScript>();

        currentBulletPositive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //VARIABLES TO USE IN HUD
        lifePlayer = _player.GetLife();

        //FUNCTIONS BULLETS 
        isChargingNegativeBullet();
        isChargingPositiveBullet();

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

            //SLIDER COLOR ROJO
            revolverSlider.color = new Color32(255, 0, 0, 255);
            revolverSlider.fillAmount += 1.0f * Time.deltaTime;

            //PRIMERA CARGA
            if(first_charge == false)
            {
                if ((positiveBullets - 1) >= 1 && (positiveBullets - 1) <= 1.9)
                {
                    bulletImagesPositive[currentBulletPositive].color = new Color32(0, 255, 0, 255);
                    LeanTween.rotate(revolverPositiveGameobject, new Vector3(0, 0, 120.0f), 0.3f);
                    revolverSlider.fillAmount = 0;
                    first_charge = true;

                    IncrementBulletPositive(1);
                }
            }
            //SEGUNDA CARGA
            else if(second_charge == false)
            {
                if ((positiveBullets - 1) >= 2 && (positiveBullets - 1) <= 2.9)
                {
                    bulletImagesPositive[currentBulletPositive].color = new Color32(0, 255, 0, 255);
                    LeanTween.rotate(revolverPositiveGameobject, new Vector3(0, 0, 240), 0.3f);
                    revolverSlider.fillAmount = 0;
                    second_charge = true;

                    IncrementBulletPositive(1);
                }
            }
            //TERCERA CARGA
            else if(first_charge == true && second_charge == true)
            {
                if(revolverSlider.fillAmount == 1)
                {
                    bulletImagesPositive[currentBulletPositive].color = new Color32(0, 255, 0, 255);
                    LeanTween.rotate(revolverPositiveGameobject, new Vector3(0, 0, 360), 0.3f);

                    IncrementBulletPositive(1);
                }
            }


            isChargingPositive = true;
        }
        else if (isChargingPositive == true)
        {
            if (positiveBullets <= 1.9)
            {
                Debug.Log("Ha disparado una bala");
                first_charge = false;
            }
            else if(positiveBullets >= 2 && positiveBullets <= 2.9)
            {
                Debug.Log("Ha disparado dos balas");
                first_charge = false;
                second_charge = false;
            }
            else if(positiveBullets > 2.9)
            {
                Debug.Log("Ha disparado tres balas");
                first_charge = false;
                second_charge = false;
            }

            restartFillSlider();
            isChargingPositive = false;
        }
        else if(positiveBullets != 0)
        {
            positiveBullets = 0;
        }
        
    }
    #endregion

    #region IsChargingNegativeBullet
    void isChargingNegativeBullet()
    {
        if (_shootscript.GetIsChargingNegative())
        {
            negativeBullets = _shootscript.GetShootNegative();
        }
        else if(negativeBullets != 0)
        {
            negativeBullets = 0;
        }
    }
    #endregion

    #region IncrementBulletPositive
    void IncrementBulletPositive(int toInc)
    {
        currentBulletPositive += toInc;
        if (currentBulletPositive >= 3)
        {
            currentBulletPositive = currentBulletPositive % 3;
        }
    }
    #endregion

    #region RestartFillSlider
    void restartFillSlider()
    {
        revolverSlider.fillAmount = 0;
    }
    #endregion

}
