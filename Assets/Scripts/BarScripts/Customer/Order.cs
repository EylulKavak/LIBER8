using UnityEngine;
using TMPro;

public class Order : MonoBehaviour, IInteractable
{
    public string[] fruits = {"Lemon", "Orange"};
    public string[] sodas = {"Red", "Yellow", "Blue","Orange","Purple","Green"};
    private TMP_Text orderText; // UI Text to show order details
    private TMP_Text timerText; // UI Text to show the remaining time
    private TMP_Text warningText; // UI Text to show warning messages

    private string orderedFruit;
    private string orderedSoda;
    private int orderedIce;
    private bool hasOrder = false;
    private bool canOrder = true;

    private Lives lives;

    [SerializeField] private float orderTimer = 30f; // 30 seconds timer
    private bool timerRunning = false;

    private void Start()
    {
        orderText = GameObject.Find("orderText").GetComponent<TMP_Text>();
        timerText = GameObject.Find("timerText").GetComponent<TMP_Text>();
        warningText = GameObject.Find("warning").GetComponent<TMP_Text>();
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            lives = player.GetComponent<Lives>();
        }
    }
    public void Interact(PlayerInteract player)
    {
        if (!hasOrder && canOrder)
        {
            GenerateOrder();
        }
        else
        {
            CheckOrder(player);
        }
    }

    public void GenerateOrder()
    {
        orderedFruit = fruits[Random.Range(0, fruits.Length)];
        orderedSoda = sodas[Random.Range(0, sodas.Length)];
        orderedIce = Random.Range(0, 3); // 0, 1, or 2 ice cubes

        hasOrder = true;
        timerRunning = true; // Start the timer
        orderTimer = 30f; // Reset the timer

        string orderDetails = $"Order:  {orderedIce} Ice Cubes, {orderedSoda} Soda, {orderedFruit} Fruit";
        Debug.Log(orderDetails);
        
        // Update the UI text with the order details
        if (orderText != null)
        {
            orderText.text = orderDetails;
        }
    }

    public void CheckOrder(PlayerInteract player)
    {
        bool isOrderCorrect = true;

        // Check glass contents
        bool sodaCorrect = false;
        foreach (Transform child in player.glassHolder)
        {
            if (child.name.Contains(orderedSoda))
            {
                sodaCorrect = true;
            }
        }
        if (!sodaCorrect)
        {
            Debug.LogWarning("Wrong soda type.");
            if (warningText != null)
            {
                warningText.text = "Wrong soda type.";
            }
            isOrderCorrect = false;
        }

        if (player.iceCount != orderedIce)
        {
            Debug.LogWarning("Wrong amount of ice.");
            if (warningText != null)
            {
                warningText.text += "\nWrong amount of ice.";
            }
            isOrderCorrect = false;
        }

        bool fruitCorrect = false;
        foreach (Transform child in player.fruitHolder)
        {
            if (child.name.Contains(orderedFruit))
            {
                fruitCorrect = true;
            }
        }
        if (!fruitCorrect)
        {
            Debug.LogWarning("Wrong or missing fruit type.");
            if (warningText != null)
            {
                warningText.text += "\nWrong or missing fruit type.";
            }
            isOrderCorrect = false;
        }

        if (isOrderCorrect)
        {
            Debug.Log("Order is correct!");
            player.ReturnCup();
            hasOrder = false;
            timerRunning = false; // Stop the timer
            ClearUI();
        }
        else
        {
            hasOrder = false;
            timerRunning = false; // Stop the timer
            ClearUI();
            lives.liveCount--;
        }
        canOrder = false;
        CustomerAI customer = GetComponent<CustomerAI>(); // Adjust based on actual setup
        if (customer != null)
        {
            customer.OrderCompleted();
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            orderTimer -= Time.deltaTime;

            // Update the timer text
            if (timerText != null)
            {
                timerText.text = $"Kalan Zaman: {orderTimer:F1}"; // Show the time with one decimal place
            }

            if (orderTimer <= 0)
            {
                Debug.LogWarning("Order not delivered in time!");
                if (warningText != null)
                {
                    warningText.text = "Order not delivered in time!";
                }
                hasOrder = false;
                timerRunning = false;
                orderTimer = 30f; // Reset the timer for the next order
                lives.liveCount--;

                ClearUI();
            }
        }
    }
    private void ClearUI()
    {
        // Clear the UI text
        if (orderText != null)
        {
            orderText.text = "";
        }
        if (timerText != null)
        {
            timerText.text = "";
        }
        if (warningText != null)
        {
            warningText.text = "";
        }
    }
}