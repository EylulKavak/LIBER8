using UnityEngine;
using TMPro;
using System.Collections;

public class QuestSystem : MonoBehaviour
{
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public Color completedQuestColor = Color.green;
    public float fadeOutDuration = 2.0f;
    public float moveUpDistance = 50f;
    public float moveUpDuration = 2.0f;

    private string currentQuestTitle;
    private string currentQuestDescription;

    private void Start()
    {
        if (questTitleText == null || questDescriptionText == null)
        {
            Debug.LogError("Quest Texts are not assigned.");
        }
    }

    public void SetNewQuest(string title, string description)
    {
        if (!string.IsNullOrEmpty(currentQuestTitle))
        {
            StartCoroutine(FadeOutAndReplace(currentQuestTitle, currentQuestDescription, title, description));
        }
        else
        {
            questTitleText.text = title;
            questDescriptionText.text = description;
            currentQuestTitle = title;
            currentQuestDescription = description;
        }
    }

    private IEnumerator FadeOutAndReplace(string oldTitle, string oldDescription, string newTitle, string newDescription)
    {
        TextMeshProUGUI oldTitleText = Instantiate(questTitleText, questTitleText.transform.parent);
        TextMeshProUGUI oldDescriptionText = Instantiate(questDescriptionText, questDescriptionText.transform.parent);

        oldTitleText.text = oldTitle;
        oldDescriptionText.text = oldDescription;

        Color originalTitleColor = oldTitleText.color;
        Color originalDescriptionColor = oldDescriptionText.color;

        oldTitleText.color = completedQuestColor;
        oldDescriptionText.color = completedQuestColor;

        Vector3 originalTitlePosition = oldTitleText.transform.localPosition;
        Vector3 originalDescriptionPosition = oldDescriptionText.transform.localPosition;

        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / fadeOutDuration);
            oldTitleText.color = new Color(completedQuestColor.r, completedQuestColor.g, completedQuestColor.b, alpha);
            oldDescriptionText.color = new Color(completedQuestColor.r, completedQuestColor.g, completedQuestColor.b, alpha);
            oldTitleText.transform.localPosition = originalTitlePosition + Vector3.up * moveUpDistance * (t / fadeOutDuration);
            oldDescriptionText.transform.localPosition = originalDescriptionPosition + Vector3.up * moveUpDistance * (t / fadeOutDuration);
            yield return null;
        }

        Destroy(oldTitleText.gameObject);
        Destroy(oldDescriptionText.gameObject);

        questTitleText.text = newTitle;
        questDescriptionText.text = newDescription;
        currentQuestTitle = newTitle;
        currentQuestDescription = newDescription;
    }
}
