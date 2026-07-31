using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int targetTrash = 30;
    private int collectedTrash = 0;
    public GameObject completedPanel;

    public TextMeshProUGUI counterText;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        UpdateCounter();
    }

    public void TrashCollected()
    {
        collectedTrash++;
        UpdateCounter();

        if (collectedTrash >= targetTrash)
        {
            WinGame();
        }
    }

    void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = $"{collectedTrash}/{targetTrash}";
    }

    void WinGame()
    {
        Debug.Log("You Win!");

        TrashSpawner spawner = FindFirstObjectByType<TrashSpawner>();
        if (spawner != null)
            spawner.CancelInvoke();

        Time.timeScale = 0f;
        completedPanel.SetActive(true);
    }
}