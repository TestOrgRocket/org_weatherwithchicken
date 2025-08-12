using UnityEngine;

public class ChickenMoodSnow : MonoBehaviour
{
    public GameObject snow;
    public GameObject freezing;
    public GameObject cold;
    void OnEnable()
    {
        snow.SetActive(false);
        freezing.SetActive(false);
        cold.SetActive(false);
        string weather = WeatherCheck.CURRENT_WEATHER;
        if (weather == "snowy")
        {
            snow.SetActive(true);
        }
        else if (weather == "freezing")
        {
            freezing.SetActive(true);
        }
        else if (weather == "cold")
        {
            cold.SetActive(true);
        }
    }
}