using TMPro;
using UnityEngine;

public class TempeGameManager : MonoBehaviour
{
    public static TempeGameManager Instance;

    public int targetScore = 30;
    public int score = 0;

    public TMP_Text scoreText;
    public GameObject winPanel;

    void Awake()
    {
        Instance = this;
        Debug.Log("Awake: " + GetInstanceID());
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddPoint()
    {
        Debug.Log("Before: " + score);
        score++;

        Debug.Log("After: " + score);

        UpdateUI();

        if (score >= targetScore)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void LosePoint()
    {
        Debug.Log("LosePoint called");
        score = Mathf.Max(0, score - 1);
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = score + " / " + targetScore;
    }
}