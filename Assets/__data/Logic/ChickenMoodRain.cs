using UnityEngine;

public class ChickenMoodRain : MonoBehaviour
{
    public GameObject Rainy, Windy;
    void OnEnable()
    {
        Rainy.SetActive(false);
        Windy.SetActive(false);
        string weather = WeatherCheck.CURRENT_WEATHER;
        if (weather == "rainy")
        {
            Rainy.SetActive(true);
        }
        else if (weather == "windy")
        {
            Windy.SetActive(true);
        }
    }
}