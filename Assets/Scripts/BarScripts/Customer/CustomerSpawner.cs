using UnityEngine;
using UnityEngine.Events;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs; // Array of customer prefabs
    public Transform spawnPoint; // Point where customers will spawn
    public float spawnInterval; // Time between spawns
    public int totalCustomersToSpawn;
    private int customersSpawned;
    private int customersServed = 0; // Track the number of customers served

    private GameObject lastSpawnedPrefab;

    [Header("Events")]
    public UnityEvent onAllCustomersServed;

    private void Start()
    {
        customersSpawned = 0;
        InvokeRepeating("SpawnCustomer", 0f, spawnInterval);
    }

    private void SpawnCustomer()
    {
        if (customersSpawned >= totalCustomersToSpawn)
        {
            gameObject.SetActive(false);
            return;
        }

        GameObject customerPrefab = GetRandomCustomerPrefab();
        if (customerPrefab != null)
        {
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            customersSpawned++;
        }
    }

    private GameObject GetRandomCustomerPrefab()
    {
        if (customerPrefabs.Length == 0)
            return null;

        GameObject newPrefab;
        do
        {
            newPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        } while (newPrefab == lastSpawnedPrefab);

        lastSpawnedPrefab = newPrefab;
        return newPrefab;
    }

    public void NotifyCustomerServed()
    {
        customersServed++;
        if (customersServed >= totalCustomersToSpawn)
        {
            onAllCustomersServed.Invoke();
        }
    }
}
