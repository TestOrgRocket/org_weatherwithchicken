using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalScrollRectAutoSizing : MonoBehaviour
{
    public float cellSize;
    public void AdjustSize()
    {
        int childCount = transform.childCount;
        float newSize = cellSize * childCount;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newSize, rt.sizeDelta.y);
        rt.anchoredPosition = new Vector2(newSize / 2f + 450f, rt.anchoredPosition.y);
    }
}