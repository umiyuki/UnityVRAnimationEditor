using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Node : MonoBehaviour {

    public GenerateNodes generateNodes;
    public Transform followTarget ;
    BoxCollider targetCollider;
    Rigidbody targetRigidbody;
    float colliderSize = 0.15f;
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

    // Use this for initialization
    protected virtual void Start () {
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

        followTarget.gameObject.layer = LayerMask.NameToLayer("Node");

        targetCollider = followTarget.gameObject.AddComponent<BoxCollider>();
        var fixedJointGrabAttach = followTarget.gameObject.AddComponent<VRTK.GrabAttachMechanics.VRTK_FixedJointGrabAttach>();
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


    Vector3 savedPosition;
    Quaternion savedQuaternion;
    Vector3 savedScale;

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
      
        //UnityEditor.Undo.FlushUndoRecordObjects();

        grabbing = false;
    }

    private void RecordObject()
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
        undoID++;

        followTarget.localPosition = tempPosition;
        followTarget.localRotation = tempRotation;
        followTarget.localScale = tempScale;

        if (generateNodes.RecordAllChildObjects)
        {
            generateNodes.RecordAllUngrab();
        }

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

        UpdateFollow();

        if (grabbing)
        {
            RecordObject();
        }
    }

    protected virtual void UpdateFollow()
    {
        var targetScale = followTarget.lossyScale;
        transform.localScale = targetScale * colliderSize;

        //targetのcolliderサイズが0.2になるように調整
        targetCollider.size = new Vector3(colliderSize , colliderSize , colliderSize );

        transform.position = followTarget.position;
        transform.rotation = followTarget.rotation;
    }

    int triggerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ControllerHead")
        {
            meshFilter.sharedMesh = normalMesh;
            renderer.material = highlightedMaterial;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ControllerHead")
        {
            meshFilter.sharedMesh = wireframeMesh;
            renderer.material = normalMaterial;
        }

    }
}
