using UnityEngine;

public static class ScoreManager
{
    private const string HighScoreKey = "HIGH_SCORE";

    public static int HighScore { get; private set; }

    public static void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public static void SaveHighScore()
    {
        int currentScore = PlayerController.Instance.killCounter;

        if (currentScore > HighScore)
        {
            HighScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }
    }

    public static void ResetHighScore()
    {
        HighScore = 0;
        PlayerPrefs.Save();
    }

    public static int GetHighScore()
    {
        return HighScore;
    }
}