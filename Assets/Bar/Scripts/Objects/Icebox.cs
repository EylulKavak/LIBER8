using UnityEngine;
using System.Collections;

public class IceBox : MonoBehaviour, IInteractable
{
    public GameObject icePrefab;
    public Transform spawnPosition;
    [SerializeField]
    private AudioClip iceSound;

    
    public void Interact(PlayerInteract player)
    {
        if (player.HasCup())
        {
            if (player.iceCount < 2)
            {
                if (player.iceHolder != null)
                {
                    GameObject ice = Instantiate(icePrefab, spawnPosition.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(iceSound, transform.position);
                    StartCoroutine(MoveIceParabolically(ice, player.iceHolder));
                    player.iceCount++;
                    Debug.Log("Ice added to cup. Ice count: " + player.iceCount);
                }
                else
                {
                    Debug.Log("IceHolder not found on the cup.");
                }
            }
            else
            {
                Debug.Log("You cannot add more than 2 ice cubes.");
            }
        }
        else
        {
            player.ShowMessage("Please pick up a cup first!");
        }
    }

    private IEnumerator MoveIceParabolically(GameObject ice, Transform target)
    {
        float duration = 0.5f; // Hareket süresi (saniye)s
        Vector3 startPosition = ice.transform.position;
        Quaternion startRotation = ice.transform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // iceHolder'ın güncel pozisyonunu al
            Vector3 currentEndPosition = target.position;

            // Parabolik hareketi hesapla
            Vector3 currentPos = CalculateParabolicPoint(startPosition, currentEndPosition, t);

            ice.transform.position = currentPos;
            ice.transform.rotation = Quaternion.Lerp(startRotation, target.rotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Son konumu doğrula
        ice.transform.position = target.position;
        ice.transform.rotation = target.rotation;
        ice.transform.SetParent(target);
    }

    private Vector3 CalculateParabolicPoint(Vector3 start, Vector3 end, float t)
    {
        // Parabolik noktanın hesaplanması
        float mt = 1.0f - t;
        Vector3 a = mt * mt * start;
        Vector3 b = 2.0f * mt * t * (start + (end - start) * 0.5f + Vector3.up * 2.0f); // Orta nokta eklenir
        Vector3 c = t * t * end;
        return a + b + c;
    }
    
}
