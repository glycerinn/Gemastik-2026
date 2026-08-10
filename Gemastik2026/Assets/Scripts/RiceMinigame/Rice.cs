using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rice : MonoBehaviour
{
    public enum RiceStage
    {
        Seed, Young, Nearing, Ready
    }

    public RiceStage stage;
    public Image image;

    [Header("Warna Tahapan")]
    public Color seedColor = new Color(0.45f, 0.25f, 0.1f);
    public Color youngColor = Color.green;
    public Color nearingColor = Color.yellow;
    public Color readyColor = new Color(1f, 0.8f, 0f); // Emas

    private Vector3 originalScale;
    private bool isAnimating = false;

    private void Awake()
    {
        originalScale = transform.localScale;
        if (image == null) image = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void SetStage(RiceStage newStage)
    {
        stage = newStage;
        UpdateVisual();
    }

    public void Grow()
    {
        if (stage != RiceStage.Ready)
        {
            stage++;
            UpdateVisual();
        }
    }

    public void UpdateVisual()
    {
        if (image == null) return;

        // Pastikan warna sesuai stage dan transparansi (Alpha) kembali penuh (1.0)
        Color targetColor = GetTargetColor();
        targetColor.a = 1f;
        image.color = targetColor;
    }

    public Color GetTargetColor()
    {
        switch (stage)
        {
            case RiceStage.Seed: return seedColor;
            case RiceStage.Young: return youngColor;
            case RiceStage.Nearing: return nearingColor;
            case RiceStage.Ready: return readyColor;
            default: return Color.white;
        }
    }

    public void PlayJuicyHarvest(System.Action onCompleteCallback)
    {
        if (!gameObject.activeInHierarchy)
        {
            onCompleteCallback?.Invoke();
            return;
        }

        if (isAnimating)
        {
            StopAllCoroutines();
        }

        StartCoroutine(HarvestAnimationRoutine(onCompleteCallback));
    }

    private IEnumerator HarvestAnimationRoutine(System.Action onCompleteCallback)
    {
        isAnimating = true;
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 punchScale = originalScale * 1.3f;

        // 1. Animasi Membesar lalu Mengecil
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            if (percent < 0.5f)
            {
                transform.localScale = Vector3.Lerp(originalScale, punchScale, percent * 2f);
            }
            else
            {
                transform.localScale = Vector3.Lerp(punchScale, Vector3.zero, (percent - 0.5f) * 2f);
            }

            if (image != null)
            {
                Color c = readyColor;
                c.a = Mathf.Lerp(1f, 0f, percent);
                image.color = c;
            }

            yield return null;
        }

        transform.localScale = Vector3.zero;

        // Panggil callback logika panen (tambah skor & grow padi lain)
        onCompleteCallback?.Invoke();

        // 2. Ubah ke Benih (Seed) dan Animasi Membesar Kembali
        stage = RiceStage.Seed;

        elapsed = 0f;
        float growBackDuration = 0.15f;

        // Reset warna ke benih dengan Alpha = 1 (Tampak jelas)
        if (image != null)
        {
            Color sColor = seedColor;
            sColor.a = 1f;
            image.color = sColor;
        }

        while (elapsed < growBackDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, elapsed / growBackDuration);
            yield return null;
        }

        transform.localScale = originalScale;
        isAnimating = false;
        UpdateVisual();
    }
}