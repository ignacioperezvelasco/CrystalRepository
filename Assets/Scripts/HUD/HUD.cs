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
    [Header("Referenced scripts")]
    private rvMovementPers _player;

    //HUD sliders
    [Header("HUD sliders")]
    public Slider sliderLifePlayer;

    //PRIVATE
    private int lifePlayer;

    // Start is called before the first frame update
    void Start()
    {
        _player = player.GetComponent<rvMovementPers>();
    }

    // Update is called once per frame
    void Update()
    {
        //VARIABLES TO USE IN HUD
        lifePlayer = _player.GetLife();

        //SLIDER LIFE PLAYER
        SliderPlayerLife();
    }


    #region SliderPlayerLife
    void SliderPlayerLife()
    {
        sliderLifePlayer.value = lifePlayer;
        Debug.Log(lifePlayer);
    }
    #endregion

}
