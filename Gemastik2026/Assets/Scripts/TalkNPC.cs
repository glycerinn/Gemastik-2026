using UnityEngine;
using Yarn.Unity;

public class TalkNPC : MonoBehaviour
{
    public static TalkNPC CurrentNPC;

    [Header("Dialogue")]
    public DialoguePortrait dialoguePortrait;
    public DialogueRunner dialogueRunner;
    public string dialogueNode;
    public Sprite dialogueSprite;
    
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    public float fadeSpeed = 5f;
    public GameObject InteractOption;

    private CanvasGroup canvasGroup;

    public void Awake()
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        canvasGroup = InteractOption.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = InteractOption.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        Debug.Log("Registered StartGame");
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

        float targetAlpha = distance <= interactDistance ? 1f : 0f;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
    }

    public void Talk()
    {
        CurrentNPC = this;
        Debug.Log("Called");
        dialogueRunner.StartDialogue(dialogueNode);
    }
}