using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    public string[] fruits = { "Orange", "Lemon" };
    public string[] sodas = {};
    //public GameObject orderPanel; // UI panel to show order details

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
        Debug.Log($"Order: {orderedFruit} fruit, {orderedSoda} soda, {orderedIce} ice cubes.");
        // You can update your orderPanel UI here to show the order details
    }

    private void CheckOrder(PlayerInteract player)
    {
        bool isOrderCorrect = true;

        // Check glass contents
        bool sodaCorrect = false;
        foreach (Transform child in player.glassHolder)
        {
            // Assuming the filled glass's name indicates the soda type, e.g., "GlassFilledRed"
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

        // Check ice count
        if (player.iceCount != orderedIce)
        {
            Debug.LogWarning("Wrong amount of ice.");
            isOrderCorrect = false;
        }

        // Check fruit type and count
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
            // Reset player and customer states
            player.ReturnCup();
            hasOrder = false;
        }
    }
}
