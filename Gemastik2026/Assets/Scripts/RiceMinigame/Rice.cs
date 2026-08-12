using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Rice : MonoBehaviour, IPointerClickHandler
{
    public enum RiceStage
    {
        Seed, Young, Nearing, Ready
    }

    public RiceStage stage = RiceStage.Ready;

    [Header("UI / Component References")]
    public Image image;
    public SpriteRenderer spriteRenderer;

    [Header("Visual Assets (Sprite Tiap Tahapan)")]
    public Sprite seedSprite;
    public Sprite youngSprite;
    public Sprite nearingSprite;
    public Sprite readySprite;

    [Header("Juicy Animation Settings")]
    public float growthAnimDuration = 0.25f;
    public float punchScaleMultiplier = 1.2f;

    private Vector3 originalScale;
    [HideInInspector] public bool isAnimating = false;

    private void Awake()
    {
        originalScale = transform.localScale;
        if (image == null) image = GetComponent<Image>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // Mengunci rasio gambar agar tidak gepeng
        if (image != null)
        {
            image.preserveAspect = true;
        }
    }

    private void Start()
    {
        UpdateVisualInstant();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TryHarvest();
    }

    private void OnMouseDown()
    {
        TryHarvest();
    }

    private void TryHarvest()
    {
        if (RiceGameManager.Instance != null)
        {
            RiceGameManager.Instance.OnRiceClicked(this);
        }
    }

    public void SetStage(RiceStage newStage, bool animate = false)
    {
        if (animate && gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimateGrowthRoutine(newStage));
        }
        else
        {
            stage = newStage;
            UpdateVisualInstant();
        }
    }

    public void Grow()
    {
        if (stage != RiceStage.Ready && !isAnimating)
        {
            RiceStage nextStage = stage + 1;
            SetStage(nextStage, true);
        }
    }

    public void UpdateVisualInstant()
    {
        Sprite targetSprite = GetSpriteForStage(stage);

        if (image != null) image.sprite = targetSprite;
        if (spriteRenderer != null) spriteRenderer.sprite = targetSprite;

        SetAlpha(1f);
        transform.localScale = originalScale;
    }

    private IEnumerator AnimateGrowthRoutine(RiceStage newStage)
    {
        isAnimating = true;
        stage = newStage;

        float halfDuration = growthAnimDuration * 0.5f;
        float elapsed = 0f;

        Vector3 shrinkScale = originalScale * 0.7f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, shrinkScale, t);
            SetAlpha(Mathf.Lerp(1f, 0.3f, t));
            yield return null;
        }

        Sprite targetSprite = GetSpriteForStage(stage);
        if (image != null) image.sprite = targetSprite;
        if (spriteRenderer != null) spriteRenderer.sprite = targetSprite;

        elapsed = 0f;
        Vector3 punchScale = originalScale * punchScaleMultiplier;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(shrinkScale, punchScale, t);
            SetAlpha(Mathf.Lerp(0.3f, 1f, t));
            yield return null;
        }

        elapsed = 0f;
        float bounceDuration = 0.1f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bounceDuration;
            transform.localScale = Vector3.Lerp(punchScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
        SetAlpha(1f);
        isAnimating = false;
    }

    public IEnumerator HarvestRoutine(System.Action onHarvestComplete)
    {
        isAnimating = true;

        float duration = 0.18f;
        float elapsed = 0f;
        Vector3 punchScale = originalScale * 1.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (t < 0.5f)
                transform.localScale = Vector3.Lerp(originalScale, punchScale, t * 2f);
            else
                transform.localScale = Vector3.Lerp(punchScale, Vector3.zero, (t - 0.5f) * 2f);

            SetAlpha(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        transform.localScale = Vector3.zero;
        onHarvestComplete?.Invoke();

        stage = RiceStage.Seed;
        Sprite seedSp = GetSpriteForStage(RiceStage.Seed);
        if (image != null) image.sprite = seedSp;
        if (spriteRenderer != null) spriteRenderer.sprite = seedSp;

        elapsed = 0f;
        float respawnDuration = 0.15f;
        while (elapsed < respawnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / respawnDuration;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        transform.localScale = originalScale;
        SetAlpha(1f);
        isAnimating = false;
    }

    private Sprite GetSpriteForStage(RiceStage targetStage)
    {
        switch (targetStage)
        {
            case RiceStage.Seed: return seedSprite;
            case RiceStage.Young: return youngSprite;
            case RiceStage.Nearing: return nearingSprite;
            case RiceStage.Ready: return readySprite;
            default: return readySprite;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (image != null)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }
}