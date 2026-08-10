using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    public enum State { Idle, Waiting, Running, Result }

    [Header("UI References")]
    public RectTransform trackArea;
    public RectTransform successArea;
    public RectTransform marker;
    public TextMeshProUGUI attemptText;
    public GameObject completedPanel;

    [Header("Input")]
    public InputActionReference stopAction;

    [Header("Settings")]
    public int totalAttempts = 30;
    private int currentAttempt = 0;
    public Vector2 biteWaitRange = new Vector2(0.75f, 1.25f);
    public bool randomizeZone = true;
    public Vector2 zoneCenterClamp = new Vector2(0.15f, 0.85f);
    public bool autoStartonEnable = true;

    [Header("Dynamic Difficulty")]
    public float baseSpeed = 1.5f;
    public float speedIncreasePerCatch = 0.25f;
    public float maxSpeed = 4.0f;

    public Vector2 baseZoneSizeRange = new Vector2(0.18f, 0.32f);
    public float zoneShrinkPerCatch = 0.015f;
    public float minZoneSize = 0.08f;

    private float currentSpeed;
    private Vector2 currentZoneSizeRange;
    private int consecutiveCatches = 0;

    [Header("Polish Effects")]
    public Image markerImage;
    public Color normalColor = Color.white;
    public Color successColor = new Color(0.2f, 0.8f, 0.2f); // Hijau
    public Color missColor = new Color(0.8f, 0.2f, 0.2f); // Merah

    public UnityEvent onCatch;
    public UnityEvent onMiss;

    private State state = State.Idle;
    private float t;
    private int dir = 1;
    private float biteTimer;

    private Vector3 originalMarkerScale;
    private Vector2 originalTrackPos;

    private void Awake()
    {
        if (marker) originalMarkerScale = marker.localScale;
        if (trackArea) originalTrackPos = trackArea.anchoredPosition;
        ResetDifficulty();
    }

    private void OnEnable()
    {
        if (stopAction != null && stopAction.action != null)
        {
            stopAction.action.performed += OnStopPerformed;
            stopAction.action.Enable();
        }

        if (autoStartonEnable) StartFishing();
    }

    private void OnDisable()
    {
        if (stopAction != null && stopAction.action != null)
        {
            stopAction.action.performed -= OnStopPerformed;
            stopAction.action.Disable();
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Waiting:
                biteTimer -= Time.deltaTime;
                if (biteTimer <= 0f)
                {
                    state = State.Running;
                }
                break;
            case State.Running:
                UpdateMarker();
                break;
        }
    }

    private void OnStopPerformed(InputAction.CallbackContext ctx)
    {
        if (state == State.Running)
        {
            Evaluate();
        }
    }

    public void StartFishing()
    {
        if (currentAttempt >= totalAttempts)
            return;

        if (!ValidateRefs()) return;

        if (markerImage) markerImage.color = normalColor;
        marker.localScale = originalMarkerScale;

        // KUNCI PERBAIKAN: Kembalikan posisi X marker ke tengah jalur (0) agar tidak melompat/bergeser
        var markerPos = marker.anchoredPosition;
        markerPos.x = 0f;
        marker.anchoredPosition = markerPos;

        if (randomizeZone) RandomizeZone();

        biteTimer = Random.Range(biteWaitRange.x, biteWaitRange.y);
        state = State.Waiting;
    }

    public void CancelFishing()
    {
        state = State.Idle;
    }

    public void UpdateMarker()
    {
        t += dir * currentSpeed * Time.deltaTime;

        if (t >= 1f) { t = 1f; dir = -1; }
        else if (t < 0f) { t = 0f; dir = 1; }

        ApplyMarkerPosition();
    }

    public void ApplyMarkerPosition()
    {
        if (!trackArea || !marker) return;
        float y = Mathf.Lerp(GetTrackBottom(), GetTrackTop(), t);
        var pos = marker.anchoredPosition;
        pos.y = y;
        marker.anchoredPosition = pos;
    }

    private void Evaluate()
    {
        bool isSuccess = IsMarkerInsideZone();

        if (isSuccess)
        {
            currentAttempt++;
            consecutiveCatches++;
            UpdateDifficulty();

            if (attemptText)
                attemptText.text = $"Fish: {currentAttempt}/{totalAttempts}";

            onCatch?.Invoke();

            // JALUR SUKSES: Langsung mainkan efek visual secara independen dan LANJUT INSTAN
            StartCoroutine(CatchPolishRoutine());

            if (currentAttempt < totalAttempts)
            {
                StartFishing(); // Langsung lanjut tanpa jeda menunggu!
            }
            else
            {
                Debug.Log("Fishing complete!");
                state = State.Result;
                if (completedPanel) completedPanel.SetActive(true);
            }
        }
        else
        {
            // JALUR GAGAL: Ubah state ke Result (menghentikan game sejenak) dan reset kesulitan
            state = State.Result;
            ResetDifficulty();
            onMiss?.Invoke();

            StartCoroutine(MissPolishRoutine());
        }
    }

    private void UpdateDifficulty()
    {
        currentSpeed = Mathf.Clamp(baseSpeed + (consecutiveCatches * speedIncreasePerCatch), baseSpeed, maxSpeed);

        float shrinkAmount = consecutiveCatches * zoneShrinkPerCatch;
        currentZoneSizeRange.x = Mathf.Max(baseZoneSizeRange.x - shrinkAmount, minZoneSize);
        currentZoneSizeRange.y = Mathf.Max(baseZoneSizeRange.y - shrinkAmount, minZoneSize);
    }

    private void ResetDifficulty()
    {
        consecutiveCatches = 0;
        currentSpeed = baseSpeed;
        currentZoneSizeRange = baseZoneSizeRange;
    }

    // Efek Sukses: Berjalan mulus di latar belakang tanpa mengunci permainan
    private IEnumerator CatchPolishRoutine()
    {
        if (markerImage) markerImage.color = successColor;

        float duration = 0.35f;
        float elapsed = 0f;
        Vector3 targetScale = originalMarkerScale * 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / duration;

            float scaleProgress = Mathf.Sin(p * Mathf.PI);
            marker.localScale = Vector3.Lerp(originalMarkerScale, targetScale, scaleProgress);

            float shakeIntensity = (1f - p) * 4f;
            trackArea.anchoredPosition = originalTrackPos + new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));

            yield return null;
        }

        marker.localScale = originalMarkerScale;
        trackArea.anchoredPosition = originalTrackPos;
        if (markerImage) markerImage.color = normalColor;
    }

    // Efek Gagal: Memberikan jeda waktu berhenti sebelum ikan berikutnya dimulai
    private IEnumerator MissPolishRoutine()
    {
        if (markerImage) markerImage.color = missColor;

        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 targetScale = originalMarkerScale * 0.7f;

        // Simpan posisi X awal marker sebelum digetarkan
        float defaultX = marker.anchoredPosition.x;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / duration;

            marker.localScale = Vector3.Lerp(originalMarkerScale, targetScale, p);

            // Efek getar kecil pada sumbu X
            float shakeIntensity = 2f;
            var pos = marker.anchoredPosition;
            pos.x = defaultX + Random.Range(-shakeIntensity, shakeIntensity);
            marker.anchoredPosition = pos;

            yield return null;
        }

        // Kembalikan posisi X ke normal sebelum jeda/restart
        var resetPos = marker.anchoredPosition;
        resetPos.x = defaultX;
        marker.anchoredPosition = resetPos;

        // Jeda waktu berhenti sejenak saat pemain gagal
        yield return new WaitForSeconds(0.4f);

        if (currentAttempt < totalAttempts)
        {
            StartFishing();
        }
        else
        {
            Debug.Log("Fishing complete!");
            if (completedPanel) completedPanel.SetActive(true);
        }
    }

    private bool IsMarkerInsideZone()
    {
        if (!successArea || !marker) return false;

        float markerY = marker.anchoredPosition.y;
        float zoneHalf = successArea.rect.height * 0.5f;
        float zoneCenter = successArea.anchoredPosition.y;
        float zoneMin = zoneCenter - zoneHalf;
        float zoneMax = zoneCenter + zoneHalf;

        return markerY >= zoneMin && markerY <= zoneMax;
    }

    private void RandomizeZone()
    {
        if (!trackArea || !successArea) return;

        float trackH = trackArea.rect.height;
        float zoneFrac = Random.Range(currentZoneSizeRange.x, currentZoneSizeRange.y);
        float zoneH = Mathf.Clamp(zoneFrac, minZoneSize, 0.9f) * trackH;

        float minCenter = Mathf.Lerp(GetTrackBottom(), GetTrackTop(), zoneCenterClamp.x);
        float maxCenter = Mathf.Lerp(GetTrackBottom(), GetTrackTop(), zoneCenterClamp.y);
        float centerY = Random.Range(minCenter, maxCenter);

        var size = successArea.sizeDelta; size.y = zoneH; successArea.sizeDelta = size;

        var pos = successArea.anchoredPosition;
        pos.y = Mathf.Clamp(centerY, GetTrackBottom() + zoneH * 0.05f, GetTrackTop() - zoneH * 0.5f);
        successArea.anchoredPosition = pos;
    }

    private float GetTrackBottom() => -trackArea.rect.height * 0.5f;
    private float GetTrackTop() => trackArea.rect.height * 0.5f;

    private bool ValidateRefs()
    {
        if (!trackArea || !marker || !successArea)
        {
            Debug.LogError("MissingRefs");
            return false;
        }
        return true;
    }

    public void PressStop() => OnStopPerformed(default);
    public void Retry() => StartFishing();
}