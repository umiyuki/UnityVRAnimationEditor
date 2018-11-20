using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OpenAnimationWindow : MonoBehaviour {

    [SerializeField] GameObject animationWindowObject;

    private void OnEnable()
    {
        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += DoStartMenuPressed;
    }

    private void OnDisable()
    {
        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed -= DoStartMenuPressed;
    }

    private void DoStartMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (animationWindowObject.activeSelf)
        {
            animationWindowObject.SetActive(false);
        }
        else
        {
            animationWindowObject.SetActive(true);
        }
    }

}
