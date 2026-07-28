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
        title = uIDocument.rootVisualElement.Q("HeaderTitle") as Label;
        button.RegisterCallback<ClickEvent>(OnPlayGame);
    }

    private void Start()
    {
        title.schedule.Execute(loopingTitle).StartingIn(100);
    }

    private void OnPlayGame(ClickEvent evt)
    {
        Debug.Log("pressed");
        SceneManager.LoadScene("SideScroller");
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }

    private void loopingTitle()
    {
        Debug.Log("happen");
        title.ToggleInClassList("header-grow");
        title.RegisterCallback<TransitionEndEvent>(
            evt => title.ToggleInClassList("header-grow")
        );
    }
}
