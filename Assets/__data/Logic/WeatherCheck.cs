using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherCheck : MonoBehaviour
{
    public GameObject LoadingScreen;
    string popaMamonta = "5f8f1a564b7846b383582731250708";
    public static string CURRENT_WEATHER;

    bool _cityChecked = false;
    public void OnCityChanged()
    {
        _cityChecked = false;
    }
    public void CheckSelectedCity()
    {
        if(_cityChecked) return;
        StartCoroutine(requestCoroutine());
    }
    public IEnumerator requestCoroutine()
    {
        string checkingCity = SettingsController.instance.currentCity;
        LoadingScreen.SetActive(true);
        // UnityWebRequest uwr = UnityWebRequest.Get(
        //     $"https://api.weatherapi.com/v1/current.json?key={popaMamonta}&q={checkingCity}&aqi=no"
        // );
        UnityWebRequest uwr = UnityWebRequest.Get(
            $"https://api.weatherapi.com/v1/forecast.json?key={popaMamonta}&q={checkingCity}&days=1&aqi=no"
        );
        yield return uwr.SendWebRequest();
        LoadingScreen.SetActive(false);
        if (UnityWebRequest.Result.ConnectionError == uwr.result)
        {
            Debug.LogError($"something went wrong: {uwr.error}");
        }
        else if (uwr.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
            var response = uwr.downloadHandler.text;
            Debug.Log($"<color=green>{response}</color>");
            weatherData = JsonUtility.FromJson<WeatherApiResponse>(response);
            yield return CalculateWeather(weatherData.current);
            // for (int i = 0; i < Mathf.Min(24, weatherData.forecast.forecastday[0].hour.Length); i++)
            // {
            //     hourData = weatherData.forecast.forecastday[0].hour[i];
            //     Debug.Log($"Hour {i}: {hourData.time} - {hourData.temp_c}°C, {hourData.condition.text}");
            // }
            _cityChecked = true;
        }
    }
    public Hour hourData;
    public WeatherApiResponse weatherData;
    public IEnumerator CalculateWeather(Current currentWeather)
    {
        float temp = currentWeather.temp_c;
        float windSpeed = currentWeather.wind_kph;
        float precipitation = currentWeather.precip_mm;
        int conditionCode = currentWeather.condition.code;
        string conditionText = currentWeather.condition.text.ToLower();

        // Определяем погоду по температуре и условиям
        if (temp <= -10f)
        {
            CURRENT_WEATHER = "freezing";
        }
        else if (temp < 0f)
        {
            CURRENT_WEATHER = "cold";
        }
        else if (temp >= 30f)
        {
            CURRENT_WEATHER = "hot";
        }
        else if (precipitation > 0f || conditionText.Contains("rain"))
        {
            CURRENT_WEATHER = "rainy";
        }
        else if (conditionText.Contains("snow") || conditionText.Contains("sleet") || conditionCode == 1066 || conditionCode == 1114 || conditionCode == 1219 || conditionCode == 1222)
        {
            CURRENT_WEATHER = "snowy";
        }
        else if (windSpeed > 20f || conditionText.Contains("wind"))
        {
            CURRENT_WEATHER = "windy";
        }
        else if (conditionText.Contains("storm") || conditionText.Contains("thunder"))
        {
            CURRENT_WEATHER = "stormy";
        }
        else if (conditionCode == 1000)
        {
            CURRENT_WEATHER = "sunny";
        }
        else
        {
            CURRENT_WEATHER = "sunny";
        }

        Debug.Log($"Current weather set to: {CURRENT_WEATHER}");
        yield return null;
    }
    [System.Serializable]
    public class WeatherApiResponse
    {
        public Current current;
        public Forecast forecast;
    }
    [System.Serializable]
    public class Current
    {
        public float temp_c;
        public Condition condition;
        public float wind_kph;
        public float precip_mm;
    }
    [System.Serializable]
    public class Forecast
    {
        public ForecastDay[] forecastday;
    }
    [System.Serializable]
    public class ForecastDay
    {
        public Hour[] hour; // Почасовые данные
        public string date;
    }
    [System.Serializable]
    public class Hour
    {
        public string time;
        public float temp_c;
        public Condition condition;
        public float wind_kph;
        public float precip_mm;
    }
    [System.Serializable]
    public class Condition
    {
        public string text;
        public int code;
    }
}