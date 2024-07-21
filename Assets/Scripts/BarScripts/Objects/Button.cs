using UnityEngine;

public class TapButton : MonoBehaviour, IInteractable
{
    public GameObject yellowCupPrefab;
    public GameObject redCupPrefab;
    public GameObject blueCupPrefab;
    public GameObject greenCupPrefab;
    public GameObject purpleCupPrefab;
    public GameObject orangeCupPrefab;
    [SerializeField]
    private AudioClip fillSound;

    public PlayerInteract.CupColor buttonColor;

    public void Interact(PlayerInteract player)
    {
        if (player.isInUse)
        {
            if (player.IsIntermediateColor())
            {
                player.ShowMessage("You already have a mixed cup. You can't add more. You need to return the current cup and get a new one.");
                return;
            }

            PlayerInteract.CupColor newColor = GetNewCupColor(player.currentCupColor, buttonColor);
            GameObject filledCupPrefab = GetCupPrefabByColor(newColor);

            ReplaceGlassWithFilledCup(player.glassHolder, filledCupPrefab, player.currentCupColor, newColor);
            player.currentCupColor = newColor;
            player.isInUse = false;
        }
        else
        {
            Debug.Log("There is no cup at the tap or you already filled the cup");
        }
    }

    private void ReplaceGlassWithFilledCup(Transform glassHolder, GameObject filledCupPrefab, PlayerInteract.CupColor currentColor, PlayerInteract.CupColor newColor)
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
            if (newColor == PlayerInteract.CupColor.Green || newColor == PlayerInteract.CupColor.Purple || newColor == PlayerInteract.CupColor.Orange)
            {
                string fadeAnimation = GetFadeAnimation(currentColor, newColor);
                filledCupAnimator.SetTrigger(fadeAnimation);
            }
            else
            {
                filledCupAnimator.SetTrigger("Fill");
            }
        }
        else
        {
            Debug.LogWarning("Filled cup prefab does not have an Animator component.");
        }

        AudioSource.PlayClipAtPoint(fillSound, transform.position);
    }

    private PlayerInteract.CupColor GetNewCupColor(PlayerInteract.CupColor currentColor, PlayerInteract.CupColor buttonColor)
    {
        if (currentColor == PlayerInteract.CupColor.None)
            return buttonColor;

        if ((currentColor == PlayerInteract.CupColor.Blue && buttonColor == PlayerInteract.CupColor.Yellow) ||
            (currentColor == PlayerInteract.CupColor.Yellow && buttonColor == PlayerInteract.CupColor.Blue))
        {
            return PlayerInteract.CupColor.Green;
        }
        if ((currentColor == PlayerInteract.CupColor.Blue && buttonColor == PlayerInteract.CupColor.Red) ||
            (currentColor == PlayerInteract.CupColor.Red && buttonColor == PlayerInteract.CupColor.Blue))
        {
            return PlayerInteract.CupColor.Purple;
        }
        if ((currentColor == PlayerInteract.CupColor.Yellow && buttonColor == PlayerInteract.CupColor.Red) ||
            (currentColor == PlayerInteract.CupColor.Red && buttonColor == PlayerInteract.CupColor.Yellow))
        {
            return PlayerInteract.CupColor.Orange;
        }

        return buttonColor;
    }

    private GameObject GetCupPrefabByColor(PlayerInteract.CupColor color)
    {
        switch (color)
        {
            case PlayerInteract.CupColor.Yellow:
                return yellowCupPrefab;
            case PlayerInteract.CupColor.Red:
                return redCupPrefab;
            case PlayerInteract.CupColor.Blue:
                return blueCupPrefab;
            case PlayerInteract.CupColor.Green:
                return greenCupPrefab;
            case PlayerInteract.CupColor.Purple:
                return purpleCupPrefab;
            case PlayerInteract.CupColor.Orange:
                return orangeCupPrefab;
            default:
                return null;
        }
    }

    private string GetFadeAnimation(PlayerInteract.CupColor currentColor, PlayerInteract.CupColor newColor)
    {
        if (currentColor == PlayerInteract.CupColor.Blue && newColor == PlayerInteract.CupColor.Green)
            return "FadeBlueGreen";
        if (currentColor == PlayerInteract.CupColor.Yellow && newColor == PlayerInteract.CupColor.Green)
            return "FadeYellowGreen";
        if (currentColor == PlayerInteract.CupColor.Blue && newColor == PlayerInteract.CupColor.Purple)
            return "FadeBluePurple";
        if (currentColor == PlayerInteract.CupColor.Red && newColor == PlayerInteract.CupColor.Purple)
            return "FadeRedPurple";
        if (currentColor == PlayerInteract.CupColor.Yellow && newColor == PlayerInteract.CupColor.Orange)
            return "FadeYellowOrange";
        if (currentColor == PlayerInteract.CupColor.Red && newColor == PlayerInteract.CupColor.Orange)
            return "FadeRedOrange";

        return "Fill";
    }
}

