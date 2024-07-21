using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPump : MonoBehaviour, IInteractableGas
{
    [SerializeField] private int GasStationNumber;
    public Transform pumpNuzzleHolder;
    private GameObject currentNuzzle = null;

    public void Interact(PlayerInteractGas player)
    {
        if (currentNuzzle == null && player.HasNuzzle())
        {
            GameObject nuzzle = player.itemHolder.transform.GetChild(1).gameObject;
            Nuzzle nuzzleScript = nuzzle.GetComponent<Nuzzle>();

            if (nuzzleScript != null && nuzzleScript.nuzzleNumber == GasStationNumber)
            {
                currentNuzzle = nuzzle;
                player.PickUpNuzzle(null);
                StartCoroutine(MoveNuzzleToPlace(currentNuzzle, pumpNuzzleHolder));
                currentNuzzle = null;
            }
            else
            {
                Debug.LogWarning("Wrong nuzzle for this gas station!");
            }
        }
    }

    private IEnumerator MoveNuzzleToPlace(GameObject nuzzle, Transform target)
    {
        float duration = 0.5f;
        Vector3 startPosition = nuzzle.transform.position;
        Quaternion startRotation = nuzzle.transform.rotation;
        Quaternion targetRotation = target.rotation;

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
