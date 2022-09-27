using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public float SpawnRate = 6.0f;
    private float spawnTimer;

    private void Start() 
    {
        spawnTimer = SpawnRate;
    }

    private void Update() 
    {
        // If the player has died, stop spawning in enemies.
        if (GameManager.GetInstance().pcRef.health.IsDead) return;

        // Count down the timer to spawn an enemy
        if (spawnTimer > 0) {
            spawnTimer -= Time.deltaTime;
        } else {
            Spawn();
            spawnTimer = SpawnRate;
        }
    }

    public void Spawn() 
    {
        GameObject spawnedObj = Instantiate(PrefabToSpawn, transform.position, Quaternion.identity);
    }

}
