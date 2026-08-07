using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; // Wajib untuk Unity 6

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Pengaturan Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.8f;
    public float holdBlackDuration = 0.3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void TeleportWithFade(Transform player, Vector3 targetPosition)
    {
        StartCoroutine(FadeAndTeleportRoutine(player, targetPosition));
    }

    private IEnumerator FadeAndTeleportRoutine(Transform player, Vector3 targetPosition)
    {
        // 1. Layar perlahan hitam
        yield return StartCoroutine(FadeRoutine(1f));

        // Simpan posisi lama untuk menghitung selisih jarak
        Vector3 oldPosition = player.position;

        // 2. Reset kecepatan fisika player (Unity 6 API)
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d != null) rb2d.linearVelocity = Vector2.zero;

        Rigidbody rb3d = player.GetComponent<Rigidbody>();
        if (rb3d != null) rb3d.linearVelocity = Vector3.zero;

        // 3. Pindahkan posisi Player ke titik tujuan
        player.position = targetPosition;

        // 4. Warp Kamera Cinemachine v3 (Unity 6) dengan Casting
        if (Camera.main != null)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null && brain.ActiveVirtualCamera != null)
            {
                // PERBAIKAN: Ubah/Cast ICinemachineCamera menjadi CinemachineCamera
                var cinemachineCam = brain.ActiveVirtualCamera as CinemachineCamera;

                if (cinemachineCam != null)
                {
                    cinemachineCam.OnTargetObjectWarped(player, targetPosition - oldPosition);
                }
            }
        }

        // 5. Tahan sebentar saat layar HITAM PEKAT
        yield return new WaitForSecondsRealtime(holdBlackDuration);

        // 6. Layar perlahan terang kembali
        yield return StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.blocksRaycasts = true;

        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            float progress = time / fadeDuration;
            fadeCanvasGroup.alpha = Mathf.SmoothStep(startAlpha, targetAlpha, progress);

            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}