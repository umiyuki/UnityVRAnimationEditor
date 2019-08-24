using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

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
    static System.Object _animWindowObject;
    static System.Object _controlInterfaceObject;

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
        _animWindowObject = _animWindowState.GetValue(_animEditorObject);
        _controlInterfaceObject = _controlInterfaceState.GetValue(_animWindowObject);
    }

    public static AnimationClip GetAnimationWindowCurrentClip()
    {

        if (_window != null)
        {
            System.Object clip = _windowStateType.InvokeMember("get_activeAnimationClip", BindingFlags.InvokeMethod | BindingFlags.Public, null,_animWindowObject , null);

            return (AnimationClip)clip;
        }

        return null;

    }

    public static GameObject GetAnimationWindowCurrentRootGameObject()
    {
        if(_window!=null)
        {
            System.Object obj = _windowStateType.InvokeMember("get_activeRootGameObject", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            return (GameObject)obj;
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
            System.Object playing = _windowStateType.InvokeMember("get_playing", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            ret = (bool)playing;
        }

        return ret;
    }

    public static void GoToTime(float time)
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("ScrubTime", BindingFlags.InvokeMethod | BindingFlags.Public, null, _controlInterfaceObject, new object[1] { time });
        }
    }

    public static void NextFrame()
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("GoToNextFrame", BindingFlags.InvokeMethod | BindingFlags.Public, null, _controlInterfaceObject, null);
        }
    }

    public static void PreviousFrame()
    {
        if (_window != null)
        {
            _controlInterfaceType.InvokeMember("GoToPreviousFrame", BindingFlags.InvokeMethod | BindingFlags.Public, null, _controlInterfaceObject, null);
        }
    }

    public static void ResampleAnimation()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("ResampleAnimation", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }

    public static void Repaint()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("Repaint", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }

    public static void StartPlayback()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StartPlayback", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }

    public static void StopPlayback()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StopPlayback", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }

    public static void SetCurrentFrame(int frame)
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("set_currentFrame", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, new object[1] { frame });
        }
    }

    public static int GetCurrentFrame()
    {
        int ret = 0;

        if (_window != null)
        {
            System.Object frame = _windowStateType.InvokeMember("get_currentFrame", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            ret = (int)frame;
        }

        return ret;
    }

    public static void StartRecording()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StartRecording", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }

    public static void StopRecording()
    {
        if (_window != null)
        {
            _windowStateType.InvokeMember("StopRecording", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);
        }
    }


    public static bool IsPlaying()
    {
        bool ret = false;

        if (_window != null)
        {
            System.Object isPlaying = _windowStateType.InvokeMember("get_playing", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            ret = (bool)isPlaying;
        }

        return ret;
    }

    public static bool IsRecording()
    {
        bool ret = false;

        if (_window != null)
        {
            System.Object isRecording = _windowStateType.InvokeMember("get_recording", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            ret = (bool)isRecording;
        }

        return ret;
    }

    public static void SelectNowDopeLine()
    {
        if (_window != null)
        {
            System.Object dopeLines = _windowStateType.InvokeMember("get_dopelines", BindingFlags.InvokeMethod | BindingFlags.Public, null, _animWindowObject, null);

            
        }
    }

    public static void PrintMethods()
    {
            Debug.Log("Methods");
            MemberInfo[] methods = _animEditorType.GetMembers(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            Debug.Log("Methods : " + methods.Length);
            for (int i = 0; i < methods.Length; i++)
            {
            MemberInfo currentInfo = methods[i];
                Debug.Log(currentInfo.ToString());
            }
    }
}