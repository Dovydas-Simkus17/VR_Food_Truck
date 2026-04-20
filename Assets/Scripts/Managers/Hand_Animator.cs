using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Hand_Animator : MonoBehaviour
{
    [SerializeField] private NearFarInteractor nearFarInteractor;
    [SerializeField] private SkinnedMeshRenderer handMesh;
    [SerializeField] private InputActionReference selectActionRef;
    [SerializeField] private InputActionReference activateActionRef;
    [SerializeField] private Animator handAnimator;

    private static readonly int activateAnim = Animator.StringToHash("activate");
    private static readonly int selectAnim = Animator.StringToHash("select");
    private void Awake()
    {
        nearFarInteractor.selectEntered.AddListener(OnGrab);
        nearFarInteractor.selectExited.AddListener(OnRelease);

    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        //Debug.Log("Something grab");
        handMesh.enabled = false;
    }
    private void OnRelease(SelectExitEventArgs args)
    {
        //Debug.Log("Something let go");
        handMesh.enabled = true;

    }

    private void Update()
    {
        handAnimator.SetFloat(activateAnim, activateActionRef.action.ReadValue<float>());
        handAnimator.SetFloat(selectAnim, selectActionRef.action.ReadValue<float>());
    }
}
