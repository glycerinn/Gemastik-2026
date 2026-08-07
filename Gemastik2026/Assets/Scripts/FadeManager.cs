using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

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
            return;
        }
    }

    private void OnEnable()
    {
        // Mendaftarkan event setiap kali scene baru selesai dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Membersihkan event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Saat game pertama kali dinyalakan, pastikan layar langsung terang
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    // Fungsi ini otomatis terpanggil SETIAP KALI pindah scene berhasil dilakukan
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari otomatis CanvasGroup jika hilang / belum terhubung di scene baru
        if (fadeCanvasGroup == null)
        {
            GameObject canvasObj = GameObject.FindWithTag("FadeCanvas"); // Pastikan Canvas Anda ber-tag "FadeCanvas" atau dicari manual
            if (canvasObj != null)
            {
                fadeCanvasGroup = canvasObj.GetComponent<CanvasGroup>();
            }
        }

        // Jika CanvasGroup ada, set layar ke posisi hitam pekat (1f) lalu lakukan Fade In (menuju 0f)
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StartCoroutine(FadeRoutine(0f));
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

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadSceneRoutine(sceneName));
    }

    private IEnumerator FadeAndLoadSceneRoutine(string sceneName)
    {
        // 1. Layar perlahan jadi hitam (Fade Out)
        yield return StartCoroutine(FadeRoutine(1f));

        // 2. Berikan jeda singkat opsional agar layar benar-benar menahan warna hitam sejenak
        yield return new WaitForSecondsRealtime(0.2f);

        // 3. Setelah layar benar-benar gelap gulita, baru pindah scene
        SceneManager.LoadScene(sceneName);

        // (Selanjutnya event OnSceneLoaded akan otomatis mengambil alih untuk melakukan Fade In di scene baru)
    }
}