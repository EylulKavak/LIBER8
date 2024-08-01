using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
    }

}
