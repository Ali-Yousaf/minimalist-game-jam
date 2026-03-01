using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name;
        public GameObject prefab;
        public float baseSpawnInterval = 5f; // initial spawn interval
        public float minSpawnInterval = 1f;  // minimum interval after scaling
        public int unlockKills = 0;           // kills required to start spawning
        [HideInInspector] public float timer = 0f;
    }

    public EnemyType[] enemies; 

    public Camera mainCamera;  
    public float spawnOffset = 2f; 
    public bool spawningEnabled = true;

    private void Update()
    {
        if (!spawningEnabled) return;

        int kills = PlayerController.Instance.killCounter;

        foreach (var enemy in enemies)
        {
            if (kills >= enemy.unlockKills)
            {
                float scaledInterval = Mathf.Max(enemy.baseSpawnInterval - kills * 0.03f, enemy.minSpawnInterval);
                enemy.timer += Time.deltaTime;

                if (enemy.timer >= scaledInterval)
                {
                    SpawnEnemy(enemy.prefab);
                    enemy.timer = 0f;
                }
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        // Get camera bounds
        Vector3 camPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        Vector3 spawnPos = Vector3.zero;

        // Randomly choose a side to spawn: top, bottom, left, right
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // top
                spawnPos = new Vector3(Random.Range(camPos.x - camWidth/2, camPos.x + camWidth/2),
                                       camPos.y + camHeight/2 + spawnOffset,
                                       0);
                break;
            case 1: // bottom
                spawnPos = new Vector3(Random.Range(camPos.x - camWidth/2, camPos.x + camWidth/2),
                                       camPos.y - camHeight/2 - spawnOffset,
                                       0);
                break;
            case 2: // left
                spawnPos = new Vector3(camPos.x - camWidth/2 - spawnOffset,
                                       Random.Range(camPos.y - camHeight/2, camPos.y + camHeight/2),
                                       0);
                break;
            case 3: // right
                spawnPos = new Vector3(camPos.x + camWidth/2 + spawnOffset,
                                       Random.Range(camPos.y - camHeight/2, camPos.y + camHeight/2),
                                       0);
                break;
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}