using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Notepad : MonoBehaviour
{
    public float maxProgress = 100f;
    
    public Customer currentCustomer;
    public NotePadUI notePadUI;
    public void AddProgress(float amount)
    {
        if (currentCustomer == null) { return; }
        currentCustomer.progress += amount;
        currentCustomer.progress = Mathf.Clamp(currentCustomer.progress, 0, maxProgress);
        //Increase UI
        notePadUI.UpdateUI();
        if (currentCustomer.progress >= maxProgress && !currentCustomer.isComplete())
        {
            //Delete Ui
            RevealOrder();
        }
    }

    private void RevealOrder()
    {
        Transform noteSpawn = currentCustomer.noteSpawnPoint;
        GameObject obj = NotePool.SharedInstance.GetFromPool();

        obj.transform.position = noteSpawn.position;
        obj.transform.rotation = noteSpawn.rotation;
        obj.GetComponent<OrderNote>().Setup(currentCustomer.currentRecipe);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(ReleaseAfterDelay(rb, 0.5f)); ;
        Debug.Log("Order revealed!");
        currentCustomer.orderNotePrefab = obj;
        currentCustomer.Completed();
    }

    IEnumerator ReleaseAfterDelay(Rigidbody rb, float delay)
    {
        rb.isKinematic = true;

        yield return new WaitForSeconds(delay);

        rb.isKinematic = false;
    }

    public void ResetPad(Customer cust)
    {
        cust.progress = 0;
        cust.orderNotePrefab.SetActive(false);
        notePadUI.ResetUI();
    }
    public bool CanWrite()
    {
        return currentCustomer != null && currentCustomer.playerInRange;
    }
}