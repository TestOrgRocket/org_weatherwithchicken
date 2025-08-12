using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Linq;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        instance = FindObjectOfType<SettingsController>(true);
    }

    public string currentCity;
    public string cityRegion;
    public Dropdown cityDropdown;
    public InputField filterInput;

    public CurrentCityPanel cityPanel;

    public void GetData()
    {
        string savedCity = PlayerPrefs.GetString("currentCity", "notpicked");
        string saveRegion = PlayerPrefs.GetString("currentRegion", "notpicked");
        if (savedCity == "notpicked")
        {
            currentCity = "NEW YORK";
            cityRegion = "United States";
            cityPanel.SetCity(currentCity);
            cityPanel.SetRegion(cityRegion);
            PlayerPrefs.SetString("currentCity", currentCity);
            PlayerPrefs.SetString("currentRegion", cityRegion);
        }
        else
        {
            currentCity = savedCity;
        }
        if (saveRegion == "notpicked")
        {
            cityRegion = "United States".ToUpper();
            PlayerPrefs.SetString("currentRegion", cityRegion);
        }
        else
        {
            cityRegion = saveRegion;
        }
    }
    void OnEnable()
    {
        filterInput.text = "";
    }
    public void ChangeCity()
    {
        string currentPick = cityDropdown.options[cityDropdown.value].text.ToUpper();
        Debug.Log("New city: " + currentPick);
        currentCity = currentPick.Split(",")[0].Trim();
        cityRegion = currentPick.Split(",")[1].Trim();
        PlayerPrefs.SetString("currentCity", currentCity);
        PlayerPrefs.SetString("currentRegion", cityRegion);
        AppController.instance.OnCityChanged();
    }

    public void OnInputValueChange()
    {
        StopAllCoroutines();
        StartCoroutine(FilterData());
    }

    IEnumerator FilterData()
    {
        yield return new WaitForSeconds(1f);
        FilterOptions();
    }

    public void FilterOptions()
    {
        if (!AppController.instance.isCitiesLoaded) return;

        string searchText = filterInput.text.ToUpper();

        var filteredCities = AppController.instance.allCities
        .Where(city =>
        {
            string cleanCity = city.Item1.Replace("`", "").Trim().ToUpper();
            string cleanCountry = city.Item2.Replace("`", "").Trim().ToUpper();
            string cleanSearch = searchText.Replace("`", "").Trim();

            return cleanCity.Contains(cleanSearch) ||
                cleanCountry.Contains(cleanSearch);
        })
        .Select(city =>
        {
            string displayCity = city.Item1.Replace("`", "").Trim();
            string displayCountry = city.Item2.Replace("`", "").Trim();
            return new Tuple<string, string>(displayCity, displayCountry);
        })
        .OrderBy(city => city.Item1)
        .Take(100)
        .ToList();


        // Очищаем и обновляем Dropdown
        cityDropdown.ClearOptions();

        // Создаем список вариантов в формате "Город, Страна"
        var dropdownOptions = filteredCities
            .Select(city => new Dropdown.OptionData($"{city.Item1}, {city.Item2}"))
            .ToList();

        cityDropdown.AddOptions(dropdownOptions);

        // Восстанавливаем текущий выбранный город, если он есть в отфильтрованном списке
        if (!string.IsNullOrEmpty(currentCity))
        {
            int index = filteredCities.FindIndex(c => c.Item1.ToUpper() == currentCity);
            if (index >= 0)
            {
                cityDropdown.value = index;
                cityDropdown.RefreshShownValue();
            }
        }
    }
}
