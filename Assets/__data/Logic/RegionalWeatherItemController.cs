using UnityEngine;
using UnityEngine.UI;

public class RegionalWeatherItemController : MonoBehaviour
{
    public Text cityName, temperature, weather;
    public void SetData(string cityName, string temperature, string weather)
    {
        this.cityName.text = cityName;
        this.temperature.text = temperature;
        this.weather.text = weather;
    }
}