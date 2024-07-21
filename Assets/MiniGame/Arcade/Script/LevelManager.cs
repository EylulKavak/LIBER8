using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    ScoreKeeper scoreKeeper;
    private void Awake()
    {
        scoreKeeper=FindObjectOfType<ScoreKeeper>();
    }
    public void loadGame()
    {
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("ArcadeGame");
    }
    public void loadMainMenu()
    {
        SceneManager.LoadScene("ArcadeMenü");
    }
    public void loadGameOver()
    {
       StartCoroutine(WaitAndLoad("ArcadeGameOver",sceneLoadDelay));
    }
    public void QuitGame()
    {
        Debug.Log("Quittingg...");
        Application.Quit();
    }
    IEnumerator WaitAndLoad(string sceneName,float delay)
    {
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(sceneName);
    }
}
