using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOutline : MonoBehaviour
{
    public GameObject door;
    public GameObject doorLight;
    public Material normalMaterial;
    public Material outlineMaterial;
    private SpriteRenderer sr;
    private bool playerInRange;
    public string sceneName;

    [Header("Pengaturan Fade Transisi")]
    public float fadeDuration = 0.8f; // Sesuaikan dengan durasi fade di FadeManager

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.material = normalMaterial;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            // Matikan visual pintu jika diinginkan
            if (door != null) door.SetActive(false);

            // Jalankan transisi pindah scene via FadeManager
            StartCoroutine(LoadSceneWithFade(sceneName));
        }
    }

    private IEnumerator LoadSceneWithFade(string targetSceneName)
    {
        // 1. Cek apakah FadeManager tersedia di Scene
        if (FadeManager.Instance != null)
        {
            // Panggil proses fade hitam (kita buat coroutine custom khusus pindah scene)
            yield return StartCoroutine(FadeOutAndLoad(targetSceneName));
        }
        else
        {
            // Fallback jika FadeManager tidak ada: langsung load scene biasa
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private IEnumerator FadeOutAndLoad(string targetSceneName)
    {
        // Memanfaatkan CanvasGroup yang ada di FadeManager untuk menggelapkan layar
        CanvasGroup canvasGroup = FadeManager.Instance.fadeCanvasGroup;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            // Efek layar perlahan menjadi hitam
            while (time < fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                float progress = time / fadeDuration;
                canvasGroup.alpha = Mathf.SmoothStep(startAlpha, 1f, progress);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        // 2. Setelah layar benar-benar hitam, pindah scene
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            sr.material = outlineMaterial;
            if (doorLight != null) doorLight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            sr.material = normalMaterial;
            if (doorLight != null) doorLight.SetActive(false);
            if (door != null) door.SetActive(true);
        }
    }
}