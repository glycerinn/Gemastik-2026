using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isLoading = false;

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isLoading)
                return;

            if (pausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                onPause();
            }
        }
    }

    public void onPause()
    {
        if (isLoading)
            return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        if (isLoading)
            return;

        isLoading = true;

        // Make sure the game isn't still paused during the transition
        Time.timeScale = 1f;

        // Use the FadeManager to transition to Main Menu
        FadeManager.Instance.LoadSceneWithFade("MainMenu");
    }

    public void Resume()
    {
        if (isLoading)
            return;

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
}