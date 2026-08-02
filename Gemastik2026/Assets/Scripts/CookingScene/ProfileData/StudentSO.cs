using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nutrition/Student")]
public class StudentSO : ScriptableObject
{
    [Header("Profile")]
    public string studentName;
    public Sprite portrait;

    [Header("Nutrition")]
    public NutritionProblem nutritionProblem;

    [Header("Favorite Foods")]
    public List<FoodSO> favoriteCarbs;
    public List<FoodSO> favoriteProteins;
    public List<FoodSO> favoriteFats;
}