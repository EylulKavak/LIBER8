using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public CinemachineInputProvider inputProvider;
    [HideInInspector] public PlayerInputs playerInputs;
    void Awake()
    {
        playerInputs = new PlayerInputs();
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
    }
    private void OnEnable()
    {
        playerInputs.UI.Enable();
        playerInputs.UI.Pause.performed += HandlePause;
    }
    private void OnDisable()
    {
        playerInputs.UI.Pause.performed -= HandlePause;
        playerInputs.UI.Disable();
    }
    private void HandlePause(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
                InputsHandler.instance.SetInputs(false);
                inputProvider.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    public void Continue()
    {
        InputsHandler.instance.SetInputs(true);
        inputProvider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.SetActive(false);
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
