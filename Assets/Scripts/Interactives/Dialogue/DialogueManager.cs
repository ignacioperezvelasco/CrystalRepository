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
    [SerializeField] float dialogueSpeed = 20;
    public GameObject canvasDialogue;
    public Animator dialogueAnimator;

    public Text nameText;
    public Text dialogueText;


    public DialogueTrigger triggerDialogue;

    private Queue<string> sentences;

    bool isplayerInside = false;
    bool dialogueIsStarted = false;
    
    Transform playerTransform;

    private IEnumerator coroutine;

    bool isTyping = false;
    bool writeFullSentence = false;
    bool activated = false;
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
        if (dialogueIsStarted && Input.anyKeyDown && !isTyping)
        {
            //Enseñamos la siguiente frase
            DisplayNextSentence();
        }
        else if (dialogueIsStarted && Input.anyKeyDown && isTyping)
        {
            writeFullSentence = true;
        }

        if (isplayerInside && !activated)
        {

            activated = true;
            InitializeDialogue();

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

        dialogueIsStarted = true;
        //Iniciamos el dialogo
        triggerDialogue.TriggerDialogue();
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

        coroutine = TypeSentence(sentence);
        StartCoroutine(coroutine);
    }
    #endregion

    #region TYPE SENTENCE
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            //Si se han saltado el texto
            if (writeFullSentence)
            {
                writeFullSentence = false;
                dialogueText.text = sentence;
                isTyping = false;

                //Paramos la corutina
                StopCoroutine(coroutine);
            }
            else
            {
                dialogueText.text += letter;
                if (letter == '.' || letter == ',' || letter == '?' || letter == '!')
                {
                    yield return new WaitForSeconds(0.75f);
                }
                else
                {
                    yield return new WaitForSeconds(1 / dialogueSpeed);
                }
            }
            

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
            dialogueText.text = "";

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
