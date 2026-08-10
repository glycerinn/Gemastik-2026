using System.Collections;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    public PlateSlot[] plateSlots;

    public float carbPercent;
    public float proteinPercent;
    public float fatPercent;

    [Header("Animasi Transisi")]
    public float transitionDuration = 0.25f; // Kecepatan makanan menghilang dari piring

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

            // Alih-alih Destroy instan, kita panggil animasi pudar
            StartCoroutine(FadeOutAndDestroy(slot.currentItem.gameObject));
            slot.currentItem = null; // Kosongkan data slot agar logika game bisa langsung lanjut
        }
    }

    // Coroutine untuk membuat makanan mengecil & memudar sebelum dihancurkan
    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();

        cg.blocksRaycasts = false; // Mencegah makanan diklik saat sedang menghilang

        float time = 0;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float progress = time / transitionDuration;

            if (rt != null) rt.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);
            if (cg != null) cg.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        Destroy(obj);
    }
}