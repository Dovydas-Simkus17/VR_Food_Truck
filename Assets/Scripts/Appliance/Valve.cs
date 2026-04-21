using ABCodeworld.OmniDoor3D;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class Valve : MonoBehaviour
{
    public HingeJoint hinge;
    public float unscrewProgress = 0f;
    public float unscrewSpeed = 0.7f;

    private float lastAngle = 0f;
    public OmniDoor3DController door;

    private XRGrabInteractable grab;
    private bool isGrabbed;
    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        grab.selectEntered.AddListener(_ => isGrabbed = true);
        grab.selectExited.AddListener(_ => isGrabbed = false);
        
    }
    void FixedUpdate()
    {


        if (isGrabbed)
        {
            hinge.useLimits = false; // optional depending setup
        }

        float currentAngle = hinge.angle;
        if (float.IsNaN(currentAngle)) {
            currentAngle = 0f;
        }
        // Clean delta using Unity's built-in safe wrap handling
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        // Convert movement into progress
        unscrewProgress += Mathf.Abs(delta) * unscrewSpeed;

        // Clamp properly
        unscrewProgress = Mathf.Clamp01(unscrewProgress);
        if (unscrewProgress >= 1f)
        {
            door.OpenDoor?.Invoke();
            unscrewProgress = 0f;
        }
        lastAngle = currentAngle;
    }
 
}
