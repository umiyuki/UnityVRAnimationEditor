using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ScalingGrabbingObject : MonoBehaviour {

    private VRTK_InteractGrab grab;

    protected float prevTouchPadX;
    protected float touchMarginSumX = 0f; //タッチ中のタッチパッド移動量の合計

    protected float prevTouchPadY;
    protected float touchMarginSumY = 0f;

    protected const float TOUCH_MARGIN = 0.2f; //アソビの量

    public bool IsOnSwipeHorizontal = false;
    public bool IsOnSwipeVertical = false;

    public float diffTouchPadX = 0f;
    public float diffTouchPadY = 0f;

    // Use this for initialization
    void Start () {
        grab = GetComponent<VRTK_InteractGrab>();
	}
	
	// Update is called once per frame
	void Update () {
        var grabbingObject = grab.GetGrabbedObject();

        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(gameObject);

        IsOnSwipeHorizontal = false;
        IsOnSwipeVertical = false;
        diffTouchPadX = 0f;
        diffTouchPadY = 0f;

        //タッチパッドのスワイプを取得
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.TouchDown, reference))
        {
            touchMarginSumX = 0f;
            touchMarginSumY = 0f;
            var prevTouchAxis = VRTK_SDK_Bridge.GetControllerAxis(SDK_BaseController.ButtonTypes.Touchpad, reference);
            prevTouchPadX = prevTouchAxis.x;
            prevTouchPadY = prevTouchAxis.y;
        }

        if (grabbingObject != null && !Node.scaleConstraint)
        {

            if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, reference))
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

            //左右スワイプでスケール変更
            if (IsOnSwipeHorizontal)
            {
                Vector3 newScale = (1f + diffTouchPadX) * grabbingObject.transform.localScale;
                grabbingObject.transform.localScale = newScale;
            }
        }
	}
}
