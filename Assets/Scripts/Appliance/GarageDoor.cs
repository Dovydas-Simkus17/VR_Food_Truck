using Unity.VRTemplate;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    public Transform door;

    public Vector3 closedPosition;
    public Vector3 openPosition;

    public HingeJoint valve;

    void Update()
    {
        float t = valve.angle / 360f*10;
        if (float.IsNaN(t))
        {
            t = 0f;
        }
        if(t > (360f * 10))
        {
            t = 360f * 10;
        }
        door.localPosition = Vector3.Lerp(closedPosition, openPosition, t);
        //door.localRotation = Quaternion.Lerp(
        //    Quaternion.Euler(0, 0, 0),
        //    Quaternion.Euler(-270, 0, 0),
        //    t
        //);
    }
}
