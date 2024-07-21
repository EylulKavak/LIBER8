using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DartThrower : MonoBehaviour
{
    public GameObject dartPrefab;
    public Transform dartSpawnPoint;
    public Transform dartBoard;
    public float throwForce = 10f;
    public int maxThrows = 3;
    private int currentThrows = 0;
    private int totalScore = 0;

    // Dartboard area definitions
    private Vector3 bullseyeCenter;
    private float bullseyeRadius = 0.1f;
    private Vector3 outerRingCenter;
    private float outerRingRadius = 0.2f;
    private Vector3 middleRingCenter;
    private float middleRingRadius = 0.3f;
    private Vector3 innerRingCenter;
    private float innerRingRadius = 0.4f;

    // UI element for displaying score
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public Button restartButton;

    // UI element for start panel
    public GameObject startPanel;
    public Button startButton;

    void Start()
    {
        // Set the dartboard area centers relative to the dartBoard's position
        bullseyeCenter = dartBoard.position;
        outerRingCenter = dartBoard.position;
        middleRingCenter = dartBoard.position;
        innerRingCenter = dartBoard.position;

        // Initialize the score text
        UpdateScoreText();

        // Hide game over panel at start
        gameOverPanel.SetActive(false);

        // Show start panel at start
        startPanel.SetActive(true);

        // Assign restart button click event
        restartButton.onClick.AddListener(RestartGame);

        // Assign start button click event
        startButton.onClick.AddListener(StartGame);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentThrows < maxThrows && !startPanel.activeSelf)
        {
            Debug.Log("Mouse button down detected.");
            ThrowDart();
            currentThrows++;
        }

        // Check if all throws are used
        if (currentThrows >= maxThrows)
        {
            StartCoroutine(EndGame());
        }
    }

    void ThrowDart()
    {
        Debug.Log("ThrowDart method called.");

        if (dartPrefab == null)
        {
            Debug.LogError("Dart prefab not assigned!");
            return;
        }

        if (dartSpawnPoint == null)
        {
            Debug.LogError("Dart spawn point not assigned!");
            return;
        }

        if (dartBoard == null)
        {
            Debug.LogError("Dart board not assigned!");
            return;
        }

        GameObject dart = Instantiate(dartPrefab, dartSpawnPoint.position, dartPrefab.transform.rotation);
        Rigidbody rb = dart.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody component in Dart prefab!");
            return;
        }

        dart.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Vector3 direction = (dartBoard.position - dartSpawnPoint.position).normalized;
        direction.y += 0.05f;

        rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
        Debug.Log("Darts thrown!");

        // Puanlama için bir yöntem çağırabiliriz
        StartCoroutine(CalculateScore(dart));
    }

    IEnumerator CalculateScore(GameObject dart)
    {
        yield return new WaitForSeconds(0.2f);

        float distance = Vector3.Distance(dart.transform.position, dartBoard.position);
        int score = 0;

        if (distance <= bullseyeRadius)
        {
            score = 50; // Bullseye
        }
        else if (distance <= outerRingRadius)
        {
            score = 25; // Outer ring
        }
        else if (distance <= middleRingRadius)
        {
            score = 10; // Middle ring
        }
        else if (distance <= innerRingRadius)
        {
            score = 5; // Inner ring
        }

        totalScore += score;
        Debug.Log("Current Throw Score: " + score);
        Debug.Log("Total Score: " + totalScore);

        // Update the score text
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        // Show game over panel
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        // Hide start panel and start the game
        startPanel.SetActive(false);
    }
}
