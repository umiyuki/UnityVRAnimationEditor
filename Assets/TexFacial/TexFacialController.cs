using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TexFacialController : MonoBehaviour
{
    [SerializeField] RenderTexture targetRenderTex;
    [SerializeField] Material alphaBlendMaterial;
    [SerializeField] Texture2D transparentTexture;

    [Serializable]
    public class TexFacialElement
    {
        public string name;
        public Texture2D texture;
    }

    [Serializable]
    public class CheckChangeValue<T>
    {
        T value;
        public UnityAction onChange;

        public bool SetAndCheckChange(T newValue)
        {
            if (value.Equals(newValue))
            {
                return false;
            }
            value = newValue;
            if (onChange != null)
            {
                onChange.Invoke();
            }
            return true;
        }
    }

    bool isDirtyTarget = false; //なにかテクスチャが変更されたらダーティ

    [SerializeField]public List<TexFacialElement> baseElements;
    [SerializeField]public int selectBaseIndex;
    CheckChangeValue<int> checkChangeBase = new CheckChangeValue<int>();

    [SerializeField] public List<TexFacialElement> eyeblowElements;
    [SerializeField] public int selectEyeblowIndex;
    CheckChangeValue<int> checkChangeEyeblow = new CheckChangeValue<int>();

    [SerializeField] public List<TexFacialElement> eyeElements;
    [SerializeField] public int selectEyeIndex;
    CheckChangeValue<int> checkChangeEye = new CheckChangeValue<int>();

    [SerializeField] public List<TexFacialElement> mouthElements;
    [SerializeField] public int selectMouthIndex;
    CheckChangeValue<int> checkChangeMouth = new CheckChangeValue<int>();

    [SerializeField] public List<TexFacialElement> otherElements;
    [SerializeField] public int other1Index;
    CheckChangeValue<int> checkChangeOther1 = new CheckChangeValue<int>();
    [SerializeField] public int other2Index;
    CheckChangeValue<int> checkChangeOther2 = new CheckChangeValue<int>();
    [SerializeField] public int other3Index;
    CheckChangeValue<int> checkChangeOther3 = new CheckChangeValue<int>();
    [SerializeField] public int other4Index;
    CheckChangeValue<int> checkChangeOther4 = new CheckChangeValue<int>();

    [SerializeField] Texture2D blinkTexture; //まばたきの眼テクスチャ
    [SerializeField] Texture2D[] lipSyncTextures; //0:口閉じ 1:あ 2:い 3:う 4:え 5:お

    public void SetDirty()
    {
        isDirtyTarget = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        checkChangeBase.onChange =  SetDirty;
        checkChangeEyeblow.onChange = SetDirty;
        checkChangeEye.onChange = SetDirty;
        checkChangeMouth.onChange = SetDirty;
        checkChangeOther1.onChange = SetDirty;
        checkChangeOther2.onChange = SetDirty;
        checkChangeOther3.onChange = SetDirty;
        checkChangeOther4.onChange = SetDirty;

        isDirtyTarget = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkChangeBase.SetAndCheckChange(selectBaseIndex);
        checkChangeEyeblow.SetAndCheckChange(selectEyeblowIndex);
        checkChangeEye.SetAndCheckChange(selectEyeIndex);
        checkChangeMouth.SetAndCheckChange(selectMouthIndex);
        checkChangeOther1.SetAndCheckChange(other1Index);
        checkChangeOther2.SetAndCheckChange(other2Index);
        checkChangeOther3.SetAndCheckChange(other3Index);
        checkChangeOther4.SetAndCheckChange(other4Index);

        //テクスチャが差し替わったら
        if (isDirtyTarget)
        {
            if (selectBaseIndex >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture1", baseElements[selectBaseIndex].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture1", transparentTexture);
            }

            if (selectEyeblowIndex >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture2", eyeblowElements[selectEyeblowIndex].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture2", transparentTexture);
            }

            if (selectEyeIndex >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture3", eyeElements[selectEyeIndex].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture3", transparentTexture);
            }

            if (selectMouthIndex >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture4", mouthElements[selectMouthIndex].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture4", transparentTexture);
            }

            if (other1Index >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture5", otherElements[other1Index].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture5", transparentTexture);
            }

            if (other2Index >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture6", otherElements[other2Index].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture6", transparentTexture);
            }

            if (other3Index >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture7", otherElements[other3Index].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture7", transparentTexture);
            }

            if (other4Index >= 0)
            {
                alphaBlendMaterial.SetTexture("_Texture8", otherElements[other4Index].texture);
            }
            else
            {
                alphaBlendMaterial.SetTexture("_Texture8", transparentTexture);
            }

            alphaBlendMaterial.SetTexture("_Texture9", transparentTexture);

            //レンダーテクスチャ描画
            Graphics.Blit(transparentTexture, targetRenderTex, alphaBlendMaterial);

            isDirtyTarget = false;
        }
    }
}
