using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;

    public bool gameStarted = false;

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        gameStarted = true;
        LoadGame();
    }

    public void LoadGame()
    {
        ScoreManager.LoadHighScore();
    }

    public void ResetStats()
    {
        ScoreManager.ResetHighScore();
        KillCounter.Instance.ResetAllKills();
        FindAnyObjectByType<StatsUI>().UpdateStatsUI();
    }
}
