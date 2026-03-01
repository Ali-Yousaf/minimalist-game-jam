using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance;

    public enum EnemyType
    {
        Square,
        Triangle,
        Fast,
        Tank,
    }

    [Header("Total Kills")]
    public int totalKills;

    [Header("Individual Kills")]
    public int squareKills;
    public int triangleKills;
    public int fastKills;
    public int tankKills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        else
            Destroy(gameObject);

        LoadAllKills();
    }

    // Call this from EnemyHealth when an enemy dies
    public void AddKill(EnemyType type)
    {
        totalKills++;

        switch (type)
        {
            case EnemyType.Square:
                squareKills++;
                break;

            case EnemyType.Triangle:
                triangleKills++;
                break;

            case EnemyType.Fast:
                fastKills++;
                break;

            case EnemyType.Tank:
                tankKills++;
                break;
        }

        SaveAllKills();
    }

    public int GetKills(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Square:   return squareKills;
            case EnemyType.Triangle: return triangleKills;
            case EnemyType.Fast:     return fastKills;
            case EnemyType.Tank:     return tankKills;
        }
        return 0;
    }

    public int GetTotalKills() => totalKills;

    // ── Persistence ──────────────────────────────────────────────

    private void SaveAllKills()
    {
        PlayerPrefs.SetInt(PlayerPrefsManager.TotalKillKey,    totalKills);
        PlayerPrefs.SetInt(PlayerPrefsManager.SquareKillKey,   squareKills);
        PlayerPrefs.SetInt(PlayerPrefsManager.TriangleKillKey, triangleKills);
        PlayerPrefs.SetInt(PlayerPrefsManager.FastKillKey,     fastKills);
        PlayerPrefs.SetInt(PlayerPrefsManager.TankKillKey,     tankKills);
        PlayerPrefs.Save();
    }

    private void LoadAllKills()
    {
        totalKills    = PlayerPrefs.GetInt(PlayerPrefsManager.TotalKillKey,    0);
        squareKills   = PlayerPrefs.GetInt(PlayerPrefsManager.SquareKillKey,   0);
        triangleKills = PlayerPrefs.GetInt(PlayerPrefsManager.TriangleKillKey, 0);
        fastKills     = PlayerPrefs.GetInt(PlayerPrefsManager.FastKillKey,     0);
        tankKills     = PlayerPrefs.GetInt(PlayerPrefsManager.TankKillKey,     0);
    }

    /// <summary>Wipes all kill data from memory and PlayerPrefs.</summary>
    public void ResetAllKills()
    {
        totalKills = squareKills = triangleKills = fastKills = tankKills = 0;
        SaveAllKills();
    }
}