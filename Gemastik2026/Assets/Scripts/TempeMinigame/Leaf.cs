using System.Collections;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [Header("Sprite Settings")]
    public Sprite openLeafSprite;     // Sprite daun pisang terbuka
    public Sprite wrappedLeafSprite;  // Sprite daun pisang terbungkus tempe

    [Header("Juicy Animation Settings")]
    public float fadeDuration = 0.35f;
    public float respawnDelay = 0.5f;

    private bool occupied;
    private SpriteRenderer sr;
    private Collider2D col;
    private Vector3 originalScale;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.instance;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        originalScale = transform.localScale;

        if (openLeafSprite != null)
            sr.sprite = openLeafSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (occupied) return;
        if (!other.CompareTag("Tempe")) return;

        audioManager.playWrapSFX();
        occupied = true;

        DragTemp temp = other.GetComponent<DragTemp>();
        if (temp != null)
        {
            temp.wasPlacedSuccessfully = true;
            if (temp.source != null)
            {
                temp.source.SquareFinished();
            }
        }

        // Tambah skor game
        TempeGameManager.Instance.AddPoint();

        // Hapus tempe mentah yang ditarik
        Destroy(other.gameObject);

        // Jalankan animasi juicy bungkus daun
        StartCoroutine(WrapAndRespawnRoutine());
    }

    IEnumerator WrapAndRespawnRoutine()
    {
        // 1. Ganti Sprite menjadi Daun Terbungkus Tempe
        if (wrappedLeafSprite != null)
        {
            sr.sprite = wrappedLeafSprite;
        }

        // 2. Efek Membal / Pop Scale (Juicy Bounce)
        float elapsed = 0f;
        float bounceDuration = 0.12f;
        Vector3 punchScale = originalScale * 1.25f; // Membesar 25%

        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, punchScale, elapsed / bounceDuration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(punchScale, originalScale, elapsed / bounceDuration);
            yield return null;
        }
        transform.localScale = originalScale;

        // Tahan sebentar setelah terbungkus
        yield return new WaitForSeconds(0.2f);

        // 3. Fade Out (Transparan perlahan)
        col.enabled = false; // Matikan collider saat menghilang
        Color initialColor = sr.color;
        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Jeda sebelum daun baru muncul kembali
        yield return new WaitForSeconds(respawnDelay);

        // 4. Reset Sprite ke Daun Terbuka & Fade In + Pop Up
        if (openLeafSprite != null)
        {
            sr.sprite = openLeafSprite;
        }

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeDuration;

            // Fade in alpha
            float alpha = Mathf.Lerp(0f, 1f, progress);
            sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            // Pop in scale dari kecil ke ukuran normal
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            yield return null;
        }

        // Reset penuh
        sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        transform.localScale = originalScale;
        col.enabled = true;
        occupied = false;
    }
}