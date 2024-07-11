using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour, IInteractable
{
    public string[] fruits = {};
    public string[] sodas = {};
    public TMP_Text orderText; // UI Text to show order details

    private string orderedFruit;
    private string orderedSoda;
    private int orderedIce;
    private bool hasOrder = false;

    public void Interact(PlayerInteract player)
    {
        if (!hasOrder)
        {
            GenerateOrder();
        }
        else
        {
            CheckOrder(player);
        }
    }

    private void GenerateOrder()
    {
        orderedFruit = fruits[Random.Range(0, fruits.Length)];
        orderedSoda = sodas[Random.Range(0, sodas.Length)];
        orderedIce = Random.Range(0, 3); // 0, 1, or 2 ice cubes

        hasOrder = true;
        string orderDetails = $"Order: {orderedFruit} fruit, {orderedSoda} soda, {orderedIce} ice cubes.";
        Debug.Log(orderDetails);
        
        // Update the UI text with the order details
        if (orderText != null)
        {
            orderText.text = orderDetails;
        }
    }

    private void CheckOrder(PlayerInteract player)
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
            isOrderCorrect = false;
        }

        if (player.iceCount != orderedIce)
        {
            Debug.LogWarning("Wrong amount of ice.");
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
            isOrderCorrect = false;
        }

        if (isOrderCorrect)
        {
            Debug.Log("Order is correct!");
            player.ReturnCup();
            hasOrder = false;

            // Clear the UI text
            if (orderText != null)
            {
                orderText.text = "";
            }
        }
    }
}
