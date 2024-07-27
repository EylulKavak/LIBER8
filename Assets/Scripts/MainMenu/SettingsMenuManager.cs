using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public TMP_Dropdown resDropDown, qualityDropdown;
    public Slider volumeSlider, brightnessSlider;
    public AudioMixer mixer;
    public Volume globalVolume; 
    private LiftGammaGain liftGammaGain;
    [HideInInspector] public string volume = "Volume", resolution = "Resolution", quality = "QualityLevel", mouse = "MouseSensitivity", brightness = "Brightness";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetStartSoundValues();
        SetStartResValues();
        SetStartQualityValues();
        SetStartBrightnessValues();

        volumeSlider.onValueChanged.AddListener(SetSoundValues);
        brightnessSlider.onValueChanged.AddListener(SetBrightnessValue);
        resDropDown.onValueChanged.AddListener(SetResolutionValues);
        qualityDropdown.onValueChanged.AddListener(SetQualityLevel);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    private void SetStartQualityValues()
    {
        qualityDropdown.ClearOptions();
        List<string> options = new List<string>();

        string[] qualityNames = QualitySettings.names;
        foreach (string name in qualityNames)
        {
            options.Add(name);
        }

        qualityDropdown.AddOptions(options);

        int savedQualityLevel = PlayerPrefs.GetInt(quality, 2);
        qualityDropdown.value = savedQualityLevel;

        qualityDropdown.RefreshShownValue();
        Debug.Log($"SetStartQualityValues called. Saved Quality Level: {savedQualityLevel}");
    }

    public void SetQualityLevel(int level)
    {
        Debug.Log($"SetQualityLevel called with value: {level}");
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt(quality, level);
        Debug.Log($"Quality Level set to {level}");
    }

    private void SetStartResValues()
    {
        Resolution[] resolutions = Screen.resolutions;
        resDropDown.ClearOptions();
        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            string resString = res.width + " x " + res.height + " " + Convert.ToInt32(res.refreshRateRatio.value) + "Hz";
            options.Add(resString);
        }

        resDropDown.AddOptions(options);

        if (PlayerPrefs.HasKey(resolution))
        {
            string savedRes = PlayerPrefs.GetString(resolution);
            for (int i = 0; i < resDropDown.options.Count; i++)
            {
                if (resDropDown.options[i].text == savedRes)
                {
                    resDropDown.value = i;
                    break;
                }
            }
        }
        else
        {
            resDropDown.value = resDropDown.options.Count - 1;
        }

        resDropDown.RefreshShownValue();
        Debug.Log($"SetStartResValues called. Current Resolution: {resDropDown.options[resDropDown.value].text}");
    }

    public void SetResolutionValues(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (index >= 0 && index < resolutions.Length)
        {
            Debug.Log($"SetResolutionValues called with value: {index}");
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
            PlayerPrefs.SetString(resolution, resDropDown.options[index].text);
            Debug.Log($"Resolution set to {resDropDown.options[index].text}");
        }
    }

    private void SetStartBrightnessValues()
    {
        if (globalVolume == null)
        {
            Debug.LogError("GlobalVolume is not assigned.");
            return;
        }

        if (!globalVolume.profile.TryGet(out liftGammaGain))
        {
            Debug.LogError("LiftGammaGain is not found in the GlobalVolume profile.");
            return;
        }

        if (PlayerPrefs.HasKey(brightness))
        {
            float savedBrightness = PlayerPrefs.GetFloat(brightness);
            brightnessSlider.value = savedBrightness;
            SetBrightnessValue(savedBrightness);
            Debug.Log($"SetStartBrightnessValues called. Saved Brightness: {savedBrightness}");
        }
        else
        {
            brightnessSlider.value = 0; // Default brightness value
            SetBrightnessValue(0);
            Debug.Log($"SetStartBrightnessValues called. Default Brightness: 0");
        }
    }

    public void SetBrightnessValue(float brightness)
    {
        Debug.Log($"SetBrightnessValue called with value: {brightness}");
        liftGammaGain.gain.Override(new Vector4(0, 0, 0, brightness));
        PlayerPrefs.SetFloat("Brightness", brightness);
        Debug.Log($"Brightness set to {brightness}");
    }

    public void SetStartSoundValues()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        SetSoundValues(savedVolume);
    }

    public void SetSoundValues(float soundLvl)
    {
        Debug.Log($"SetSoundValues called with value: {soundLvl}");

        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        if (musicPlayer != null)
        {
            AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = soundLvl;
            }
        }

        PlayerPrefs.SetFloat("Volume", soundLvl);
        Debug.Log($"Sound level set to {soundLvl}");
    }

    public void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Exit() => Application.Quit();

    public void ApplySettings()
    {
        Debug.Log("ApplySettings called");

        SetSoundValues(volumeSlider.value);
        SetBrightnessValue(brightnessSlider.value);
        SetResolutionValues(resDropDown.value);
        SetQualityLevel(qualityDropdown.value);
    }
}
