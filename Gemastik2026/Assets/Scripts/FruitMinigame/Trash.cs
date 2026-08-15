using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IPointerClickHandler
{
    [Header("Kecepatan Jatuh (Y)")]
    public float fallSpeedMin = 300f;
    public float fallSpeedMax = 500f;

    [Header("Gerakan Dinamis (Meliuk & Berputar)")]
    public float swayAmountMin = 40f;   // Seberapa lebar ayunan minimal
    public float swayAmountMax = 90f;   // Seberapa lebar ayunan maksimal
    public float swaySpeedMin = 2f;     // Seberapa cepat mengayun minimal
    public float swaySpeedMax = 4.5f;   // Seberapa cepat mengayun maksimal
    public float rotationSpeedMax = 60f;// Batas kecepatan berputar (derajat/detik)

    [Header("Ukuran Buah")]
    public float sizeMin = 70f;
    public float sizeMax = 100f;

    [Header("Transisi Fade")]
    public float fadeInDuration = 0.25f;
    public float fadeOutDuration = 0.3f;

    private float currentFallSpeed;
    private float currentSwayAmount;
    private float currentSwaySpeed;
    private float currentRotationSpeed;
    private float randomTimeOffset;

    private RectTransform rect;
    private Image image;
    private bool isCaught = false;
    private bool isMissed = false;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.instance;
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        // 1. Kecepatan Jatuh Dasar
        float baseSpeed = Random.Range(fallSpeedMin, fallSpeedMax);
        float multiplier = GameManager.instance != null ? GameManager.instance.GetSpeedMultiplier() : 1f;
        currentFallSpeed = baseSpeed * multiplier;

        // 2. Acak Parameter Gerakan Dinamis untuk Tiap Buah
        currentSwayAmount = Random.Range(swayAmountMin, swayAmountMax);
        currentSwaySpeed = Random.Range(swaySpeedMin, swaySpeedMax);

        // Acak putaran (bisa ke kanan/positif atau ke kiri/negatif)
        currentRotationSpeed = Random.Range(-rotationSpeedMax, rotationSpeedMax);

        // Offset waktu agar buah yang muncul bersamaan tidak mengayun ke arah yang sama persis
        randomTimeOffset = Random.Range(0f, 100f);

        // 3. Atur Ukuran
        float size = Random.Range(sizeMin, sizeMax);
        rect.sizeDelta = new Vector2(size, size);

        // 4. Fade In saat Muncul
        if (image != null)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    void Update()
    {
        if (isCaught) return;

        float swayX = Mathf.Sin(Time.time * currentSwaySpeed + randomTimeOffset) * currentSwayAmount;

        rect.anchoredPosition += new Vector2(swayX, -currentFallSpeed) * Time.deltaTime;

        rect.Rotate(0f, 0f, currentRotationSpeed * Time.deltaTime);

        if (!isMissed && rect.anchoredPosition.y < -1080 / 2f - 50f)
        {
            audioManager.playBananaFallSFX();
            isMissed = true;
            StartCoroutine(MissFadeOutRoutine());
        }
    }

    IEnumerator FadeInRoutine()
    {
        Color c = image.color;
        c.a = 0f;
        image.color = c;

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            image.color = c;
            yield return null;
        }

        c.a = 1f;
        image.color = c;
    }

    IEnumerator MissFadeOutRoutine()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.FruitMissed();
        }

        Color initialColor = image != null ? image.color : Color.white;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;

            if (image != null)
            {
                image.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, t));
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    public void FadeOutAndDestroy(float duration = 0.25f)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(QuickFadeOutRoutine(duration));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator QuickFadeOutRoutine(float duration)
    {
        isMissed = true;
        Color initialColor = image != null ? image.color : Color.white;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (image != null)
            {
                image.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, t));
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCaught || isMissed) return;
        isCaught = true;
        audioManager.playBananaCollectSFX();

        if (GameManager.instance != null)
        {
            GameManager.instance.TrashCollected(transform.position);
        }

        StartCoroutine(JuicyCatchRoutine());
    }

    IEnumerator JuicyCatchRoutine()
    {
        Vector3 initialScale = rect.localScale;
        Vector3 punchScale = initialScale * 1.25f;

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.localScale = Vector3.Lerp(initialScale, punchScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        float shrinkDuration = 0.08f;
        Color initialColor = image != null ? image.color : Color.white;

        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / shrinkDuration;
            rect.localScale = Vector3.Lerp(punchScale, Vector3.zero, t);

            if (image != null)
            {
                image.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0f, t));
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}