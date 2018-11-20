using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GripControlRootObject : MonoBehaviour
{

    bool gripPressing = false;
    [SerializeField] GameObject rootObject;

    Vector3 prevPosition;
    Quaternion prevRotation;

    float moveFactor = 3f;
    float rotateFactor = 2f;
    float lerpFactor = 10f;
    Vector3 targetPosition;
    Quaternion targetRotation;

    protected float prevTouchPadX;
    protected float touchMarginSumX = 0f; //タッチ中のタッチパッド移動量の合計

    protected float prevTouchPadY;
    protected float touchMarginSumY = 0f;

    protected const float TOUCH_MARGIN = 0.2f; //アソビの量

    public bool IsOnSwipeHorizontal = false;
    public bool IsOnSwipeVertical = false;

    public float diffTouchPadX = 0f;
    public float diffTouchPadY = 0f;

    // Update is called once per frame
    void Update()
    {
        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(gameObject);

        IsOnSwipeHorizontal = false;
        IsOnSwipeVertical = false;
        diffTouchPadX = 0f;
        diffTouchPadY = 0f;

        //グリップ押してる
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            //タッチパッドのスワイプを取得
            if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.TouchDown, reference))
            {
                touchMarginSumX = 0f;
                touchMarginSumY = 0f;
                var prevTouchAxis = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference);
                prevTouchPadX = prevTouchAxis.x;
                prevTouchPadY = prevTouchAxis.y;
            }
            else if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, reference))
            {
                var nowTouchAxis = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference);
                float nowTouchPadX = nowTouchAxis.x;
                diffTouchPadX = nowTouchPadX - prevTouchPadX;
                if (touchMarginSumX > TOUCH_MARGIN) //タッチ移動量がマージンを超えたら
                {
                    IsOnSwipeHorizontal = true;
                }
                prevTouchPadX = nowTouchPadX;
                touchMarginSumX += Mathf.Abs(diffTouchPadX);

                float nowTouchPadY = nowTouchAxis.y;
                diffTouchPadY = nowTouchPadY - prevTouchPadY;
                if (touchMarginSumY > TOUCH_MARGIN) //タッチ移動量がマージンを超えたら
                {
                    IsOnSwipeVertical = true;
                }
                prevTouchPadY = nowTouchPadY;
                touchMarginSumY += Mathf.Abs(diffTouchPadY);
            }
            else if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.TouchUp, reference))
            {
                targetPosition = rootObject.transform.position;
                targetRotation = rootObject.transform.rotation;
                prevPosition = rootObject.transform.InverseTransformPoint(transform.parent.position);
                prevRotation = Quaternion.Inverse(rootObject.transform.rotation) * transform.parent.rotation;
            }
        }


        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.PressDown, reference))
        {
            targetPosition = rootObject.transform.position;
            targetRotation = rootObject.transform.rotation;
            prevPosition = rootObject.transform.InverseTransformPoint(transform.parent.position);
            prevRotation = Quaternion.Inverse(rootObject.transform.rotation) * transform.parent.rotation;
        }
        //グリップ押してる
        else if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, reference))
        {
            //移動
            var diffPosition = rootObject.transform.InverseTransformPoint(transform.parent.position) - prevPosition;
            targetPosition = targetPosition - rootObject.transform.TransformVector(diffPosition) * moveFactor;
            if (!VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, reference))
            {
                rootObject.transform.position = Vector3.Lerp(rootObject.transform.position, targetPosition, Time.deltaTime * lerpFactor);
            }

            prevPosition = rootObject.transform.InverseTransformPoint( transform.parent.position);

            /*
            //回転
            var diffRotation = Quaternion.Inverse(rootObject.transform.rotation) * transform.parent.rotation * Quaternion.Inverse(prevRotation);
            var euler = diffRotation.eulerAngles;
            diffRotation = Quaternion.Euler(0f, diffRotation.eulerAngles.y * rotateFactor, 0f);
            targetRotation = targetRotation * diffRotation;
            if (!VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, reference))
            {
                //var lerpQ = Quaternion.Lerp(rootObject.transform.rotation, targetRotation, Time.deltaTime * lerpFactor);
                float angle;
                Vector3 axis;
                diffRotation.ToAngleAxis(out angle, out axis);
                rootObject.transform.RotateAround(transform.parent.position, axis, angle);
            }

            prevRotation = Quaternion.Inverse(rootObject.transform.rotation) * transform.parent.rotation;
            */
        }

        //左右スワイプでスケール変更
        if (IsOnSwipeHorizontal)
        {
            Vector3 newScale = (1f - diffTouchPadX) * rootObject.transform.localScale;
            ScaleAround(rootObject, transform.position, newScale);
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

        prevRotation = transform.rotation;

    }

    public void TurnRight()
    {

        var diffRotation = Quaternion.Euler(0f, 30f, 0f);

        float angle;
        Vector3 axis;
        diffRotation.ToAngleAxis(out angle, out axis);
        rootObject.transform.RotateAround(transform.position, axis, angle);

        prevRotation = transform.rotation;

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
