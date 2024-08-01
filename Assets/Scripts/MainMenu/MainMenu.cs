using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PanelManager panelManager;
    [HideInInspector] public int save;
    private void Awake()
    {
        save = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 1;
    }
    public void Continue()
    {
        SceneManager.LoadScene(save);
    }
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


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
