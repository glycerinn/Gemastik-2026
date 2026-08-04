using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Day")]
    public int currentDay = 1;
    public bool newspaperShownToday;

    [Header("Today's Newspaper")]
    public NutritionProblem currentProblem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewDay()
    {
        currentProblem = (NutritionProblem)Random.Range(0, 4);

        newspaperShownToday = false;

        Debug.Log($"Day {currentDay}");
        Debug.Log($"Today's problem: {currentProblem}");
    }

    public void FinishDay()
    {
        currentDay++;
        Debug.Log("Current Day: " + currentDay);
    }
}