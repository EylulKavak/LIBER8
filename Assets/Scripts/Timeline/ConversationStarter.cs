using DialogueEditor;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{
    private void Start()
    {
        ConversationManager.Instance.StartConversation(GetComponent<NPCConversation>());
    }
}
