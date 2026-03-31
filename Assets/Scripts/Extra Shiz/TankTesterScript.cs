using UnityEngine;

public class TankTesterScript : MonoBehaviour
{
    public GameObject tank;
    public GameObject tankHealthBar;

    public PlayerController player;
    public Spawner spawner;

    public bool enableTesting = false;

    void Awake()
    {
        
    }

    void Update()
    {
        if(enableTesting)
        {
            StartTest();
        }
    }

    public void StartTest()
    {
        tank.SetActive(true);
        tankHealthBar.SetActive(true);
        player.EnableMovement();
        player.EnableDash();
        spawner.spawningEnabled = true;
    }
}
