using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MiniGameUIEvents : MonoBehaviour
{
    private UIDocument uIDocument;
    private Button button;

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
        SceneManager.LoadScene("SideScroller");
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }
}
