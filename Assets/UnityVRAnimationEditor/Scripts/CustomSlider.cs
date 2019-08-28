using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomSlider : Slider
{
    public bool scrubbing = false;
    [SerializeField]PlayManually playManually;

    public override void OnPointerDown(PointerEventData pEventData)
    {
        base.OnPointerDown(pEventData);

        playManually.StartScrub();
        scrubbing = true;

        if (onValueChanged != null)
        {
            onValueChanged.Invoke(m_Value);
        }
    }
    public override void OnPointerUp(PointerEventData pEventData)
    {
        base.OnPointerUp(pEventData);

        playManually.EndScrub();
        scrubbing = false;
    }
}