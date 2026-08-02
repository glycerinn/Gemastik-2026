using TMPro;
using UnityEngine;

public class PieChart : MonoBehaviour
{
    public PieSlice carbSlice;
    public PieSlice proteinSlice;
    public PieSlice fatSlice;
    public TMP_Text carbText;
    public TMP_Text proteinText;
    public TMP_Text fatText;

    public void SetChart(NutritionTarget target)
    {
        float carbs = target.carbs;
        float protein = target.protein;
        float fat = target.fat;

        float total = carbs + protein + fat;

        carbs /= total;
        protein /= total;
        fat /= total;

        float angle = 0;

        carbSlice.SetSlice(carbs, angle);
        angle -= carbs * 360f;

        proteinSlice.SetSlice(protein, angle);
        angle -= protein * 360f;

        fatSlice.SetSlice(fat, angle);

        float start = 0;
        PositionLabel(carbText.rectTransform, start, target.carbs * 3.6f, 70f);

        start += target.carbs * 3.6f;
        PositionLabel(proteinText.rectTransform, start, target.protein * 3.6f, 70f);

        start += target.protein * 3.6f;
        PositionLabel(fatText.rectTransform, start, target.fat * 3.6f, 70f);

        carbText.text = $"C{target.carbs}%";
        proteinText.text = $"P{target.protein}%";
        fatText.text = $"F{target.fat}%";
    }

    void PositionLabel(RectTransform label, float startAngle, float sweepAngle, float radius)
    {
        float angle = -(startAngle + sweepAngle / 2f) - 90f;

        float radians = angle * Mathf.Deg2Rad;

        Vector2 pos = new Vector2(
            Mathf.Cos(radians),
            Mathf.Sin(radians)
        ) * radius;

        label.anchoredPosition = pos;
        label.localRotation = Quaternion.identity;
    }
}