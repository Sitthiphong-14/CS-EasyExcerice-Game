using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterBar : MonoBehaviour
{
    [Header("UI config")]
    private List<GameObject> CounterPoints = new List<GameObject>();
    public GameObject CounterPrefab;
    public GridLayoutGroup layout;
    public RectTransform rectTransform;

    [Header("Counter Value")]
    public int maxValue;
    private int currentValue;

    public void SetStartCounterValue(int maxValue)
    {
        this.maxValue = maxValue;
        this.currentValue = 0;

        layout.cellSize = new Vector2(rectTransform.sizeDelta.x - (layout.padding.right + layout.padding.left),
                                    (rectTransform.sizeDelta.y - (layout.padding.top + layout.padding.bottom)) / maxValue - layout.spacing.y);
        for (int i = 0; i < maxValue; i++)
        {
            CounterPoints.Add(Instantiate(CounterPrefab, this.transform) as GameObject);
        }
    }

    public void SetCounterValue(int value)
    {
        this.currentValue = value;
        for (int i = 0; i < currentValue; i++)
        {
            if (i < maxValue)
            {
                CounterPoints[i].GetComponent<Image>().color = Color.blue;
            }
        }
    }

    public void ClearCounter()
    {
        for (int i = 0; i < maxValue; i++)
        {
            CounterPoints[i].GetComponent<Image>().color = Color.white;
        }
    }
}