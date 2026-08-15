using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BookHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverHeight = 8f;
    public float animationSpeed = 5f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Coroutine moveCoroutine;
    private AudioManager audioManager;
    
    private void Awake()
    {
        audioManager = AudioManager.instance;
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.playHoverSFX();
        StartMove(originalPosition + Vector2.up * hoverHeight);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartMove(originalPosition);
    }

    void StartMove(Vector2 target)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveTo(target));
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(rectTransform.anchoredPosition, target) > 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                target,
                Time.unscaledDeltaTime * animationSpeed
            );

            yield return null;
        }

        rectTransform.anchoredPosition = target;
    }
}