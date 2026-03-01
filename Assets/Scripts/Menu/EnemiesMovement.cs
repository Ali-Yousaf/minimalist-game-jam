using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float spawnOffset = 2f;
    public bool spawningEnabled = true;

    [Header("References")]
    public Camera mainCamera;

    private float timer;

    private void Update()
    {
        if (!spawningEnabled) return;
        if (enemyPrefabs.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject randomEnemy =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 camPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        Vector3 spawnPos = Vector3.zero;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Top
                spawnPos = new Vector3(
                    Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2),
                    camPos.y + camHeight / 2 + spawnOffset,
                    0f);
                break;

            case 1: // Bottom
                spawnPos = new Vector3(
                    Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2),
                    camPos.y - camHeight / 2 - spawnOffset,
                    0f);
                break;

            case 2: // Left
                spawnPos = new Vector3(
                    camPos.x - camWidth / 2 - spawnOffset,
                    Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2),
                    0f);
                break;

            case 3: // Right
                spawnPos = new Vector3(
                    camPos.x + camWidth / 2 + spawnOffset,
                    Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2),
                    0f);
                break;
        }

        GameObject spawnedEnemy =
            Instantiate(randomEnemy, spawnPos, Quaternion.identity);

        spawnedEnemy.transform.localScale = Vector3.one * 0.5f;
    }
}