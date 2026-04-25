using System;
using Unity.Mathematics;
using UnityEngine;

public class MazeFlare : MonoBehaviour
{
    [SerializeField] private int flareCount = 5;
    
    [SerializeField] private GameObject flarePrefab;
    
    private Transform player;
    
    public bool canSpawnFlares = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canSpawnFlares)
        {
            SpawnFlare();
        }
    }

    private void SpawnFlare()
    {
        if(flareCount <= 0)
        {
            print("Out of flares");
            return;
        }

        Instantiate(flarePrefab, player.position, Quaternion.identity);
        flareCount--;
    }
}
