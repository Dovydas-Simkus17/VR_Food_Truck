using UnityEngine;
public class Valve : MonoBehaviour
{
    public HingeJoint hinge;

    public float unscrewProgress = 0f; // 0 = closed, 1 = fully open
    public float unscrewSpeed = 0.01f;

    private float lastAngle;

    public float autoCloseSpeed = 0.1f;

    void FixedUpdate()
    {
        float currentAngle = hinge.angle;
        float delta = currentAngle - lastAngle;
        Debug.Log(currentAngle);
        if (float.IsNaN(currentAngle))
        {
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
