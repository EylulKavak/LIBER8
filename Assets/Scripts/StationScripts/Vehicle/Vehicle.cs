using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    private NavMeshAgent agent;
    private CarFuelPump carFuelPump; // CarFuelPump scriptinin referansı
    private GasPumpUI gasPumpUI;
    public bool carFuelled = false;
    public bool isFilledOnce = false;
    private Transform despawnPoint;

    private void Start()
    {
        despawnPoint = GameObject.Find("DespawnPosition").transform;
        // NavMeshAgent ve CarFuelPump referanslarını al
        agent = GetComponent<NavMeshAgent>();
        carFuelPump = GetComponentInChildren<CarFuelPump>();
        
        // GasPumpUI objesini bul ve GasPumpUI bileşenini al
        GameObject gasPumpUIObject = GameObject.Find("GasPumpUI"); // Gerekirse ismi ayarla
        if (gasPumpUIObject != null)
        {
            gasPumpUI = gasPumpUIObject.GetComponent<GasPumpUI>();
        }
        else
        {
            Debug.LogError("GasPumpUI object not found!");
        }
    }

    private void Update()
    {
        StartCoroutine(WaitAndDestroy());
        if (agent != null)
        {
            if (carFuelPump != null && carFuelPump.pumpNuzzleHolder.childCount == 1 && Input.GetKey(KeyCode.H))
            {
                isFilledOnce = true;
            }

            if (isFilledOnce && carFuelPump.pumpNuzzleHolder.childCount == 0)
            {
                carFuelled = true;
            }

            if (carFuelled)
            {
                if (agent != null)
                {
                    isFilledOnce = false;
                    transform.SetParent(null); // Araç benzinlik noktasının childından çıkacak

                    if (despawnPoint != null)
                    {
                        agent.SetDestination(despawnPoint.position);
                        agent.speed = 15f;
                    }
                    else
                    {
                        Debug.LogWarning("DespawnPosition not found.");
                    }
                }
            }
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        // Despawn noktasına ulaşıldığında biraz bekleyip aracı yok eder
        while (Vector3.Distance(transform.position, despawnPoint.position) > 1f)
        {
            yield return null; // Bir sonraki frame'e kadar bekle
        }
        Destroy(gameObject); // Aracı yok et
    }
}
