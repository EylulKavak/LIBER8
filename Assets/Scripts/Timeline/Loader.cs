using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
