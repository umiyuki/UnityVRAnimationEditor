using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ScalingGrabbingObject : MonoBehaviour {

    private VRTK_InteractGrab grab;

    private WrapControllerInput wrapControllerInput;

    // Use this for initialization
    void Start () {
        grab = GetComponent<VRTK_InteractGrab>();
        wrapControllerInput = GetComponent<WrapControllerInput>();
	}
	
	// Update is called once per frame
	void Update () {
        var grabbingObject = grab.GetGrabbedObject();

        VRTK_ControllerReference reference = VRTK_ControllerReference.GetControllerReference(gameObject);

        if (grabbingObject != null && !Node.scaleConstraint)
        {

            //左右スワイプでスケール変更
            if (wrapControllerInput.IsOnSwipeHorizontal)
            {
                Vector3 newScale = (1f + wrapControllerInput.diffTouchPadX) * grabbingObject.transform.localScale;
                grabbingObject.transform.localScale = newScale;
            }
        }
	}
}
