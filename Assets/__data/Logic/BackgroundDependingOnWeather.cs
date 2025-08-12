using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundDependingOnWeather : MonoBehaviour
{
    public Sprite sunnyBG,
        snowyBG,
        rainyBG,
        hotBG,
        stormyBG;

    Image _bg;
    void Start()
    {
        _bg = GetComponent<Image>();
    }

    public IEnumerator asyncGetWeather()
    {
        WeatherCheck weatherCheck = FindObjectOfType<WeatherCheck>(true);
        Debug.Log("weatrher check: " + weatherCheck);
        yield return weatherCheck.requestCoroutine();
        ChangeBG(WeatherCheck.CURRENT_WEATHER);
    }

    void ChangeBG(string weather)
    {
        if (_bg == null) _bg = GetComponent<Image>();
        Debug.Log($"Weather: {weather}");
        _bg.sprite = weather.ToUpper() switch
        {
            "SUNNY" => sunnyBG,
            "SNOWY" => snowyBG,
            "RAINY" => rainyBG,
            "HOT" => hotBG,
            "STORMY" => stormyBG,
            "WINDY" => rainyBG,
            "COLD" => snowyBG,
            "FREEZING" => snowyBG,
            _ => sunnyBG
        };
    }
}
