using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public WeatherCheck weatherCheck;
    public RegionalWeatherCheck regionalWeatherCheck;
    public CurrentCityPanel cityPanel;
    public List<BackgroundDependingOnWeather> backgroundDependingOnWeather;
    public SettingsController settingsController;
    public GameObject LoadingScreen;
    public GameObject menuScreen;
    public GameObject settingsScreen;
    public GameObject hourlyWeatherScreen;
    public GameObject regionalWeatherScreen;
    public GameObject processScreen;
    public GameObject funnyChicken;
    string _activeCity;
    string _cityRegion;

    public List<Tuple<string, string>> allCities = new List<Tuple<string, string>>();
    public bool isCitiesLoaded = false;

    public string GetRegion() => _cityRegion;

    IEnumerator Start()
    {
        ReturnToMenu();
        Debug.Log("Async On Enable AppController.cs");
        yield return LoadCitiesData();
        settingsController.GetData();
        _activeCity = settingsController.currentCity;
        _cityRegion = settingsController.cityRegion;
        ChangeCityPanelData();
        cityPanel.Activate();
        cityPanel.ShowCityAndRegion();
        ChangeAppBackgrounds();
        Debug.Log($"activeCity = {_activeCity}, activeRegion = {_cityRegion}");
    }
    IEnumerator LoadCitiesData()
    {
        yield return CitiesController.LOAD_DATA(() =>
        {
            allCities = new List<Tuple<string, string>>(CitiesController.cityRegionList);
            isCitiesLoaded = true;
        });
    }
    public void OpenFunnyChicken()
    {
        menuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        processScreen.SetActive(false);
        regionalWeatherScreen.SetActive(false);
        hourlyWeatherScreen.SetActive(false);
        funnyChicken.SetActive(true);
        cityPanel.Deactivate();
    }
    public void OpenRegionalWeatherScreen()
    {
        menuScreen.SetActive(false);
        processScreen.SetActive(false);
        settingsScreen.SetActive(false);
        hourlyWeatherScreen.SetActive(false);
        funnyChicken.SetActive(false);
        regionalWeatherScreen.SetActive(true);
        cityPanel.Deactivate();
    }
    public void OpenHourlyWeatherScreen()
    {
        menuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        processScreen.SetActive(false);
        regionalWeatherScreen.SetActive(false);
        funnyChicken.SetActive(false);
        hourlyWeatherScreen.SetActive(true);
        cityPanel.Deactivate();
    }
    public void OpenSettings()
    {
        menuScreen.SetActive(false);
        hourlyWeatherScreen.SetActive(false);
        processScreen.SetActive(false);
        regionalWeatherScreen.SetActive(false);
        funnyChicken.SetActive(false);
        settingsScreen.SetActive(true);
        cityPanel.Deactivate();
    }
    public void ReturnToMenu()
    {
        settingsController.gameObject.SetActive(false);
        regionalWeatherScreen.SetActive(false);
        settingsScreen.SetActive(false);
        processScreen.SetActive(false);
        hourlyWeatherScreen.SetActive(false);
        funnyChicken.SetActive(false);
        menuScreen.SetActive(true);
        cityPanel.Activate();
    }
    public void OnCityChanged()
    {
        weatherCheck.OnCityChanged();
        regionalWeatherCheck.OnCityChange();
        _activeCity = settingsController.currentCity;
        _cityRegion = settingsController.cityRegion;
        Debug.Log($"<color=cyan>City changed to: {_activeCity}, {_cityRegion}</color>");
        ChangeCityPanelData();
        StartCoroutine(weatherCheck.requestCoroutine());
        ChangeAppBackgrounds();
    }
    public void ChangeAppBackgrounds()
    {
        foreach (var bg in backgroundDependingOnWeather)
        {
            StartCoroutine(bg.asyncGetWeather());
        }
    }
    public void ChangeCityPanelData()
    {
        cityPanel.SetCity(_activeCity);
        cityPanel.SetRegion(_cityRegion);
        cityPanel.ShowCityAndRegion();
    }

    public static AppController instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        instance = FindObjectOfType<AppController>(true);
    }
}