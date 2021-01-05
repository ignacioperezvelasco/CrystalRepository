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
    [Header("Revolver POSITIVE")]
    public GameObject revolverPositiveGameobject;
    public List<Image> bulletImagesPositive;

    //REVOLVER NEGATIVE IMAGE
    [Header("Revolver NEGATIVE")]
    public GameObject revolverNegativeImageGameobject;
    public List<Image> bulletImagesNegative;

    //PRIVATE VARIABLES
    private int lifePlayer;
    public float negativeBullets;
    public float positiveBullets;
    private bool isChargingPositive;
    private bool isCharginNegative;
    private int currentBulletPositive;

    // Start is called before the first frame update
    void Start()
    {
        isChargingPositive = false;
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
        if(_shootscript.GetIsChargingPositive())
        {
            positiveBullets = _shootscript.GetShootPositive();
            isChargingPositive = true;
        }
        else if (isChargingPositive == true)
        {
            //Debug.Log("Ha disparado");

            if(positiveBullets <= 1.9)
            {
                Debug.Log("Ha disparado una bala");
                bulletImagesPositive[currentBulletPositive].color = new Color32(100, 100, 100, 255);
                LeanTween.rotate(revolverPositiveGameobject, new Vector3(0, 0, (120.0f * (currentBulletPositive + 1))), 0.3f);
                Debug.Log(revolverPositiveGameobject.GetComponent<RectTransform>().rotation.z);
                IncrementBulletPositive(1);
            }
            else if(positiveBullets >= 2 && positiveBullets <= 2.9)
            {
                Debug.Log("Ha disparado dos balas");
            }
            else if(positiveBullets > 2.9)
            {
                Debug.Log("Ha disparado tres balas");
            }

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

}
