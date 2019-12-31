using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class TexFacialEditor : MonoBehaviour
{
    [SerializeField] Transform baseScrollContent;
    [SerializeField] Transform eyeblowScrollContent;
    [SerializeField] Transform eyeScrollContent;
    [SerializeField] Transform mouthScrollContent;
    [SerializeField] Transform other1ScrollContent;
    [SerializeField] Transform other2ScrollContent;
    [SerializeField] Transform other3ScrollContent;
    [SerializeField] Transform other4ScrollContent;

    List<Toggle> baseToggles = new List<Toggle>();
    List<Toggle> eyeblowToggles = new List<Toggle>();
    List<Toggle> eyeToggles = new List<Toggle>();
    List<Toggle> mouthToggles = new List<Toggle>();
    List<Toggle> other1Toggles = new List<Toggle>();
    List<Toggle> other2Toggles = new List<Toggle>();
    List<Toggle> other3Toggles = new List<Toggle>();
    List<Toggle> other4Toggles = new List<Toggle>();

    [SerializeField]GameObject prefabNode;
    TexFacialController targetTexFacial;
    string targetPath = null;

    float animationTime;

    [SerializeField] GameObject[] otherCanvasObjects;

    // 表示された
    public void Show()
    {
        GameObject rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        if (rootObject == null)
        {
            Debug.Log("アニメーションビューで編集オブジェクトが選択されてません");
            return;
        }
        Transform rootObjectTransform = rootObject.transform;

        gameObject.SetActive(true); //表示

        foreach (var obj in otherCanvasObjects)
        {
            obj.SetActive(false);
        }

        //ターゲットを検出。Humanoid形式のAnimatorを探す
        TexFacialController[] texFacialControllers = rootObjectTransform.GetComponentsInChildren<TexFacialController>();
        targetTexFacial = null;
        if (texFacialControllers.Length > 0)
        {
            targetTexFacial = texFacialControllers[0];
        }

        if (targetTexFacial == null)
        {
            Debug.Log("アニメーションビューで編集中のオブジェクトにTexFacialControllerが見つかりません");
        }
        else
        {
            targetPath = AnimationRecorderHelper.GetTransformPathName(rootObjectTransform, targetTexFacial.transform);

            //スクロールビューに表示するボタン更新
            ResetScrollViews();

            //現在選択中のフェイシャルに合わせてトグル選択
            UpdateScrollViewToggles();
        }

        animationTime = wAnimationWindowHelper.GetCurrentTime();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        foreach (var obj in otherCanvasObjects)
        {
            obj.SetActive(true);
        }
    }

    public void OnClickAddKeyButton()
    {
        AnimationClip clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            Debug.Log("編集中のアニメーションクリップが存在しません");
            return;
        }

        SetKey(clip, "selectBaseIndex", targetTexFacial.selectBaseIndex);
        SetKey(clip, "selectEyeblowIndex", targetTexFacial.selectEyeblowIndex);
        SetKey(clip, "selectEyeIndex", targetTexFacial.selectEyeIndex);
        SetKey(clip, "selectMouthIndex", targetTexFacial.selectMouthIndex);
        SetKey(clip, "other1Index", targetTexFacial.other1Index);
        SetKey(clip, "other2Index", targetTexFacial.other2Index);
        SetKey(clip, "other3Index", targetTexFacial.other3Index);
        SetKey(clip, "other4Index", targetTexFacial.other4Index);
    }

    void SetKey(AnimationClip clip, string propertyName, int value)
    {
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.path = targetPath;
        curveBinding.propertyName = propertyName;
        curveBinding.type = typeof(TexFacialController);
        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, curveBinding);
        if (curve == null)
        {
            curve = new AnimationCurve();
        }
        int keyIndex = AnimationWindowController.SetKey(curve, animationTime, value);

        //Constantカーブにする
        AnimationUtility.SetKeyLeftTangentMode(curve, keyIndex, AnimationUtility.TangentMode.Constant);
        AnimationUtility.SetKeyRightTangentMode(curve, keyIndex, AnimationUtility.TangentMode.Constant);

        AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
    }

    // Update is called once per frame
    void Update()
    {
        float nowAnimationTime = wAnimationWindowHelper.GetCurrentTime();
        //アニメーションがシークされた？
        if (animationTime != nowAnimationTime)
        {
            animationTime = nowAnimationTime;

            UpdateScrollViewToggles();
        }
    }

    void UpdateScrollViewToggles()
    {
        if (targetTexFacial == null) { return; }

        baseToggles[targetTexFacial.selectBaseIndex + 1].isOn = true;
        eyeblowToggles[targetTexFacial.selectEyeblowIndex + 1].isOn = true;
        eyeToggles[targetTexFacial.selectEyeIndex + 1].isOn = true;
        mouthToggles[targetTexFacial.selectMouthIndex + 1].isOn = true;
        other1Toggles[targetTexFacial.other1Index + 1].isOn = true;
        other2Toggles[targetTexFacial.other2Index + 1].isOn = true;
        other3Toggles[targetTexFacial.other3Index + 1].isOn = true;
        other4Toggles[targetTexFacial.other4Index + 1].isOn = true;
    }

    void ResetScrollViews()
    {
        if (targetTexFacial == null) { return; }

        ClearScrollViewContent(baseScrollContent);
        ClearScrollViewContent(eyeblowScrollContent);
        ClearScrollViewContent(eyeScrollContent);
        ClearScrollViewContent(mouthScrollContent);
        ClearScrollViewContent(other1ScrollContent);
        ClearScrollViewContent(other2ScrollContent);
        ClearScrollViewContent(other3ScrollContent);
        ClearScrollViewContent(other4ScrollContent);

        CreateScrollViewContent(targetTexFacial.baseElements, baseScrollContent, baseToggles, (index) => {
            targetTexFacial.selectBaseIndex = index;
        });

        CreateScrollViewContent(targetTexFacial.eyeblowElements, eyeblowScrollContent, eyeblowToggles, (index) => {
            targetTexFacial.selectEyeblowIndex = index;
        });

        CreateScrollViewContent(targetTexFacial.eyeElements, eyeScrollContent, eyeToggles, (index) => {
            targetTexFacial.selectEyeIndex = index;
        });

        CreateScrollViewContent(targetTexFacial.mouthElements, mouthScrollContent, mouthToggles, (index) => {
            targetTexFacial.selectMouthIndex = index;
        });

        CreateScrollViewContent(targetTexFacial.otherElements, other1ScrollContent, other1Toggles, (index) => {
            targetTexFacial.other1Index = index;
        });

        CreateScrollViewContent(targetTexFacial.otherElements, other2ScrollContent, other2Toggles, (index) => {
            targetTexFacial.other2Index = index;
        });

        CreateScrollViewContent(targetTexFacial.otherElements, other3ScrollContent, other3Toggles, (index) => {
            targetTexFacial.other3Index = index;
        });

        CreateScrollViewContent(targetTexFacial.otherElements, other4ScrollContent, other4Toggles, (index) => {
            targetTexFacial.other4Index = index;
        });
    }

    void CreateScrollViewContent(List<TexFacialController.TexFacialElement> elements, Transform content, List<Toggle> toggles, UnityAction<int> onToggle)
    {
        var toggleGroup = content.GetComponent<ToggleGroup>();

        toggles.Clear();

        //無選択（-1）の場合
        {
            var nodeObject = Instantiate(prefabNode, content, false);
            nodeObject.SetActive(true);
            var toggle = nodeObject.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            var text = nodeObject.GetComponentInChildren<Text>();
            text.text = "None";

            int index = -1;
            toggle.onValueChanged.AddListener((enable) => {
                if (!enable) { return; }

                if (onToggle != null)
                {
                    onToggle.Invoke(index);
                }
            });

            toggles.Add(toggle);
        }

        for(int i=0; i< elements.Count;i++)
        {
            var element = elements[i];
            var nodeObject = Instantiate(prefabNode, content, false);
            nodeObject.SetActive(true);
            var toggle = nodeObject.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            var text = nodeObject.GetComponentInChildren<Text>();
            text.text = element.name;

            int index = i;
            toggle.onValueChanged.AddListener((enable) => {
                if (!enable) { return; }

                if (onToggle != null)
                {
                    onToggle.Invoke(index);
                }
            });

            toggles.Add(toggle);
        }
    }

    void ClearScrollViewContent(Transform content)
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
