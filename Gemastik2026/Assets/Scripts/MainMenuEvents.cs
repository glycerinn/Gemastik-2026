using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uIDocument;
    private Label title;
    private Button button;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        uIDocument = GetComponent<UIDocument>();

        button = uIDocument.rootVisualElement.Q("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(OnPlayGame);
    }

    private void Start()
    {
        int random = Random.Range(0, 3);

        if (random == 0)
        {
            audioManager.playLakeA();
        }
        else if (random == 1)
        {
            audioManager.playMountainA();
        }
        else
        {
            audioManager.playVillageA();
        }
    }

    private void OnPlayGame(ClickEvent evt)
    {
        Debug.Log("pressed");
        audioManager.playClickSFX();
        // Pastikan waktu berjalan normal
        Time.timeScale = 1f;

        // Panggil FadeManager untuk melakukan transisi fade hitam sebelum masuk ke game
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.LoadSceneWithFade("Cutscene");
        }
        else
        {
            // Fallback jika FadeManager belum terpasang di Main Menu
            SceneManager.LoadScene("Cutscene");
        }
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }
}
