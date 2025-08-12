using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegionalWeatherCheck : MonoBehaviour
{
    string popaMamonta = "5f8f1a564b7846b383582731250708";
    public GameObject itemPrefab;
    public VericalScrollRectAutoSizing vscrs;
    public Transform itemsParent;
    public GameObject LoadingScreen;
    public GameObject ProcessScreen;

    List<Tuple<string, string>> filteredCities = new List<Tuple<string, string>>();

    public int maxSimultaneousRequests = 5;

    private Dictionary<string, WeatherData> regionalWeatherData = new Dictionary<string, WeatherData>();
    private Queue<string> citiesToProcess = new Queue<string>();
    private int activeRequests = 0;

    string _region = "";
    bool _isDone = false;
    void OnEnable()
    {
        _region = AppController.instance.GetRegion();
        StartCoroutine(GetWeatherForRegion(_region));
    }

    public void OnCityChange()
    {
        _isDone = false;
    }

    public IEnumerator GetWeatherForRegion(string regionName)
    {
        LoadingScreen.SetActive(true);
        // 1. Get all cities in the region from your existing data
        if (!_isDone)
        {
            List<string> citiesInRegion = CitiesController.GetCitiesInRegions(regionName);

            if (citiesInRegion == null || citiesInRegion.Count == 0)
            {
                Debug.LogError($"No cities found for region: {regionName}");
                yield break;
            }

            // 2. Initialize processing queue
            regionalWeatherData.Clear();
            citiesToProcess = new Queue<string>(citiesInRegion);

            // 3. Start processing in batches
            ProcessScreen.SetActive(true);
            while (citiesToProcess.Count > 0 || activeRequests > 0)
            {
                // Start new requests if we're under the limit
                while (activeRequests < maxSimultaneousRequests && citiesToProcess.Count > 0)
                {
                    string city = citiesToProcess.Dequeue();
                    StartCoroutine(FetchCityWeather(city));
                    activeRequests++;
                }
                ProcessScreen.GetComponent<Text>().text = $"{citiesInRegion.Count - citiesToProcess.Count}/{citiesInRegion.Count} cities processed";
                yield return null; // Wait a frame between checks
            }
            _isDone = true;
            ProcessScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Skipping GetWeatherForRegion");
        }
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        yield return new WaitUntil(() => itemsParent.childCount == 0);
        foreach (KeyValuePair<string, WeatherData> kvp in regionalWeatherData)
        {
            GameObject cityWeather = Instantiate(itemPrefab, itemsParent);
            RegionalWeatherItemController cityWeatherController = cityWeather.GetComponent<RegionalWeatherItemController>();
            cityWeatherController.SetData(kvp.Key, kvp.Value.current.temp_c.ToString(), kvp.Value.current.condition.text);
        }
        vscrs.AdjustSize();
        LoadingScreen.SetActive(false);
    }

    private IEnumerator FetchCityWeather(string cityName)
    {
        string url = $"https://api.weatherapi.com/v1/current.json?key={popaMamonta}&q={cityName}&aqi=no";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                WeatherData data = JsonUtility.FromJson<WeatherData>(request.downloadHandler.text);
                regionalWeatherData[cityName] = data;
            }
            else
            {
                Debug.LogWarning($"Failed to get weather for {cityName}: {request.error}");
            }

            activeRequests--;
        }
    }

    [System.Serializable]
    public class WeatherData
    {
        public Location location;
        public Current current;
    }

    [System.Serializable]
    public class Location
    {
        public string name;
        public string region;
        public string country;
    }

    [System.Serializable]
    public class Current
    {
        public float temp_c;
        public Condition condition;
    }

    [System.Serializable]
    public class Condition
    {
        public string text;
        public int code;
    }
}