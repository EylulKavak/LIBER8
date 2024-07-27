using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance = null;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f); 
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
