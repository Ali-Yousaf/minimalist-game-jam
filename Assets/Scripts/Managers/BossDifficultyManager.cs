using UnityEngine;

public class BossDifficultyManager : MonoBehaviour
{
    [SerializeField] private int[] healthThresholds = {1500, 1000, 500};
    
    private int thresholdIndex = 0;
    private TankMovement tankMovement;
    private TankHealth tankHealth;

    void Start()
    {
        tankMovement = GetComponent<TankMovement>();
        tankHealth = GetComponent<TankHealth>();
    }

    void Update()
    {
        CheckHealthThreshold();
    }

    private void CheckHealthThreshold()
    {
        if (thresholdIndex < healthThresholds.Length &&
            tankHealth.currentHealth < healthThresholds[thresholdIndex])
        {
            IncreaseDifficulty();
            thresholdIndex++;
        }
    }

    private void IncreaseDifficulty()
    {
        print("Diff Increased");
        tankMovement.moveSpeed += 4;
        tankMovement.retreatSpeed += 4;

        tankMovement.fireRate -= 0.2f;
        tankMovement.burstCount += 5;
    }
}
