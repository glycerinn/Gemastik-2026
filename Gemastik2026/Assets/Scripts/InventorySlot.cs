using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public void SetFood(FoodSO food)
    {
        icon.sprite = food.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
    }
}