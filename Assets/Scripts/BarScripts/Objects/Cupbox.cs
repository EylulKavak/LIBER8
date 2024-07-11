using UnityEngine;
using System.Collections;

public class CupBox : MonoBehaviour, IInteractable
{
    public GameObject cupPrefab;
    private GameObject currentCup;
    public Transform spawnPosition;
    private float transitionDuration = 0.5f; // Geçiş süresi
    [SerializeField]
    private AudioClip cupSound;


    public void Interact(PlayerInteract player)
    {
        if (player.HasCup())
        {
            player.ReturnCup();
        }
        else if(!player.isInUse)
        {
            AudioSource.PlayClipAtPoint(cupSound, transform.position);
            StartCoroutine(SpawnAndMoveCup(spawnPosition, player.glassHolder));
            player.PickUpCup(currentCup);
        }

        if(player.isInUse)
        {
            Debug.Log("First take your cup from soda machine");
        }
    }

    private IEnumerator SpawnAndMoveCup(Transform spawnTransform, Transform targetTransform)
    {
        // Bardağı spawnla
        currentCup = Instantiate(cupPrefab, spawnTransform.position, spawnTransform.rotation);
        Vector3 startPosition = currentCup.transform.position;
        Quaternion startRotation = currentCup.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            if (currentCup == null) yield break; // Eğer bardak silinirse Coroutine'i durdur

            // Bardak her zaman hedef pozisyona doğru hareket eder
            currentCup.transform.position = Vector3.Lerp(startPosition, targetTransform.position, elapsedTime / transitionDuration);
            currentCup.transform.rotation = Quaternion.Lerp(startRotation, targetTransform.rotation, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Geçiş tamamlandığında bardağı hedef konuma ve hedefin altına yerleştir
        if (currentCup != null)
        {
            currentCup.transform.position = targetTransform.position;
            currentCup.transform.rotation = targetTransform.rotation;
            currentCup.transform.SetParent(targetTransform); // Hedefin çocuğu olur
        }
    }
}
