using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private static PlayerPrefsManager Instance;

    // KEYS
    public const string TotalKillKey    = "TotalKills";
    public const string SquareKillKey   = "SquareKills";
    public const string TriangleKillKey = "TriangleKills";
    public const string FastKillKey     = "FastKills";
    public const string TankKillKey     = "TankKills";
    public const string BomberKillKey   = "BomberKills";

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
    // ENEMY KILLS
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

            case KillCounter.EnemyType.Bomber:
                PlayerPrefs.SetInt(BomberKillKey, amount);
                break;
        }

        PlayerPrefs.Save();
    }

    public static int LoadEnemyKills(KillCounter.EnemyType type)
    {
        switch (type)
        {
            case KillCounter.EnemyType.Square:   return PlayerPrefs.GetInt(SquareKillKey, 0);
            case KillCounter.EnemyType.Triangle: return PlayerPrefs.GetInt(TriangleKillKey, 0);
            case KillCounter.EnemyType.Fast:     return PlayerPrefs.GetInt(FastKillKey, 0);
            case KillCounter.EnemyType.Tank:     return PlayerPrefs.GetInt(TankKillKey, 0);
            case KillCounter.EnemyType.Bomber:   return PlayerPrefs.GetInt(BomberKillKey, 0);
        }
        return 0;
    }

    // =========================
    // RESET
    // =========================

    public static void ResetAllKills()
    {
        PlayerPrefs.DeleteKey(TotalKillKey);
        PlayerPrefs.DeleteKey(SquareKillKey);
        PlayerPrefs.DeleteKey(TriangleKillKey);
        PlayerPrefs.DeleteKey(FastKillKey);
        PlayerPrefs.DeleteKey(TankKillKey);
        PlayerPrefs.DeleteKey(BomberKillKey);

        PlayerPrefs.Save();
    }
}