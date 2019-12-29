using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class BoxButton : MonoBehaviour
{
    [SerializeField] Mesh wireframeMesh;
    [SerializeField] Mesh normalMesh;

    MeshFilter meshFilter;
    Renderer renderer;

    [SerializeField] public Material normalMaterial;
    [SerializeField] public Material highlightedMaterial;

    [SerializeField] ScaleTween scaleTween;
    [SerializeField] VRTK.VRTK_InteractableObject interactableObject;

    [SerializeField] UnityEvent OnClickButton;

    // Start is called before the first frame update
    void Start()
    {
        //normalMesh = GetComponent<MeshFilter>().sharedMesh;
        //wireframeMesh = GenerateNodes.GetWireFrameCubeMesh();

        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<Renderer>();
        renderer.material = normalMaterial;
    }

    protected virtual void OnEnable()
    {
        interactableObject.InteractableObjectUsed += InteractableObjectUsed;
    }

    protected virtual void OnDisable()
    {
        interactableObject.InteractableObjectUsed -= InteractableObjectUsed;
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        scaleTween.StartTween();
        if (OnClickButton != null)
        {
            OnClickButton.Invoke();
        }
        interactableObject.StopUsing();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "GrabSphere")
        {
            //meshFilter.sharedMesh = normalMesh;
            renderer.material = highlightedMaterial;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "GrabSphere")
        {
            //meshFilter.sharedMesh = wireframeMesh;
            renderer.material = normalMaterial;
        }
    }
}
