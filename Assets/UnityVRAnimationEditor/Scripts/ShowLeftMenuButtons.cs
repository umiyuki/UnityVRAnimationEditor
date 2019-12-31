using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLeftMenuButtons : MonoBehaviour
{
    [SerializeField] Button animationsButton;
    [SerializeField] Button handControllerButton;
    [SerializeField] Button texFacialButton;
    string noneText = "----";

    [SerializeField] Text targetObjectText;
    [SerializeField] Text targetAnimationText;

    // Update is called once per frame
    void Update()
    {
        var rootObject = wAnimationWindowHelper.GetAnimationWindowCurrentRootGameObject();
        var targetAnimation = wAnimationWindowHelper.GetAnimationWindowCurrentClip();

        if (rootObject == null)
        {
            if (animationsButton.interactable) { animationsButton.interactable = false; }
            if (handControllerButton.interactable) { handControllerButton.interactable = false; }
            if (texFacialButton.interactable) { texFacialButton.interactable = false; }
        }
        else if (targetAnimation == null)
        {
            if (!animationsButton.interactable) { animationsButton.interactable = true; }
            if (handControllerButton.interactable) { handControllerButton.interactable = false; }
            if (texFacialButton.interactable) { texFacialButton.interactable = false; }
        }
        else
        {
            if (!animationsButton.interactable) { animationsButton.interactable = true; }
            if (!handControllerButton.interactable) { handControllerButton.interactable = true; }
            if (!texFacialButton.interactable) { texFacialButton.interactable = true; }
        }

        if (rootObject != null)
        {
            if (targetObjectText.text != rootObject.name)
            {
                targetObjectText.text = rootObject.name;
            }
        }
        else if (targetObjectText.text != noneText)
            {
                targetObjectText.text = noneText;
        }

        if (targetAnimation != null)
        {
            if (targetAnimationText.text != targetAnimation.name)
            {
                targetAnimationText.text = targetAnimation.name;
            }
        }
        else if (targetAnimationText.text != noneText)
        {
            targetAnimationText.text = noneText;
        }
    }
}
