using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public int targetTrash = 50;
    private int collectedTrash = 0;     // Skor Total
    private int currentStreak = 0;      // Tingkat Kesulitan (Akan reset ke 0 jika jatuh)

    [Header("Dynamic Difficulty Settings")]
    public float baseSpawnInterval = 1.2f;
    public float minSpawnInterval = 0.35f;
    public float speedIncreasePerPoint = 0.06f;
    public float intervalDecreasePerPoint = 0.025f;

    [Header("Miss & Resume Settings")]
    public float pauseDelayAfterMiss = 1.5f;

    [Header("Camera Shake Settings (Ringan)")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 3.0f;

    [Header("UI Panels")]
    public GameObject completedPanel;
    public TextMeshProUGUI counterText;

    [Header("Juicy Effects")]
    public Camera mainCamera;
    public ParticleSystem catchParticlePrefab;

    private bool isHandlingMiss = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Start()
    {
        UpdateCounter();
    }

    // SEKARANG MENGGUNAKAN 'currentStreak', BUKAN SKOR
    public float GetSpeedMultiplier()
    {
        return 1f + (currentStreak * speedIncreasePerPoint);
    }

    // SEKARANG MENGGUNAKAN 'currentStreak', BUKAN SKOR
    public float GetCurrentSpawnInterval()
    {
        float calculatedInterval = baseSpawnInterval - (currentStreak * intervalDecreasePerPoint);
        return Mathf.Max(minSpawnInterval, calculatedInterval);
    }

    public void TrashCollected(Vector3 fruitPosition)
    {
        if (isHandlingMiss) return;

        collectedTrash++;   // Tambah skor total
        currentStreak++;    // Tambah kecepatan (Streak)
        UpdateCounter();

        if (catchParticlePrefab != null)
        {
            ParticleSystem p = Instantiate(catchParticlePrefab, fruitPosition, Quaternion.identity);
            Destroy(p.gameObject, 1f);
        }

        if (collectedTrash >= targetTrash)
        {
            WinGame();
        }
    }

    public void FruitMissed()
    {
        if (isHandlingMiss) return;
        StartCoroutine(HandleMissRoutine());
    }

    IEnumerator HandleMissRoutine()
    {
        isHandlingMiss = true;

        StartCoroutine(ShakeCameraRoutine());

        TrashSpawner spawner = FindFirstObjectByType<TrashSpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
            spawner.ClearAllTrash();
        }

        // HUKUMAN:
        // 1. Skor hanya turun 1 (Pemain tidak frustrasi)
        collectedTrash = Mathf.Max(0, collectedTrash - 1);

        // 2. KESULITAN (Kecepatan) RESET TOTAL KEMBALI KE AWAL!
        currentStreak = 0;

        UpdateCounter();

        yield return new WaitForSeconds(pauseDelayAfterMiss);

        if (spawner != null)
        {
            spawner.StartSpawning();
        }

        isHandlingMiss = false;
    }

    IEnumerator ShakeCameraRoutine()
    {
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = $"{collectedTrash}/{targetTrash}";
    }

    void WinGame()
    {
        TrashSpawner spawner = FindFirstObjectByType<TrashSpawner>();
        if (spawner != null) spawner.StopSpawning();

        Time.timeScale = 0f;
        if (completedPanel != null) completedPanel.SetActive(true);
    }
}