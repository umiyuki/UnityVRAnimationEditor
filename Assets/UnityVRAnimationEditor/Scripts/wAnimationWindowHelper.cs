using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;

public class wAnimationWindowHelper
{
    static System.Type animationWindowType = null;
    static UnityEngine.Object _window;

    static BindingFlags _flags;
    static FieldInfo _animEditor;

    static Type _animEditorType;
    static System.Object _animEditorObject;
    static FieldInfo _animWindowState;
    static Type _windowStateType;
    static Type _controlInterfaceType;
    static FieldInfo _controlInterfaceState;
    static System.Object _animWindowStateObject;
    static System.Object _controlInterfaceObject;
    static Type _clipPopupCallbackInfoType;

    public static void init()
    {
        _window = GetOpenAnimationWindow();
        if (_window == null)
        {
            Debug.LogWarning("Animation window is not open.");
            return;
        }

        _flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        _animEditor = GetAnimationWindowType().GetField("m_AnimEditor", _flags);

        _animEditorType = _animEditor.FieldType;
        _animEditorObject = _animEditor.GetValue(_window);
        _animWindowState = _animEditorType.GetField("m_State", _flags);
        _windowStateType = _animWindowState.FieldType;
        _controlInterfaceState = _windowStateType.GetField("m_ControlInterface", _flags);
        _controlInterfaceType = _controlInterfaceState.FieldType;
        _animWindowStateObject = _animWindowState.GetValue(_animEditorObject);
        _controlInterfaceObject = _controlInterfaceState.GetValue(_animWindowStateObject);

        //PrintMethods();
    }

    public static void AddClipToAnimationPlayerComponent(AnimationClip clip)
    {
        //activeAnimationPlayerを取得
        System.Object activeAnimationPlayer = _windowStateType.InvokeMember("get_activeAnimationPlayer", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

        Type projectWindowUtilType = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditorInternal.AnimationWindowUtility");
        var addClipToAnimationPlayerComponentMethod = projectWindowUtilType.GetMethod("AddClipToAnimationPlayerComponent", BindingFlags.Static | BindingFlags.Public);
        var result = addClipToAnimationPlayerComponentMethod.Invoke(null, new object[2] { activeAnimationPlayer, clip });
    }

    public static string GetAcitiveFolderPath()
    {
        Type projectWindowUtilType = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.ProjectWindowUtil");
        var getActiveFolderPathMethod = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
        System.Object path = getActiveFolderPathMethod.Invoke(null, null);
        return (string)path;
    }

    public static void SetActiveAnimationClip(AnimationClip clip)
    {
        //var methodInfo = _windowStateType.GetProperty("activeAnimationClip").GetSetMethod();
        //methodInfo.Invoke(_animWindowStateObject, new object[1] { clip });

        /*
        var instance = _animWindowStateObject;
        var dg_set_activeAnimationClip = (Action<AnimationClip>)Delegate.CreateDelegate(typeof(Action<AnimationClip>), instance, instance.GetType().GetProperty("activeAnimationClip").GetSetMethod());
        dg_set_activeAnimationClip(clip);*/

        if (_window != null)
        {
            _windowStateType.InvokeMember("set_activeAnimationClip", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, new object[1] { clip });
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static AnimationClip GetAnimationWindowCurrentClip()
    {

        if (_window != null)
        {
            System.Object clip = _windowStateType.InvokeMember("get_activeAnimationClip", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null,_animWindowStateObject , null);

            return (AnimationClip)clip;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return null;

    }

    public static GameObject GetAnimationWindowCurrentRootGameObject()
    {
        if (_window != null)
        {
            System.Object obj = _windowStateType.InvokeMember("get_activeRootGameObject", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            return (GameObject)obj;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return null;
    }

    static System.Type GetAnimationWindowType()
    {
        if (animationWindowType == null)
        {
            animationWindowType = System.Type.GetType("UnityEditor.AnimationWindow,UnityEditor");
        }
        return animationWindowType;
    }

    static UnityEngine.Object GetOpenAnimationWindow()
    {
        UnityEngine.Object[] openAnimationWindows = Resources.FindObjectsOfTypeAll(GetAnimationWindowType());
        if (openAnimationWindows.Length > 0)
        {
            return openAnimationWindows[0];
        }
        return null;
    }

    public static bool GetPlaying()
    {
        bool ret = false;

        if (_window != null)
        {
            System.Object playing = _windowStateType.InvokeMember("get_playing", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            ret = (bool)playing;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return ret;
    }

    public static void GoToTime(float time)
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("SetCurrentTime", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _controlInterfaceObject, new object[1] { time });
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void KeyModified()
    {
        /*
        animEditor.SaveCurveEditorKeySelection();
        animEditor.controlInterface.ProcessCandidates();
        animEditor.UpdateSelectedKeysToCurveEditor();
        */
        _animEditorType.InvokeMember("SaveCurveEditorKeySelection", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animEditorObject, null);
        _controlInterfaceType.InvokeMember("ProcessCandidates", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _controlInterfaceObject, null);
        _animEditorType.InvokeMember("UpdateSelectedKeysToCurveEditor", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animEditorObject, null);
    }

    public static void NextFrame()
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("GoToNextFrame", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _controlInterfaceObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void PreviousFrame()
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("GoToPreviousFrame", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _controlInterfaceObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void ResampleAnimation()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("ResampleAnimation", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void Repaint()
    {
        if (_window != null)
        {
            _animEditorType.InvokeMember("Repaint", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animEditorObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void StartPlayback()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StartPlayback", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void StopPlayback()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StopPlayback", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void SetCurrentFrame(int frame)
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("set_currentFrame", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, new object[1] { frame });
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static int GetCurrentFrame()
    {
        int ret = 0;

        if (_window != null)
        {
            System.Object frame = _windowStateType.InvokeMember("get_currentFrame", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            ret = (int)frame;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return ret;
    }

    public static float GetCurrentTime()
    {
        float ret = 0;

        if (_window != null)
        {
            System.Object frame = _windowStateType.InvokeMember("get_currentTime", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            ret = (float)frame;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return ret;
    }

    public static void StartRecording()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StartRecording", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void StopRecording()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StopRecording", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }


    public static bool IsPlaying()
    {
        bool ret = false;

        if (_window != null)
        {
            System.Object isPlaying = _windowStateType.InvokeMember("get_playing", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            ret = (bool)isPlaying;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return ret;
    }

    public static bool IsRecording()
    {
        bool ret = false;

        if (_window != null)
        {
            System.Object isRecording = _windowStateType.InvokeMember("get_recording", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);

            ret = (bool)isRecording;
        }
        else
        {
            Debug.Log("_window == null!");
        }

        return ret;
    }

    public static void SelectNowDopeLine()
    {
        if (_window != null)
        {
            System.Object dopeLines = _windowStateType.InvokeMember("get_dopelines", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance, null, _animWindowStateObject, null);          
        }
        else
        {
            Debug.Log("_window == null!");
        }
    }

    public static void PrintMethods()
    {
            Debug.Log("Methods");
            MemberInfo[] methods = _windowStateType.GetMembers(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("Methods : " + methods.Length);
            for (int i = 0; i < methods.Length; i++)
            {
            MemberInfo currentInfo = methods[i];
                Debug.Log(currentInfo.ToString());
            }
    }
}