using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationWindowController : MonoBehaviour {

    [SerializeField] Transform getWindowObject; //ウインドウを取得するオブジェクト
    [SerializeField] GameObject setWindowObject; //取得したウインドウをセットするオブジェクト
    bool init = false;
    WindowsInput.InputSimulator inputSimulator;

    [SerializeField] RawImage targetRawImage;
    [SerializeField] UnityEditor.Experimental.EditorVR.Helpers.EditorWindowCapture editorWindowCapture;
    [SerializeField] PlayManually playManually;

    // Use this for initialization
    IEnumerator Start () { 
        
        wAnimationWindowHelper.init();
        inputSimulator = new WindowsInput.InputSimulator();

        /*
        var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene(); //現在のシーンを取得
        string activeSceneName = activeScene.name;
        if (activeSceneName == string.Empty)
        {
            activeSceneName = "Untitled"; 
        }
        getWindowObject.GetComponent<uWindowCapture.UwcWindowTexture>().partialWindowTitle = activeSceneName; //ウインドウ名には現在のシーン名が含まれてるハズなので指定

        yield return null; //ウインドウが更新されるのを待つ

        int waitCount = 0;
        while (waitCount < 300)
        {
            if (getWindowObject.childCount == 0)
            {
                waitCount++;
                yield return null;
            }
            else
            {
                break;
            }
        }

        if (getWindowObject.childCount == 0)
        {
            Debug.Log("Unityエディタの子ウインドウが見つかりません");
            yield break;
        }

        var child = getWindowObject.GetChild(0);//最初の子がアニメーションビューだと決めてかかって取得
        var material = child.GetComponent<MeshRenderer>().sharedMaterial; //マテリアル取得
        childWindow = child.GetComponent<uWindowCapture.UwcWindowTexture>();
        Debug.Log("title:" + childWindow.window.threadId);

        setWindowObject.GetComponent<MeshRenderer>().sharedMaterial = material;//取得したマテリアルをセット

        getWindowObject.GetComponent<uWindowCapture.UwcWindowTexture>().captureRequestTiming = uWindowCapture.WindowTextureCaptureTiming.Manual;//もうUnity本体のウインドウは更新不要
        */

        while (targetRawImage.texture == null)
        {
            yield return null;
        }

        init = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!init) { return; }

        //ウインドウのサイズ調整
        float width = (float)editorWindowCapture.m_Position.width / 1500f;
        float height = (float)editorWindowCapture.m_Position.height / 1500f;
        if (editorWindowCapture.m_Position.width == 0)
        {
            width = 0.5f;
        }
        if (editorWindowCapture.m_Position.height == 0)
        {
            height = 0.5f;
        }
        setWindowObject.transform.localScale = new Vector3(width, height, 1f);

        //ウインドウの位置調整
        var pos = setWindowObject.transform.localPosition;
        pos.z = 0.10f + height * 0.5f;
        setWindowObject.transform.localPosition = pos;

        /*
        //ウインドウのサイズ調整
        float width = (float)childWindow.window.width / 1500f;
        float height = (float)childWindow.window.height / 1500f;
        if (childWindow.window.width == 0)
        {
            width = 0.5f;
        }
        if (childWindow.window.height == 0)
        {
            height = 0.5f;
        }
        setWindowObject.transform.localScale = new Vector3(width, height, 1f);

        //ウインドウの位置調整
        var pos = setWindowObject.transform.localPosition;
        pos.z = 0.04f + height * 0.5f;
        setWindowObject.transform.localPosition = pos;
        */

        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("きた");
            var i = new WindowsInput.InputSimulator();
            i.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        }*/

    }

    public Vector2? GetMousePosInEditorWindowFromUV(Vector2 textureCoord)
    {
        //if (childWindow == null) { return null; }
        //Debug.Log(textureCoord);
        var rect = editorWindowCapture.m_Position;
        var windowLocalX = (int)(textureCoord.x * rect.width);
        var windowLocalY = (int)(textureCoord.y * rect.height);
        Vector2 pos = new Vector2(windowLocalX, windowLocalY);

        return pos;
    }

    public void OnTouch()
    {
        inputSimulator.Mouse.LeftButtonDown();
        //pointer.Touch(pos);
    }

    public void OnRelease()
    {
        inputSimulator.Mouse.LeftButtonUp();
        //pointer.Release(pos);
    }

    public void OnRightTouch()
    {
        inputSimulator.Mouse.RightButtonDown();
        //pointer.Touch(pos);
    }

    public void OnRightRelease()
    {
        inputSimulator.Mouse.RightButtonUp();
        //pointer.Release(pos);
    }

    public void DownAlt()
    {
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
    }

    public void UpAlt()
    {
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
    }

    int countFramePrevFrame = 0;
    public void PressPrevFrame()
    {
        countFramePrevFrame = 0;
        PrevFrame();
    }

    public void HoldPrevFrame()
    {
        //6フレームに一回入力
        if (countFramePrevFrame > 20 && countFramePrevFrame % 4 == 0)
        {
            PrevFrame();
        }
        countFramePrevFrame++;
    }

    public void PrevFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(null, WindowsInput.Native.VirtualKeyCode.OEM_COMMA);
    }

    int countFrameNextFrame = 0;
    public void PressNextFrame()
    {
        countFrameNextFrame = 0;
        NextFrame();
    }

    public void HoldNextFrame()
    {
        //6フレームに一回入力
        if (countFrameNextFrame > 20 && countFrameNextFrame % 4 == 0)
        {
            NextFrame();
        }
        countFrameNextFrame++;
    }

    public void NextFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(null, WindowsInput.Native.VirtualKeyCode.OEM_PERIOD);
    }

    public void PrevKeyFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LMENU, WindowsInput.Native.VirtualKeyCode.OEM_COMMA);
    }

    public void NextKeyFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LMENU, WindowsInput.Native.VirtualKeyCode.OEM_PERIOD);
    }

    public void FirstKeyFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LSHIFT, WindowsInput.Native.VirtualKeyCode.OEM_COMMA);
    }

    public void LastKeyFrame()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LSHIFT, WindowsInput.Native.VirtualKeyCode.OEM_PERIOD);
    }

    public void AddKeyAll()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(null, WindowsInput.Native.VirtualKeyCode.VK_K);
    }

    public void AddKeyModified()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LSHIFT, WindowsInput.Native.VirtualKeyCode.VK_K);
    }

    public void TogglePlaying()
    {
        /*
        if (wAnimationWindowHelper.IsPlaying())
        {
            wAnimationWindowHelper.StopPlayback();
        }
        else
        {
            wAnimationWindowHelper.StartPlayback();
        }*/

        playManually.TogglePlay();

    }

    public void ToggleRecording()
    {
        if (wAnimationWindowHelper.IsRecording())
        {
            wAnimationWindowHelper.StopRecording();
        }
        else
        {
            wAnimationWindowHelper.StartRecording();
        }
    }

    public void Undo()
    {
        UnityEditor.Undo.PerformUndo();
    }

    public void Redo()
    {
        UnityEditor.Undo.PerformRedo();
    }

    public void Copy()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_C);
    }

    public void Paste()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_V);
    }

    public void Del()
    {
        inputSimulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_6);
        inputSimulator.Keyboard.ModifiedKeyStroke(null, WindowsInput.Native.VirtualKeyCode.DELETE);
    }

    public void OnChangeMoveConstraintToggle(bool value)
    {
        Node.moveConstraint = !value;
    }

    public void OnChangeRotateConstraintToggle(bool value)
    {
        Node.rotateConstraint = !value;
    }

    public void OnChangeScaleConstraintToggle(bool value)
    {
        Node.scaleConstraint = !value;
    }
}
