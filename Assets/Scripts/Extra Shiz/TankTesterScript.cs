using UnityEngine;

public class TankTesterScript : MonoBehaviour
{
    public GameObject tank;
    public GameObject tankHealthBar;

    public PlayerController player;
    public Spawner spawner;

    public bool enableTesting = false;
    private bool hasStarted = false;

    void Update()
    {
        if (enableTesting && !hasStarted)
        {
            StartTest();
            hasStarted = true;
        }
    }

    public void StartTest()
    {
        tank.SetActive(true);
        tankHealthBar.SetActive(true);
        player.EnableMovement();
        player.EnableDash();
        spawner.spawningEnabled = false;
    }
}
