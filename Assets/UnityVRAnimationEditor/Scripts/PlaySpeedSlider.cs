using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlaySpeedSlider : MonoBehaviour
{
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [SerializeField] Slider slider;
    [SerializeField] Text text;
    [SerializeField] FloatEvent onChangePlaySpeed;

    // Start is called before the first frame update
    void Start()
    {
        OnChangeSlider(slider.value);   
    }

    public void OnChangeSlider(float value)
    {
        var playSpeed = value * 0.1f;

        text.text = "x" + playSpeed.ToString("0.0");

        if (onChangePlaySpeed != null)
        {
            onChangePlaySpeed.Invoke(playSpeed);
        }
    }
}
