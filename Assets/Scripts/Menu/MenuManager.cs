using System;
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
    [HideInInspector] public string volume = "Volume", resolution = "Resulotion", quality = "QualityLevel", mouse = "MouseSensitivity", brightness = "Brightness";

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetStartSoundValues(); SetStartResValues(); SetStartQualityValues(); SetStartBrightnessValues();
        volumeSlider.onValueChanged.AddListener(SetSoundValues);
        brightnessSlider.onValueChanged.AddListener(SetBrightnessValue);
        resDropDown.onValueChanged.AddListener(SetResolutionValues);
        qualityDropdown.onValueChanged.AddListener(SetQualityLevel);
    }

    private void SetStartQualityValues()
    {
        //string[] qualityNames = { "Performant", "Balanced", "High Fidelity" };
        //qualityDropdown.ClearOptions();
        //qualityDropdown.AddOptions(qualityNames.ToList());

        int savedQualityLevel = PlayerPrefs.GetInt(quality, 2);
        qualityDropdown.value = savedQualityLevel;
    }

    public void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt(quality, level);
    }

    private void SetStartResValues()
    {
        Resolution[] resolutions = Screen.resolutions;
        resDropDown.ClearOptions();
        foreach (Resolution res in resolutions)
        {
            string resString = res.width + " x " + res.height + " " + Convert.ToInt32(res.refreshRateRatio.value) + "Hz";
            resDropDown.options.Add(new TMP_Dropdown.OptionData(resString));
        }

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
    }

    public void SetResolutionValues(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (index >= 0 && index < resolutions.Length)
        {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
            PlayerPrefs.SetString(resolution, resDropDown.options[index].text);
        }
    }
    private void SetStartBrightnessValues()
    {
        globalVolume.profile.TryGet(out liftGammaGain);
        if (PlayerPrefs.HasKey(brightness))
        {
            float savedBrightness = PlayerPrefs.GetFloat(brightness);
            brightnessSlider.value = savedBrightness;
            SetBrightnessValue(savedBrightness);
        }
        else
        {
            brightnessSlider.value = 0; // Default brightness value
            SetBrightnessValue(0);
        }
    }

    public void SetBrightnessValue(float brightness)
    {
        liftGammaGain.gain.Override(new Vector4(0, 0, 0, brightness));
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetStartSoundValues() => volumeSlider.value = PlayerPrefs.GetFloat(volume, 0);

    public void SetSoundValues(float soundLvl)
    {
        mixer.SetFloat(volume, soundLvl);
        PlayerPrefs.SetFloat(volume, soundLvl);
    }

    public void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); Time.timeScale = 1;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); Time.timeScale = 1;
    }
    public void Exit() => Application.Quit();
}