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
        float currentAngle = hinge.angle;
        if (float.IsNaN(currentAngle)) {
            currentAngle = 0f;
        }
        if (delta > 0f)
        {
            unscrewProgress += delta * unscrewSpeed;
        }
        else
        {
            // Slowly close if not actively turning
            //unscrewProgress -= autoCloseSpeed * Time.fixedDeltaTime;
        }

        unscrewProgress = Mathf.Clamp01(unscrewProgress);

        lastAngle = currentAngle;
    }
 
}
