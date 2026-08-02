using UnityEngine;

public class PlateManager : MonoBehaviour
{
    public PlateSlot[] plateSlots;

    public float carbPercent;
    public float proteinPercent;
    public float fatPercent;

    public void CalculatePlate()
    {
        carbPercent = 0;
        proteinPercent = 0;
        fatPercent = 0;

        foreach (PlateSlot slot in plateSlots)
        {
            if (slot.currentItem == null)
                continue;

            switch (slot.currentItem.food.category)
            {
                case FoodCategory.Carb:
                    carbPercent += slot.slotPercentage;
                    break;

                case FoodCategory.Protein:
                    proteinPercent += slot.slotPercentage;
                    break;

                case FoodCategory.Fat:
                    fatPercent += slot.slotPercentage;
                    break;
            }
        }

        Debug.Log($"Carbs: {carbPercent}%");
        Debug.Log($"Protein: {proteinPercent}%");
        Debug.Log($"Fat: {fatPercent}%");
    }

    public bool IsPlateFull()
    {
        foreach (PlateSlot slot in plateSlots)
        {
            if (slot.currentItem == null)
                return false;
        }

        return true;
    }

    public bool AllFoodsAreFavorites(StudentSO student)
    {
        foreach (PlateSlot slot in plateSlots)
        {
            FoodSO food = slot.currentItem.food;

            bool favorite =
                student.favoriteCarbs.Contains(food) ||
                student.favoriteProteins.Contains(food) ||
                student.favoriteFats.Contains(food);

            if (!favorite)
                return false;
        }

        return true;
    }

    public void ClearPlate()
    {
        foreach (PlateSlot slot in plateSlots)
        {
            if (slot.currentItem == null)
                continue;

            Destroy(slot.currentItem.gameObject);
            slot.currentItem = null;
        }
    }
}