using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statsText;

    [Header("Colors")]
    [SerializeField] private string titleColor = "#FFD700";   // Gold
    [SerializeField] private string valueColor = "#00FFFF";   // Cyan
    [SerializeField] private string labelColor = "#FFFFFF";   // White

    private void Start()
    {
        if (statsText == null)
        {
            Debug.LogError("Stats TextMeshProUGUI is not assigned!");
            return;
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        int total     = PlayerPrefsManager.LoadTotalKills();
        int square    = PlayerPrefsManager.LoadEnemyKills(KillCounter.EnemyType.Square);
        int triangle  = PlayerPrefsManager.LoadEnemyKills(KillCounter.EnemyType.Triangle);
        int fast      = PlayerPrefsManager.LoadEnemyKills(KillCounter.EnemyType.Fast);
        int tank      = PlayerPrefsManager.LoadEnemyKills(KillCounter.EnemyType.Tank);
        int highScore = ScoreManager.GetHighScore(); 

        statsText.text =
        $"<color={titleColor}><b>Player Stats</b></color>\n\n" +
        $"<color={labelColor}>High Score:  </color><color={valueColor}><b>{highScore}</b></color>\n\n" +
        $"<color={labelColor}>Total Kills:  </color><color={valueColor}><b>{total}</b></color>\n" +
        $"<color={labelColor}>Square Kills:  </color><color={valueColor}>{square}</color>\n" +
        $"<color={labelColor}>Triangle Kills:  </color><color={valueColor}>{triangle}</color>\n" +
        $"<color={labelColor}>Fast Kills:  </color><color={valueColor}>{fast}</color>\n" +
        $"<color={labelColor}>Tank Kills:  </color><color={valueColor}>{tank}</color>";
    }
}