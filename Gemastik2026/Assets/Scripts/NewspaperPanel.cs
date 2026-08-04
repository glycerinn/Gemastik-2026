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
    public Image panelImage;
    public NewspaperType[] newspapers;

    void Start()
    {
        if (!DayManager.Instance.newspaperShownToday)
        {
            ShowNewspaper();

            DayManager.Instance.newspaperShownToday = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowNewspaper()
    {
        DayManager.Instance.StartNewDay();

        foreach (NewspaperType paper in newspapers)
        {
            if (paper.problem ==
                DayManager.Instance.currentProblem)
            {
                panelImage.color = paper.color;
                break;
            }
        }

        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseNewspaper()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}