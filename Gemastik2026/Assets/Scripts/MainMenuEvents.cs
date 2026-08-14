using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    public UIDocument uIDocument;
    private Label title;
    private Button button;
    private Button settingsbutton;
    private Button quitbutton;
    private AudioManager audioManager;
    public SettingsManager settings;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        uIDocument = GetComponent<UIDocument>();

        button = uIDocument.rootVisualElement.Q("StartGameButton") as Button;
        button.RegisterCallback<ClickEvent>(OnPlayGame);

        settingsbutton = uIDocument.rootVisualElement.Q("SettingsButton") as Button;
        settingsbutton.RegisterCallback<ClickEvent>(OnSettings);

        quitbutton = uIDocument.rootVisualElement.Q("QuitButton") as Button;
        quitbutton.RegisterCallback<ClickEvent>(OnQuit);
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

    private void OnSettings(ClickEvent evt)
    {
        Debug.Log("pressed");
        audioManager.playClickSFX();
        // Pastikan waktu berjalan normal
        Time.timeScale = 1f;

        settings.SetUp();
        uIDocument.enabled = false;
    }

    private void OnQuit(ClickEvent evt)
    {
        Debug.Log("pressed");
        audioManager.playClickSFX();
        // Pastikan waktu berjalan normal
        Time.timeScale = 1f;

        Application.Quit();
    }

    private void OnDisable()
    {
        button.UnregisterCallback<ClickEvent>(OnPlayGame);
    }
}
