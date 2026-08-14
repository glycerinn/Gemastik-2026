using System.Collections;
using UnityEngine;

public class BookUI : MonoBehaviour
{
    public GameObject bookPanel;

    [Header("Animation")]
    public float animationDuration = 0.3f;
    public float startScale = 0.7f;
    public float startHeight = 50f;

    private RectTransform bookRect;
    private Vector2 originalPosition;
    private Vector3 originalScale;

    private AudioManager audioManager;

    private void Awake()
    {
        bookRect = bookPanel.GetComponent<RectTransform>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        originalPosition = bookRect.anchoredPosition;
        originalScale = bookRect.localScale;
    }

    public void OpenBook()
    {
        audioManager.playBookClickSFX();
        bookPanel.SetActive(true);

        bookRect.localScale = originalScale * startScale;
        bookRect.anchoredPosition =
            originalPosition + Vector2.down * startHeight;

        StartCoroutine(OpenAnimation());

        Time.timeScale = 0f;
    }

    IEnumerator OpenAnimation()
    {
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = timer / animationDuration;
            progress = Mathf.SmoothStep(0f, 1f, progress);

            bookRect.localScale = Vector3.Lerp(
                originalScale * startScale,
                originalScale,
                progress
            );

            bookRect.anchoredPosition = Vector2.Lerp(
                originalPosition + Vector2.down * startHeight,
                originalPosition,
                progress
            );

            yield return null;
        }

        bookRect.localScale = originalScale;
        bookRect.anchoredPosition = originalPosition;
    }

    public void CloseBook()
    {
        StartCoroutine(CloseAnimation());
    }

    IEnumerator CloseAnimation()
    {
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = timer / animationDuration;
            progress = Mathf.SmoothStep(0f, 1f, progress);

            bookRect.localScale = Vector3.Lerp(
                originalScale,
                originalScale * startScale,
                progress
            );

            bookRect.anchoredPosition = Vector2.Lerp(
                originalPosition,
                originalPosition + Vector2.down * startHeight,
                progress
            );

            yield return null;
        }

        bookRect.localScale = originalScale * startScale;
        bookRect.anchoredPosition =
            originalPosition + Vector2.down * startHeight;

        bookPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}