using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourlyWeatherItemController : MonoBehaviour
{
    public Text hourText, weatherText, temperatureText;

    public void SetHourWeather(string hour, string weather, string temperature)
    {
        hourText.text = hour;
        weatherText.text = weather;
        temperatureText.text = temperature;
    }
}
