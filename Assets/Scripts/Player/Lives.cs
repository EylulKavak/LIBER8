using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public float liveCount = 3f;
    private float previousLiveCount;

    private void Start()
    {
        previousLiveCount = liveCount;
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
}
