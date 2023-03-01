using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderUpdater : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    private string parameterName;
    private float parameterValue;

    public void UpdateSlider(string name, float value)
    {
        parameterName = name;
        parameterValue = value;

        nameText.text = name;
        valueText.text = value.ToString("F2");
    }

    public void UpdateSliderValue(float value)
    {
        parameterValue = value;
        valueText.text = value.ToString("F2");
    }
}
