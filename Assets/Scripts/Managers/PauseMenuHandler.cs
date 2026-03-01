using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject statsPanel;

    private bool isPaused = false;

    private void Start()
    {
        // Ensure both panels start hidden
        pauseMenuPanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (statsPanel.activeSelf)
            {
                // If stats panel open, hide it first
                HideStatsPanel();
            }
            else
            {
                TogglePause();
            }
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        statsPanel.SetActive(false);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void HidePauseMenu()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        statsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowStatsPanel()
    {
        statsPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}