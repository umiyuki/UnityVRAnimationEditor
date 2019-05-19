using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

//RiftだったらRadialMenuの位置調整
public class AdjustRadialMenuPosition : MonoBehaviour
{
    SDK_BaseHeadset.HeadsetType headsetType = SDK_BaseHeadset.HeadsetType.Undefined;

    [SerializeField] Vector3 OculusRiftPosition;
    [SerializeField] Vector3 OculusRiftRotation;

    // Update is called once per frame
    void Update()
    {
        if (headsetType == SDK_BaseHeadset.HeadsetType.Undefined)
        { headsetType = VRTK_DeviceFinder.GetHeadsetType(); }

        if (headsetType == SDK_BaseHeadset.HeadsetType.OculusRift)
        {
            transform.localPosition = OculusRiftPosition;
            transform.localRotation = Quaternion.Euler(OculusRiftRotation);
        }
    }
}
