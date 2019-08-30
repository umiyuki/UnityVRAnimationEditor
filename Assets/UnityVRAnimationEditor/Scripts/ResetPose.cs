using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

public class ResetPose : MonoBehaviour
{
    [MenuItem("UVAE/ResetPose")]
    static private void ReflectionRestoreToBindPose()
    {
        var rootObject = Selection.activeGameObject;

        if (rootObject == null)
            return;
        Type type = Type.GetType("UnityEditor.AvatarSetupTool, UnityEditor");
        if (type != null)
        {
            MethodInfo info = type.GetMethod("SampleBindPose", BindingFlags.Static | BindingFlags.Public);
            if (info != null)
            {
                info.Invoke(null, new object[] { rootObject });
            }
        }
    }
}
