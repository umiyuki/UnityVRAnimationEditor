using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardMainCamera : MonoBehaviour
{
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        var target = Camera.main;
        if (target == null) { return; }

        transform.LookAt(2 * transform.position - target.transform.position);
    }
}
