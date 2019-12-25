using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InitAnimationClip
{
    [MenuItem("UVAE/InitAnimationClip")]
    static void Setup()
    {
        wAnimationWindowHelper.init();

        AnimationClip clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            Debug.LogError("アニメーションウインドウが存在しないかアニメーションクリップが選択されてません");
            return;
        }

        GameObject rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        if (rootObject == null)
        {
            Debug.LogError("アニメーションウインドウが存在しないかアニメーターが選択されてません");
            return;
        }

        DoInit(clip, rootObject.transform);
    }

    public static void DoInit(AnimationClip clip, Transform rootObjectTransform)
    {
        clip.EnsureQuaternionContinuity();

        AnimationWindowController.SetKeyAllNodes(clip, rootObjectTransform, 0f);

        AssetDatabase.SaveAssets();
    }

}
