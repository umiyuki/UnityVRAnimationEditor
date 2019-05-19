using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WrapControllerInput : MonoBehaviour
{
    SDK_BaseHeadset.HeadsetType headsetType = SDK_BaseHeadset.HeadsetType.Undefined;

    protected float prevTouchPadX;
    protected float touchMarginSumX = 0f; //タッチ中のタッチパッド移動量の合計

    protected float prevTouchPadY;
    protected float touchMarginSumY = 0f;

    protected const float TOUCH_MARGIN = 0.2f; //アソビの量

    public bool IsOnSwipeHorizontal = false;
    public bool IsOnSwipeVertical = false;

    public float diffTouchPadX = 0f;
    public float diffTouchPadY = 0f;

    [SerializeField] private SteamVR_TrackedObject thisTrackedObject;

    // Update is called once per frame
    void Update()
    {

        //スワイプ値リセット
        IsOnSwipeHorizontal = false;
        IsOnSwipeVertical = false;
        diffTouchPadX = 0f;
        diffTouchPadY = 0f;

        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(gameObject);

        if (headsetType == SDK_BaseHeadset.HeadsetType.Undefined)
        { headsetType = VRTK_DeviceFinder.GetHeadsetType(); }

        if (headsetType == SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            //Riftでのタッチパッドの移動量取得
            var axis = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference);

            var absX = Mathf.Abs(axis.x);
            var absY = Mathf.Abs(axis.y);
            if (absX > absY) //X軸スワイプ
            {
                if (absX > TOUCH_MARGIN)
                {
                    IsOnSwipeHorizontal = true;
                    diffTouchPadX = (axis.x - TOUCH_MARGIN) * 0.05f * Time.deltaTime * 30f;
                }
            }
            else //Y軸スワイプ
            {
                if (absY > TOUCH_MARGIN)
                {
                    IsOnSwipeVertical = true;
                    diffTouchPadY = (axis.y - TOUCH_MARGIN) * 0.05f * Time.deltaTime * 30f;
                }
            }
        }
        else //Rift以外の場合
        {
            //Viveでのタッチパッドの移動量取得
            if(VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.TouchDown,reference))
            {
                touchMarginSumX = 0f;
                touchMarginSumY = 0f;
                prevTouchPadX = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference).x;
                prevTouchPadY = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference).y;
            }
            else if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, reference))
            {
                float nowTouchPadX = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference).x;
                diffTouchPadX = nowTouchPadX - prevTouchPadX;
                if (touchMarginSumX > TOUCH_MARGIN) //タッチ移動量がマージンを超えたら
                {
                    IsOnSwipeHorizontal = true;
                }
                prevTouchPadX = nowTouchPadX;
                touchMarginSumX += Mathf.Abs(diffTouchPadX);

                float nowTouchPadY = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference).y;
                diffTouchPadY = nowTouchPadY - prevTouchPadY;
                if (touchMarginSumY > TOUCH_MARGIN) //タッチ移動量がマージンを超えたら
                {
                    IsOnSwipeVertical = true;
                }
                prevTouchPadY = nowTouchPadY;
                touchMarginSumY += Mathf.Abs(diffTouchPadY);
            }
        }
        }
    }