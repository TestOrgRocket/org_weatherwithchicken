using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public string currentCity;
    public Dropdown cityDropdown;

    public Text currentCityText;
    public void OnEnable()
    {
        string savedCity = PlayerPrefs.GetString("currentCity","notpicked");
        if(savedCity == "notpicked")
        {
            
        }
        currentCity = cityDropdown.options[cityDropdown.value].text.ToUpper();
        currentCityText.text = currentCity;
    }

    public void ChangeCity()
    {
        currentCity = cityDropdown.options[cityDropdown.value].text.ToUpper();
    }
}
