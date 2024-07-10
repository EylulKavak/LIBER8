using UnityEngine;
using System.Collections;

public class FruitBox : MonoBehaviour, IInteractable
{
    public GameObject fruitPrefab;
    public Transform spawnPosition;

    public void Interact(PlayerInteract player)
    {
        if (player.HasCup())
        {
            if (player.fruitCount < 1)
            {
                if (player.fruitHolder != null)
                {
                    GameObject fruit = Instantiate(fruitPrefab, spawnPosition.position, Quaternion.identity);
                    player.fruitCount++;
                    StartCoroutine(MoveFruitParabolically(fruit, player.fruitHolder));
                    Debug.Log("Fruit added to cup.");
                }
                else
                {
                    Debug.Log("FruitHolder not found on the cup.");
                }
            }
            else
            {
                Debug.Log("There is already a fruit in the cup!");
            }
        }
        else
        {
            player.ShowMessage("Please pick up a cup first!");
        }
    }

    private IEnumerator MoveFruitParabolically(GameObject fruit, Transform target)
    {
        float duration = 0.7f; // Hareket süresi (saniye)
        Vector3 startPosition = fruit.transform.position;
        Quaternion startRotation = fruit.transform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // fruitHolder'ın güncel pozisyonunu al
            Vector3 currentEndPosition = target.position;

            // Parabolik hareketi hesapla
            Vector3 currentPos = CalculateParabolicPoint(startPosition, currentEndPosition, t);

            fruit.transform.position = currentPos;
            fruit.transform.rotation = Quaternion.Lerp(startRotation, target.rotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Son konumu doğrula
        fruit.transform.position = target.position;
        fruit.transform.rotation = target.rotation;
        fruit.transform.SetParent(target);
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
