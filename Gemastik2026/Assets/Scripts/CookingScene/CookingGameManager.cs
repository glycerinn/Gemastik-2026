using System.Collections;
using UnityEngine;

public class CookingGameManager : MonoBehaviour
{
    [Header("Managers")]
    public PlateManager plateManager;
    public StudentManager studentManager;
    public FoodGenerator foodGenerator;
    public ResultUI resultsUI;

    [Header("UI")]
    public GameObject submitButton;
    private CanvasGroup submitCanvasGroup;
    private Coroutine submitFadeCoroutine;
    private bool isSubmitVisible = false;

    private int successfulMeals;
    private int failedMeals;

    private void Awake()
    {
        submitCanvasGroup = submitButton.GetComponent<CanvasGroup>();
        if (submitCanvasGroup == null) submitCanvasGroup = submitButton.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        submitCanvasGroup.alpha = 0f;
        submitCanvasGroup.interactable = false;
        submitCanvasGroup.blocksRaycasts = false;
        isSubmitVisible = false;
    }

    public void CheckPlateFilled()
    {
        bool isFull = true;
        foreach (PlateSlot slot in plateManager.plateSlots)
        {
            if (slot.currentItem == null)
            {
                isFull = false;
                break;
            }
        }

        if (isFull && !isSubmitVisible)
        {
            isSubmitVisible = true;
            FadeSubmitButton(1f, true);
        }
        else if (!isFull && isSubmitVisible)
        {
            isSubmitVisible = false;
            FadeSubmitButton(0f, false);
        }
    }

    private void FadeSubmitButton(float targetAlpha, bool interactable)
    {
        if (submitFadeCoroutine != null) StopCoroutine(submitFadeCoroutine);
        submitFadeCoroutine = StartCoroutine(FadeSubmitRoutine(targetAlpha, interactable));
    }

    private IEnumerator FadeSubmitRoutine(float targetAlpha, bool interactable)
    {
        float startAlpha = submitCanvasGroup.alpha;
        float time = 0;
        float duration = 0.3f;

        if (interactable) submitCanvasGroup.blocksRaycasts = true;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            submitCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

            float scale = Mathf.Lerp(startAlpha == 0 ? 0.8f : 1f, targetAlpha == 1 ? 1f : 0.8f, progress);
            submitButton.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        submitCanvasGroup.alpha = targetAlpha;
        submitButton.transform.localScale = new Vector3(targetAlpha == 1 ? 1f : 0.8f, targetAlpha == 1 ? 1f : 0.8f, 1f);
        submitCanvasGroup.interactable = interactable;

        if (!interactable) submitCanvasGroup.blocksRaycasts = false;
    }

    public void SubmitMeal()
    {
        plateManager.CalculatePlate();
        bool success = CheckMeal();

        if (success)
            successfulMeals++;
        else
            failedMeals++;

        plateManager.ClearPlate();
        foodGenerator.ResetChoices();

        isSubmitVisible = false;
        FadeSubmitButton(0f, false);

        if (studentManager.HasMoreStudents())
        {
            studentManager.NextStudent();
        }
        else
        {
            resultsUI.ShowResults(successfulMeals, failedMeals);
        }
    }

    private bool CheckMeal()
    {
        StudentSO student = studentManager.CurrentStudent;
        NutritionTarget target = NutritionTargets.GetTarget(student.nutritionProblem);

        bool percentagesCorrect =
            Mathf.Approximately(plateManager.carbPercent, target.carbs) &&
            Mathf.Approximately(plateManager.proteinPercent, target.protein) &&
            Mathf.Approximately(plateManager.fatPercent, target.fat);

        bool favoritesCorrect = plateManager.AllFoodsAreFavorites(student);

        if (!percentagesCorrect) Debug.Log("Wrong nutrition percentages.");
        if (!favoritesCorrect) Debug.Log("Contains food the student dislikes.");

        bool success = percentagesCorrect && favoritesCorrect;
        Debug.Log(success ? "Meal Successful!" : "Meal Failed!");
        return success;
    }

    public int GetSuccessfulMeals()
    {
        return successfulMeals;
    }

    public int GetFailedMeals()
    {
        return failedMeals;
    }
}