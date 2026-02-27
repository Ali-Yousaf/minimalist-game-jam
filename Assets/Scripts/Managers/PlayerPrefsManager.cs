using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{   
    private static PlayerPrefsManager Instance;

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    private const string KillKey = "TotalKills";

    public static void SaveKills(int amount)
    {
        PlayerPrefs.SetInt(KillKey, amount);
        PlayerPrefs.Save();
    }

    public static int LoadKills()
    {
        return PlayerPrefs.GetInt(KillKey, 0);
    }
}