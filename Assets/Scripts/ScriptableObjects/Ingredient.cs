using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Ingredient : MonoBehaviour
{
    public KitchenObjectSO currentState;
    float cookTimer = 0f;

    bool isCooking;
    Renderer mat;
    MeshFilter meshF;
    private XRGrabInteractable grab;
    private Rigidbody rb;
    public void Start()
    {
        mat = GetComponent<Renderer>();
        meshF = GetComponent<MeshFilter>();
        UpdateVisuals();
        if (!(currentState.stateName.Equals("Tomato")))
        {
            UpdateCollider();
        }
    }
    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectExited.AddListener(OnReleased);
    }
    public void Cook(float amount)
    {
        if (currentState.nextState == null)
        {
            return;
        }
        if (isCooking)
        {
            cookTimer += amount;
        }
        else
        {
            return;
        }

        if (cookTimer > currentState.timeTillNextState)
        {
            cookTimer = 0f;
            currentState = currentState.nextState;

            UpdateVisuals();
        }
    }
    public void Cut()
    {
        //Debug.Log("Somebody Tried to cut us");
        if (currentState.isCuttable)
        {
            currentState = currentState.nextState;

            UpdateVisuals();
        }

    }
    void UpdateVisuals()
    {

        mat.material = currentState.material;
        if (meshF != null && currentState.mesh != null)
        {
            meshF.mesh = currentState.mesh;
            UpdateCollider();
        }

    }

    public void SetIsCookingTrue()
    {
        this.isCooking = true;
    }
    public void SetIsCookingfalse()
    {
        this.isCooking = false;

    }
    void UpdateCollider()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider>();
        }

        box.size = meshF.mesh.bounds.size;
        box.center = meshF.mesh.bounds.center;
    }

    void OnReleased(SelectExitEventArgs args)
    {

        rb.isKinematic = false;
        rb.useGravity = true;

    }

    public KitchenObjectSO GetCurrentState()
    {
        return currentState;
    }
    public void Setup(KitchenObjectSO newKitchenObjectso)
    {
        currentState = newKitchenObjectso;

    }
}