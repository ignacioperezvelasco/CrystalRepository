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
    public GameObject revolverGameobject;
    public Image revolverSlider;
    public List<Image> bulletImagesPositive;
    public AnimationCurve curveCooldown;

    //IMAGES NEGATIVE AND POSITIVE
    [Header("Images positive and negative")]
    public Image positiveImage;
    public Image negativeImage;
    public Image borderPositiveImage;
    public Image borderNegativeImage;

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
    private Color32 positiveColor;
    private Color32 negativeColor;
    private Color32 defaultColor;
    private int idRotateCooldown;
    private int idRotateFinishShoot;

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
        positiveColor = new Color32(255, 0, 0, 255);
        negativeColor = new Color32(0, 0, 255, 255);
        defaultColor = new Color32(255, 255, 255, 255);

    }

    // Update is called once per frame
    void Update()
    {
        //VARIABLES TO USE IN HUD
        lifePlayer = (int)_player.GetLife();

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
        //EN CASO QUE NO ESTE CARGANDO
        if(positiveBullets == 0)
        {
            //BORDER IMAGE
            borderPositiveImage.enabled = false;
        }

        //COLOR IMAGE IF THEY ARE NOT COOLDOWN
        if (_shootscript.GetCooldownPositive())
        {
            ColorAlphaImage(positiveImage, 1.0f);
        }

        if (_shootscript.GetIsChargingPositive())
        {
            positiveBullets = _shootscript.GetShootPositive();

            //EN CASO QUE NO SEA UN CLICK
            if ((positiveBullets) >= 0.1)
            {
                //BORDER IMAGE
                borderPositiveImage.enabled = true;
            }

            if ((positiveBullets) >= 1.1)
            {
                //SLIDER COLOR ROJO
                revolverSlider.color = new Color32(255, 0, 0, 255);
                revolverSlider.fillAmount += 1.0f * Time.deltaTime;

                //PRIMERA CARGA
                if (first_charge == false)
                {
                    if ((positiveBullets) >= 1 && (positiveBullets) <= 1.9)
                    {
                        ChangeColorBullet(currentBulletPositive, positiveColor);
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
                        ChangeColorBullet(currentBulletPositive, positiveColor);
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
                        ChangeColorBullet(currentBulletPositive, positiveColor);
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
            RestartColorImages();

            //COLOR ALPHA IMAGE
            ColorAlphaImage(positiveImage, 0.5f);

            isChargingPositive = false;
        }
        else if(_shootscript.GetTryingShootPositive())
        {
            //COOLDOWN SHOOT
            if (!LeanTween.isTweening(idRotateCooldown) && !LeanTween.isTweening(idRotateFinishShoot))
            {
                RotateRevolverCooldown(60);
            }
            _shootscript.SetTryingShootPositive(false);
        }
        else if(positiveBullets != 0)
        {
            positiveBullets = 0;
        }
        currentBulletNegative = currentBulletPositive;
    }
    #endregion

    #region IsChargingNegativeBullet
    void isChargingNegativeBullet()
    {
        //EN CASO QUE NO ESTE CARGANDO
        if (negativeBullets == 0)
        {
            //BORDER IMAGE
            borderNegativeImage.enabled = false;
        }

        //COLOR IMAGE IF THEY ARE NOT COOLDOWN
        if (_shootscript.GetCooldownNegative())
        {
            ColorAlphaImage(negativeImage, 1.0f);
        }

        if (_shootscript.GetIsChargingNegative())
        {
            //EN CASO QUE NO SEA UN CLICK
            if ((negativeBullets) >= 0.1)
            {
                //BORDER IMAGE
                borderNegativeImage.enabled = true;
            }

            negativeBullets = _shootscript.GetShootNegative();

            //EN CASO QUE MANTENGAMOS PULSADO EL CLICK
            if(negativeBullets >= 1.1)
            {
                //SLIDER COLOR ROJO
                revolverSlider.color = new Color32(0, 0, 255, 255);
                revolverSlider.fillAmount += 1.0f * Time.deltaTime;

                //PRIMERA CARGA
                if (first_charge == false)
                {
                    if ((negativeBullets) >= 1 && (negativeBullets) <= 1.9)
                    {
                        ChangeColorBullet(currentBulletNegative, negativeColor);
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
                        ChangeColorBullet(currentBulletNegative, negativeColor);
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
                        ChangeColorBullet(currentBulletNegative, negativeColor);
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
            RestartColorImages();

            //COLOR IMAGE WHEN SHOOT
            ColorAlphaImage(negativeImage, 0.5f);

            isChargingNegative = false;

            //BORDER IMAGE
            borderNegativeImage.enabled = false;
        }
        else if (_shootscript.GetTryingShootNegative())
        {
            //COOLDOWN SHOOT
            if (!LeanTween.isTweening(idRotateCooldown) && !LeanTween.isTweening(idRotateFinishShoot))
            {
                RotateRevolverCooldown(60);
            }
            _shootscript.SetTryingShootNegative(false);
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
        revolverSlider.fillAmount = 0;
    }
    #endregion

    #region RestartColorImages
    void RestartColorImages()
    {
        ChangeColorBullet(0, defaultColor);
        ChangeColorBullet(1, defaultColor);
        ChangeColorBullet(2, defaultColor);
    }
    #endregion

    #region RotateRevolver
    void RotateRevolver(float haveToRotate)
    {
        idRotateFinishShoot = LeanTween.rotateAroundLocal(revolverGameobject.gameObject, Vector3.forward, haveToRotate, 0.3f).id;
        //LeanTween.rotateAroundLocal(revolverGameobject.gameObject, Vector3.forward, haveToRotate, 0.3f).setEase(curveCooldown);
    }
    #endregion

    #region RotateRvolverCooldown
    void RotateRevolverCooldown(float haveToRotate)
    {
        idRotateCooldown = LeanTween.rotateAroundLocal(revolverGameobject.gameObject, Vector3.forward, 60.0f, 0.5f).setEase(curveCooldown).id;
    }
    #endregion

    #region RestartRevolverCharges
    void RestartRevolverCharges(float bullet)
    {
        if (bullet <= 1.9)
        {
            RotateRevolver(120.0f);
            first_charge = false;
        }
        else if (bullet >= 2 && bullet <= 2.9)
        {
            RotateRevolver(240.0f);
            first_charge = false;
            second_charge = false;
        }
        else if (bullet >= 2.9)
        {
            RotateRevolver(360.0f);
            first_charge = false;
            second_charge = false;
            third_charge = false;
        }
    }
    #endregion

    #region ChangeColorBullet
    void ChangeColorBullet(int bullet, Color32 color)
    {
        bulletImagesPositive[bullet].color = color;
    }
    #endregion

    #region ColorAlphaImage
    void ColorAlphaImage(Image img, float alpha)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }
    #endregion
}
