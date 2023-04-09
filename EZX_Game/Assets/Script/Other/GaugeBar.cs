using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetStartValue(float startValue)
    {
        slider.value = 0;
        slider.maxValue = startValue;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}
