using NUnit.Framework.Internal;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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
        //if (!(currentState.stateName.Equals("Tomato")))
        //{
        //UpdateCollider();
        //}
    }
    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectExited.AddListener(OnReleased);
        grab.selectEntered.AddListener(OnGrabbed);
        XRSocketInteractor socket = GetComponentInChildren<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnPlaced);
    }
    void OnPlaced(SelectEnterEventArgs args)
    {
        Rigidbody rb = args.interactableObject.transform.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
    void OnReleased(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        StartCoroutine(SnapNextFrame());

    }

    private IEnumerator SnapNextFrame()
    {
        yield return null; // wait 1 frame

        TrySnapToSocket();
    }
    void TrySnapToSocket()
    {
        XRSocketInteractor socket = GetClosestSocket();

        if (socket == null) return;

        IXRSelectInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null && !socket.hasSelection)
        {
            socket.StartManualInteraction(grabInteractable);
        }


    }

    XRSocketInteractor GetClosestSocket()
    {
        XRSocketInteractor[] sockets = FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);
        XRSocketInteractor closest = null;
        float minDist = 0.2f; // snapping range

        foreach (var s in sockets)
        {
            float dist = Vector3.Distance(transform.position, s.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = s;
            }
        }

        return closest;
    }
    void OnGrabbed(SelectEnterEventArgs args)
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
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
        XRSocketInteractor socket = GetComponentInChildren<XRSocketInteractor>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider>();
        }
        box.size = meshF.mesh.bounds.size;
        box.center = meshF.mesh.bounds.center;
        socket.transform.position = GetTopCenter(box);
    }
    Vector3 GetTopCenter(Collider col)
    {
        Bounds b = col.bounds;
        float minY = 0.21f;
        Vector3 newTopCenter = Vector3.zero;
        if (b.max.y < 0.2f)
        {
            newTopCenter = new Vector3(
            b.center.x,
            minY,
            b.center.z);
        }
        else
        {
            newTopCenter = new Vector3(
            b.center.x,
            b.max.y,
            b.center.z);
        }
        
        return newTopCenter;
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