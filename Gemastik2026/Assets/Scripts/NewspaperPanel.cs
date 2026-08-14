using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewspaperType
{
    public NutritionProblem problem;
    public Color color;
}

public class NewspaperPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject newspaperWindow;
    public Image panelImage;

    [Header("Newspapers")]
    public NewspaperType[] newspapers;

    [Header("Animation")]
    public float openDuration = 0.5f;
    public float closeDuration = 0.3f;
    public float startDistance = 700f;

    private RectTransform newspaperRect;
    private Vector2 originalPosition;
    private Coroutine currentAnimation;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        newspaperRect = newspaperWindow.GetComponent<RectTransform>();
        originalPosition = newspaperRect.anchoredPosition;
    }

    private void Start()
    {
        if (!DayManager.Instance.newspaperShownToday)
        {
            DayManager.Instance.StartNewDay();

            SetNewspaperColor();

            DayManager.Instance.newspaperShownToday = true;

            OpenWithAnimation();
        }
        else
        {
            newspaperWindow.SetActive(false);
        }
    }

    void SetNewspaperColor()
    {
        foreach (NewspaperType paper in newspapers)
        {
            if (paper.problem == DayManager.Instance.currentProblem)
            {
                panelImage.color = paper.color;
                break;
            }
        }
    }

    public void OpenWithAnimation()
    {
        audioManager.playNewsOpenSFX();
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        newspaperWindow.SetActive(true);

        Vector2 startPosition =
            originalPosition + Vector2.up * startDistance;

        newspaperRect.anchoredPosition = startPosition;

        currentAnimation = StartCoroutine(SlideIn());

        Time.timeScale = 0f;
    }

    IEnumerator SlideIn()
    {
        Vector2 startPosition =
            originalPosition + Vector2.up * startDistance;

        float timer = 0f;

        while (timer < openDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(timer / openDuration);

            // Smooth movement
            progress = Mathf.SmoothStep(0f, 1f, progress);

            newspaperRect.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    originalPosition,
                    progress
                );

            yield return null;
        }

        newspaperRect.anchoredPosition = originalPosition;

        currentAnimation = null;
    }

    public void CloseNewspaper()
    {
        audioManager.playNewsCloseSFX();
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(SlideOut());
    }

    IEnumerator SlideOut()
    {
        Vector2 endPosition =
            originalPosition + Vector2.down * startDistance;

        float timer = 0f;

        while (timer < closeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(timer / closeDuration);

            progress = Mathf.SmoothStep(0f, 1f, progress);

            newspaperRect.anchoredPosition =
                Vector2.Lerp(
                    originalPosition,
                    endPosition,
                    progress
                );

            yield return null;
        }

        newspaperRect.anchoredPosition = endPosition;

        newspaperWindow.SetActive(false);

        // Reset for next opening
        newspaperRect.anchoredPosition = originalPosition;

        currentAnimation = null;

        Time.timeScale = 1f;
    }

    public void OpenNewspaper()
    {
        SetNewspaperColor();
        OpenWithAnimation();
    }
}