using ABCodeworld.OmniDoor3D;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GarageButton : XRSimpleInteractable
{
    public OmniDoor3DController door;
    private bool playerInside = false;

    protected override void Awake()
    {
        base.Awake();
        selectEntered.AddListener(OnPressed);
    }

    public void OnPressed(SelectEnterEventArgs args)
    {
        door.OpenDoor?.Invoke();
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return playerInside && base.IsSelectableBy(interactor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

}