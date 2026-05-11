using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnInterval = 4f;
    public int maxMeteors = 5;
    public float minX = -8f, maxX = 8f, minZ = -8f, maxZ = 8f;
    public float spawnY = 20f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        if (FindObjectsByType<MeteorLogic>(FindObjectsSortMode.None).Length >= maxMeteors) return;
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Instantiate(meteorPrefab, new Vector3(x, spawnY, z), Quaternion.identity);
    }
}
