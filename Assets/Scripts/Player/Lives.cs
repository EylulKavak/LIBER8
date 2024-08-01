using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Lives : MonoBehaviour
{
    public float liveCount = 3f;
    private float previousLiveCount;

    [SerializeField] private Image image;

    private void Start()
    {
        previousLiveCount = liveCount;
        if (image != null)
        {
            // Start with the image disabled
            image.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckLives();
        Restart();
    }

    private void CheckLives()
    {
        if (liveCount < previousLiveCount)
        {
            Debug.Log($"Can azaldı! Kalan can: {liveCount}");
            previousLiveCount = liveCount;
            if (image != null)
            {
                StartCoroutine(FlashRed());
            }
        }
    }

    private void Restart()
    {
        if (liveCount < 0.5f)
        {
            Debug.Log("Yeniden Deneyin");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator FlashRed()
    {
        float duration = 0.5f; // Duration of one cycle (0 to max alpha and back to 0)
        float maxAlpha = 0.4f; // Maximum alpha value

        // Enable the image and set its alpha to 0
        image.gameObject.SetActive(true);
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, maxAlpha, t / duration);
            image.color = color;
            yield return null;
        }

        // Ensure the image reaches full alpha
        color.a = maxAlpha;
        image.color = color;

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(maxAlpha, 0, t / duration);
            image.color = color;
            yield return null;
        }

        // Ensure the image is fully transparent and disable it
        color.a = 0f;
        image.color = color;
        image.gameObject.SetActive(false);
    }
}
