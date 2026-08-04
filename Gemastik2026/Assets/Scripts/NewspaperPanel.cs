using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewspaperType
{
    public NutritionProblem problem;
    public Color color;
}

public class NewspaperPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject newspaperWindow;
    public Image panelImage;

    [Header("Newspapers")]
    public NewspaperType[] newspapers;

    private void Start()
    {
        if (!DayManager.Instance.newspaperShownToday)
        {
            ShowNewspaper();
            DayManager.Instance.newspaperShownToday = true;
        }
        else
        {
            newspaperWindow.SetActive(false);
        }
    }

    public void ShowNewspaper()
    {
        // Only generate a new day if this newspaper hasn't been shown yet.
        if (!DayManager.Instance.newspaperShownToday)
        {
            DayManager.Instance.StartNewDay();
        }

        foreach (NewspaperType paper in newspapers)
        {
            if (paper.problem == DayManager.Instance.currentProblem)
            {
                panelImage.color = paper.color;
                break;
            }
        }

        newspaperWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseNewspaper()
    {
        newspaperWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenNewspaper()
    {
        newspaperWindow.SetActive(true);
        Time.timeScale = 0;
    }
}