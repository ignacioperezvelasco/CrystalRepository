using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    #region VARIABLES
    //Para el texto
    [Header("DIALOGUE")]
    public GameObject canvasDialogue;
    public Animator dialogueAnimator;

    public Text nameText;
    public Text dialogueText;


    public DialogueTrigger triggerDialogue;

    private Queue<string> sentences;

    bool isplayerInside = false;
    bool dialogueIsStarted = false;

    //Para la cámara
    [Header("CAMERA")]
    public Transform cameraDialogueHanlder;
    public Transform newPositionCamera;
    Camera cameraMain;
    Vector3 lastCameraPosition;
    Vector3 lastCamerRotation;
    float lastFOV;
    Transform playerTransform;
    #endregion

    #region START
    void Start()
    {
        sentences = new Queue<string>();

        canvasDialogue.SetActive(false);

        cameraMain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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

        if (isplayerInside && Input.GetKeyDown(KeyCode.E) && !dialogueIsStarted)
        {
            //activamos el canvas
            canvasDialogue.SetActive(true);

            //Activamos la animación
            dialogueAnimator.SetBool("isOpen", true);

            //Iniciamos el dialogo
            dialogueIsStarted = true;
            triggerDialogue.TriggerDialogue();

            //Guardamos la posición de la cámara
            lastCameraPosition = cameraMain.transform.position;
            lastCamerRotation = cameraMain.transform.rotation.eulerAngles;
            lastFOV = cameraMain.fieldOfView;

            //Movemos la cámara
            cameraMain.transform.DOMove(newPositionCamera.position, 1f);
            cameraMain.transform.DORotate(newPositionCamera.rotation.eulerAngles, 1f);
            cameraMain.DOFieldOfView(65,1f);

        }      
        
    }
    #endregion

    #region START DIALOGUE
    public void StartDialogue(Dialogue dialogue)
    {
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

        //Dejamos la cámara donde estaba antes de empezar el dialogo
        cameraMain.transform.DOMove(lastCameraPosition, 1f);
        cameraMain.transform.DORotate(lastCamerRotation, 1f);
        cameraMain.DOFieldOfView(lastFOV, 1f);

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
        //desactivamos el Canvas
        canvasDialogue.SetActive(false);
        dialogueIsStarted = false;
    }
    #endregion

}
