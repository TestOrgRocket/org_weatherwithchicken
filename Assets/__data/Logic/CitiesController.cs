using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CountryData
{
    public string iso2;
    public string iso3;
    public string country;
    public List<string> cities;
}

[System.Serializable]
public class CityCountryCollection
{
    public bool error;
    public string msg;
    public List<CountryData> data;
}

public class CitiesController : MonoBehaviour
{
    public static List<Tuple<string, string>> cityRegionList = new List<Tuple<string, string>>();
    private static bool isDataLoaded = false;

    public static IEnumerator LOAD_DATA(Action onComplete = null)
    {
        if (isDataLoaded)
        {
            onComplete?.Invoke();
            yield break;
        }

        cityRegionList.Clear();

        // Асинхронная загрузка текстового ресурса
        ResourceRequest request = Resources.LoadAsync<TextAsset>("city_country_data");
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError("City data file not found in Resources!");
            yield break;
        }

        TextAsset jsonFile = (TextAsset)request.asset;
        CityCountryCollection collection = null;

        // Парсинг JSON в фоновом потоке
        yield return new WaitForBackgroundThread();
        try
        {
            collection = JsonUtility.FromJson<CityCountryCollection>(jsonFile.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON parsing failed: {e.Message}");
        }
        yield return new WaitForEndOfFrame();

        if (collection == null || collection.data == null)
        {
            Debug.LogError("Failed to parse city data");
            yield break;
        }

        // Обработка данных
        foreach (CountryData countryData in collection.data)
        {
            string countryName = countryData.country;

            foreach (string city in countryData.cities)
            {
                // Добавляем пару (город, страна)
                cityRegionList.Add(new Tuple<string, string>(city, countryName));
            }
        }

        isDataLoaded = true;
        Debug.Log($"Loaded {cityRegionList.Count} cities");
        onComplete?.Invoke();
    }

    // Вспомогательный класс для фоновой работы
    public class WaitForBackgroundThread : CustomYieldInstruction
    {
        private bool isDone = false;
        private System.Threading.Thread thread;

        public WaitForBackgroundThread()
        {
            thread = new System.Threading.Thread(() =>
            {
                // Любая фоновая работа
                isDone = true;
            });

            thread.Start();
        }

        public override bool keepWaiting => !isDone;
    }

    public static List<string> GetCitiesInRegions(string searchRegion)
    {
        List<string> filteredCities = new List<string>();
        foreach (Tuple<string, string> tuple in cityRegionList)
        {
            if (tuple.Item2.Equals(searchRegion, System.StringComparison.OrdinalIgnoreCase))
            {
                filteredCities.Add(tuple.Item1);
            }
        }
        return filteredCities;
    }
}