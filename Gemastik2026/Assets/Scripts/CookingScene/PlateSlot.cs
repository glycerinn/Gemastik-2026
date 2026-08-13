using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlateSlot : MonoBehaviour, IDropHandler
{
    [Range(0, 100)]
    public float slotPercentage;

    public DraggableItem currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item =
            eventData.pointerDrag.GetComponent<DraggableItem>();

        if (item == null)
            return;

        if (currentItem != null)
            return;

        // Put item into this plate slot
        item.transform.SetParent(transform, false);

        RectTransform rt = item.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;

        currentItem = item;

        // Mark choice as used
        if (item.homeSlot != null)
        {
            item.homeSlot.hasBeenChosen = true;
        }

        // Change raw sprite to plated sprite
        Image itemImage = item.GetComponent<Image>();

        if (itemImage != null && item.food != null)
        {
            itemImage.sprite = item.food.platedIcon;
        }

        // Update plate
        FindFirstObjectByType<PlateManager>().CalculatePlate();
        FindFirstObjectByType<CookingGameManager>().CheckPlateFilled();
    }
}