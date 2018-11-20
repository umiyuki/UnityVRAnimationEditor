using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

//アプリ開始時にコントローラのレンダーキューを差し替え（ノードより後に描画する）
public class ControllerMaterialChanger : MonoBehaviour {
    
    

    void Update()
    {
        VRTK_ControllerReference reference = null;
        reference = VRTK_ControllerReference.GetControllerReference(gameObject);
        if (reference == null)
        { return; }
        
        var model = reference.model;
        if (model == null)
        { return; }

        var renderers = model.transform.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0)
        { return; }

        Material mat = null;
        foreach (var renderer in renderers)
        {
            //マテリアルを最初の１個だけ抜き出す
            if (mat == null)
            {
                mat = renderer.material;
                mat.renderQueue = 2002;//queueをセット
            }

            renderer.material = mat;
        }

        Destroy(this); //差し替え終わったらこのコンポーネントを破棄
    }
}
