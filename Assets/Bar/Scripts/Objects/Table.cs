using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour, IInteractable
{
    public Transform[] tablePositions;
    private GameObject currentCup = null;
    private int nextPositionIndex = 0;

    public void Interact(PlayerInteract player)
    {
        if (currentCup == null && player.HasCup())
        {
            currentCup = player.itemHolder.transform.GetChild(0).gameObject;
            player.PickUpCup(null);
            Transform targetPosition = GetNextTablePosition();
            StartCoroutine(MoveCupToTable(currentCup, targetPosition));
            player.isInUse = true;
            currentCup = null;
        }
    }

    private Transform GetNextTablePosition()
    {
        Transform target = tablePositions[nextPositionIndex];
        nextPositionIndex = (nextPositionIndex + 1) % tablePositions.Length;
        return target;
    }

    private IEnumerator MoveCupToTable(GameObject cup, Transform target)
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
