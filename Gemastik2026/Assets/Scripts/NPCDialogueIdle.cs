using UnityEngine;

public class NPCDialogueIdle : MonoBehaviour
{
    public Transform player;
    public GameObject InteractOption;

    public float interactDistance = 6f;

    [Header("Fade")]
    public float fadeSpeed = 5f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = InteractOption.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = InteractOption.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        float targetAlpha = distance <= interactDistance ? 1f : 0f;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
    }
}