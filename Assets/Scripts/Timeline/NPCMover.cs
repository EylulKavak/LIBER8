using UnityEngine;
using UnityEngine.AI;

public class NPCMover : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2)));
    }
    public void Go(Transform transform)
    {
        agent.SetDestination(transform.position);
    }
}
