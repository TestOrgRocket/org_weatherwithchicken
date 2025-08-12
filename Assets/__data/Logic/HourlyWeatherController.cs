using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static WeatherCheck;

[System.Serializable]
public class WeatherCondition
{
    public int code;
    public string day;
    public string night;
    public int icon;
}

public class HourlyWeatherController : MonoBehaviour
{
    public GameObject hourlyWeatherItemPrefab;
    public Transform hourlyWeatherItemsParent;
    public WeatherCheck weatherCheck;
    public HorizontalScrollRectAutoSizing horizontalScrollRectAutoSizing;
    public ChickenMoodHourlyController moodController;
    public TextAsset weatherConditionsJson;

    private List<Image> buttons = new List<Image>();
    private List<WeatherCondition> weatherConditions = new List<WeatherCondition>();

    void Awake()
    {
        weatherConditionsJson = Resources.Load<TextAsset>("weather_conditions");
        if (weatherConditionsJson == null)
        {
            Debug.LogError("Weather conditions JSON not found in Resources!");
            return;
        }

        try
        {
            weatherConditions = new List<WeatherCondition>(JsonHelper.FromJson(weatherConditionsJson.text));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse weather conditions: {e.Message}");
        }
    }

    void OnEnable()
    {
        StartCoroutine(delayedActions());
    }

    IEnumerator delayedActions()
    {
        // Clear existing items
        foreach (Transform child in hourlyWeatherItemsParent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        yield return new WaitUntil(() => hourlyWeatherItemsParent.childCount == 0);

        // Check if we have forecast data
        if (weatherCheck.weatherData == null || weatherCheck.weatherData.forecast == null ||
            weatherCheck.weatherData.forecast.forecastday.Length == 0)
        {
            Debug.LogError("No forecast data available");
            yield break;
        }

        ForecastDay day = weatherCheck.weatherData.forecast.forecastday[0];

        // Create items for each hour
        foreach (Hour hour in day.hour)
        {
            GameObject item = Instantiate(hourlyWeatherItemPrefab, hourlyWeatherItemsParent);
            HourlyWeatherItemController itemController = item.GetComponent<HourlyWeatherItemController>();

            // Format time (show only hour part)
            string displayTime = DateTime.Parse(hour.time).ToString("HH:mm");

            itemController.SetHourWeather(displayTime, hour.condition.text, hour.temp_c.ToString());

            // Add button functionality
            Button button = item.GetComponent<Button>();
            button.onClick.AddListener(() => OnHourSelected(itemController, hour));

            buttons.Add(item.GetComponent<Image>());
        }

        // Select current hour by default
        SelectCurrentHour();
        horizontalScrollRectAutoSizing.AdjustSize();
    }

    void SelectCurrentHour()
    {
        if (weatherCheck.weatherData == null || weatherCheck.weatherData.forecast == null ||
            weatherCheck.weatherData.forecast.forecastday.Length == 0)
            return;

        DateTime currentTime = DateTime.Now;
        ForecastDay day = weatherCheck.weatherData.forecast.forecastday[0];

        for (int i = 0; i < day.hour.Length; i++)
        {
            DateTime hourTime = DateTime.Parse(day.hour[i].time);
            if (hourTime.Hour == currentTime.Hour)
            {
                // Found current hour - select it
                OnHourSelected(hourlyWeatherItemsParent.GetChild(i).GetComponent<HourlyWeatherItemController>(),
                               day.hour[i]);
                break;
            }
        }
    }

    void OnHourSelected(HourlyWeatherItemController itemController, Hour hourData)
    {
        // Highlight selected button
        foreach (Image button in buttons)
        {
            button.color = Color.white;
        }
        itemController.GetComponent<Image>().color = Color.yellow;

        // Determine mood based on weather condition
        int mood = GetMoodFromWeather(hourData);
        moodController.ChangeMood(mood);
    }

    int GetMoodFromWeather(Hour hour)
    {
        // First check temperature extremes
        if (hour.temp_c >= 30f) return 2; // Hot
        if (hour.temp_c < 0f) return 4;    // Snow

        // Then check weather condition codes
        WeatherCondition condition = weatherConditions.Find(c => c.code == hour.condition.code);
        if (condition != null)
        {
            string conditionText = hour.condition.text.ToLower();

            if (conditionText.Contains("rain") || condition.code == 1063 || condition.code == 1180 ||
                condition.code == 1183 || condition.code == 1186 || condition.code == 1189 ||
                condition.code == 1192 || condition.code == 1195)
            {
                return 0; // Rain
            }
            else if (conditionText.Contains("snow") || condition.code == 1066 || condition.code == 1114 ||
                     condition.code == 1210 || condition.code == 1213 || condition.code == 1216 ||
                     condition.code == 1219 || condition.code == 1222 || condition.code == 1225)
            {
                return 4; // Snow
            }
            else if (conditionText.Contains("storm") || conditionText.Contains("thunder") ||
                     condition.code == 1087 || condition.code == 1273 || condition.code == 1276 ||
                     condition.code == 1279 || condition.code == 1282)
            {
                return 5; // Stormy
            }
            else if (condition.code == 1000)
            {
                return 1; // Sunny
            }
        }

        // Default to sunny
        return 1;
    }
}

// Helper class for JSON array deserialization
public static class JsonHelper
{
    [System.Serializable]
    private class WeatherConditionWrapper
    {
        public WeatherCondition[] conditions;
    }

    public static WeatherCondition[] FromJson(string json)
    {
        WeatherConditionWrapper wrapper = JsonUtility.FromJson<WeatherConditionWrapper>(json);
        return wrapper.conditions;
    }
}