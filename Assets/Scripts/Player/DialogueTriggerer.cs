using DialogueEditor;
using UnityEngine;

public class DialogueTriggerer : MonoBehaviour
{
    public float rayDistance = 2f;
    public LayerMask npcLayer;
    public GameObject interactText;
    NPCConversation currentConversation;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, npcLayer))
        {
            currentConversation = hit.collider.GetComponent<NPCConversation>();
            if (currentConversation.enabled)
            {
                interactText.SetActive(true);
            }
        }
        else
        {
            currentConversation = null;
            interactText.SetActive(false);
        }
    }
    public void StartDialgoue()
    {
        if (currentConversation != null && currentConversation.enabled)
        {
            ConversationManager.Instance.StartConversation(currentConversation);
        }
    }
    private void OnEnable()
    {
        Invoke(nameof(End), 0.2f);
    }
    private void End()
    {
        ConversationManager.Instance.EndButtonSelected();
    }
}
