using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EditingAnimationClipInfo : MonoBehaviour
{
    [System.Serializable]public class FloatEvent : UnityEvent<float> { }
    AnimationClip editingClip = null;
    float clipFrameLength = 0;
    float clipFrameRate = 0;
    [SerializeField] FloatEvent OnChangeFrameLength;
    [SerializeField] FloatEvent OnChangeFrameRate;
    [SerializeField] UnityEvent EventChangeEditingClip;

    public float GetFrameLength() {return clipFrameLength; }
    public float GetFrameRate() {return clipFrameRate; }

    // Update is called once per frame
    void Update()
    {
        var nowClip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (nowClip == null)
        {
            //現在編集中のクリップ無し
            return;
        }

        if (editingClip == null || editingClip != nowClip)
        {
            editingClip = nowClip;
            ChangeFrameLength(editingClip.frameRate * editingClip.length);
            ChangeFrameRate(editingClip.frameRate);

            if (EventChangeEditingClip != null) { EventChangeEditingClip.Invoke(); }
        }

        if (editingClip != null)
        {
            //フレームレートをアニメーションウインドウで変更されてたら追従
            if (editingClip.frameRate != clipFrameRate)
            {
                ChangeFrameRate(editingClip.frameRate);
            }

            //フレームレングスは増えた時だけ追従
            if (editingClip.length * clipFrameRate > clipFrameLength)
            {
                ChangeFrameLength(editingClip.length * clipFrameRate);
            }
        }
    }

    public void ChangeFrameLength(float newLength)
    {
        if (clipFrameLength == newLength) { return; }

        clipFrameLength = newLength;
        if (OnChangeFrameLength != null)
        {
            OnChangeFrameLength.Invoke(clipFrameLength);
        }
    }

    public void ChangeFrameRate(float newFrameRate)
    {
        if (clipFrameRate == newFrameRate) { return; }

        clipFrameRate = newFrameRate;

        if (editingClip != null)
        {
            editingClip.frameRate = clipFrameRate;
        }

        if (OnChangeFrameRate != null)
        {
            OnChangeFrameRate.Invoke(newFrameRate);
        }
    }
}
