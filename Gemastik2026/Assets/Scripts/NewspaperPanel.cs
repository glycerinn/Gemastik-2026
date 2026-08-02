using UnityEngine;
using UnityEngine.UI;

public class NewspaperPanel : MonoBehaviour
{
    [Header("UI")]
    public Image panelImage;

    [Header("Possible Colors")]
    public Color[] colors = new Color[4];

    private void Start()
    {
        RandomizeColor();

        // Optional: Pause the game while the newspaper is open.
        Time.timeScale = 0f;
    }

    void RandomizeColor()
    {
        if (colors.Length == 0)
            return;

        int randomIndex = Random.Range(0, colors.Length);
        panelImage.color = colors[randomIndex];
    }

    public void CloseNewspaper()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}