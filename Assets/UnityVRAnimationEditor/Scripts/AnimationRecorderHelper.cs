using UnityEngine;
using System.Collections;

public static class AnimationRecorderHelper
{

    public static string GetTransformPathName(Transform rootTransform, Transform targetTransform)
    {

        string returnName = targetTransform.name;
        Transform tempObj = targetTransform;

        // it is the root transform
        if (tempObj == rootTransform)
            return "";

        while (tempObj.parent != rootTransform)
        {
            //今編集中のアニメーターと無関係なオブジェクトを操作している場合、親は見つからない
            if (tempObj.parent == null)
            {
                return null;
            }

            returnName = tempObj.parent.name + "/" + returnName;
            tempObj = tempObj.parent;
        }

        return returnName;
    }

}