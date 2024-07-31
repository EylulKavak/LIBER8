using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    private QueueManager queueManager;
    private CustomerSpawner customerSpawner;
    private NavMeshAgent agent;
    private Transform despawnPoint;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        despawnPoint = GameObject.Find("DespawnPoint").transform;

        customerSpawner = FindObjectOfType<CustomerSpawner>();
        queueManager = FindObjectOfType<QueueManager>();

        if (queueManager != null)
        {
            queueManager.AddCustomerToQueue(this);
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
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (queueManager != null && queueManager.IsFirstCustomer(this))
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Rotate the first customer to face the opposite direction (180 degrees)
                Vector3 backwardDirection = -Vector3.right; // Facing towards negative x-axis
                transform.rotation = Quaternion.LookRotation(backwardDirection);
            }
        }
    }
}
