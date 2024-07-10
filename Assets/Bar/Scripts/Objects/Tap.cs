using UnityEngine;
using System.Collections;

public class Tap : MonoBehaviour, IInteractable
{
    public Transform tapHolder;
    private GameObject currentCup = null;

    public void Interact(PlayerInteract player)
    {
        if (currentCup == null && player.HasCup())
        {
            currentCup = player.itemHolder.transform.GetChild(0).gameObject;
            player.PickUpCup(null);
            StartCoroutine(MoveCupToTap(currentCup, tapHolder));
            player.isInUse = true;
            currentCup = null;
        }
    }

    private IEnumerator MoveCupToTap(GameObject cup, Transform target)
    {
        float duration = 0.5f;
        Vector3 startPosition = cup.transform.position;
        Quaternion startRotation = cup.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (cup == null) yield break;

            cup.transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            cup.transform.rotation = Quaternion.Lerp(startRotation, target.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (cup != null)
        {
            cup.transform.position = target.position;
            cup.transform.rotation = target.rotation;
            cup.transform.SetParent(target);
        }
    }
}

