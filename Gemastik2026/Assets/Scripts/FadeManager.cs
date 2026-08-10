using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Pengaturan Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.5f; // Durasi animasi fade (detik)
    public float holdBlackDuration = 0.2f; // Waktu tahan hitam saat loading

    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // PASTI KAN Canvas & FadeManager ikut abadi berpindah scene
            DontDestroyOnLoad(transform.root.gameObject);

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.gameObject.SetActive(true);
                fadeCanvasGroup.alpha = 1f;
                fadeCanvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            // Hapus duplikat jika masuk ke scene yang tidak sengaja dipasangi FadeManager
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
        // Fade In dari hitam saat game pertama kali dijalankan
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1f;
            StartFade(0f);
        }
    }

    // Fungsi ini OTOMATIS berjalan setiap kali scene baru selesai dimuat
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeCanvasGroup == null)
        {
            GameObject canvasObj = GameObject.FindWithTag("FadeCanvas");
            if (canvasObj != null)
            {
                fadeCanvasGroup = canvasObj.GetComponent<CanvasGroup>();
            }
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1f; // Mulai dari hitam pekat
            StartFade(0f);              // Perlahan menjadi terang (Fade In)
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadSceneRoutine(sceneName));
    }

    private IEnumerator FadeAndLoadSceneRoutine(string sceneName)
    {
        // 1. Fade Out ke hitam
        yield return StartCoroutine(FadeRoutine(1f));

        // 2. Tahan sebentar di layar hitam
        yield return new WaitForSecondsRealtime(holdBlackDuration);

        // 3. Pindah Scene (OnSceneLoaded akan mengambil alih Fade In setelah scene terbuka)
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"[FadeManager Error] Scene '{sceneName}' tidak ditemukan di Build Settings!");
        }
    }

    // Fungsi pembantu animasi Fade
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

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.blocksRaycasts = true;

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