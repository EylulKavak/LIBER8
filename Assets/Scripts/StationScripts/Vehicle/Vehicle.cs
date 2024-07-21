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

    private void Start()
    {
        // NavMeshAgent ve CarFuelPump referanslarını al
        agent = GetComponent<NavMeshAgent>();
        carFuelPump = GetComponentInChildren<CarFuelPump>();
        
        // Find the GasPumpUI object and get the GasPumpUI component from its child
        GameObject gasPumpUIObject = GameObject.Find("GasPumpUI"); // Adjust the name if necessary
        if (gasPumpUIObject != null)
        {
            gasPumpUI = gasPumpUIObject.GetComponent<GasPumpUI>();
        }
        else
        {
            Debug.LogError("GasPumpUI object not found!");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found in children.");
        }
        
        if (carFuelPump == null)
        {
            Debug.LogError("CarFuelPump component not found in children.");
        }
    }

    private void Update()
    {
        // Check if the agent or its destination is still valid
        if (agent != null)
        {
            if (carFuelPump != null && carFuelPump.pumpNuzzleHolder.childCount == 1 && Input.GetKey(KeyCode.H))
            {
                isFilledOnce = true;
            }

            if (isFilledOnce && carFuelPump.pumpNuzzleHolder.childCount == 0)
            {
                // Arabanın benzini dolduruldu anlamına gelir
                carFuelled = true;
            }

            if (carFuelled)
            {
                // Araç benzin aldıysa
                if (agent != null)
                {
                    isFilledOnce = false;
                    // Araç, benzinlik noktasının childından çıkacak
                    transform.SetParent(null);

                    // Despawn noktasına hareket et
                    Transform despawnPoint = GameObject.Find("DespawnPosition")?.transform;
                    if (despawnPoint != null)
                    {
                        agent.SetDestination(despawnPoint.position);
                        // Araç hızını 15 yap
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
}
