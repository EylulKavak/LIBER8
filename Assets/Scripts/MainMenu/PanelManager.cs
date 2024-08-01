using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject controllerPanel;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
        controllerPanel.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(true);
        controllerPanel.SetActive(false);
    }

    public void ShowControllerPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        controllerPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
