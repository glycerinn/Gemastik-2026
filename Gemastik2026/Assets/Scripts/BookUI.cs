using UnityEngine;

public class BookUI : MonoBehaviour
{
    public GameObject bookPanel;

    public void OpenBook()
    {
        bookPanel.SetActive(true);
        Time.timeScale = 0f;   // Optional: pause game
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}