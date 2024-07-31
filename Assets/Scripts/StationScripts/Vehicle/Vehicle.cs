using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour
{
    private NavMeshAgent agent;
    private CarFuelPump carFuelPump;
    private GasPumpUI gasPumpUI;
    public bool carFuelled = false;
    public bool isFilledOnce = false;
    private Transform despawnPoint;
    private float refuelTime = 30f;
    private bool isTimerRunning = false;
    private float timer;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fillImage;
    private Lives lives;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        lives = player.GetComponent<Lives>();

        despawnPoint = GameObject.Find("DespawnPosition").transform;
        agent = GetComponent<NavMeshAgent>();
        carFuelPump = GetComponentInChildren<CarFuelPump>();
        fuelSlider.gameObject.SetActive(false);

        GameObject gasPumpUIObject = GameObject.Find("GasPumpUI");
        if (gasPumpUIObject != null)
        {
            gasPumpUI = gasPumpUIObject.GetComponent<GasPumpUI>();
        }
        else
        {
            Debug.LogError("GasPumpUI object not found!");
        }
    }

    private void Update()
    {
        StartCoroutine(WaitAndDestroy());
        if (agent != null)
        {
            if (!isTimerRunning && Vector3.Distance(transform.position, agent.destination) < 1f)
            {
                StartCoroutine(StartRefuelTimer());
            }

            if (isTimerRunning)
            {
                timer -= Time.deltaTime;
                fuelSlider.value = timer / refuelTime;
                UpdateSliderColor();

                if (timer <= 0)
                {
                    carFuelled = false; // In case it wasn't set already
                    MoveToDespawn();
                }
            }

            if (carFuelPump != null && carFuelPump.pumpNuzzleHolder.childCount == 1 && Input.GetKey(KeyCode.H))
            {
                isFilledOnce = true;
            }

            if (isFilledOnce && carFuelPump.pumpNuzzleHolder.childCount == 0)
            {
                carFuelled = true;
                StopCoroutine(StartRefuelTimer());
                isTimerRunning = false;
                fuelSlider.gameObject.SetActive(false);
            }

            if (carFuelled)
            {
                isFilledOnce = false;
                transform.SetParent(null);
                MoveToDespawn();
            }
        }
    }

    private IEnumerator StartRefuelTimer()
    {
        isTimerRunning = true;
        timer = refuelTime;
        fuelSlider.gameObject.SetActive(true);

        while (timer > 0 && !carFuelled)
        {
            yield return null;
        }

        if (!carFuelled)
        {
            MoveToDespawn();
            lives.liveCount--;
        }

        isTimerRunning = false;
    }

    private void UpdateSliderColor()
    {
        if (fillImage != null)
        {
            float percent = fuelSlider.value;
            if (percent > 0.5f)
            {
                fillImage.color = Color.Lerp(Color.yellow, Color.green, (percent - 0.5f) * 2);
            }
            else
            {
                fillImage.color = Color.Lerp(Color.red, Color.yellow, percent * 2);
            }
        }
    }

    private void MoveToDespawn()
    {
        if (despawnPoint != null)
        {
            agent.SetDestination(despawnPoint.position);
            agent.speed = 15f;
        }
        else
        {
            Debug.LogWarning("DespawnPosition not found.");
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        while (Vector3.Distance(transform.position, despawnPoint.position) > 1f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}