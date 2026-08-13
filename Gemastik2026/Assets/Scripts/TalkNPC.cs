using UnityEngine;
using Yarn.Unity;

public class TalkNPC : MonoBehaviour
{
    public static TalkNPC CurrentNPC;

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;
    public string dialogueNode;
    public Sprite dialogueSprite;
    
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        Debug.Log("Registered StartGame");

        dialogueRunner.AddCommandHandler("show_character",ShowDialogueCharacter);
    } 

    private void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = distance <= interactDistance
                ? highlightColor
                : normalColor;
        }

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Talk();
        }
    }

    public void Talk()
    {
        CurrentNPC = this;
        Debug.Log("Called");
        dialogueRunner.StartDialogue(dialogueNode);
    }

    public void ShowDialogueCharacter()
    {
        DialoguePortrait.Instance.ShowCharacter(dialogueSprite);
    }
}