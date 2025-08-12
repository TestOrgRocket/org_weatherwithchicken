using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMoodFunnyController : MonoBehaviour
{
    public GameObject rain, sunny, hot1, hot2, snow, stormy;

    void OnEnable()
    {
        disableAll();
        string weather = WeatherCheck.CURRENT_WEATHER;
        if (weather == "rainy" || weather == "windy")
        {
            rain.SetActive(true);
        }
        else if (weather == "sunny")
        {
            sunny.SetActive(true);
        }
        else if (weather == "hot")
        {
            float rndVal = Random.value;
            hot1.SetActive(rndVal > 0.5f);
            hot2.SetActive(rndVal <= 0.5f);
        }
        else if (weather == "snowy" || weather == "freezing" || weather == "cold")
        {
            snow.SetActive(true);
        }
        else if (weather == "stormy")
        {
            stormy.SetActive(true);
        }
    }
    void disableAll()
    {
        rain.SetActive(false);
        sunny.SetActive(false);
        hot1.SetActive(false);
        hot2.SetActive(false);
        snow.SetActive(false);
        stormy.SetActive(false);
    }
}
