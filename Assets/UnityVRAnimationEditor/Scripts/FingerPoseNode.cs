using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerPoseNode : MonoBehaviour
{
    [SerializeField] public Text textName;
    [SerializeField] Text textValue;
    [SerializeField] public Slider slider;

    public void OnValueChangedSlider(float value)
    {
        textValue.text = value.ToString("0.##");
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}
