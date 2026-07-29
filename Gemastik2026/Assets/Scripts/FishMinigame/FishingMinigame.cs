using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    public enum State{Idle, Waiting, Running, Result}

    public RectTransform trackArea;
    public RectTransform successArea;
    public RectTransform marker;
    public TextMeshProUGUI attemptText;
    public GameObject completedPanel;

    public InputActionReference stopAction;

    public int totalAttempts = 30;
    private int currentAttempt = 0;
    
    public float speed = 1.5f;
    public Vector2 biteWaitRange = new Vector2(0.75f, 1.25f);
    public bool randomizeZone = true;
    public Vector2 zoneSizeRange = new Vector2(0.18f, 0.32f);
    public Vector2 zoneCenterClamp = new Vector2(0.15f, 0.85f);
    public bool autoStartonEnable = true;

    public UnityEvent onCatch;
    public UnityEvent onMiss;

    private State state = State.Idle;
    private float t;
    private int dir = 1;
    private float biteTimer;

    private void OnEnable()
    {
        if(stopAction != null && stopAction.action != null)
        {
            stopAction.action.performed += OnStopPerformed;
            stopAction.action.Enable();
        }

        if(autoStartonEnable) StartFishing();
    }

    private void OnDisable()
    {
        if(stopAction != null && stopAction.action != null)
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
                if(biteTimer <= 0f)
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
        if(state == State.Running)
        {
            Evaluate();
        }
    }


    public void StartFishing()
    {
        if (currentAttempt >= totalAttempts)
            return;

        if(!ValidateRefs()) return;

        if(randomizeZone) RandomizeZone();

        biteTimer = Random.Range(biteWaitRange.x, biteWaitRange.y);
        state = State.Waiting;
    }

    public void CancelFishing()
    {
        state = State.Idle;
    }

    public void UpdateMarker()
    {
        t += dir * speed * Time.deltaTime;

        if(t >= 1f){t = 1f; dir = -1;}
        else if(t < 0f){t = 0f; dir = 1;}

        ApplyMarkerPosition();
    }

    public void ApplyMarkerPosition()
    {
        if(!trackArea || !marker) return;
        float y = Mathf.Lerp(GetTrackBottom(), GetTrackTop(), t);
        var pos = marker.anchoredPosition;
        pos.y = y;
        marker.anchoredPosition = pos;
    }

    private void Evaluate()
    {
        state = State.Result;

        bool isSuccess = IsMarkerInsideZone();
        if (isSuccess)
        {
            currentAttempt++;
            if (attemptText)
                attemptText.text = $"Fish: {currentAttempt}/{totalAttempts}";
            onCatch?.Invoke();
        }
        else
        {
            onMiss?.Invoke();
        }

        if (currentAttempt < totalAttempts)
        {
            StartFishing();
        }
        else
        {
            Debug.Log("Fishing complete!");
            completedPanel.SetActive(true);
        }
    }

    private bool IsMarkerInsideZone()
    {
        if(!successArea||!marker) return false;

        float markerY = marker.anchoredPosition.y;
        float zoneHalf = successArea.rect.height * 0.5f;
        float zoneCenter = successArea.anchoredPosition.y;
        float zoneMin = zoneCenter - zoneHalf;
        float zoneMax = zoneCenter + zoneHalf;

        return markerY >= zoneMin && markerY <= zoneMax;
    }

    private void RandomizeZone()
    {
        if(!trackArea || !successArea) return;

        float trackH = trackArea.rect.height;
        float zoneFrac = Random.Range(zoneSizeRange.x, zoneSizeRange.y);
        float zoneH = Mathf.Clamp(zoneFrac, 0.05f, 0.9f) * trackH;

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
        if(!trackArea || !marker || !successArea)
        {
            Debug.LogError("MissingRefs");
            return false;
        }
        return true;
    }

    public void PressStop() => OnStopPerformed(default);
    public void Retry() => StartFishing();
}
