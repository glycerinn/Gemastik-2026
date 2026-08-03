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

    private int successfulMeals;
    private int failedMeals;

    private void Start()
    {
        submitButton.SetActive(false);
    }

    public void CheckPlateFilled()
    {
        foreach (PlateSlot slot in plateManager.plateSlots)
        {
            if (slot.currentItem == null)
            {
                submitButton.SetActive(false);
                return;
            }
        }

        submitButton.SetActive(true);
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
        submitButton.SetActive(false);

        // Continue to the next student or finish the day
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

        NutritionTarget target =
            NutritionTargets.GetTarget(student.nutritionProblem);

        bool percentagesCorrect =
            Mathf.Approximately(plateManager.carbPercent, target.carbs) &&
            Mathf.Approximately(plateManager.proteinPercent, target.protein) &&
            Mathf.Approximately(plateManager.fatPercent, target.fat);

        bool favoritesCorrect =
            plateManager.AllFoodsAreFavorites(student);

        if (!percentagesCorrect)
            Debug.Log("Wrong nutrition percentages.");

        if (!favoritesCorrect)
            Debug.Log("Contains food the student dislikes.");

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