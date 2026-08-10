using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ChoiceSlot homeSlot;
    public FoodSO food;

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (canvas == null)
            Debug.LogError("No Canvas found!");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // [KODE BARU] Cek apakah item ini diangkat dari piring (PlateSlot)
        PlateSlot previousSlot = originalParent.GetComponent<PlateSlot>();
        if (previousSlot != null)
        {
            // Kosongkan data di slot piring lama karena makanannya sedang diangkat
            previousSlot.currentItem = null;

            // Perbarui perhitungan nilai nutrisi dan tombol Submit secara real-time
            FindFirstObjectByType<PlateManager>().CalculatePlate();
            FindFirstObjectByType<CookingGameManager>().CheckPlateFilled();
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Jika item dilepas di tempat yang BUKAN kotak piring yang kosong (masih ngambang di UI)
        if (transform.parent == canvas.transform)
        {
            // Kembalikan ke tempat asalnya semula
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;

            // [KODE BARU] Jika asalnya dari piring, pulihkan kembali datanya ke piring tersebut
            PlateSlot previousSlot = originalParent.GetComponent<PlateSlot>();
            if (previousSlot != null)
            {
                previousSlot.currentItem = this;

                // Perbarui lagi kalkulasi nutrisi karena makanan dikembalikan
                FindFirstObjectByType<PlateManager>().CalculatePlate();
                FindFirstObjectByType<CookingGameManager>().CheckPlateFilled();
            }
        }
    }
}