using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManually : MonoBehaviour
{
    [SerializeField] AnimationWindowController animationWindowController;
    static bool isPlay = false;

    public static bool IsPlaying() { return isPlay; }

    public static float nowFrameTime = 0f;
    public static float prevFrameTime = 0f;

    int lastTimeFrame = 0;
    bool goLoop = false;

    float playSpeed = 1f;

    [SerializeField] GameObject textPlayingObject;
    [SerializeField] EditingAnimationClipInfo editingAnimationClipInfo;

    bool enableLoopMarker = false;
    [SerializeField] LoopMarker[] loopMarkers;

    bool enableLoop = true;

    public void OnToggleEnableLoop(bool enable)
    {
        enableLoop = enable;
    }

    public void OnToggleEnableLoopMarker(bool enable)
    {
        enableLoopMarker = enable;
    }

    public void TogglePlay()
    {
        float frameRate = editingAnimationClipInfo.GetFrameRate();
        if (frameRate == 0) { return; }

        isPlay = !isPlay;

        //再生開始
        if (isPlay)
        {
            nowFrameTime = wAnimationWindowHelper.GetCurrentTime();
            textPlayingObject.SetActive(true);

            //ループスタートマーカーから開始
            if (enableLoopMarker)
            {
                float startTime, endTime;
                GetStartEndTimeFromLoopMarker(out startTime, out endTime);
                if (nowFrameTime < startTime || nowFrameTime > endTime)
                {
                    GoToTime(startTime);
                    nowFrameTime = startTime;
                }
            }
        }
        else//再生終了
        {
            //nowFrameTime = 0f;
            textPlayingObject.SetActive(false);
        }

        prevFrameTime = nowFrameTime;

        wAnimationWindowHelper.Repaint();
    }

    bool tempIsPlay = false;
    public void StartScrub()
    {
        tempIsPlay = isPlay;
        isPlay = false;
    }

    public void EndScrub()
    {
        if (tempIsPlay) { isPlay = true; }

        wAnimationWindowHelper.Repaint();
    }

    public void Scrub(int frame)
    {
        float frameRate = editingAnimationClipInfo.GetFrameRate();
        if (frameRate == 0) { return; }

        float setTime = (float)frame / frameRate;
        GoToTime(setTime);
        nowFrameTime = setTime;
        prevFrameTime = nowFrameTime;
    }

    void GetStartEndTimeFromLoopMarker(out float startTime, out float endTime)
    {
        List<float> markerTimeList = new List<float>();
        foreach (var marker in loopMarkers)
        {
            markerTimeList.Add(marker.GetTime());
        }
        var markerTimeArr = markerTimeList.ToArray();
        startTime = Mathf.Min(markerTimeArr);
        endTime = Mathf.Max(markerTimeArr);
    }

    void GoToTime(float time)
    {
        float frameRate = editingAnimationClipInfo.GetFrameRate();
        if (frameRate == 0) { return; }

        if (!wAnimationWindowHelper.GetIsLinkedWithSequencer()) //非timeline
        {
            wAnimationWindowHelper.GoToTime(time);
        }
        else //timeline
        {
            int frame = Mathf.FloorToInt( time * frameRate);
            wAnimationWindowHelper.SetCurrentTime(time);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPlay) { return; }

        float frameRate = editingAnimationClipInfo.GetFrameRate();
        if (frameRate == 0) { return; }

        //ループのスタートと終了を取得
        float startTime = 0f;
        float endTime = editingAnimationClipInfo.GetFrameLength() / frameRate;
        if (enableLoopMarker)
        {
            GetStartEndTimeFromLoopMarker(out startTime, out endTime);
        }

        //Debug.Log("isLinkedSequencer:" + wAnimationWindowHelper.GetIsLinkedWithSequencer());

        //if (wAnimationWindowHelper.IsRecording())
        //{
        /*
        Debug.Log("start");
        foreach (var animEvent in clip.events)
        {
            Debug.Log(animEvent.time);
        }*/

        /*
        int currentFrame = wAnimationWindowHelper.GetCurrentFrame();

        while (wAnimationWindowHelper.GetCurrentFrame() > lastTimeFrame + 2)//フレームスキップが起きてる
        {

            //戻す
            wAnimationWindowHelper.PreviousFrame();
            //wAnimationWindowHelper.SetCurrentFrame(lastTimeFrame + 1);
            //wAnimationWindowHelper.ResampleAnimation();
            Debug.Log(wAnimationWindowHelper.GetCurrentFrame());
        }

        lastTimeFrame = currentFrame;*/

        if (goLoop)
        {
            GoToTime(startTime);
            goLoop = false;
            if (!enableLoop)
            {
                TogglePlay();
            }
        }
        /*else if (nowFrameTime > nextFrameTime)
        {
            //ループ処理
            int finalFrame = Mathf.RoundToInt(clip.length * clip.frameRate);

            int currentFrame = wAnimationWindowHelper.GetCurrentFrame();
            //Debug.Log(currentFrame);

            nowFrameTime = nextFrameTime;

            if (currentFrame >= finalFrame)
            {
                goLoop = true;
            }
            //animationWindowController.NextFrame();
            //wAnimationWindowHelper.NextFrame();
            wAnimationWindowHelper.GoToTime(nowFrameTime);
            //wAnimationWindowHelper.Repaint();
            //wAnimationWindowHelper.GoToTime(nowFrameTime);

            nextFrameTime += 1f / (float)clip.frameRate;
        }*/
        else
        {
            GoToTime(nowFrameTime);
        }

        prevFrameTime = nowFrameTime;
        nowFrameTime += Time.deltaTime * playSpeed;

        if (nowFrameTime >= endTime)
        {
                goLoop = true;
                nowFrameTime = startTime;
        }

        //Debug.LogWarning("prevFrameTime:" + prevFrameTime);
        //Debug.LogWarning("nowFrameTime:" + nowFrameTime);

        //}

        /*
        if (nowFrameTime >= nextFrameTime)
        {

            //ループ処理
            int finalFrame = Mathf.RoundToInt(clip.length * clip.frameRate);

            int currentFrame = wAnimationWindowHelper.GetCurrentFrame();

            if (currentFrame >= finalFrame)
            {
                wAnimationWindowHelper.SetCurrentFrame(0);
            }
            else
            {
                animationWindowController.PressNextFrame();
            }

            nextFrameTime += 1f /  (float)clip.frameRate;
        }

        nowFrameTime += Time.deltaTime;
        */

    }

    public void OnChangePlaySpeed(float value)
    {
        playSpeed = value;
    }
}
