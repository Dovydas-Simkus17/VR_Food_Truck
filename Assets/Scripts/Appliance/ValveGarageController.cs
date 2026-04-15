using UnityEngine;
using ABCodeworld.OmniDoor3D;

public class ValveGarageController : MonoBehaviour
{
    public OmniDoor3D door;

    [Header("Valve")]
    public Transform valveWheel;
    public float degreesPerPanel = 360f;
    public int panelCount = 4;

    [Header("Auto Close")]
    public float dropSpeed = 0.05f;

    private float accumulatedDegrees;
    private float lastAngle;
    private bool isTurning;

    void Start()
    {
        lastAngle = valveWheel.localEulerAngles.z;
    }

    void FixedUpdate()
    {
        float currentAngle = valveWheel.localEulerAngles.z;
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
        lastAngle = currentAngle;
        //Debug.Log(lastAngle);
        if (isTurning)
        {
            accumulatedDegrees += Mathf.Abs(delta);

            float totalDegrees = degreesPerPanel * panelCount;
            door.OpenAmount = accumulatedDegrees / totalDegrees;
        }
        else
        {
            // slow mechanical drop
            door.OpenAmount -= dropSpeed * Time.fixedDeltaTime;

            // keep valve synced visually with door drop
            accumulatedDegrees = door.OpenAmount * degreesPerPanel * panelCount;
        }
    }

    public void SetTurning(bool active)
    {
        isTurning = active;
    }
}