using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class TeleporterNPC : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;
    public string dialogueNode;

    [Header("Player & Interaction")]
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) player = pObj.transform;
        }
    }

    private void Update()
    {
        if (dialogueRunner == null || dialogueRunner.IsDialogueRunning || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = distance <= interactDistance ? highlightColor : normalColor;
        }

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Talk();
        }
    }

    public void Talk()
    {
        dialogueRunner.StartDialogue(dialogueNode);
    }

  
    [YarnCommand("load_scene")]
    public static void LoadSceneCommand(string sceneName)
    {
        sceneName = sceneName.Trim().Trim('"');

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade(sceneName);
        }
        else
        {
            Debug.LogWarning("[FadeManager] Instance null, berpindah scene secara instan.");
            SceneManager.LoadScene(sceneName);
        }
    }
}