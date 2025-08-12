using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMoodHourlyController : MonoBehaviour
{
    public GameObject rain, sunny, hot_v1, hot_v2, snow, stromy;

    public void ChangeMood(int mood)
    {
        deactivateAll();
        if (mood == 0)
        {
            rain.SetActive(true);
        }
        else if (mood == 1)
        {
            sunny.SetActive(true);
        }
        else if (mood == 2 || mood == 3)
        {
            if (Random.value > 0.5f)
            {
                hot_v1.SetActive(true);
            }
            else
            {
                hot_v2.SetActive(true);
            }
        }
        else if (mood == 4)
        {
            snow.SetActive(true);
        }
        else if (mood == 5)
        {
            stromy.SetActive(true);
        }
    }
    
    void deactivateAll()
    {
        rain.SetActive(false);
        sunny.SetActive(false);
        hot_v1.SetActive(false);
        hot_v2.SetActive(false);
        snow.SetActive(false);
        stromy.SetActive(false);
    }
}
