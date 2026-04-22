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
    public AudioSource audioSource;
    bool isCooking;
    Renderer mat;
    MeshFilter meshF;
    private XRGrabInteractable grab;
    private Rigidbody rb;
    private Coroutine coroutine;

    private static readonly WaitForFixedUpdate _fixedUpdateWait = new WaitForFixedUpdate();

    public void Start()
    {
        mat = GetComponent<Renderer>();
        meshF = GetComponent<MeshFilter>();
        audioSource.clip = currentState.cookingClip;
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
        //XRSocketInteractor socket = GetComponentInChildren<XRSocketInteractor>();
        //socket.selectEntered.AddListener(OnPlaced);
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
        //StartCoroutine(SnapNextFrame());

    }

    private IEnumerator SnapNextFrame()
    {
        yield return _fixedUpdateWait; // wait 1 frame

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
        StopCoroutine(SnapNextFrame());


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

    //private void OnDisable()
    //{
    //    StopCoroutine(SnapNextFrame());
    //}
    public void Cook(float amount)
    {
        if (currentState.nextState == null)
        {
            return;
        }
        if (isCooking && !currentState.isCuttable)
        {
            cookTimer += amount;
            if (!audioSource.isPlaying && audioSource != null && amount > 0 && currentState.cookingClip != null)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource != null)
            {
                audioSource.Stop();
            }
        }
        
        if (cookTimer > currentState.timeTillNextState)
        {
            if(audioSource != null)
            {
                audioSource.Stop();
            }
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
            audioSource.loop = false;
            audioSource.Play();
            currentState = currentState.nextState;

            UpdateVisuals();
        }

    }
    void ResetPhysics()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.isKinematic = false;

        rb.WakeUp();
    }
    void UpdateVisuals()
    {

        mat.material = currentState.material;
        if (meshF != null && currentState.mesh != null)
        {

            meshF.mesh = currentState.mesh;
            UpdateCollider();
        }
        ResetPhysics();
        rb.WakeUp();

        audioSource.clip = currentState.cookingClip;
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
        //XRSocketInteractor socket = GetComponentInChildren<XRSocketInteractor>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider>();
        }
        box.size = meshF.mesh.bounds.size;
        box.center = meshF.mesh.bounds.center;
        //socket.attachTransform.localPosition = GetTopCenter(box);
    }
    Vector3 GetTopCenter(Collider col)
    {
        Bounds b = meshF.mesh.bounds; 

        return new Vector3(
            b.center.x,
            b.max.y + b.extents.y + 0.05f,
            b.center.z
        );

        //return newTopCenter;
    }
    public KitchenObjectSO GetCurrentState()
    {
        return currentState;
    }
    public void Setup(KitchenObjectSO newKitchenObjectso)
    {
        currentState = newKitchenObjectso;
        mat = GetComponent<Renderer>();
        meshF = GetComponent<MeshFilter>();
        UpdateVisuals();

    }
}