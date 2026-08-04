using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    [Header("Starting Foods")]
    public List<FoodSO> startingFoods = new();

    [SerializeField] private readonly List<FoodSO> unlockedFoods = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            unlockedFoods.AddRange(startingFoods);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<FoodSO> GetAvailableFoods()
    {
        return unlockedFoods;
    }

    public void UnlockFood(FoodSO food)
    {
        if (unlockedFoods.Contains(food))
            return;

        unlockedFoods.Add(food);

        Debug.Log("Unlocked " + food.foodName);
    }

    public bool HasFood(FoodSO food)
    {
        return unlockedFoods.Contains(food);
    }
}