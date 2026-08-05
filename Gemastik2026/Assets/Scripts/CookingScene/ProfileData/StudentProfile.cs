using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentProfileUI : MonoBehaviour
{
    public Image portrait;

    public TMP_Text studentName;
    public TMP_Text carbText;
    public TMP_Text proteinText;
    public TMP_Text fatText;
    public TMP_Text problemText;

    public PieChart pieChart;

    public void Display(StudentSO student)
    {
        portrait.sprite = student.portrait;
        studentName.text = student.studentName;

        carbText.text = FoodList(student.favoriteCarbs);
        proteinText.text = FoodList(student.favoriteProteins);
        fatText.text = FoodList(student.favoriteFats);

        problemText.text = GetProblemNote(student.nutritionProblem);

        NutritionTarget target = NutritionTargets.GetTarget(student.nutritionProblem);
        pieChart.SetChart(target);
    }

    string GetProblemNote(NutritionProblem problem)
    {
        switch (problem)
        {
            case NutritionProblem.Healthy:
                return "Healthy diet. Maintain a balanced meal.";

            case NutritionProblem.OverweightMalnutrition:
                return "Low carbohydrate intake. Increase carbohydrates.";

            case NutritionProblem.ProteinMalnutrition:
                return "Low protein intake. Increase protein-rich foods.";

            case NutritionProblem.FatMalnutrition:
                return "Low healthy fat intake. Increase healthy fats.";

            default:
                return "";
        }
    }

    string FoodList(System.Collections.Generic.List<FoodSO> foods)
    {
        if (foods.Count == 0)
            return "-";

        string result = "";

        for (int i = 0; i < foods.Count; i++)
        {
            result += foods[i].foodName;

            if (i < foods.Count - 1)
                result += ", ";
        }

        return result;
    }
}