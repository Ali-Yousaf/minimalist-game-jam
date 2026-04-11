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

        if(Input.GetKeyDown(KeyCode.T))
        {
            StartTest();
            hasStarted = true;
        }
    }

    public void StartTest()
    {
        player.EnableMovement();
        player.EnableDash();
        spawner.spawningEnabled = false;
        BossFightManager.Instance.EnableBossFight();
    }
}
