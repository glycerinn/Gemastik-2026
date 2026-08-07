using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uIDocument;
    private Label title;
    private Button button;

    private void Awake()
    {
        uIDocument = GetComponent<UIDocument>();

        button = uIDocument.rootVisualElement.Q("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(OnPlayGame);
    }

    private void Start()
    {

    }

    private void OnPlayGame(ClickEvent evt)
    {
        Debug.Log("pressed");

        // Pastikan waktu berjalan normal
        Time.timeScale = 1f;

        // Panggil FadeManager untuk melakukan transisi fade hitam sebelum masuk ke game
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade("SideScroller");
        }
        else
        {
            // Fallback jika FadeManager belum terpasang di Main Menu
            SceneManager.LoadScene("SideScroller");
        }
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }
}
