using UnityEngine;

public class TapButton : MonoBehaviour, IInteractable
{
    public GameObject filledCupPrefab;
    [SerializeField]
    private AudioClip fillSound;

    public void Interact(PlayerInteract player)
    {
        if (player.isInUse)
        {
            ReplaceGlassWithFilledCup(player.glassHolder, filledCupPrefab);
            player.isInUse = false;
        }
        else
        {
            Debug.Log("There is no cup at the tap or you already filled the cup");
        }
    }

    private void ReplaceGlassWithFilledCup(Transform glassHolder, GameObject filledCupPrefab)
    {
        foreach (Transform child in glassHolder)
        {
            Destroy(child.gameObject);
        }

        GameObject filledCup = Instantiate(filledCupPrefab, glassHolder.position, glassHolder.rotation);
        filledCup.transform.SetParent(glassHolder);

        Animator filledCupAnimator = filledCup.GetComponent<Animator>();
        if (filledCupAnimator != null)
        {
            filledCupAnimator.SetTrigger("Fill");
        }
        else
        {
            Debug.LogWarning("Filled cup prefab does not have an Animator component.");
        }
        AudioSource.PlayClipAtPoint(fillSound, transform.position);
    }
}
