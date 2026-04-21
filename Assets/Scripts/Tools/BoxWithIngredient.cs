using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class BoxWithIngredient : MonoBehaviour
{
    public List<XRSocketInteractor> sockets;
    public GameObject ingredientPrefab;
    public Transform spawnPoint;
    public Image onFront;
    private void OnEnable()
    {
        foreach (var socket in sockets)
        {
            socket.selectExited.AddListener(OnChanged);
        }
    }

    private void OnDisable()
    {
        foreach (var socket in sockets)
        {
            socket.selectExited.RemoveListener(OnChanged);
        }
    }

    private void OnChanged(SelectExitEventArgs args)
    {
        CheckEmpty();
    }

    private void CheckEmpty()
    {
        foreach (var socket in sockets)
        {
            if (socket.hasSelection)
                return;
        }

        gameObject.SetActive(false);
        BoxSpawner.sharedInstance.decreaseSpawn();
    }
    public void Setup(KitchenObjectSO ingredient)
    {
        Debug.Log("We are here in Box");
        SpawnIngredient(ingredient);
    }

    void SpawnIngredient(KitchenObjectSO ingredient)
    {
        foreach (var socket in sockets)
        {
            if(socket.hasSelection) { return; }
            GameObject obj = IngredientPool.SharedInstance.GetFromPool(ingredient);
            Debug.Log("This is our ingredient " + obj);
            if (obj == null) { return; }
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = spawnPoint.rotation;
            onFront.sprite = ingredient.icon;
            IXRSelectInteractable grab = obj.GetComponent<XRGrabInteractable>();
            socket.StartManualInteraction(grab);
            

        }
    }
}
