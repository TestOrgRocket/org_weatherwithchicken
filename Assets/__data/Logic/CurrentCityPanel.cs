using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentCityPanel : MonoBehaviour
{
    public Text cityText;
    public string region, city;
    public void SetCity(string city)
    {
        this.city = city;
    }
    public void SetRegion(string region)
    {
        this.region = region;
    }
    public void ShowCity()
    {
        cityText.text = $"{city}";
    }
    public void ShowRegion()
    {
        cityText.text = $"{region}";
    }
    public void ShowCityAndRegion()
    {
        cityText.text = $"{city}, {region}";
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
