using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class Localization : MonoBehaviour
{
    public void Local(int x)
    {        
        var availableLocales = LocalizationSettings.AvailableLocales.Locales;
        if (x >= 0 && x < availableLocales.Count)
        {
            LocalizationSettings.SelectedLocale = availableLocales[x];
        }
        else
        {
            Debug.LogError("Invalid locale index");
        }
    }
}
