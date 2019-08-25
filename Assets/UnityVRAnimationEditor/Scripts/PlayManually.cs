using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManually : MonoBehaviour
{
    [SerializeField]AnimationWindowController animationWindowController;
    public bool isPlay = false;

    public float nextFrameTime = 0f;
    public float nowFrameTime = 0f;

    int lastTimeFrame = 0;
    bool goLoop = false;

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
            wAnimationWindowHelper.SetCurrentFrame(0);
            nowFrameTime = 0;
            nextFrameTime = 0;
            goLoop = false;
        }
            else if (nowFrameTime > nextFrameTime)
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
                    animationWindowController.PressNextFrame();
                    //wAnimationWindowHelper.NextFrame();
                    //wAnimationWindowHelper.GoToTime(nowFrameTime);

                nextFrameTime += 1f / (float)clip.frameRate;
            }
            else
            {
                wAnimationWindowHelper.GoToTime(nowFrameTime);
                //wAnimationWindowHelper.ResampleAnimation();
            }

            nowFrameTime += Time.deltaTime;
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
}
