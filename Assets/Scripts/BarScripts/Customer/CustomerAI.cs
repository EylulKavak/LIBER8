using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private QueueManager queueManager;
    private CustomerSpawner customerSpawner;
    private NavMeshAgent agent;
    private Transform despawnPoint;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        despawnPoint = GameObject.Find("DespawnPoint").transform;

        customerSpawner = FindObjectOfType<CustomerSpawner>();

        queueManager = FindObjectOfType<QueueManager>(); // Find the queue manager in the scene
        if (queueManager != null)
        {
            queueManager.AddCustomerToQueue(this); // Add customer to the queue
        }
    }

    public void SetDestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    public void OrderCompleted()
    {
        if (queueManager != null)
        {
            queueManager.RemoveCustomerFromQueue(this);
        }
        SetDestination(despawnPoint);
        StartCoroutine(WaitAndDestroy());
        customerSpawner.NotifyCustomerServed();
    }

    private IEnumerator WaitAndDestroy()
    {
        while (Vector3.Distance(transform.position, despawnPoint.position) > 1f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2)));
    }
}
