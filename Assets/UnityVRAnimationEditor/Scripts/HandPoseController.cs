using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Events;
using UnityEditor;

public class HandPoseController : MonoBehaviour
{
    [SerializeField] Animator handModelAnimator;
    [SerializeField] GameObject handControllerParent;
    [SerializeField] GameObject uiParent;

    [SerializeField] Transform[] handIkTargets;
    [SerializeField] Transform[] handIkTargetInitTransforms;
    [SerializeField] Transform[] tempSaveHandModelTranfroms;

    HumanPoseHandler targetHandler;
    HumanPose targetPose;
    HumanPoseHandler handModelHandler;
    HumanPose handModelPose;

    Animator targetAnimator = null;

    string targetPath = null;

    int leftHandMusclesStartIndex = 55;
    int rightHandMusclesStartIndex = 75;
    int halfHandMusclesCount = 20;

    float animationTime;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonShow()
    {
        GameObject rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        if (rootObject == null)
        {
            Debug.Log("アニメーションビューで編集オブジェクトが選択されてません");
            return;
        }
        Transform rootObjectTransform = rootObject.transform;

        handControllerParent.SetActive(true);
        uiParent.SetActive(false);

        //ターゲットを検出。Humanoid形式のAnimatorを探す
        Animator[] humanoidAnimatorTransforms = rootObjectTransform.GetComponentsInChildren<Animator>();
        targetAnimator = null;
        foreach (var animator in humanoidAnimatorTransforms)
        {
            if (animator.isHuman)
            {
                targetAnimator = animator;
                break;
            }
        }

        if (targetAnimator == null)
        {
            Debug.Log("アニメーションビューで編集中のオブジェクトにヒューマノイドのアニメーターが見つかりません");
        }
        else
        {
            targetPath = AnimationRecorderHelper.GetTransformPathName(rootObjectTransform, targetAnimator.transform);

            targetHandler = new HumanPoseHandler(targetAnimator.avatar, targetAnimator.transform);
            //ターゲットモデルからハンドモデルに手のポーズをコピーする
            CopyHandPoseTargetToHandModel();
        }

        animationTime = wAnimationWindowHelper.GetCurrentTime();
    }

    void CopyHandPoseTargetToHandModel()
    {        
        targetHandler.GetHumanPose(ref targetPose);

        handModelHandler = new HumanPoseHandler(handModelAnimator.avatar, handModelAnimator.transform);

        HandHumanoidPoseChange(() => {
            for (int i = 0; i < targetPose.muscles.Length; i++)
            {
                var muscleName = HumanTrait.MuscleName[i];
                if (TraitLeftHandPropMap.ContainsKey(muscleName)) //左手ポーズプロパティ
                {
                    handModelPose.muscles[i] = targetPose.muscles[i];
                }
                else if (TraitRightHandPropMap.ContainsKey(muscleName)) //右手ポーズプロパティ
                {
                    handModelPose.muscles[i] = targetPose.muscles[i];
                }
            }
        });
    }

    private void HandHumanoidPoseChange(UnityAction doAction)
    {
        handModelHandler.GetHumanPose(ref handModelPose);

        //一時的保存
        Vector3[] tempSavePositions = new Vector3[tempSaveHandModelTranfroms.Length];
        Quaternion[] tempSaveRotations = new Quaternion[tempSaveHandModelTranfroms.Length];
        Vector3[] tempSaveScales = new Vector3[tempSaveHandModelTranfroms.Length];

        for (int i = 0; i < tempSaveHandModelTranfroms.Length; i++)
        {
            tempSavePositions[i] = tempSaveHandModelTranfroms[i].localPosition;
            tempSaveRotations[i] = tempSaveHandModelTranfroms[i].localRotation;
            tempSaveScales[i] = tempSaveHandModelTranfroms[i].localScale;
        }

        //処理
        if (doAction != null) { doAction.Invoke(); }

        handModelHandler.SetHumanPose(ref handModelPose); //セット

        //一時保存したものを復帰
        for (int i = 0; i < tempSaveHandModelTranfroms.Length; i++)
        {
            tempSaveHandModelTranfroms[i].localPosition = tempSavePositions[i];
            tempSaveHandModelTranfroms[i].localRotation = tempSaveRotations[i];
            tempSaveHandModelTranfroms[i].localScale = tempSaveScales[i];
        }

        //IKターゲットの位置をリセット
        for (int i = 0; i < handIkTargets.Length; i++)
        {
            handIkTargets[i].position = handIkTargetInitTransforms[i].position;
        }

    }

    private void Update()
    {
        float nowAnimationTime = wAnimationWindowHelper.GetCurrentTime();
        //アニメーションがシークされた？
        if (animationTime != nowAnimationTime)
        {
            animationTime = nowAnimationTime;

            CopyHandPoseTargetToHandModel();

            //IKターゲットの位置をリセット
            for (int i = 0; i < handIkTargets.Length; i++)
            {
                handIkTargets[i].position = handIkTargetInitTransforms[i].position;
            }
        }

        //ハンドモデルからターゲットモデルに手のポーズをコピーする
        targetHandler.GetHumanPose(ref targetPose);
        handModelHandler.GetHumanPose(ref handModelPose);

        for (int i = 0; i < targetPose.muscles.Length; i++)
        {
            var muscleName = HumanTrait.MuscleName[i];
            if (TraitLeftHandPropMap.ContainsKey(muscleName)) //左手ポーズプロパティ
            {
               targetPose.muscles[i] = handModelPose.muscles[i];
            }
            else if (TraitRightHandPropMap.ContainsKey(muscleName)) //右手ポーズプロパティ
            {
               targetPose.muscles[i] = handModelPose.muscles[i];
            }
        }

        targetHandler.SetHumanPose(ref handModelPose); //セット
    }

    public void AddKeyLeft()
    {
        AddKey(leftHandMusclesStartIndex, TraitLeftHandPropMap);
    }

    public void AddKeyRight()
    {
        AddKey(rightHandMusclesStartIndex, TraitRightHandPropMap);
    }

    void AddKey(int startIndex, Dictionary<string,string> traitPropMap)
    {
        AnimationClip clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null)
        {
            Debug.Log("編集中のアニメーションクリップが存在しません");
            return;
        }

        for (int i = 0; i < halfHandMusclesCount; i++)
        {
            int rightIndex = startIndex + i;
            float muscleValue = handModelPose.muscles[rightIndex];
            string muscleName = HumanTrait.MuscleName[rightIndex];

            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.path = targetPath;
            curveBinding.propertyName = traitPropMap[muscleName];
            curveBinding.type = typeof(Animator);
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, curveBinding);
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            AnimationWindowController.SetKey(curve, animationTime, muscleValue);
            AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
        }
    }

    public void CopyHandPoseLeftToRight()
    {
        HandHumanoidPoseChange(() => {
            for (int i = 0; i < halfHandMusclesCount; i++)
            {
                int leftIndex = leftHandMusclesStartIndex + i;
                int rightIndex = rightHandMusclesStartIndex + i;
                handModelPose.muscles[rightIndex] = handModelPose.muscles[leftIndex];
            }
        });
    }

    public void CopyHandPoseRightToLeft()
    {
        HandHumanoidPoseChange(() => {
            for (int i = 0; i < halfHandMusclesCount; i++)
            {
                int leftIndex = leftHandMusclesStartIndex + i;
                int rightIndex = rightHandMusclesStartIndex + i;
                handModelPose.muscles[leftIndex] = handModelPose.muscles[rightIndex];
            }
        });
    }

    public void ResetHandPoseLeft()
    {
        HandHumanoidPoseChange(() => {
            for (int i = 0; i < halfHandMusclesCount; i++)
            {
                int leftIndex = leftHandMusclesStartIndex + i;
                handModelPose.muscles[leftIndex] = 0.5f;
            }
        });
    }

    public void ResetHandPoseRight()
    {
        HandHumanoidPoseChange(() => {
            for (int i = 0; i < halfHandMusclesCount; i++)
            {
                int rightIndex = rightHandMusclesStartIndex + i;
                handModelPose.muscles[rightIndex] = 0.5f;
            }
        });
    }

    public void OnButtonClose()
    {
        handControllerParent.SetActive(false);
        uiParent.SetActive(true);
    }


    public static Dictionary<string, string> TraitLeftHandPropMap = new Dictionary<string, string>
    {
            {"Left Thumb 1 Stretched", "LeftHand.Thumb.1 Stretched"},
            {"Left Thumb Spread", "LeftHand.Thumb.Spread"},
            {"Left Thumb 2 Stretched", "LeftHand.Thumb.2 Stretched"},
            {"Left Thumb 3 Stretched", "LeftHand.Thumb.3 Stretched"},
            {"Left Index 1 Stretched", "LeftHand.Index.1 Stretched"},
            {"Left Index Spread", "LeftHand.Index.Spread"},
            {"Left Index 2 Stretched", "LeftHand.Index.2 Stretched"},
            {"Left Index 3 Stretched", "LeftHand.Index.3 Stretched"},
            {"Left Middle 1 Stretched", "LeftHand.Middle.1 Stretched"},
            {"Left Middle Spread", "LeftHand.Middle.Spread"},
            {"Left Middle 2 Stretched", "LeftHand.Middle.2 Stretched"},
            {"Left Middle 3 Stretched", "LeftHand.Middle.3 Stretched"},
            {"Left Ring 1 Stretched", "LeftHand.Ring.1 Stretched"},
            {"Left Ring Spread", "LeftHand.Ring.Spread"},
            {"Left Ring 2 Stretched", "LeftHand.Ring.2 Stretched"},
            {"Left Ring 3 Stretched", "LeftHand.Ring.3 Stretched"},
            {"Left Little 1 Stretched", "LeftHand.Little.1 Stretched"},
            {"Left Little Spread", "LeftHand.Little.Spread"},
            {"Left Little 2 Stretched", "LeftHand.Little.2 Stretched"},
            {"Left Little 3 Stretched", "LeftHand.Little.3 Stretched"},
    };

    public static Dictionary<string, string> TraitRightHandPropMap = new Dictionary<string, string>
    {
            {"Right Thumb 1 Stretched", "RightHand.Thumb.1 Stretched"},
            {"Right Thumb Spread", "RightHand.Thumb.Spread"},
            {"Right Thumb 2 Stretched", "RightHand.Thumb.2 Stretched"},
            {"Right Thumb 3 Stretched", "RightHand.Thumb.3 Stretched"},
            {"Right Index 1 Stretched", "RightHand.Index.1 Stretched"},
            {"Right Index Spread", "RightHand.Index.Spread"},
            {"Right Index 2 Stretched", "RightHand.Index.2 Stretched"},
            {"Right Index 3 Stretched", "RightHand.Index.3 Stretched"},
            {"Right Middle 1 Stretched", "RightHand.Middle.1 Stretched"},
            {"Right Middle Spread", "RightHand.Middle.Spread"},
            {"Right Middle 2 Stretched", "RightHand.Middle.2 Stretched"},
            {"Right Middle 3 Stretched", "RightHand.Middle.3 Stretched"},
            {"Right Ring 1 Stretched", "RightHand.Ring.1 Stretched"},
            {"Right Ring Spread", "RightHand.Ring.Spread"},
            {"Right Ring 2 Stretched", "RightHand.Ring.2 Stretched"},
            {"Right Ring 3 Stretched", "RightHand.Ring.3 Stretched"},
            {"Right Little 1 Stretched", "RightHand.Little.1 Stretched"},
            {"Right Little Spread", "RightHand.Little.Spread"},
            {"Right Little 2 Stretched", "RightHand.Little.2 Stretched"},
            {"Right Little 3 Stretched", "RightHand.Little.3 Stretched"},
    };

    /*
    bool isInit = false;

    [SerializeField] Transform leftHandScrollViewContent;
    [SerializeField] Transform rightHandScrollViewContent;

    [SerializeField] GameObject prefabfingerPoseNode;

    Dictionary<string, float> fingerValueDictionary = new Dictionary<string, float>(); //それぞれの指の値。keyはtraitName
    Dictionary<string, FingerPoseNode> fingerPoseNodeDictionary = new Dictionary<string, FingerPoseNode>();

    public class TargetAnimator
    {
        public TargetAnimator(string _path, Animator _animator)
        {
            path = _path;
            animator = _animator;
        }
        public string path;
        public Animator animator;
    }

    private void Start()
    {
        Init();
    }

    // Start is called before the first frame update
    void Init()
    {
        //既存ノード削除
        for (int i = leftHandScrollViewContent.childCount - 1; i >= 0; i--)
        {
            Destroy(leftHandScrollViewContent.GetChild(i).gameObject);
        }

        for (int i = rightHandScrollViewContent.childCount - 1; i >= 0; i--)
        {
            Destroy(rightHandScrollViewContent.GetChild(i).gameObject);
        }

        void GenerateNode(string traitName, Transform content)
        {
            GameObject node = Instantiate(prefabfingerPoseNode, content, false);
            node.SetActive(true);
            FingerPoseNode fingerPoseNode = node.GetComponent<FingerPoseNode>();

            fingerPoseNode.name = traitName;
            fingerPoseNode.textName.text = traitName;
            fingerPoseNode.slider.value = 0f;
            fingerPoseNode.slider.onValueChanged.AddListener((value) => {
                OnValueChangedSlider(traitName, value);
            });

            fingerValueDictionary.Add(traitName, 0f);
            fingerPoseNodeDictionary.Add(traitName, fingerPoseNode);
        }

        //スクロールビューに指ノード作成
        foreach (var pair in TraitHandPropMap)
        {
            //Left
            GenerateNode("Left" + pair.Key, leftHandScrollViewContent);
            //Right
            GenerateNode("Right" + pair.Key, rightHandScrollViewContent);
        }

        isInit = true;
    }

    //スライダーが操作されたら値を変更
    public void OnValueChangedSlider(string traitName, float sliderValue)
    {
        if (!fingerValueDictionary.ContainsKey(traitName)) {
            Debug.Log("存在しないkey");
            return;         
        }
        fingerValueDictionary[traitName] = sliderValue;
    }

    public void Show()
    {
        if (!isInit) { Init(); }

        //ターゲットを検出。Humanoid形式のAnimatorを探す
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

}
