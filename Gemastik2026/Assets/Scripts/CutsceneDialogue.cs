using System.Collections;
using TMPro;
using UnityEngine;

public class CutsceneDialogue : MonoBehaviour
{
    public Transform dialogueContainer;
    public GameObject dialogueLinePrefab;

    [TextArea]
    public string[] dialogues;

    public float typingSpeed = 0.05f;

    private int currentLine;
    private Coroutine typingCoroutine;

    private bool isTyping;
    private string currentText;

    private TextMeshProUGUI currentTextObject;


    private void Start()
    {
        ShowDialogue();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }


    private void HandleClick()
    {
        // Finish current text
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            currentTextObject.text = currentText;
            isTyping = false;
        }

        // Create next line
        else
        {
            currentLine++;

            if (currentLine < dialogues.Length)
            {
                ShowDialogue();
            }
            else
            {
                Debug.Log("Cutscene finished");
            }
        }
    }


    private void ShowDialogue()
    {
        currentText = dialogues[currentLine];

        GameObject newLine = Instantiate(
            dialogueLinePrefab,
            dialogueContainer
        );

        currentTextObject = newLine.GetComponent<TextMeshProUGUI>();

        typingCoroutine = StartCoroutine(
            TypeDialogue(currentText)
        );
    }


    private IEnumerator TypeDialogue(string text)
    {
        isTyping = true;

        currentTextObject.text = "";

        foreach(char letter in text)
        {
            currentTextObject.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}