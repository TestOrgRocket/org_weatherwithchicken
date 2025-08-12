using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VericalScrollRectAutoSizing : MonoBehaviour
{
    public float cellSize;
    public void AdjustSize()
    {
        int childCount = transform.childCount;
        float newSize = cellSize * childCount / 2 + 1;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, newSize);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newSize / 2f + 450f);
    }
}
