using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public PanelManager panelManager;

    public void PlayGame()
    {
        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        if (musicPlayer != null)
        {
            AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

  
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void GoToSettingsMenu()
    {
        panelManager.ShowSettingsMenu();
    }

    public void GoToMainMenu()
    {
        panelManager.ShowMainMenu();
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        Application.Quit();
    }
}
