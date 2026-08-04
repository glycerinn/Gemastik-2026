using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventorySlot[] slots;

    public static InventoryUI Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        foreach (InventorySlot slot in slots)
            slot.Clear();

        for (int i = 0; i < TownGameManager.Instance.collectedIngredients.Count; i++)
        {
            if (i >= slots.Length)
                break;

            slots[i].SetFood(TownGameManager.Instance.collectedIngredients[i]);
        }
    }
}