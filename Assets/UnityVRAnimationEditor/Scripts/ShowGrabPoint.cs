using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ShowGrabPoint : MonoBehaviour
{
    [SerializeField] VRTK_InteractGrab interactGrab;

    // Start is called before the first frame update
    void Update()
    {
        Rigidbody target = null;
        target = interactGrab.controllerAttachPoint;

        if (target != null)
        {
            transform.position = target.transform.position;
            Destroy(this);
        }
    }

}
