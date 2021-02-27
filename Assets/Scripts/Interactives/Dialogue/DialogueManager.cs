using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    #region VARIABLES
    //Para el player
    rvMovementPers playerMovement;
    //Para el texto
    [Header("DIALOGUE")]
    [SerializeField] float startingDelay = 1;
    public GameObject canvasDialogue;
    public Animator dialogueAnimator;

    public Text nameText;
    public Text dialogueText;


    public DialogueTrigger triggerDialogue;

    private Queue<string> sentences;

    bool isplayerInside = false;
    bool dialogueIsStarted = false;
    
    Transform playerTransform;
    #endregion

    #region START
    void Start()
    {
        //Buscamos el player movement
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();

        sentences = new Queue<string>();

        canvasDialogue.SetActive(false);
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        if (dialogueIsStarted && Input.anyKeyDown)
        {
            //Enseñamos la siguiente frase
            DisplayNextSentence();
        }

        if (isplayerInside && !dialogueIsStarted)
        {

            dialogueIsStarted = true;
            Invoke("InitializeDialogue", startingDelay);

            //activamos el canvas
            canvasDialogue.SetActive(true);

            //Activamos la animación
            dialogueAnimator.SetBool("isOpen", true);

        }      
        
    }
    #endregion

    #region INITIALIZE DIALOGUE
    void InitializeDialogue()
    {

        //Iniciamos el dialogo
        triggerDialogue.TriggerDialogue();

        /* //Guardamos la posición de la cámara
         lastCameraPosition = cameraMain.transform.position;
         lastCamerRotation = cameraMain.transform.rotation.eulerAngles;
         lastFOV = cameraMain.fieldOfView;

         //Movemos la cámara
         cameraMain.transform.DOMove(newPositionCamera.position, 1f);
         cameraMain.transform.DORotate(newPositionCamera.rotation.eulerAngles, 1f);
         cameraMain.DOFieldOfView(65,1f);*/
    }
    #endregion

    #region START DIALOGUE
    public void StartDialogue(Dialogue dialogue)
    {
        //Paramos al jugador
        playerMovement.StopMovement();

        //Cogemos el texto
        nameText.text = dialogue.nameNPC;

        //limpiamos la queue de frases
        sentences.Clear();

        //llenamos la queue de frases con el dialogo
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    #endregion

    #region DISPLAY NEXT SENTENCE
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    #endregion

    #region TYPE SENTENCE
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;

        }

    }
    #endregion

    #region END DIALOGUE
    public void EndDialogue()
    {

        //Activamos la animación del Dialogo
        dialogueAnimator.SetBool("isOpen", false);

        ////Dejamos la cámara donde estaba antes de empezar el dialogo
        //cameraMain.transform.DOMove(lastCameraPosition, 1f);
        //cameraMain.transform.DORotate(lastCamerRotation, 1f);
        //cameraMain.DOFieldOfView(lastFOV, 1f);

        Invoke("DeactivateCanvas", 0.45f);

    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isplayerInside = true;
            playerTransform = other.GetComponent<Transform>();
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isplayerInside = false;
        }
    }
    #endregion

    #region DEACTIVATE CANVAS
    void DeactivateCanvas()
    {
        //Dejamos volver a mover al player
        playerMovement.ResumeMovement();

        //desactivamos el Canvas
        canvasDialogue.SetActive(false);
        //dialogueIsStarted = false;
    }
    #endregion

}
