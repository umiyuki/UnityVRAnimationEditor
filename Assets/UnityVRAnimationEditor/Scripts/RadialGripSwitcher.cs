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
    [SerializeField] UnityEvent OnHoverEnterRight;
    [SerializeField] UnityEvent OnHoldRight;
    [SerializeField] UnityEvent OnClickAndGripRight;
    [SerializeField] UnityEvent OnHoldAndGripRight;
    [SerializeField] UnityEvent OnClickUp;
    [SerializeField] UnityEvent OnHoldUp;
    [SerializeField] UnityEvent OnClickAndGripUp;
    [SerializeField] UnityEvent OnHoldAndGripUp;
    [SerializeField] UnityEvent OnClickDown;
    [SerializeField] UnityEvent OnHoldDown;
    [SerializeField] UnityEvent OnClickAndGripDown;
    [SerializeField] UnityEvent OnHoldAndGripDown;

    SDK_BaseHeadset.HeadsetType HeadsetType {
        get {
            if (headsetType == SDK_BaseHeadset.HeadsetType.Undefined)
            {
                headsetType = VRTK_DeviceFinder.GetHeadsetType();
            }
            return headsetType;
        }
    }
    SDK_BaseHeadset.HeadsetType headsetType = SDK_BaseHeadset.HeadsetType.Undefined;

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

    public void DoHoverEnterLeft()
    {
        if (HeadsetType != SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            return;
        }

        DoClickLeft();
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

    public void DoHoverEnterRight()
    {
        if (HeadsetType != SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            return;
        }

        DoClickRight();
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

    public void DoClickUp()
    {

        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnClickAndGripUp != null)
            {
                OnClickAndGripUp.Invoke();
            }
        }
        else
        {
            if (OnClickUp != null)
            {
                OnClickUp.Invoke();
            }
        }
    }

    public void DoHoverEnterUp()
    {
        if (HeadsetType != SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            return;
        }

        DoClickUp();
    }

    public void DoHoldUp()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnHoldAndGripUp != null)
            {
                OnHoldAndGripUp.Invoke();
            }
        }
        else
        {
            if (OnHoldUp != null)
            {
                OnHoldUp.Invoke();
            }
        }
    }

    public void DoClickDown()
    {

        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnClickAndGripDown != null)
            {
                OnClickAndGripDown.Invoke();
            }
        }
        else
        {
            if (OnClickDown != null)
            {
                OnClickDown.Invoke();
            }
        }
    }

    public void DoHoverEnterDown()
    {
        if (HeadsetType != SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            return;
        }

        DoClickDown();
    }

    public void DoHoldDown()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(controllerScriptAliasObject);
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            if (OnHoldAndGripDown != null)
            {
                OnHoldAndGripDown.Invoke();
            }
        }
        else
        {
            if (OnHoldDown != null)
            {
                OnHoldDown.Invoke();
            }
        }
    }
}
