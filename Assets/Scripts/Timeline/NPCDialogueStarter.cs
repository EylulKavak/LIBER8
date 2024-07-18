using DialogueEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPCDialogueStarter : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    NPCConversation conversation;
    Animator animator;
    bool order = false;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        conversation = GetComponent<NPCConversation>();
        animator = GetComponent<Animator>();
    }
    public void GoPlayer()
    {
        agent.SetDestination(player.position);
        order = true;
    }
    private void Update()
    {
        if (order)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) < 3)
            {
                ConversationManager.Instance.StartConversation(conversation);
                order = false;
            }
        }
        animator.SetFloat("Speed", Mathf.Sqrt(Mathf.Pow(agent.velocity.x, 2) + Mathf.Pow(agent.velocity.z, 2)));
    }
}
