using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;

    [SerializeField] private Spawner spawner;
    [SerializeField] private bool bossFightEnabled = false;

    [SerializeField] private GameObject tank;
    [SerializeField] private GameObject tankHealthBar;

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
        if (tankHealthBar != null)
            tankHealthBar.SetActive(false);

        if (tank != null)
            tank.SetActive(false);
    }

    private void Update()
    {
        if (!bossSpawned && PlayerController.Instance.killCounter >= 500)
        {
            EnableBossFight();

            if (spawner != null)
                spawner.spawningEnabled = false;

            bossSpawned = true; 
        }
    }

    public void EnableBossFight()
    {
        bossFightEnabled = true;

        if (tank != null)
            tank.SetActive(true);

        if (tankHealthBar != null)
            tankHealthBar.SetActive(true);
    }

    public void BossDied()
    {
        bossFightEnabled = false;

        if (tankHealthBar != null)
            tankHealthBar.SetActive(false);

        if (spawner != null)
            spawner.spawningEnabled = true;
    }
}