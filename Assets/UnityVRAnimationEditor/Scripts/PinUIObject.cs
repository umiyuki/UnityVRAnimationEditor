using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinUIObject : MonoBehaviour {

    bool pinning = false;
    [SerializeField] Transform pinParentObject;
    Transform unPinParentObject;
    [SerializeField] Transform uiObject;

	// Use this for initialization
	void Start () {
        unPinParentObject = uiObject.parent;
	}


    public void TogglePin(bool toggle)
    {
        if (toggle)
        {
            uiObject.SetParent(pinParentObject, true);
            pinning = true;
        }
        else
        {
            uiObject.SetParent(unPinParentObject, true);
            uiObject.localPosition = Vector3.zero;
            uiObject.localRotation = Quaternion.identity;
            uiObject.localScale = Vector3.one;
            pinning = false;
        }
    }
}
