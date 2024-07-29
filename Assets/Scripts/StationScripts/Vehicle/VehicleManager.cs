using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleManager : MonoBehaviour
{
    public GameObject[] vehiclePrefabs; // Araç prefab'leri
    public Transform spawnPoint; // Araçların spawn edileceği noktalar
    public Transform despawnPoint; // Araçların despawn edileceği nokta
    public Transform[] fuelStations; // Benzinlik noktaları
    public float minSpawnTime = 100f; // Minimum spawn süresi
    public float maxSpawnTime = 150f; // Maksimum spawn süresi
    public int maxVehiclesPerFuelStation = 1; // Bir benzinlik noktasında maksimum araç sayısı
    public int maxVehiclesToSpawn = 10; // Spawn edilecek maksimum araç sayısı

    private GameObject lastSpawnedVehiclePrefab; // Son spawn edilen araç prefab'ı
    private int vehiclesSpawned = 0; // Spawn edilen toplam araç sayısı

    private void Start()
    {
        StartCoroutine(SpawnVehicles());
    }

    private IEnumerator SpawnVehicles()
    {
        while (vehiclesSpawned < maxVehiclesToSpawn)
        {
            // Araçları spawn et
            SpawnVehicle();

            // Rastgele bir süre bekle
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }

        // Maksimum araç sayısına ulaşıldıysa, scripti kapat
        gameObject.SetActive(false);
    }

    private void SpawnVehicle()
    {
        GameObject vehiclePrefab;
        do
        {
            // Rastgele bir araç prefab'ı seç
            vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        } while (vehiclePrefab == lastSpawnedVehiclePrefab); // Aynı araç prefab'ını seçmemek için

        lastSpawnedVehiclePrefab = vehiclePrefab; // Son araç prefab'ını güncelle

        GameObject vehicle = Instantiate(vehiclePrefab, spawnPoint.position, spawnPoint.rotation);
        vehiclesSpawned++; // Spawn edilen araç sayısını artır

        NavMeshAgent agent = vehicle.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            // %60 ihtimalle benzinlik noktalarından birinin önüne git
            if (Random.value < 0.6f)
            {
                Transform target = FindAvailableFuelStation();
                if (target != null)
                {
                    // Araç, boşta olan benzinlik noktasına yönlendir
                    agent.SetDestination(target.position);
                    vehicle.transform.SetParent(target); // Aracı benzinlik noktasının çocuğu yap
                    // Benzinlikteyken hızını 7 yap
                    agent.speed = 7f;
                }
                else
                {
                    // Boşta benzinlik noktası yoksa despawn noktasına git
                    agent.SetDestination(despawnPoint.position);
                    // Benzinlikte değilken hızını 15 yap
                    agent.speed = 15f;
                }
            }
            else
            {
                // %40 ihtimalle despawn noktasına git
                agent.SetDestination(despawnPoint.position);
                // Benzinlikte değilken hızını 15 yap
                agent.speed = 15f;
            }
        }
    }

    private Transform FindAvailableFuelStation()
    {
        // Boşta olan benzinlik noktalarını listele
        var availableStations = new List<Transform>();

        foreach (Transform station in fuelStations)
        {
            if (station.childCount < maxVehiclesPerFuelStation)
            {
                availableStations.Add(station);
            }
        }

        // Boşta olan benzinlik noktalarından birini rastgele seç
        if (availableStations.Count > 0)
        {
            return availableStations[Random.Range(0, availableStations.Count)];
        }

        return null; // Eğer boşta benzinlik noktası yoksa
    }
}
