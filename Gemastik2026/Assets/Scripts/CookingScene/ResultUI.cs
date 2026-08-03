using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject panel;

    public TMP_Text successText;
    public TMP_Text failedText;
    public TMP_Text resultText;

    public Button continueButton;

    public string nextScene;
    public string mainMenuScene;

    public void ShowResults(int success, int failed)
    {
        panel.SetActive(true);

        successText.text = $"Successful Meals : {success}";
        failedText.text = $"Failed Meals : {failed}";

        bool passed = success > failed;

        resultText.text = passed ? "PASS" : "FAIL";

        continueButton.onClick.RemoveAllListeners();

        if (passed)
            continueButton.onClick.AddListener(LoadNextScene);
        else
            continueButton.onClick.AddListener(LoadMainMenu);
    }

    void LoadNextScene()
    {
        DayManager.Instance.FinishDay();
        SceneManager.LoadScene(nextScene);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}