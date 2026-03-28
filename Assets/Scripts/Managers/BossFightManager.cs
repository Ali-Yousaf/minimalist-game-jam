using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;
    [SerializeField] private Spawner spawer;
    [SerializeField] private bool bossFightEnabled = false;
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private GameObject tankHealthBar;


    void Awake()
    {
        if(Instance == null)
            Instance = this;

        else
            Destroy(gameObject);    
    }

    void Start()
    {
        tankHealthBar.SetActive(false);
    }

    void Update()
    {
        if(PlayerController.Instance.killCounter == 500)
        {
            EnableBossFight();
            spawer.spawningEnabled = false;
        }
    }

    public void EnableBossFight()
    {
        bossFightEnabled = true;
        Instantiate(tankPrefab, transform.position, Quaternion.identity);
        tankHealthBar.SetActive(true);
    }

    public void BossDied()
    {
        bossFightEnabled = false;
        tankHealthBar.SetActive(false);
    }
}
