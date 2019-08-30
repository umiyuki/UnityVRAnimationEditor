using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManually : MonoBehaviour
{
    [SerializeField] AnimationWindowController animationWindowController;
    static bool isPlay = false;

    public static bool IsPlaying() { return isPlay; }

    public float nextFrameTime = 0f;
    public static float nowFrameTime = 0f;
    public static float prevFrameTime = 0f;

    int lastTimeFrame = 0;
    bool goLoop = false;

    float playSpeed = 1f;

    [SerializeField] GameObject textPlayingObject;

    public void TogglePlay()
    {
        var clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            return;
        }

        isPlay = !isPlay;

        //再生開始
        if (isPlay)
        {
            nowFrameTime = wAnimationWindowHelper.GetCurrentTime();
            nextFrameTime = nowFrameTime + 1f / (float)clip.frameRate;
            textPlayingObject.SetActive(true);
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
        var clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            return;
        }

        float setTime = (float)frame / (float)clip.frameRate;
        wAnimationWindowHelper.GoToTime(setTime);
        nowFrameTime = setTime;
        prevFrameTime = nowFrameTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlay) { return; }

        var clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            return;
        }

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
            wAnimationWindowHelper.GoToTime(0f);
            goLoop = false;
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
            wAnimationWindowHelper.GoToTime(nowFrameTime);
        }

        prevFrameTime = nowFrameTime;
        nowFrameTime += Time.deltaTime * playSpeed;

        if (nowFrameTime >= clip.length)
        {
            goLoop = true;
            nowFrameTime = 0;
            nextFrameTime = 0;
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
