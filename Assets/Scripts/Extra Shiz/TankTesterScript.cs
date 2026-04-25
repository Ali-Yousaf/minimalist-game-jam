using UnityEngine;

public class TankTesterScript : MonoBehaviour
{
    public GameObject tank;
    public GameObject tankHealthBar;

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
        PlayerController.Instance.EnableMovement();
        PlayerController.Instance.EnableDash();
        PlayerController.Instance.laserDamage = 5000;
        spawner.spawningEnabled = false;
        BossFightManager.Instance.EnableBossFight();
    }
}
