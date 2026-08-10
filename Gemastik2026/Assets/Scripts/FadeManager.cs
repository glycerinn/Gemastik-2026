using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Pengaturan Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 2.0f; // Menggunakan durasi 2 detik sesuai keinginan Anda
    public float holdBlackDuration = 0.3f;

    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // FITUR UTAMA: Saat game pertama kali dinyalakan, 
            // kunci layar ke warna hitam pekat (alpha = 1) sebelum frame pertama digambar
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 1f;
                fadeCanvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Saat game pertama kali dijalankan (Main Menu), 
        // jalankan Fade In dari Hitam (1) ke Terang (0)
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StartFade(0f);
        }
    }

    // Fungsi ini otomatis terpanggil SETIAP KALI pindah scene berhasil dilakukan
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari CanvasGroup otomatis jika belum terhubung
        if (fadeCanvasGroup == null)
        {
            GameObject canvasObj = GameObject.FindWithTag("FadeCanvas");
            if (canvasObj != null)
            {
                fadeCanvasGroup = canvasObj.GetComponent<CanvasGroup>();
            }
        }

        // Jalankan Fade In setiap kali berpindah ke scene baru
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f; // Pastikan layar hitam pekat terlebih dahulu
            StartFade(0f);              // Perlahan menjadi terang
        }
    }

    public void TeleportWithFade(Transform player, Vector3 targetPosition)
    {
        StartCoroutine(FadeAndTeleportRoutine(player, targetPosition));
    }

    private IEnumerator FadeAndTeleportRoutine(Transform player, Vector3 targetPosition)
    {
        yield return StartCoroutine(FadeRoutine(1f));

        Vector3 oldPosition = player.position;

        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d != null) rb2d.linearVelocity = Vector2.zero;

        Rigidbody rb3d = player.GetComponent<Rigidbody>();
        if (rb3d != null) rb3d.linearVelocity = Vector3.zero;

        player.position = targetPosition;

        if (Camera.main != null)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null && brain.ActiveVirtualCamera != null)
            {
                var cinemachineCam = brain.ActiveVirtualCamera as CinemachineCamera;
                if (cinemachineCam != null)
                {
                    cinemachineCam.OnTargetObjectWarped(player, targetPosition - oldPosition);
                }
            }
        }

        yield return new WaitForSecondsRealtime(holdBlackDuration);
        yield return StartCoroutine(FadeRoutine(0f));
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadSceneRoutine(sceneName));
    }

    private IEnumerator FadeAndLoadSceneRoutine(string sceneName)
    {
        // 1. Fade Out ke hitam
        yield return StartCoroutine(FadeRoutine(1f));

        // 2. Beri jeda sejenak saat hitam pekat
        yield return new WaitForSecondsRealtime(holdBlackDuration);

        // 3. Pindah Scene (OnSceneLoaded akan otomatis mengambil alih setelah ini)
        SceneManager.LoadScene(sceneName);
    }

    // Pembantu agar Coroutine fade tidak bentrok/saling tumpuk
    private void StartFade(float targetAlpha)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.blocksRaycasts = true;

        // Tunggu 1 frame agar delta time setelah loading stabil
        yield return null;

        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(time / fadeDuration);
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