using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private float textSpeed = 0.05f;

    private Queue<DialogueData.DialogueLine> dialogueLines = new();
    private Coroutine dialogueCoroutine;
    private string currentLine;
    private bool isTyping;
    public bool playingDialogue;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        playingDialogue = false;
        isTyping = false;
    }


    private void Update()
    {
        if (playingDialogue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isTyping)
            {

                StopCoroutine(dialogueCoroutine);
                dialogueText.text = currentLine;
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        if (playingDialogue) return;
        //GameManager.Instance.isPaused = true; // PAUSE HERE
        List<DialogueData.DialogueLine> lines = dialogueData.getLines();
        int speakerCount = dialogueData.getSpeakerCount();
        foreach (DialogueData.DialogueLine line in lines)
        {
            dialogueLines.Enqueue(line);
        }
        dialoguePanel.SetActive(true);
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueData.DialogueLine dialogueLine = dialogueLines.Dequeue();
        currentLine = dialogueLine.lineText;
        string speakerName = dialogueLine.speakerName;

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        if (speakerName != null)
        {
            speakerNameText.text = speakerName;
        } else
        {
            speakerNameText.text = "";
        }
        dialogueCoroutine = StartCoroutine(DisplayLine(currentLine));
        playingDialogue = true;
    }

    IEnumerator DisplayLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    private void EndDialogue()
    {
        //GameManager.Instance.isPaused = false; // UNPAUSE HERE
        dialoguePanel.SetActive(false);
        playingDialogue = false;
    }

}
