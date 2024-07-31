using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class VehicleManager : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;
    public Transform spawnPoint;
    public Transform despawnPoint;
    public Transform[] fuelStations;
    public float minSpawnTime = 100f;
    public float maxSpawnTime = 150f;
    public int maxVehiclesPerFuelStation = 1;
    public int maxVehiclesToSpawn = 10;

    private GameObject lastSpawnedVehiclePrefab;
    private int vehiclesSpawned = 0;

    public UnityEvent onAllVehiclesServed;

    private void Start()
    {
        StartCoroutine(SpawnVehicles());
    }

    private IEnumerator SpawnVehicles()
    {
        while (vehiclesSpawned < maxVehiclesToSpawn)
        {
            SpawnVehicle();

            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }

        if (vehiclesSpawned >= maxVehiclesToSpawn)
        {
            onAllVehiclesServed.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void SpawnVehicle()
    {
        GameObject vehiclePrefab;
        do
        {
            vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        } while (vehiclePrefab == lastSpawnedVehiclePrefab);

        lastSpawnedVehiclePrefab = vehiclePrefab;

        GameObject vehicle = Instantiate(vehiclePrefab, spawnPoint.position, spawnPoint.rotation);
        vehiclesSpawned++;

        NavMeshAgent agent = vehicle.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            if (Random.value < 0.6f)
            {
                Transform target = FindAvailableFuelStation();
                if (target != null)
                {
                    agent.SetDestination(target.position);
                    vehicle.transform.SetParent(target);
                    agent.speed = 7f;
                }
                else
                {
                    agent.SetDestination(despawnPoint.position);
                    agent.speed = 15f;
                }
            }
            else
            {
                agent.SetDestination(despawnPoint.position);
                agent.speed = 15f;
            }
        }
    }

    private Transform FindAvailableFuelStation()
    {
        var availableStations = new List<Transform>();

        foreach (Transform station in fuelStations)
        {
            if (station.childCount < maxVehiclesPerFuelStation)
            {
                availableStations.Add(station);
            }
        }

        if (availableStations.Count > 0)
        {
            return availableStations[Random.Range(0, availableStations.Count)];
        }

        return null;
    }
}
