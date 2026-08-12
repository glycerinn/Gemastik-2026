using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IPointerClickHandler
{
    public float fallSpeedMin = 300f;
    public float fallSpeedMax = 500f;

    [Header("Ukuran Buah")]
    public float sizeMin = 70f;
    public float sizeMax = 100f;

    [Header("Transisi Fade")]
    public float fadeInDuration = 0.25f;  // Durasi muncul mulus saat spawn
    public float fadeOutDuration = 0.3f;   // Durasi pudar saat lolos/miss

    private float currentFallSpeed;
    private RectTransform rect;
    private Image image;
    private bool isCaught = false;
    private bool isMissed = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        // Kecepatan jatuh dengan multiplier dari GameManager
        float baseSpeed = Random.Range(fallSpeedMin, fallSpeedMax);
        float multiplier = GameManager.instance != null ? GameManager.instance.GetSpeedMultiplier() : 1f;
        currentFallSpeed = baseSpeed * multiplier;

        // Ukuran buah
        float size = Random.Range(sizeMin, sizeMax);
        rect.sizeDelta = new Vector2(size, size);

        // Jalankan Fade In saat pertama kali muncul (spawn)
        if (image != null)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    void Update()
    {
        if (isCaught) return;

        // Buah bergerak jatuh ke bawah
        rect.anchoredPosition += Vector2.down * currentFallSpeed * Time.deltaTime;

        // Cek jika buah lolos ke bawah layar (Missed)
        if (!isMissed && rect.anchoredPosition.y < -1080 / 2f - 50f)
        {
            isMissed = true;
            StartCoroutine(MissFadeOutRoutine());
        }
    }

    // Transisi 1: Fade In saat Buah Baru Spawn
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

    // Transisi 2: Fade Out saat Buah Lolos di Bawah (Miss)
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

    // Fungsi Publik untuk Menghilangkan Buah dengan Mulus (dipanggil saat spawner bersihkan layar)
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
        isMissed = true; // Kunci agar tidak bisa diklik lagi
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

        // Pop Up saat ditangkap
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.localScale = Vector3.Lerp(initialScale, punchScale, elapsed / duration);
            yield return null;
        }

        // Shrink & Fade Out
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