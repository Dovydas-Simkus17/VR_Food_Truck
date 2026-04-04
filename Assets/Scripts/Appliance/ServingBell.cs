using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ServingBell : MonoBehaviour
{
    public ServingWindow servingWindow;
    private void OnTriggerEnter(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInputInteractor>();

        if (interactor != null)
        {
            Debug.Log("Player hand hit the bell!");
            servingWindow.Serve();
        }

    }
}
