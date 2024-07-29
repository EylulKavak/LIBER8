using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GasPumpUI : MonoBehaviour
{
    public Slider fuelSlider;
    [SerializeField] private float fillingSpeed;
    public Image targetImage; // Hedef benzini gösterecek image
    public TextMeshProUGUI feedbackText; // Geri bildirim için TextMeshPro bileşeni
    public AudioSource fillingSound; // Dolum sesi için AudioSource
    private GameObject player;
    private Lives lives;
    private float currentFuel;
    private float targetFuel;
    private bool canFill = false;

    private bool isFilling;

    private const float minTargetPosition = -140f;
    private const float maxTargetPosition = 140f;

    void Start()
    {
        player = GameObject.Find("Player");
        lives = player.GetComponent<Lives>();
        // Başlangıçta slider ve hedef görüntüsünü gizle
        fuelSlider.gameObject.SetActive(false);
        targetImage.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false); // Geri bildirim metnini gizle
    }

    void Update()
    {
        if (canFill)
        {
            if (Input.GetKey(KeyCode.H))
            {
                fuelSlider.value += Time.deltaTime * fillingSpeed; // Dolma hızını ayarlayın
                if (fuelSlider.value >= 1f)
                {
                    fuelSlider.value = 1f;
                }


                // Dolum sesi çal
                if (!fillingSound.isPlaying)
                {
                    fillingSound.Play();
                }
                isFilling = true;
            }
            else
            {
                // H tuşu bırakıldığında sesi durdur
                if (fillingSound.isPlaying)
                {
                    fillingSound.Stop();
                }
            }
        }
    }

    public void StartFilling()
    {
        fuelSlider.gameObject.SetActive(true);
        targetImage.gameObject.SetActive(true);
        canFill = true;
    }

    public void StopFilling()
    {
        canFill = false;
        if(isFilling)
        {
            EvaluateFilling();
            isFilling = false;
        }
        EvaluateFilling();
        feedbackText.gameObject.SetActive(true);
        StartCoroutine(HideFeedbackAfterDelay(3f));
        fuelSlider.gameObject.SetActive(false);
        targetImage.gameObject.SetActive(false);        

        // Dolum sesi durdur
        if (fillingSound.isPlaying)
        {
            fillingSound.Stop();
        }
    }

    public void SetFuelLevel(float fuelAmount)
    {
        currentFuel = fuelAmount;
        fuelSlider.value = currentFuel;
    }

    public void SetTargetFuel(float targetAmount)
    {
        targetFuel = targetAmount;
        UpdateTargetPosition(targetFuel);
    }

    private void UpdateTargetPosition(float targetFuel)
    {
        // Hedef pozisyonu ayarla
        float targetPositionX = Mathf.Lerp(minTargetPosition, maxTargetPosition, targetFuel);
        targetImage.rectTransform.anchoredPosition = new Vector2(targetPositionX, targetImage.rectTransform.anchoredPosition.y);
    }

    private void EvaluateFilling()
    {
        float filledAmount = fuelSlider.value;
        float difference = Mathf.Abs(filledAmount - targetFuel);
        string feedback;

        if (difference < 0.01f)
        {
            feedbackText.color = Color.green;
            feedback = "Mükemmel";
        }
        else if (difference < 0.05f)
        {
            feedbackText.color = Color.blue;
            feedback = "İyi";
        }
        else if (difference < 0.1f)
        {
            feedbackText.color = Color.yellow;
            feedback = "İdare eder";
            lives.liveCount = lives.liveCount - 0.5f;
        }
        else
        {
            feedbackText.color = Color.red;
            feedback = "Kötü";
            lives.liveCount--;
        }

        // Geri bildirim metnini güncelle
        feedbackText.text = feedback;
    }
    private IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackText.gameObject.SetActive(false);
    }
}