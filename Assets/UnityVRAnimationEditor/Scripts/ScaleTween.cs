using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    Transform t;
    Vector3 savedScale;
    Coroutine coroutine;
    [SerializeField] float maxScale = 1.2f;
    [SerializeField] float duration = 0.3f;

    float tweenTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        savedScale = t.localScale;
    }

    public void StartTween()
    {
        //tween中にまたtweenが呼ばれたらリセットする
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        tweenTime = 0f;
        coroutine = StartCoroutine(CoroutineTween());
    }

    IEnumerator CoroutineTween()
    {
        while (tweenTime < duration)
        {
            float rad = Mathf.PI * tweenTime / duration;
            float lerpValue = Mathf.Sin(rad);

            float scaleValue = 1f + lerpValue * (maxScale - 1f);
            t.localScale = savedScale * scaleValue;

            tweenTime += Time.deltaTime;
            yield return null;
        }

        t.localScale = savedScale;
        coroutine = null;
    }
}
