using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InitAnimationClip
{
    static AnimationClip clip;
    static GameObject rootObject;

    [MenuItem("UVAE/InitAnimationClip")]
    static void Setup()
    {
        wAnimationWindowHelper.init();

        clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            Debug.LogError("アニメーションウインドウが存在しないかアニメーションクリップが選択されてません");
            return;
        }

        rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        if (rootObject == null)
        {
            Debug.LogError("アニメーションウインドウが存在しないかアニメーターが選択されてません");
            return;
        }

        clip.EnsureQuaternionContinuity();

        FindChild(rootObject.transform, "Node", "");
        FindChild(rootObject.transform, "IKNode", "");
        FindChild(rootObject.transform, "FKNode", "");

        AssetDatabase.SaveAssets();
    }

    static void FindChild(Transform parent, string _tag, string path)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                string setPath = path + "/" + child.name;
                if (path == "")
                {
                    setPath = child.name;
                }
                SetCurve(0f,setPath, child);
            }
            if (child.childCount > 0)
            {
                string setPath = path + "/" + child.name;
                if (path == "")
                {
                    setPath = child.name;
                }
                FindChild(child, _tag, setPath);
            }
        }
    }

    public static void SetCurve(float time, string path, Transform t)
    {
        Quaternion rot = t.localRotation;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(time, rot.x);
        //clip.SetCurve(path, typeof(Transform), "localRotation.x", curve);
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "localEulerAngles.x";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, rot.y);
        //clip.SetCurve(path, typeof(Transform), "localRotation.y", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "localEulerAngles.y";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, rot.z);
        //clip.SetCurve(path, typeof(Transform), "localRotation.z", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "localEulerAngles.z";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        /*
        curve = new AnimationCurve();
        curve.AddKey(0f, rot.w);
        //clip.SetCurve(path, typeof(Transform), "localRotation.w", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalRotation.w";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
        */

        Vector3 pos = t.localPosition;
        curve = new AnimationCurve();
        curve.AddKey(time, pos.x);
        //clip.SetCurve(path, typeof(Transform), "localPosition.x", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalPosition.x";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, pos.y);
        //clip.SetCurve(path, typeof(Transform), "localPosition.y", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalPosition.y";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, pos.z);
        //clip.SetCurve(path, typeof(Transform), "localPosition.z", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalPosition.z";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        Vector3 scale = t.localScale;
        curve = new AnimationCurve();
        curve.AddKey(time, scale.x);
        //clip.SetCurve(path, typeof(Transform), "localScale.x", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalScale.x";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, scale.y);
        //clip.SetCurve(path, typeof(Transform), "localScale.y", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalScale.y";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);

        curve = new AnimationCurve();
        curve.AddKey(time, scale.z);
        //clip.SetCurve(path, typeof(Transform), "localScale.z", curve);
        curveBinding = new EditorCurveBinding();
        curveBinding.path = path;
        curveBinding.propertyName = "m_LocalScale.z";
        curveBinding.type = typeof(Transform);
        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
    }
}
