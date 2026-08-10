using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ChoiceSlot homeSlot;
    public FoodSO food;

    private bool isBeingRemoved = false;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (canvas == null)
            Debug.LogError("No Canvas found!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Deteksi Double Click (clickCount == 2)
        if (eventData.clickCount == 2 && !isBeingRemoved)
        {
            PlateSlot parentPlate = transform.parent.GetComponent<PlateSlot>();

            // Fitur ini hanya bekerja jika item berada di atas Piring
            if (parentPlate != null)
            {
                StartCoroutine(RemoveFromPlateRoutine(parentPlate));
            }
        }
    }

    private IEnumerator RemoveFromPlateRoutine(PlateSlot plateSlot)
    {
        isBeingRemoved = true;

        // Kosongkan slot piring & update sistem gizi secara instan
        plateSlot.currentItem = null;

        PlateManager plateMgr = FindFirstObjectByType<PlateManager>();
        if (plateMgr != null) plateMgr.CalculatePlate();

        CookingGameManager gameMgr = FindFirstObjectByType<CookingGameManager>();
        if (gameMgr != null) gameMgr.CheckPlateFilled();

        // Matikan interaksi mouse agar tidak bisa ditarik saat sedang animasi hilang
        canvasGroup.blocksRaycasts = false;

        // Transisi Polish: Memudar & Mengecil
        float time = 0;
        float duration = 0.2f;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isBeingRemoved) return;

        originalParent = transform.parent;

        // Jika diangkat dari piring, bersihkan data slot piring lama
        PlateSlot previousSlot = originalParent.GetComponent<PlateSlot>();
        if (previousSlot != null)
        {
            previousSlot.currentItem = null;

            PlateManager plateMgr = FindFirstObjectByType<PlateManager>();
            if (plateMgr != null) plateMgr.CalculatePlate();

            CookingGameManager gameMgr = FindFirstObjectByType<CookingGameManager>();
            if (gameMgr != null) gameMgr.CheckPlateFilled();
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isBeingRemoved) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isBeingRemoved) return;

        canvasGroup.blocksRaycasts = true;

        // Jika dilepas di luar piring, kembalikan ke tempat asal
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;

            PlateSlot previousSlot = originalParent.GetComponent<PlateSlot>();
            if (previousSlot != null)
            {
                previousSlot.currentItem = this;

                PlateManager plateMgr = FindFirstObjectByType<PlateManager>();
                if (plateMgr != null) plateMgr.CalculatePlate();

                CookingGameManager gameMgr = FindFirstObjectByType<CookingGameManager>();
                if (gameMgr != null) gameMgr.CheckPlateFilled();
            }
        }
    }
}