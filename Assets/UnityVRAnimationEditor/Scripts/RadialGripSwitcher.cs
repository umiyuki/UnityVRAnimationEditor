using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Events;

public class RadialGripSwitcher : MonoBehaviour {

    [SerializeField] GameObject controllerScriptAliasObject;

    [SerializeField] UnityEvent OnClickLeft;
    [SerializeField] UnityEvent OnHoldLeft;
    [SerializeField] UnityEvent OnClickAndGripLeft;
    [SerializeField] UnityEvent OnHoldAndGripLeft;
    [SerializeField] UnityEvent OnClickRight;
    [SerializeField] UnityEvent OnHoldRight;
    [SerializeField] UnityEvent OnClickAndGripRight;
    [SerializeField] UnityEvent OnHoldAndGripRight;

    public void DoClickLeft()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnClickAndGripLeft != null)
            {
                OnClickAndGripLeft.Invoke();
            }
        }
        else
        {
            if (OnClickLeft != null)
            {
                OnClickLeft.Invoke();
            }
        }
    }

    public void DoHoldLeft()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnHoldAndGripLeft != null)
            {
                OnHoldAndGripLeft.Invoke();
            }
        }
        else
        {
            if (OnHoldLeft != null)
            {
                OnHoldLeft.Invoke();
            }
        }
    }

    public void DoClickRight()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnClickAndGripRight != null)
            {
                OnClickAndGripRight.Invoke();
            }
        }
        else
        {
            if (OnClickRight != null)
            {
                OnClickRight.Invoke();
            }
        }
    }

    public void DoHoldRight()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnHoldAndGripRight != null)
            {
                OnHoldAndGripRight.Invoke();
            }
        }
        else
        {
            if (OnHoldRight != null)
            {
                OnHoldRight.Invoke();
            }
        }
    }
}
