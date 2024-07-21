using UnityEngine;
using System.Collections;

public class Cup : MonoBehaviour, IInteractable
{
    private GameObject currentCup = null;
    private GameObject glass;

    public void Interact(PlayerInteract player)
    {
        if (currentCup == null)
        {
            glass = GameObject.Find("Glass");
            currentCup = glass;
            StartCoroutine(MoveCupToItemHolder(currentCup, player.itemHolder.transform));
            player.PickUpCup(currentCup);
            currentCup = null;
            player.isInUse = false;
        }
    }

    private IEnumerator MoveCupToItemHolder(GameObject cup, Transform target)
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
