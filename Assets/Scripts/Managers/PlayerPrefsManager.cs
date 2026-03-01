using UnityEngine;

/// <summary>
/// Centralises all PlayerPrefs keys.
/// KillCounter calls these directly, but helper methods are still
/// available for other systems (e.g. a stats screen reset button).
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{
    private static PlayerPrefsManager Instance;

    // ── Keys (public so KillCounter can reference them) ───────────
    public const string TotalKillKey    = "TotalKills";
    public const string SquareKillKey   = "SquareKills";
    public const string TriangleKillKey = "TriangleKills";
    public const string FastKillKey     = "FastKills";
    public const string TankKillKey     = "TankKills";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // TOTAL KILLS
    // =========================

    public static void SaveTotalKills(int amount)
    {
        PlayerPrefs.SetInt(TotalKillKey, amount);
        PlayerPrefs.Save();
    }

    public static int LoadTotalKills()
    {
        return PlayerPrefs.GetInt(TotalKillKey, 0);
    }

    // =========================
    // INDIVIDUAL ENEMY KILLS
    // =========================

    public static void SaveEnemyKills(KillCounter.EnemyType type, int amount)
    {
        switch (type)
        {
            case KillCounter.EnemyType.Square:
                PlayerPrefs.SetInt(SquareKillKey, amount);
                break;

            case KillCounter.EnemyType.Triangle:
                PlayerPrefs.SetInt(TriangleKillKey, amount);
                break;

            case KillCounter.EnemyType.Fast:
                PlayerPrefs.SetInt(FastKillKey, amount);
                break;

            case KillCounter.EnemyType.Tank:
                PlayerPrefs.SetInt(TankKillKey, amount);
                break;
        }
        PlayerPrefs.Save();
    }

    public static int LoadEnemyKills(KillCounter.EnemyType type)
    {
        switch (type)
        {
            case KillCounter.EnemyType.Square:   return PlayerPrefs.GetInt(SquareKillKey,   0);
            case KillCounter.EnemyType.Triangle: return PlayerPrefs.GetInt(TriangleKillKey, 0);
            case KillCounter.EnemyType.Fast:     return PlayerPrefs.GetInt(FastKillKey,     0);
            case KillCounter.EnemyType.Tank:     return PlayerPrefs.GetInt(TankKillKey,     0);
        }
        return 0;
    }

    // =========================
    // RESET ALL
    // =========================

    public static void ResetAllKills()
    {
        PlayerPrefs.DeleteKey(TotalKillKey);
        PlayerPrefs.DeleteKey(SquareKillKey);
        PlayerPrefs.DeleteKey(TriangleKillKey);
        PlayerPrefs.DeleteKey(FastKillKey);
        PlayerPrefs.DeleteKey(TankKillKey);
        PlayerPrefs.Save();
    }
}