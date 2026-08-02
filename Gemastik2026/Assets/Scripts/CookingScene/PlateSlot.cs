using UnityEngine;
using UnityEngine.EventSystems;

public class PlateSlot : MonoBehaviour, IDropHandler
{
    [Range(0, 100)]
    public float slotPercentage;

    public DraggableItem currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (item == null)
            return;

        if (currentItem != null)
            return;

        item.transform.SetParent(transform, false);

        RectTransform rt = item.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        currentItem = item;

        item.homeSlot.hasBeenChosen = true;

        FindFirstObjectByType<PlateManager>().CalculatePlate();
        FindFirstObjectByType<CookingGameManager>().CheckPlateFilled();
    }
}