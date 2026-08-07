using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MiniGameUIEvents : MonoBehaviour
{
    private UIDocument uIDocument;
    private Button button;
    public MinigameType minigameType;
    public FoodSO rewardFood;

    private void Awake()
    {
        uIDocument = GetComponent<UIDocument>();

        button = uIDocument.rootVisualElement.Q("BackButton") as Button;
        if (button != null)
        {
            button.RegisterCallback<ClickEvent>(OnPlayGame);
        }
    }

    private void Start()
    {

    }

    private void OnPlayGame(ClickEvent evt)
    {
        Debug.Log("pressed");

        // Simpan data game terlebih dahulu
        TownGameManager.Instance.CollectIngredient(rewardFood);
        TownGameManager.Instance.CompleteMinigame(minigameType);
        Time.timeScale = 1f;

        // PERUBAHAN DI SINI:
        // Gunakan FadeManager untuk memicu animasi layar hitam, baru kemudian pindah scene
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade("SideScroller");
        }
        else
        {
            // Fallback jika FadeManager tidak ditemukan di scene minigame
            SceneManager.LoadScene("SideScroller");
        }
    }

    private void OnDisable()
    {
        if (button != null)
        {
            button.UnregisterCallback<ClickEvent>(OnPlayGame);
        }
    }
}