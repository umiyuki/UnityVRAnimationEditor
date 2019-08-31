using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekBar : MonoBehaviour
{
    [SerializeField]CustomSlider slider;
    [SerializeField]Text text;

    [SerializeField]PlayManually playManually;
    [SerializeField] EditingAnimationClipInfo editingAnimationClipInfo;

    // Update is called once per frame
    void Update()
    {
        float frameRate = editingAnimationClipInfo.GetFrameRate();
        if (frameRate == 0) { return; }

        int finalFrame = Mathf.RoundToInt(editingAnimationClipInfo.GetFrameLength());

        slider.maxValue = finalFrame;
        var currentFrame = Mathf.RoundToInt( PlayManually.nowFrameTime * frameRate);
        if (!slider.scrubbing)
        {
            slider.value = currentFrame;
        }

        string prevText = text.text;
        string newText = currentFrame + " / " + finalFrame;
        if (prevText != newText)
        {
            text.text = newText;
        }
    }

    public void OnPointerDownSlider()
    {
        playManually.StartScrub();
    }

    public void OnPointerUpSlider()
    {
        //再生してたなら再開
        playManually.EndScrub();
    }

    public void OnValueChangeSlider(float value)
    {
        if (slider.scrubbing)
        {
            playManually.Scrub((int)value);
        }
    }
}
