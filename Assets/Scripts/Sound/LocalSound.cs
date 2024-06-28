using UnityEngine;

public class LocalSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip step, stepRun;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void StepVoice() => MovementSoundPlay(step);
    public void RunVoice() => MovementSoundPlay(stepRun);
    private void MovementSoundPlay(AudioClip audioClip)
    {
        audioSource.volume = Random.Range(0.2f, 0.6f);
        audioSource.PlayOneShot(audioClip);
    }
}
