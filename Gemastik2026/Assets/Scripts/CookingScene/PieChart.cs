using System.Collections;
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

    private Coroutine chartCoroutine;

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

        carbText.text = $"C{target.carbs}%";
        proteinText.text = $"P{target.protein}%";
        fatText.text = $"F{target.fat}%";

        // Jalankan animasi pergerakan label teks
        if (chartCoroutine != null) StopCoroutine(chartCoroutine);
        chartCoroutine = StartCoroutine(MoveLabelsRoutine(target));
    }

    private IEnumerator MoveLabelsRoutine(NutritionTarget target)
    {
        Vector2 startCarbPos = carbText.rectTransform.anchoredPosition;
        Vector2 startProteinPos = proteinText.rectTransform.anchoredPosition;
        Vector2 startFatPos = fatText.rectTransform.anchoredPosition;

        Vector2 targetCarbPos = GetLabelPosition(0, target.carbs * 3.6f, 70f);
        Vector2 targetProteinPos = GetLabelPosition(target.carbs * 3.6f, target.protein * 3.6f, 70f);
        Vector2 targetFatPos = GetLabelPosition((target.carbs + target.protein) * 3.6f, target.fat * 3.6f, 70f);

        float time = 0;
        float duration = 0.5f; // Samakan dengan durasi PieSlice

        while (time < duration)
        {
            time += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, time / duration);

            carbText.rectTransform.anchoredPosition = Vector2.Lerp(startCarbPos, targetCarbPos, p);
            proteinText.rectTransform.anchoredPosition = Vector2.Lerp(startProteinPos, targetProteinPos, p);
            fatText.rectTransform.anchoredPosition = Vector2.Lerp(startFatPos, targetFatPos, p);

            yield return null;
        }
    }

    private Vector2 GetLabelPosition(float startAngle, float sweepAngle, float radius)
    {
        float angle = -(startAngle + sweepAngle / 2f) - 90f;
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * radius;
    }
}