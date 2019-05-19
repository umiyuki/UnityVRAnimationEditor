using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GripControlRootObject : MonoBehaviour
{

    bool gripPressing = false;
    [SerializeField] GameObject rootObject;

    Vector3 prevPosition;

    float moveFactor = 3f;
    float lerpFactor = 10f;
    Vector3 targetPosition;

    WrapControllerInput wrapControllerInput;

    private void Start()
    {
        wrapControllerInput = GetComponent<WrapControllerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(gameObject);


        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.PressDown, reference))
        {

        }
        //グリップ押してる
        else if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            //左右スワイプでスケール変更
            if (wrapControllerInput.IsOnSwipeHorizontal)
            {
                Vector3 newScale = (1f - wrapControllerInput.diffTouchPadX) * rootObject.transform.localScale;
                ScaleAround(rootObject, transform.position, newScale);
            }
        }

    }

    public void TurnLeft()
    {
        //回転
        var diffRotation = Quaternion.Euler(0f, -30f, 0f);

        float angle;
        Vector3 axis;
        diffRotation.ToAngleAxis(out angle, out axis);
        rootObject.transform.RotateAround(transform.position, axis, angle);

    }

    public void TurnRight()
    {

        var diffRotation = Quaternion.Euler(0f, 30f, 0f);

        float angle;
        Vector3 axis;
        diffRotation.ToAngleAxis(out angle, out axis);
        rootObject.transform.RotateAround(transform.position, axis, angle);

    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.transform.localScale.x; // relative scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}
