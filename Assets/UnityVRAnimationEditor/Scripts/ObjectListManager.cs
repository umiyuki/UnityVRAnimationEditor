using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectListManager : MonoBehaviour
{
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject[] otherCanvasObjects;
    [SerializeField] Transform content;
    [SerializeField] GameObject prefabNodeSelectObject;

    public void Show()
    {
        canvasObject.SetActive(true);

        foreach (var obj in otherCanvasObjects)
        {
            obj.SetActive(false);
        }

        //既存のボタン削除
        for (int i = 0; i < content.childCount; i++)
        {
            var child = content.GetChild(i);
            if (child.name != "AddAnimation")
            {
                Destroy(child.gameObject);
            }
        }

        //シーン内のAnimatorかAnimationコンポーネントを持ってるオブジェクトを列挙
        var animatorObjects = GameObject.FindObjectsOfType<Animator>();
        var animationObjects = GameObject.FindObjectsOfType<Animation>();

        HashSet<GameObject> objects = new HashSet<GameObject>();
        foreach (var obj in animatorObjects)
        {
            objects.Add(obj.gameObject);
        }

        foreach (var obj in animationObjects)
        {
            objects.Add(obj.gameObject);
        }

        //ボタン作成
        foreach(var obj in objects)
        {
            var node = Instantiate(prefabNodeSelectObject, content, false);
            NodeUIButton nodeUIButton = node.GetComponent<NodeUIButton>();

            var target = obj;
            nodeUIButton.text.text = target.name;
            nodeUIButton.button.onClick.AddListener(() => {
                Selection.objects = new Object[1] { target };
            });
        }

    }

    public void Hide()
    {
        canvasObject.SetActive(false);

        foreach (var obj in otherCanvasObjects)
        {
            obj.SetActive(true);
        }
    }

}
