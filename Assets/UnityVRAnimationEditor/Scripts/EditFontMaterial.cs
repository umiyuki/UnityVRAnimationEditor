using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditFontMaterial : MonoBehaviour
{
    [SerializeField] TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh.font.material.renderQueue = 3005;        
    }

}
