using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunnyTextPickController : MonoBehaviour
{
    public List<string> possibleTexts;
    public Text textObject;
    void OnEnable()
    {
        textObject.text = possibleTexts[Random.Range(0, possibleTexts.Count)];
    }
}