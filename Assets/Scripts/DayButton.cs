using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DayButton : MonoBehaviour
{
    public bool isOpen;
    private XRSimpleInteractable interactable;
    public AudioSource source;
    private void Awake()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnPressed);
        isOpen = false;
    }

    public void OnPressed(SelectEnterEventArgs args)
    {
        if (!isOpen)
        {
            Game_Manager.sharedInstance.StartDay();
            source.Play();
            isOpen = true;
        }
    }

    //private void OnDestroy()
    //{
    //    interactable.selectEntered.RemoveListener(OnPressed);
    //}
}