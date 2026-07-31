using UnityEngine;
using UnityEngine.UI;

public class Rice : MonoBehaviour
{
    public enum RiceStage
    {
        Seed, Young, Nearing, Ready
    }

    public RiceStage stage;

    public Image image;

    public Color seedColor = new Color(0.45f, 0.25f, 0.1f);
    public Color youngColor = Color.green;
    public Color nearingColor = Color.yellow;
    public Color readyColor = new Color(1f, 0.8f, 0f);

    private void Start()
    {
        UpdateVisual();
    }

    public void SetStage(RiceStage newStage)
    {
        stage = newStage;
        UpdateVisual();
    }

    public void Grow()
    {
        if (stage != RiceStage.Ready)
        {
            stage++;
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        switch(stage)
        {
            case RiceStage.Seed:
                image.color = seedColor;
                break;

            case RiceStage.Young:
                image.color = youngColor;
                break;

            case RiceStage.Nearing:
                image.color = nearingColor;
                break;

            case RiceStage.Ready:
                image.color = readyColor;
                break;
        }
    }

    public void ClickRice()
    {
        RiceGameManager.Instance.OnRiceClicked(this);
    }
}