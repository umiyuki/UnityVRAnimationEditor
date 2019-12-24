using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BothGripScaleRotateRootObject : MonoBehaviour
{
    [SerializeField] GameObject leftControllerAlias;
    [SerializeField] GameObject rightControllerAlias;
    [SerializeField] Transform rootObjectT;   

    float moveFactor = 3f;
    float lerpFactor = 10f;

    Vector3? scalingWorldPos = null;
    Vector3 targetPosition;
    Vector3 lastLeftPos;
    Vector3 lastRightPos;

    float? initialDegree;
    Quaternion? initialRotation;

    // Update is called once per frame
    void Update()
    {
        VRTK_ControllerReference leftReference = VRTK_ControllerReference.GetControllerReference(leftControllerAlias);
        VRTK_ControllerReference rightReference = VRTK_ControllerReference.GetControllerReference(rightControllerAlias);
        bool isPressGrip = false;

        //左手のグリップ押した
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, leftReference))
        {
            //移動
            var diffPosition = rootObjectT.InverseTransformPoint(leftControllerAlias.transform.position) - lastLeftPos;
            targetPosition = targetPosition - rootObjectT.TransformVector(diffPosition) * moveFactor;
            isPressGrip = true;
        }

        //右手のグリップ押した
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, rightReference))
        {
            //移動
            var diffPosition = rootObjectT.InverseTransformPoint(rightControllerAlias.transform.position) - lastRightPos;
            targetPosition = targetPosition - rootObjectT.TransformVector(diffPosition) * moveFactor;
            isPressGrip = true;
        }

        //移動させる
        /*if (!VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, leftReference) && !VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Touch, rightReference))
        {*/
        if (isPressGrip)
        {
            rootObjectT.position = Vector3.Lerp(rootObjectT.position, targetPosition, Time.deltaTime * lerpFactor);
        }
        else
        {
            targetPosition = rootObjectT.position;
        }


        //両手のグリップ押してる
        if (VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, leftReference) && VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, rightReference))
        {

            //スケール
            float lastDistance = Vector3.Distance(lastLeftPos, lastRightPos);
            Vector3 nowLeftPos = rootObjectT.InverseTransformPoint(leftControllerAlias.transform.position);
            Vector3 nowRightPos = rootObjectT.InverseTransformPoint(rightControllerAlias.transform.position);
            float diffScale = lastDistance / Vector3.Distance(nowLeftPos, nowRightPos);

            //rootObjectT.transform.localScale = rootObjectT.transform.localScale * diffScale;
            ScaleAround(rootObjectT, (leftControllerAlias.transform.position + rightControllerAlias.transform.position) / 2f, rootObjectT.transform.localScale * (diffScale + 1f) * 0.5f);

            targetPosition = rootObjectT.position;

            //回転
            Vector2 leftControllerXZPos = new Vector2(leftControllerAlias.transform.position.x, leftControllerAlias.transform.position.z);
            Vector2 rightControllerXZPos = new Vector2(rightControllerAlias.transform.position.x, rightControllerAlias.transform.position.z);
            var diffControllerXZPos = leftControllerXZPos - rightControllerXZPos;
            float rad = Mathf.Atan2(diffControllerXZPos.y, diffControllerXZPos.x);
            float degree = rad * Mathf.Rad2Deg;
            if (initialDegree == null)
            { initialDegree = degree; }

            var diffDegree = degree - initialDegree;

            /*
            if (diffDegree > 180f)
            { diffDegree -= 360f; }
            if (diffDegree < -180f)
            {
                diffDegree += 360f;
            }*/

            //rootObjectT.Rotate(0f, diffDegree, 0f);
            rootObjectT.RotateAround((leftControllerAlias.transform.position + rightControllerAlias.transform.position) / 2f, Vector3.up, diffDegree.Value);

        }
        else
        {
            initialDegree = null;
        }

        lastLeftPos = rootObjectT.InverseTransformPoint(leftControllerAlias.transform.position);
        lastRightPos = rootObjectT.InverseTransformPoint(rightControllerAlias.transform.position);
    }

    public void ScaleAround(Transform target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.localScale.x; // relative scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.localScale = newScale;
        target.localPosition = FP;
    }


}
