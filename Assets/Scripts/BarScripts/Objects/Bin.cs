using UnityEngine;

public class Bin : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteract player)
    {
        if(player.HasCup())
        {
            player.ReturnCup();
        }else
        {
            Debug.Log("Elinizde bardak yok");
        }
    }
}
