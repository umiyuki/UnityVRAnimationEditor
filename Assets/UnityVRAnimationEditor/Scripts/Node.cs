using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEditor;
using System;
using System.Reflection;

public class Node : MonoBehaviour {

    public GenerateNodes generateNodes;
    public Transform followTarget ;
    BoxCollider targetCollider;
    Rigidbody targetRigidbody;
    public float colliderSize = 0.1f;
    VRTK.VRTK_InteractableObject interactableObject;

    MeshFilter meshFilter;
    public Mesh wireframeMesh;
    public Mesh normalMesh;

    Renderer renderer;

    [SerializeField]public Material normalMaterial;
    [SerializeField]public Material highlightedMaterial;

    public static bool moveConstraint = false;
    public static bool rotateConstraint = false;
    public static bool scaleConstraint = false;

    public static int undoID = 0;

    bool grabbing = false;

    public enum eNodeType
    {
        ROOT,
        IK,
        FK,
    }

    public eNodeType type = eNodeType.ROOT;

    bool isParent = false;//親のノードかどうか。一番親のノードから階層順にFollowTargetする
    Node[] childNodes; //子のノード
    Node parentNode; //親のノード

    Vector3 savedPosition;
    Quaternion savedQuaternion;
    Vector3 savedScale;

    static Type typeAnimationUtility;

    // Use this for initialization
    protected virtual void Start () {

        //一度だけキャッシュする
        if (typeAnimationUtility == null)
        {
            typeAnimationUtility = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.AnimationUtility");
        }

        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();
        renderer.material = normalMaterial;

        transform.localScale = new Vector3(colliderSize, colliderSize, colliderSize);
        if (followTarget == null) { return; }

        //既存Collider削除
        var colliders = followTarget.gameObject.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            DestroyImmediate(collider);
        }

        //既存Rigidbody削除
        var rigidBody = followTarget.gameObject.GetComponent<Rigidbody>();
        if (rigidBody)
        {
            DestroyImmediate(rigidBody);
        }

        //その他既存の余計な物削除
        var fixedJointGrabAttach = followTarget.GetComponent<VRTK.GrabAttachMechanics.VRTK_FixedJointGrabAttach>();
        if (fixedJointGrabAttach) { DestroyImmediate(fixedJointGrabAttach); }

        interactableObject = followTarget.GetComponent<VRTK.VRTK_InteractableObject>();
        if (interactableObject) { DestroyImmediate(interactableObject); }

        followTarget.gameObject.layer = LayerMask.NameToLayer("Node");

        targetCollider = followTarget.gameObject.AddComponent<BoxCollider>();
        targetCollider.center = Vector3.zero;
        fixedJointGrabAttach = followTarget.gameObject.AddComponent<VRTK.GrabAttachMechanics.VRTK_FixedJointGrabAttach>();
        fixedJointGrabAttach.precisionGrab = true;
        targetRigidbody = followTarget.gameObject.AddComponent<Rigidbody>();
        targetRigidbody.isKinematic = true;

        //targetにScalingNodeが付いてたらノードスケール変更
        var scalingNode = followTarget.GetComponent<ScalingNode>();
        if (scalingNode)
        {
            colliderSize *= scalingNode.scale;
        }

        /*if (type == eNodeType.FK)//FKNodeは移動できない
        {
            rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        }*/

        interactableObject = followTarget.gameObject.AddComponent<VRTK.VRTK_InteractableObject>();
        interactableObject.isGrabbable = true;
        interactableObject.InteractableObjectGrabbed += new VRTK.InteractableObjectEventHandler(DoObjectGrab);
        interactableObject.InteractableObjectUngrabbed += new VRTK.InteractableObjectEventHandler(DoObjectUngrabbed);

        //親のNodeかどうか
        var parentTarget = followTarget.parent;
        if (parentTarget == null)
        {
            isParent = true;
        }
        else
        {
            if (generateNodes.transformToNodeDic.ContainsKey(parentTarget)) //親がNodeなら子
            {
                isParent = false;
                parentNode = generateNodes.transformToNodeDic[parentTarget];
            }
            else
            {
                isParent = true;
                parentNode = null;
            }
        }

        List<Node> childNodeList = new List<Node>();
        //子のNodeをすべて取得
        for (int i = 0; i < followTarget.childCount; i++)
        {
            var child = followTarget.GetChild(i);
            if (generateNodes.transformToNodeDic.ContainsKey(child))
            {
                childNodeList.Add(generateNodes.transformToNodeDic[child]);
            }
        }
        childNodes = childNodeList.ToArray();
    }

    private void InteractObjectHighlighterHighlighted(object o, InteractObjectHighlighterEventArgs e)
    {
        if (e.interactionType == VRTK_InteractableObject.InteractionType.Touch)
        {
            meshFilter.sharedMesh = normalMesh;
        }
    }

    private void InteractObjectHighlighterUnhighlighted(object o, InteractObjectHighlighterEventArgs e)
    {
        if (e.interactionType == VRTK_InteractableObject.InteractionType.Untouch)
        {
            meshFilter.sharedMesh = wireframeMesh;
        }
    }


    private void DoObjectGrab(object sender, VRTK.InteractableObjectEventArgs e)
    {
        //Debug.Log(e.interactingObject.name);

        //移動、回転、拡縮のconstraintをrigidbodyに適用
        RigidbodyConstraints constraint = RigidbodyConstraints.None;
        if (moveConstraint || type == eNodeType.FK)
        {
            constraint |= RigidbodyConstraints.FreezePosition;
        }
        if (rotateConstraint)
        {
            constraint |= RigidbodyConstraints.FreezeRotation;
        }
        targetRigidbody.constraints = constraint;
        //Debug.Log(targetRigidbody.name);

        //Grabする前のtransformを記録しておく
        savedPosition = followTarget.localPosition;
        savedQuaternion = followTarget.localRotation;
        savedScale = followTarget.localScale;

        if (generateNodes.RecordAllChildObjects)
        {
            generateNodes.RecordAllGrab();
        }


        grabbing = true;
    }

    private void DoObjectUngrabbed(object sender, VRTK.InteractableObjectEventArgs e)
    {
        string undoIDStr = "move target" + undoID.ToString();

        var tempPosition = followTarget.localPosition;
        var tempRotation = followTarget.localRotation;
        var tempScale = followTarget.localScale;

        //Grabする前の時点と比較
        followTarget.localPosition = savedPosition;
        followTarget.localRotation = savedQuaternion;
        followTarget.localScale = savedScale;
        

        //アングラブ時にアンドゥに記録する事でアニメーションウインドウで録画できる
        UnityEditor.Undo.RecordObject(followTarget, undoIDStr);
        //UnityEditor.Undo.RegisterCompleteObjectUndo(followTarget, undoIDStr);
        undoID++;

        
        followTarget.localPosition = tempPosition;
        followTarget.localRotation = tempRotation;
        followTarget.localScale = tempScale;
        
        UnityEditor.Undo.FlushUndoRecordObjects();
        

        grabbing = false;
    }

    private void RecordObject()
    {

        AnimationClip clip = wAnimationWindowHelper.GetAnimationWindowCurrentClip();
        if (clip == null) { return; }
        if (PlayManually.IsPlaying() && clip.length < wAnimationWindowHelper.GetCurrentTime()) { return; } //クリップが終了してたら

        Transform rootObjectTransform = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject().transform;
        string path = AnimationRecorderHelper.GetTransformPathName(rootObjectTransform, followTarget.transform);

        //今編集中のアニメーターと無関係なオブジェクトならレコードしない
        if (path == null)
        {
            return;
        }

        float time;
        if (PlayManually.IsPlaying())
        {
            time = PlayManually.nowFrameTime;
        }
        else
        {
            time =wAnimationWindowHelper.GetCurrentTime();
        }

        AnimationWindowController.SetCurve(time, path, followTarget.transform, clip);

        /*
        if (generateNodes.RecordAllChildObjects)
        {
            generateNodes.RecordAllUngrab();
        }*/

        //Grabする前のtransformを記録しておく
        /*savedPosition = followTarget.localPosition;
        savedQuaternion = followTarget.localRotation;
        savedScale = followTarget.localScale;*/
    }

    // Update is called once per frame
    protected virtual void Update () {

        if (followTarget == null) { return; }

        //ターゲットが非表示ならノードも非表示にする
        if (followTarget.gameObject.activeInHierarchy)
        {
            if (!renderer.enabled)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            if (renderer.enabled)
            {
                renderer.enabled = false;
            }
        }

        if (isParent)
        {          
            UpdateFollow();
        }

    }

    private void LateUpdate()
    {
        if (grabbing)
        {
            UpdateGrabbing();

            RecordObject();
        }

    }

    protected virtual void UpdateGrabbing()
    {
        //親から実行する
        /*if (!isParent && parentNode != null)
        {
            parentNode.UpdateGrabbing();
        }*/

        followTarget.position = transform.position;
        followTarget.rotation = transform.rotation;
    }

    protected virtual void UpdateFollow()
    {
        var targetScale = followTarget.lossyScale;
        transform.localScale = targetScale * colliderSize;

        //targetのcolliderサイズが0.2になるように調整
        targetCollider.size = new Vector3(colliderSize , colliderSize , colliderSize );

        transform.position = followTarget.position;
        transform.rotation = followTarget.rotation;

        //親から順番にUpdateFollowする
        foreach (var child in childNodes)
        {
            child.UpdateFollow();
        }
    }

    int triggerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "GrabSphere")
        {
            meshFilter.sharedMesh = normalMesh;
            renderer.material = highlightedMaterial;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "GrabSphere")
        {
            meshFilter.sharedMesh = wireframeMesh;
            renderer.material = normalMaterial;
        }
    }

    static void SetEditorCurveNoSync(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
    {       
        typeAnimationUtility.InvokeMember("SetEditorCurveNoSync", System.Reflection.BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static, null, null, new object[] { clip, binding, curve });
    }
}
