using ABCodeworld.OmniDoor3D;
using UnityEngine;
using UnityEngine.Audio;
public class Valve : MonoBehaviour
{
    public HingeJoint hinge;
    public float unscrewProgress = 0f;
    public float unscrewSpeed = 0.7f;
    public float movementThreshold = 1f;

    private float lastAngle = 0f;
    public OmniDoor3DController door;
    public AudioSource valveSound;

    
    void FixedUpdate()
    {
        float currentAngle = hinge.angle;

        if (float.IsNaN(currentAngle)) {
            currentAngle = 0f;
        }

        //Clean Delta
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        //Convert movement into progress
        unscrewProgress += Mathf.Abs(delta) * unscrewSpeed;

        //Clamp properly
        unscrewProgress = Mathf.Clamp01(unscrewProgress);
        if (!valveSound.isPlaying && Mathf.Abs(delta) > movementThreshold)
        {
            valveSound.Play();
        }
        else
        {
            valveSound.Stop();
        }
        if (unscrewProgress >= 1f)
        {
            door.OpenDoor?.Invoke();
            unscrewProgress = 0f;
        }
        lastAngle = currentAngle;
    }
 
 
}
