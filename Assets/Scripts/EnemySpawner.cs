using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;

    public float minX = -8f;
    public float maxX = 8f;
    public float minZ = -8f;
    public float maxZ = 8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("astro");
        if (enemies.Length >= maxEnemies) return;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 candidate = new Vector3(randomX, 0f, randomZ);

        NavMeshHit hit;
        Vector3 spawnPosition = NavMesh.SamplePosition(candidate, out hit, 5f, NavMesh.AllAreas)
            ? hit.position + Vector3.up * 0.5f
            : new Vector3(candidate.x, 0.5f, candidate.z);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
