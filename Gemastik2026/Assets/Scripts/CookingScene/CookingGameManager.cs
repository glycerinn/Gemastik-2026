using UnityEngine;
using UnityEngine.UI;

public class CookingGameManager : MonoBehaviour
{
    public PlateManager plateManager;
    public Button submitButton;
    public StudentManager studentManager;
    public FoodGenerator foodGenerator;

    private void Start()
    {
        submitButton.gameObject.SetActive(false);
        submitButton.onClick.AddListener(SubmitMeal);
    }

    public void CheckPlateFilled()
    {
        submitButton.gameObject.SetActive(plateManager.IsPlateFull());
    }

    void SubmitMeal()
    {
        plateManager.CalculatePlate();

        CheckMeal();

        plateManager.ClearPlate();

        foodGenerator.ResetChoices();

        studentManager.NextStudent();

        submitButton.gameObject.SetActive(false);
    }

    void CheckMeal()
    {
        StudentSO student = studentManager.CurrentStudent;

        NutritionTarget target = NutritionTargets.GetTarget(student.nutritionProblem);

        bool percentagesCorrect =
            Mathf.Approximately(plateManager.carbPercent, target.carbs) &&
            Mathf.Approximately(plateManager.proteinPercent, target.protein) &&
            Mathf.Approximately(plateManager.fatPercent, target.fat);

       bool favoritesCorrect =
            plateManager.AllFoodsAreFavorites(student);

        if (!percentagesCorrect)
            Debug.Log("Mistake: Wrong nutrition percentages.");

        if (!favoritesCorrect)
            Debug.Log("Mistake: Student doesn't like one or more foods.");

        if (percentagesCorrect && favoritesCorrect)
            Debug.Log("Correct meal!");
    }
}