using UnityEngine;

public class QuestStart : MonoBehaviour
{
    [Header("Quest Information")]
    public string questTitle;
    public string questDescription;

    public QuestSystem questSystem;

    public void GiveQuest()
    {
        if (questSystem != null)
        {
            questSystem.SetNewQuest(questTitle, questDescription);
        }
        else
        {
            Debug.LogError("QuestSystem reference is not set!");
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        GiveQuest();
    }
}
