using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    #region VARIABLES

    bool alreadyActivated = false;
    Camera cameraMain;
    [Header("SHAKE")]
    [SerializeField] float duration = 1f;
    [SerializeField] float intensity = 2f;
    [SerializeField] int vibration = 15;
    [SerializeField] float random = 90;AudioSource shakeSound;

    [Header("ANIMATION")]
    [SerializeField] bool animationAssigned = false;
    [SerializeField] Animator animator;
    #endregion

    #region START
    private void Start()
    {
        shakeSound = GetComponent<AudioSource>();
        cameraMain = Camera.main;
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyActivated)
        {
            alreadyActivated = true;
            ShakeCamera();            

            if (animationAssigned)
            {
                PlayAnimation();
            }
        }
    }
    #endregion

    #region CAMERA SHAKE
    void ShakeCamera()
    {
        //Generate the shake
        cameraMain.DOShakePosition(duration,intensity,vibration, random);
        cameraMain.DOShakeRotation(duration, intensity, vibration, random);

        //Play audio
        shakeSound.Play();
    }
    #endregion

    #region PLAY ANIMATION
    void PlayAnimation()
    {
        animator.SetBool("Activate", true);
    }
    #endregion

}
