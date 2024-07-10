using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    public Transform glassHolder;
    public GameObject itemHolder;
    public Transform iceHolder;
    public Transform fruitHolder;
    private GameObject currentCup = null;
    private Animator itemHolderAnimator;
    private Animator glassAnimator;
    public bool isInUse;

    [HideInInspector] public int iceCount = 0;
    [HideInInspector] public int fruitCount = 0;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

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
            if(currentCup != null && (iceCount < 2 || fruitCount < 1))
            {
                //Hem bardak hem de itemHolder dönme animasyonu yapıyor. Sadece itemHolder dönme animasyonu yapınca bardağın üzerindeki sıvı hareket etmiyordu o yüzden aynı animasyonu ikiye böldüm.
                if (glassHolder.childCount > 0)
                {
                    glassAnimator = glassHolder.GetChild(0).GetComponent<Animator>();
                }
                glassAnimator.SetTrigger("Slosh");
                itemHolderAnimator.SetTrigger("Glass");

            }
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    public void PickUpCup(GameObject cup)
    {
        currentCup = cup;
        if (currentCup != null)
        {
            itemHolderAnimator = itemHolder.GetComponent<Animator>();
            currentCup.transform.SetParent(transform);
        }
    }

    public bool HasCup()
    {
        return currentCup != null;
    }

    public void ReturnCup()
    {
        if (currentCup != null)
        {
            foreach (Transform child in glassHolder)
            {
                Destroy(child.gameObject);
            }
            currentCup = null;
            foreach (Transform child in iceHolder)
            {
                Destroy(child.gameObject);
                iceCount = 0;
            }

            foreach (Transform child in fruitHolder)
            {
                Destroy(child.gameObject);
                fruitCount = 0;
            }
        }
    }

    public void ShowMessage(string message)
    {
        Debug.Log(message);
    }
}
