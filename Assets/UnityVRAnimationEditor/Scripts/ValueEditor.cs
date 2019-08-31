using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ValueEditor : MonoBehaviour
{
    [System.Serializable] public class FloatEvent : UnityEvent<float> { }

    [SerializeField] InputField inputField;
    [SerializeField] FloatEvent eventChangeValue;

    public void OnButtonPlus()
    {
        ChangeInputFieldValue(1f);
    }

    public void OnButtonMinus()
    {
        ChangeInputFieldValue(-1f);
    }

    public void OnButtonPlusTen()
    {
        ChangeInputFieldValue(10f);
    }

    public void OnButtonMinusTen()
    {
        ChangeInputFieldValue(-10f);
    }

    void ChangeInputFieldValue(float addValue)
    {
        float value = float.Parse(inputField.text);
        value += addValue;
        value = Mathf.Max(value, 0f); //マイナスにはしない
        inputField.text = value.ToString("0.#");

    }

    public void OnValueChangedInputField(string newStr)
    {
        float value = float.Parse(newStr);

        if (eventChangeValue != null)
        {
            eventChangeValue.Invoke(value);
        }
    }

    public void OnChangeValue(float value)
    {
        inputField.text = value.ToString("0.#");
    }
}
