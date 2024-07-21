using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarFuelPump : MonoBehaviour, IInteractableGas
{
    public Transform pumpNuzzleHolder;
    private GameObject currentNuzzle = null;
    private GasPumpUI gasPumpUI;
    private Vehicle vehicle;

    private bool isInitialized = false;

    private void Start()
    {
        // Find the GasPumpUI object and get the GasPumpUI component from its child
        GameObject gasPumpUIObject = GameObject.Find("GasPumpUI"); // Adjust the name if necessary
        if (gasPumpUIObject != null)
        {
            gasPumpUI = gasPumpUIObject.GetComponent<GasPumpUI>();
        }
        else
        {
            Debug.LogError("GasPumpUI object not found!");
        }
        vehicle = GetComponentInParent<Vehicle>();
    }
    public void Interact(PlayerInteractGas player)
    {
        if (currentNuzzle == null && player.HasNuzzle())
        {            
            if(vehicle.carFuelled)
            {
                Debug.Log("Benzin doldurma işlemi yapıldı");
            }
            else
            {
                currentNuzzle = player.itemHolder.transform.GetChild(1).gameObject;
                player.PickUpNuzzle(null);
                StartCoroutine(MoveNuzzleToPlace(currentNuzzle, pumpNuzzleHolder)); 
                gasPumpUI.StartFilling();
            }

            // İlk etkileşimde benzin ve hedef ayarla
            if (!isInitialized)
            {
                float randomCurrentFuel = Random.Range(0.1f, 0.3f);
                float randomTargetFuel = Random.Range(randomCurrentFuel + 0.2f, 1f);

                gasPumpUI.SetFuelLevel(randomCurrentFuel);
                gasPumpUI.SetTargetFuel(randomTargetFuel);
                isInitialized = true; // İlk etkileşimi tamamla
            }
            currentNuzzle = null;
            player.isInUse = true;
        }
    }

    private IEnumerator MoveNuzzleToPlace(GameObject nuzzle, Transform target)
    {
        float duration = 0.5f;
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