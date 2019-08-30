using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationListManager : MonoBehaviour
{
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject[] otherCanvasObjects;
    [SerializeField] Transform content;
    [SerializeField] GameObject prefabNodeSelectAnimation;
    [SerializeField] CustomWorldKeyboard customWorldKeyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        var rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        if (rootObject == null) {
            Debug.Log("Maybe not selected any object in Animation Window.");
            return;
        }

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

        AnimationClip[] clips =  AnimationUtility.GetAnimationClips(rootObject);

        //ボタン作成
        for (int i = 0; i < clips.Length; i++)
        {
            var node = Instantiate(prefabNodeSelectAnimation, content, false);
            NodeUIButton nodeUIButton = node.GetComponent<NodeUIButton>();

            var clip = clips[i];
            nodeUIButton.text.text = clip.name;
            nodeUIButton.button.onClick.AddListener( () => {
                wAnimationWindowHelper.SetActiveAnimationClip(clip);
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

    public void OnClickAddAnimation()
    {
        canvasObject.SetActive(false);

        //キーボードでファイル名を入力させる
        customWorldKeyboard.OpenKeyboard(AddAnimation);
    }

    void AddAnimation(string filename)
    {
        canvasObject.SetActive(true);

        //ファイル名が空ならキャンセル
        if (string.IsNullOrEmpty(filename)) { return; }

        var newClip = new AnimationClip();

        //ループ設定
        var info = AnimationUtility.GetAnimationClipSettings(newClip);
        info.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(newClip, info);

        string newClipDirectory = wAnimationWindowHelper.GetAcitiveFolderPath();
        Debug.Log("newClipDirectory:" + newClipDirectory);
        //重複しない名前を生成
        string finalPath = AssetDatabase.GenerateUniqueAssetPath(newClipDirectory + "/" + filename + ".anim");

        // 該当パスにオブジェクトアセットを生成
        AssetDatabase.CreateAsset(newClip, finalPath);

        // 未保存のアセットをアセットデータベースに保存
        //AssetDatabase.SaveAssets();

        //アニメーションウインドウで選択状態にする
        wAnimationWindowHelper.AddClipToAnimationPlayerComponent(newClip);

        //スクロールビューをリセット
        Show();
    }
}
