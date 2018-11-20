using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RootMotion.FinalIK;

public class SetupFBBIKCharacter : EditorWindow
{
    public GameObject target;
    public string message = "";
    public string errorMessage = "";

    [MenuItem("UVAE/SetupFBBIKCharacter")]
    static void Open()
    {
        GetWindow<SetupFBBIKCharacter>();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Fullbody Biped IKコンポーネントを持ったキャラクターをtargetにセットして、Setupボタンを押してください");
        target = EditorGUILayout.ObjectField("target",target, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Setup"))
        {
            message = "";
            errorMessage = "";
            Setup();
        }

        if (!string.IsNullOrEmpty(message))
        {
            EditorGUILayout.LabelField(message);
        }

        GUIStyle style;
        if (!string.IsNullOrEmpty(errorMessage))
        {
            if (errorMessage == "OK")
            {
                style = new GUIStyle();
                style.normal.textColor = Color.blue;
                EditorGUILayout.LabelField("Completed! 正常に処理完了しました", style);
            }
            else
            {
                style = new GUIStyle();
                style.normal.textColor = Color.red;
                EditorGUILayout.LabelField("Error! "+errorMessage, style);
            }
        }

    }

    GameObject CreateEffector(string name, Transform root, Transform parentTransform)
    {
        return CreateEffector(name,root, parentTransform, Vector3.zero);
    }

    GameObject CreateEffector(string name,Transform root, Transform parentTransform, Vector3 offset)
    {
        GameObject effector = new GameObject();
        effector.name = name;
        effector.tag = "IKNode";
        effector.transform.position = parentTransform.position + offset;
        effector.transform.rotation = parentTransform.rotation;
        effector.transform.SetParent(root,true);
        return effector;
    }

    void Setup()
    {
        if (target == null)
        {
            errorMessage ="targetがセットされていません";
            return;
        }

        var fbbik = target.GetComponent<FullBodyBipedIK>();
        if (fbbik == null)
        {
            errorMessage = "targetにFullbody Biped IKがセットされていません";
            return;
        }

        var head = fbbik.references.head;
        if (head == null)
        {
            errorMessage = "Fullbody Biped IKにheadのreferenceがセットされていません";
            return;
        }
        var body = fbbik.references.spine;
        if (body == null)
        {
            errorMessage = "Fullbody Biped IKにbodyのreferenceがセットされていません";
            return;
        }
        var lefthand = fbbik.references.leftHand;
        if (lefthand == null)
        {
            errorMessage = "Fullbody Biped IKにlefthandのreferenceがセットされていません";
            return;
        }
        var righthand = fbbik.references.rightHand;
        if (righthand == null)
        {
            errorMessage = "Fullbody Biped IKにrighthandのreferenceがセットされていません";
            return;
        }
        var leftForearm = fbbik.references.leftForearm;
        if (leftForearm == null)
        {
            errorMessage = "Fullbody Biped IKにleftForearmのreferenceがセットされていません";
            return;
        }
        var rightForearm = fbbik.references.rightForearm;
        if (rightForearm == null)
        {
            errorMessage = "Fullbody Biped IKにrightForearmのreferenceがセットされていません";
            return;
        }
        var leftFoot = fbbik.references.leftFoot;
        if (leftFoot == null)
        {
            errorMessage = "Fullbody Biped IKにleftFootのreferenceがセットされていません";
            return;
        }
        var rightFoot = fbbik.references.rightFoot;
        if (rightFoot == null)
        {
            errorMessage = "Fullbody Biped IKにrightFootのreferenceがセットされていません";
            return;
        }
        var leftCalf = fbbik.references.leftCalf;
        if (leftCalf == null)
        {
            errorMessage = "Fullbody Biped IKにleftCalfのreferenceがセットされていません";
            return;
        }
        var rightCalf = fbbik.references.rightCalf;
        if (rightCalf == null)
        {
            errorMessage = "Fullbody Biped IKにrightCalfのreferenceがセットされていません";
            return;
        }

        var leftShoulder = fbbik.references.leftUpperArm;
        if (leftShoulder == null)
        {
            errorMessage = "Fullbody Biped IKにleftUpperArmのreferenceがセットされていません";
            return;
        }

        var rightShoulder = fbbik.references.rightUpperArm;
        if (rightShoulder == null)
        {
            errorMessage = "Fullbody Biped IKにrightUpperArmのreferenceがセットされていません";
            return;
        }

        Transform rootTransform = target.transform;

        //eyesは推奨（なくてもＯＫ）
        var eyes = fbbik.references.eyes;
        if (eyes != null && eyes.Length > 0)
        {
            var lookatIk = target.GetComponent<LookAtIK>();
            if (lookatIk == null)
            {
                lookatIk = target.AddComponent<LookAtIK>();
            }

            List<IKSolverLookAt.LookAtBone> lookatBoneList = new List<IKSolverLookAt.LookAtBone>();
            foreach (var eye in eyes)
            {
                lookatBoneList.Add(new IKSolverLookAt.LookAtBone(eye));
            }
            lookatIk.solver.eyes = lookatBoneList.ToArray();

            lookatIk.solver.bodyWeight = 0f;
            lookatIk.solver.headWeight = 0f;
            lookatIk.solver.eyesWeight = 1f;

            GameObject lookatEffector = CreateEffector("lookat", head.transform, head, target.transform.forward * 0.3f);
            lookatIk.solver.target = lookatEffector.transform;
        }
        else
        {
            message += "eyeTargetが見つからなかったため、lookAtIKはセットしてません。必要に応じて手動でセットしてください";
        }

        GameObject headEffector = CreateEffector("head", rootTransform, head);

        GameObject bodyEffector = CreateEffector("body", rootTransform, body[0]);

        GameObject leftHandEffector = CreateEffector("leftHand", rootTransform, lefthand);

        GameObject leftElbowEffector = CreateEffector("leftElbow", rootTransform, leftForearm);
        leftElbowEffector.transform.SetParent(leftHandEffector.transform, true);

        GameObject rightHandEffector = CreateEffector("rightHand", rootTransform, righthand);

        GameObject rightElbowEffector = CreateEffector("rightElbow", rootTransform, rightForearm);
        rightElbowEffector.transform.SetParent(rightHandEffector.transform, true);

        GameObject leftFootEffector = CreateEffector("leftFoot", rootTransform, leftFoot);

        GameObject leftKneeEffector = CreateEffector("leftKnee", rootTransform, leftCalf);
        leftKneeEffector.transform.SetParent(leftFootEffector.transform, true);

        GameObject rightFootEffector = CreateEffector("rightFoot", rootTransform, rightFoot);

        GameObject rightKneeEffector = CreateEffector("rightKnee", rootTransform, rightCalf);
        rightKneeEffector.transform.SetParent(rightFootEffector.transform, true);

        GameObject leftShoulderEffector = CreateEffector("leftShoulder", rootTransform, leftShoulder);
        GameObject rightShoulderEffector = CreateEffector("rightShoulder", rootTransform, rightShoulder);

        fbbik.solver.bodyEffector.target = bodyEffector.transform;

        var fbbIkHeadEffector = headEffector.AddComponent<FBBIKHeadEffector>();
        fbbIkHeadEffector.ik = fbbik;
        fbbIkHeadEffector.positionWeight = 1f;
        fbbIkHeadEffector.rotationWeight = 1f;

        fbbik.solver.leftHandEffector.target = leftHandEffector.transform;
        fbbik.solver.leftHandEffector.positionWeight = 1f;
        fbbik.solver.leftHandEffector.rotationWeight = 1f;
        fbbik.solver.leftArmChain.bendConstraint.bendGoal = leftElbowEffector.transform;
        fbbik.solver.leftArmChain.bendConstraint.weight = 1f;
        fbbik.solver.leftShoulderEffector.target = leftShoulderEffector.transform;
        fbbik.solver.leftShoulderEffector.positionWeight = 1f;
        fbbik.solver.leftShoulderEffector.rotationWeight = 1f;
        fbbik.solver.leftFootEffector.target = leftFootEffector.transform;
        fbbik.solver.leftFootEffector.positionWeight = 1f;
        fbbik.solver.leftFootEffector.rotationWeight = 1f;
        fbbik.solver.leftLegChain.bendConstraint.bendGoal = leftKneeEffector.transform;
        fbbik.solver.leftLegChain.bendConstraint.weight = 1f;

        fbbik.solver.rightHandEffector.target = rightHandEffector.transform;
        fbbik.solver.rightHandEffector.positionWeight = 1f;
        fbbik.solver.rightHandEffector.rotationWeight = 1f;
        fbbik.solver.rightArmChain.bendConstraint.bendGoal = rightElbowEffector.transform;
        fbbik.solver.rightArmChain.bendConstraint.weight = 1f;
        fbbik.solver.rightShoulderEffector.target = rightShoulderEffector.transform;
        fbbik.solver.rightShoulderEffector.positionWeight = 1f;
        fbbik.solver.rightShoulderEffector.rotationWeight = 1f;
        fbbik.solver.rightFootEffector.target = rightFootEffector.transform;
        fbbik.solver.rightFootEffector.positionWeight = 1f;
        fbbik.solver.rightFootEffector.rotationWeight = 1f;
        fbbik.solver.rightLegChain.bendConstraint.bendGoal = rightKneeEffector.transform;
        fbbik.solver.rightLegChain.bendConstraint.weight = 1f;

        target.tag = "Node";

        errorMessage = "OK";
    }

}
