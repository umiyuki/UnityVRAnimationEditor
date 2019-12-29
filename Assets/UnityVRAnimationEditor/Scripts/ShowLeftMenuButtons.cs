using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLeftMenuButtons : MonoBehaviour
{
    [SerializeField] Button animationsButton;
    [SerializeField] Button handControllerButton;

    // Update is called once per frame
    void Update()
    {
        if (wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject() == null)
        {
            if (animationsButton.interactable) { animationsButton.interactable = false; }
            if (handControllerButton.interactable) { handControllerButton.interactable = false; }
        }
        else if (wAnimationWindowHelper.GetAnimationWindowCurrentClip() == null)
        {
            if (!animationsButton.interactable) { animationsButton.interactable = true; }
            if (handControllerButton.interactable) { handControllerButton.interactable = false; }
        }
        else
        {
            if (!animationsButton.interactable) { animationsButton.interactable = true; }
            if (!handControllerButton.interactable) { handControllerButton.interactable = true; }
        }
    }
}
