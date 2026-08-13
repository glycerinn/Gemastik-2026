using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;

    [Header("Hover Animation")]
    public float hoverScale = 1.1f;
    public float animationSpeed = 8f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        Debug.Log("InventorySlot initialized: " + gameObject.name);
    }

    public void SetFood(FoodSO food)
    {
        icon.sprite = food.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovered: " + gameObject.name);

        StartScale(originalScale * hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartScale(originalScale);
    }

    private void StartScale(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleTo(targetScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(rectTransform.localScale, targetScale) > 0.001f)
        {
            rectTransform.localScale = Vector3.Lerp(
                rectTransform.localScale,
                targetScale,
                Time.unscaledDeltaTime * animationSpeed
            );

            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}