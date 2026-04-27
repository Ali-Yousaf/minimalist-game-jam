using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;

    [SerializeField] private Spawner spawner;
    [SerializeField] public bool bossFightEnabled = false;
    [SerializeField] private int KillThresholdToSpawn = 325;
    [SerializeField] private GameObject tank;
    [SerializeField] private GameObject tankHealthBar;
    private MovieScreens movieScreens;

    private bool bossSpawned = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        movieScreens = FindAnyObjectByType<MovieScreens>();

        if (tankHealthBar != null)
            tankHealthBar.SetActive(false);

        if (tank != null)
            tank.SetActive(false);
    }

    private void Update()
    {
        if (!bossSpawned && PlayerController.Instance.killCounter >= KillThresholdToSpawn)
        {
            EnableBossFight();

            spawner.spawningEnabled = false;
            bossSpawned = true; 
        }
    }

    public void EnableBossFight()
    {
        bossFightEnabled = true;
        movieScreens.ActiveMovieScreens();
        AudioManager.Instance.PlayBossMusic();

        if (tank != null)
            tank.SetActive(true);

        if (tankHealthBar != null)
            tankHealthBar.SetActive(true);
    }

    public void BossDied()
    {
        bossFightEnabled = false;
        AudioManager.Instance.PlayMainMenuMusic();

        if (tankHealthBar != null)
            tankHealthBar.SetActive(false);

        if (spawner != null)
            spawner.spawningEnabled = true;
    }
}