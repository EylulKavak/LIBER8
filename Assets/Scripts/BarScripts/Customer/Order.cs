using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Order : MonoBehaviour, IInteractable
{
    public string[] fruits = {"Lemon", "Orange"};
    public string[] sodas = {"Red", "Yellow", "Blue","Orange","Purple","Green"};
    private TMP_Text orderText;
    private TMP_Text warningText;

    private string orderedFruit;
    private string orderedSoda;
    private int orderedIce;
    private bool hasOrder = false;
    private bool canOrder = true;

    private CustomerAI customer;
    private Lives lives;

    [SerializeField] private float orderTimer = 30f;
    private bool timerRunning = false;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image sliderFillImage;

    private void Start()
    {
        orderText = GameObject.Find("orderText").GetComponent<TMP_Text>();
        warningText = GameObject.Find("warning").GetComponent<TMP_Text>();
        customer = GetComponent<CustomerAI>();
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            lives = player.GetComponent<Lives>();
        }

        if (timerSlider != null)
        {
            timerSlider.maxValue = orderTimer;
            timerSlider.gameObject.SetActive(false); // Başlangıçta gizli
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
        orderedIce = Random.Range(0, 3);

        hasOrder = true;
        timerRunning = true;
        orderTimer = 30f;

        string orderDetails = $"Order:  {orderedIce} Ice Cubes, {orderedSoda} Soda, {orderedFruit} Fruit";
        Debug.Log(orderDetails);

        if (orderText != null)
        {
            orderText.text = orderDetails;
        }

        if (timerSlider != null)
        {
            timerSlider.value = orderTimer;
            timerSlider.gameObject.SetActive(true);
        }
    }

    public void CheckOrder(PlayerInteract player)
    {
        bool isOrderCorrect = true;

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
            timerRunning = false;
            ClearUI();
        }
        else
        {
            hasOrder = false;
            timerRunning = false;
            ClearUI();
            lives.liveCount--;
        }
        canOrder = false;
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

            if (timerSlider != null)
            {
                timerSlider.value = orderTimer;
                UpdateSliderColor();
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
                orderTimer = 30f;
                if (customer != null)
                {
                    customer.OrderCompleted();
                }
                lives.liveCount--;

                ClearUI();
            }
        }
    }

    private void ClearUI()
    {
        if (orderText != null)
        {
            orderText.text = "";
        }
        if (timerSlider != null)
        {
            timerSlider.value = 0;
            timerSlider.gameObject.SetActive(false);
        }
        if (warningText != null)
        {
            warningText.text = "";
        }
    }

    private void UpdateSliderColor()
    {
        if (sliderFillImage != null)
        {
            float percentage = timerSlider.value / timerSlider.maxValue;
            if (percentage > 0.5f)
            {
                sliderFillImage.color = Color.Lerp(Color.yellow, Color.green, (percentage - 0.5f) * 2);
            }
            else
            {
                sliderFillImage.color = Color.Lerp(Color.red, Color.yellow, percentage * 2);
            }
        }
    }
}
