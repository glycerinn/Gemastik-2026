using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject panel;
    private CanvasGroup panelCanvasGroup;

    public TMP_Text resultText;      // Header Win/Lose (PASS / FAIL)
    public TMP_Text successText;     // Score 1
    public TMP_Text failedText;      // Score 2

    public Button continueButton;

    public string nextScene;
    public string mainMenuScene;

    private void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void ShowResults(int success, int failed)
    {
        panel.SetActive(true);

        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
            panelCanvasGroup = panel.AddComponent<CanvasGroup>();

        StartCoroutine(ShowResultsSequence(success, failed));
    }

    private IEnumerator ShowResultsSequence(int success, int failed)
    {
        // 1. SIAPKAN DATA
        bool passed = success > failed;
        resultText.text = passed ? "PASS" : "FAIL";
        resultText.color = passed ? Color.green : Color.red;

        successText.text = $"Successful Meals : {success}";
        failedText.text = $"Failed Meals : {failed}";

        continueButton.onClick.RemoveAllListeners();
        if (passed)
            continueButton.onClick.AddListener(LoadNextScene);
        else
            continueButton.onClick.AddListener(LoadMainMenu);

        // 2. SEMBUNYIKAN SEMUA ELEMEN DI AWAL
        resultText.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
        failedText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // 3. ANIMASI PANEL UTAMA (Fade In & Scale)
        panelCanvasGroup.alpha = 0f;
        panel.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        float time = 0;
        float duration = 0.35f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, time / duration);

            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, p);
            float scale = Mathf.Lerp(0.8f, 1f, p);
            panel.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        // 4. MUNCULKAN ELEMEN SECARA KONSISTEN SESUAI URUTAN:

        // A. KELUARKAN WIN/LOSE (PASS/FAIL) PERTAMA
        yield return new WaitForSeconds(0.15f);
        resultText.gameObject.SetActive(true);
        StartCoroutine(PopAnimation(resultText.transform, 1.4f));

        // B. KELUARKAN SCORING PERTAMA (Successful Meals)
        yield return new WaitForSeconds(0.4f);
        successText.gameObject.SetActive(true);
        StartCoroutine(PopAnimation(successText.transform));

        // C. KELUARKAN SCORING KEDUA (Failed Meals)
        yield return new WaitForSeconds(0.35f);
        failedText.gameObject.SetActive(true);
        StartCoroutine(PopAnimation(failedText.transform));

        // D. KELUARKAN TOMBOL CONTINUE TERAKHIR
        yield return new WaitForSeconds(0.4f);
        continueButton.gameObject.SetActive(true);
        StartCoroutine(PopAnimation(continueButton.transform));
    }

    private IEnumerator PopAnimation(Transform target, float maxScale = 1.2f)
    {
        float time = 0;
        float durationOut = 0.18f;
        float durationIn = 0.12f;

        while (time < durationOut)
        {
            time += Time.deltaTime;
            float p = time / durationOut;
            float scale = Mathf.Lerp(0f, maxScale, p);
            target.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        time = 0;
        while (time < durationIn)
        {
            time += Time.deltaTime;
            float p = time / durationIn;
            float scale = Mathf.Lerp(maxScale, 1f, p);
            target.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    void LoadNextScene()
    {
        if (DayManager.Instance != null) DayManager.Instance.FinishDay();

        if (FadeManager.Instance != null)
            FadeManager.Instance.LoadSceneWithFade(nextScene);
        else
            SceneManager.LoadScene(nextScene);
    }

    void LoadMainMenu()
    {
        if (FadeManager.Instance != null)
            FadeManager.Instance.LoadSceneWithFade(mainMenuScene);
        else
            SceneManager.LoadScene(mainMenuScene);
    }
}