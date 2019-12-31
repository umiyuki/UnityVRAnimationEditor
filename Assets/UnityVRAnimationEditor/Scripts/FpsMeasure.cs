using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FpsMeasure : MonoBehaviour
{
    int frameCount;
    float nextTime;
    [SerializeField] Text text;

    // Use this for initialization
    void Start()
    {
        nextTime = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;

        if (Time.time >= nextTime)
        {
            // 1秒経ったらFPSを表示
            //Debug.Log("FPS : " + frameCount);
            text.text = "fps:" + frameCount;
            frameCount = 0;
            nextTime += 1;
        }
    }
}