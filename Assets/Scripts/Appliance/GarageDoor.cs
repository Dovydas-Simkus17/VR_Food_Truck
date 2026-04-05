using Unity.VRTemplate;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    public Transform door;

    public Vector3 closedPosition;
    public Vector3 openPosition;

    public XRKnob valve;

    void Update()
    {
        float t = valve.value;

        door.localPosition = Vector3.Lerp(closedPosition, openPosition, t);
        door.localRotation = Quaternion.Lerp(
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(-270, 0, 0),
            t
        );
    }
}
