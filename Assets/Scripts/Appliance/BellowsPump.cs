using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(HingeJoint))]
public class BellowsPump : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Pump Settings")]
    public float pumpThreshold = 10f;     // Minimum speed to count as pumping
    public float heatMultiplier = 0.1f;    // How much hear per movement
    public float maxPumpSpeed = 200f;     // Clamp to prevent exploits
    public bool onlyCompress = true;      // Only count one direction

    [Header("Smoothing")]
    public float smoothing = 10f;

    [Header("Output")]
    public float heatAmount;               // Total generated heat
    [Header("Fire")]
    public ParticleSystem fireParticles;
    public float minEmission = 10f;
    public float maxEmission = 100f;

    public float minSize = 0.5f;
    public float maxSize = 2f;

    [Header("Haptics")]
    public float hapticAmplitude = 0.2f;
    public float hapticDuration = 0.05f;

    private float lastAngle;
    private float smoothedSpeed;

    private XRBaseInputInteractor controller;

    void Awake()
    {
        if (!hinge) hinge = GetComponent<HingeJoint>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void FixedUpdate()
    {
        float currentAngle = hinge.angle;
        //Debug.Log(currentAngle);
        // Calculate angular speed
        if(Time.fixedDeltaTime <= 0f) return;
        float rawSpeed = Mathf.DeltaAngle(lastAngle,currentAngle) / Time.fixedDeltaTime;

        // Clamp extreme values
        rawSpeed = Mathf.Clamp(rawSpeed, -maxPumpSpeed, maxPumpSpeed);
        //Debug.Log("this is rawSpeed: " + rawSpeed);
        if (float.IsNaN(rawSpeed) || float.IsInfinity(rawSpeed))
        {
            rawSpeed = 0f;
        }

        if (float.IsNaN(smoothedSpeed) || float.IsInfinity(smoothedSpeed))
        {
            smoothedSpeed = 0f;
        }
        // Smooth it
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, rawSpeed, Time.fixedDeltaTime * smoothing);
        //Debug.Log("this is smooth speed: " + smoothedSpeed);
        // Determine if valid pump motion
        bool isCompressing = smoothedSpeed > pumpThreshold;

        if (onlyCompress)
        {
            if (isCompressing)
            {
                Pump(smoothedSpeed);
            }
        }
        else
        {
            if (Mathf.Abs(smoothedSpeed) > pumpThreshold)
            {
                Pump(Mathf.Abs(smoothedSpeed));
            }
        }

        lastAngle = currentAngle;

        //Cools down over time
        heatAmount = heatAmount - Time.fixedDeltaTime;
        if (heatAmount < 0f) { heatAmount = 0f; }
        UpdateFire(heatAmount);
    }

    void Pump(float speed)
    {
        float heat = speed * heatMultiplier * Time.fixedDeltaTime;
        heatAmount += heat;

        // Optional: trigger haptics
        if (controller != null)
        {
            controller.SendHapticImpulse(hapticAmplitude, hapticDuration);
        }

        // Debug
        //Debug.Log("Current heat: " + heatAmount);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        controller = args.interactorObject.transform.GetComponent<XRBaseInputInteractor>();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        controller = null;
    }
    void UpdateFire(float intensity)
    {
        var emission = fireParticles.emission;
        emission.rateOverTime = Mathf.Lerp(minEmission, maxEmission, intensity);

        var main = fireParticles.main;
        main.startSize = Mathf.Lerp(minSize, maxSize, intensity);
    }
}