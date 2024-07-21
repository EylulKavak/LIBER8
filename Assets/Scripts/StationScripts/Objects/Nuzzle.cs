using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuzzle : MonoBehaviour, IInteractableGas
{
    public int nuzzleNumber;
    private GameObject currentNuzzle = null;
    private GameObject nuzzle;
    [SerializeField] private GasPumpUI gasPumpUI;

    public void Interact(PlayerInteractGas player)
    {
        if (currentNuzzle == null)
        {
            nuzzle = GameObject.Find("NuzzleObject" + nuzzleNumber);
            currentNuzzle = nuzzle;
            StartCoroutine(MoveNuzzleToItemHolder(currentNuzzle, player.itemHolder.transform));
            player.PickUpNuzzle(currentNuzzle);
            currentNuzzle = null;
            if(player.isInUse)
            {
                gasPumpUI.StopFilling();
            }
            player.isInUse = false;
        }
    }

    private IEnumerator MoveNuzzleToItemHolder(GameObject nuzzle, Transform target)
    {
        float duration = 0.2f;
        Vector3 startPosition = nuzzle.transform.position;
        Quaternion startRotation = nuzzle.transform.rotation;
        Quaternion targetRotation = target.rotation * Quaternion.Euler(0, -90, 0);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (nuzzle == null) yield break;

            nuzzle.transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            nuzzle.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (nuzzle != null)
        {
            nuzzle.transform.position = target.position;
            nuzzle.transform.rotation = targetRotation;
            nuzzle.transform.SetParent(target);
        }
    }
}