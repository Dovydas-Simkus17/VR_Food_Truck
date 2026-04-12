using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ServingBell : MonoBehaviour
{
    public ServingWindow servingWindow;

    public float cooldown = 2f;
    private float lastRingTime;
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastRingTime < cooldown)
            return;

        lastRingTime = Time.time;

        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInputInteractor>();

        if (interactor != null)
        {
            Debug.Log("Player hand hit the bell!");
            servingWindow.Serve();
        }

    }
}
