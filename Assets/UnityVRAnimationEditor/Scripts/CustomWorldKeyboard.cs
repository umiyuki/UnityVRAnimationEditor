using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CustomWorldKeyboard : MonoBehaviour
{
    private InputField input;
    private UnityAction<string> callback;

    public void ClickKey(string character)
    {
        input.text += character;
    }

    public void Backspace()
    {
        if (input.text.Length > 0)
        {
            input.text = input.text.Substring(0, input.text.Length - 1);
        }
    }

    public void OpenKeyboard(UnityAction<string> _callback)
    {
        if (input == null) { Init(); }
        input.text = "";
        callback = _callback;
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        if (callback != null) { callback.Invoke(""); }
        input.text = "";

        gameObject.SetActive(false);
    }

    public void Enter()
    {
        //VRTK_Logger.Info("You've typed [" + input.text + "]");

        if (callback != null) { callback.Invoke(input.text); }
        input.text = "";

        gameObject.SetActive(false);
    }

    private void Init()
    {
        input = GetComponentInChildren<InputField>();
    }
}
