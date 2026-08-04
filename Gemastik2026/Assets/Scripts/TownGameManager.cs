using System.Collections.Generic;
using UnityEngine;

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager Instance;

    [Header("Today's Required Minigames")]
    public int totalMinigames = 5;

    private HashSet<MinigameType> completedMinigames = new HashSet<MinigameType>();
    
    [Header("Debug")]
    [SerializeField] private List<MinigameType> completedMinigamesDebug = new();

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

    public void StartDay()
    {
        completedMinigames.Clear();
        completedMinigamesDebug.Clear();

        Debug.Log("New day started.");
    }

    public void CompleteMinigame(MinigameType minigame)
    {
        if (completedMinigames.Add(minigame))
        {
            completedMinigamesDebug.Add(minigame);
            Debug.Log($"Completed: {minigame}");
            Debug.Log($"{completedMinigames.Count}/{totalMinigames} minigames completed.");
        }
    }

    public bool IsMinigameCompleted(MinigameType minigame)
    {
        return completedMinigames.Contains(minigame);
    }

    public bool CanEnterSchool()
    {
        return completedMinigames.Count >= totalMinigames;
    }

    public int CompletedCount()
    {
        return completedMinigames.Count;
    }
}