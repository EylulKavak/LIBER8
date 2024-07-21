using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractGas : MonoBehaviour
{
    public GameObject itemHolder;
    private GameObject currentNuzzle = null;
    public Transform playerCamera;
    public bool isInUse;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f))
        {
            IInteractableGas interactable = hit.transform.GetComponent<IInteractableGas>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    public void PickUpNuzzle(GameObject nuzzle)
    {
        currentNuzzle = nuzzle;
        if (currentNuzzle != null)
        {
            currentNuzzle.transform.SetParent(transform);
        }
    }

    public bool HasNuzzle()
    {
        return currentNuzzle != null;
    }
}