using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopMarker : MonoBehaviour
{
    [SerializeField] CustomSlider slider;
    [SerializeField] Text text;

    [SerializeField] string textHead;
    [SerializeField] float initValue = 0f; //ループスタートなら0f、ループエンドなら1f
    [SerializeField] EditingAnimationClipInfo editingAnimationClipInfo;

    public float GetTime()
    {
        return slider.value / editingAnimationClipInfo.GetFrameRate();
    }

    public float GetFrame()
    {
        return slider.value;
    }

    public void UpdateText()
    {
        text.text = textHead + " " + slider.value.ToString("0.#");
    }

    public void OnValueChangeSlider()
    {
        UpdateText();
    }

    void ChangeMaxValue()
    {
        float maxValue = editingAnimationClipInfo.GetFrameLength();
        slider.maxValue = maxValue;

        UpdateText();
    }

    //編集中のクリップが変更された時
    public void OnChangeEditingClip()
    {
        //ループマーカーの位置をリセット
        ChangeMaxValue();
        slider.value = slider.maxValue * initValue;

        UpdateText();
    }

    //クリップの長さが変更された時
    public void OnChangeFrameLength(float newLength)
    {
        ChangeMaxValue();
    }

}
