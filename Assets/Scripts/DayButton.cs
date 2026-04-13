using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DayButton : MonoBehaviour
{
    public bool isOpen;
    private XRSimpleInteractable interactable;
    private void Awake()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnPressed);
    }

    public void OnPressed(SelectEnterEventArgs args)
    {

        Game_Manager.sharedInstance.StartDay();

    }

    //private void OnDestroy()
    //{
    //    interactable.selectEntered.RemoveListener(OnPressed);
    //}
}