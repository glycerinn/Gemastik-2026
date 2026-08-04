using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MiniGameUIEvents : MonoBehaviour
{
    private UIDocument uIDocument;
    private Button button;
    public MinigameType minigameType;

    private void Awake()
    {
        uIDocument = GetComponent<UIDocument>();

        button = uIDocument.rootVisualElement.Q("BackButton") as Button;
        button.RegisterCallback<ClickEvent>(OnPlayGame);
    }

    private void Start()
    {

    }

    private void OnPlayGame(ClickEvent evt)
    {
        Debug.Log("pressed");
        TownGameManager.Instance.CompleteMinigame(minigameType);
        SceneManager.LoadScene("SideScroller");
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }
}
