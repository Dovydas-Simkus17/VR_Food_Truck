using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public RecipeSO currentRecipe;
    public Transform noteSpawnPoint;
    public GameObject orderNotePrefab;
    public Notepad notepad;
    public float progress = 0f;

    private bool alreadyCompleted = false;
    private GameObject spawnedNote;
    public float cooldown = 2f;
    private float lastRingTime;

    public bool playerInRange = false;
    public void SetOrder(RecipeSO recipe)
    {
        currentRecipe = recipe;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            notepad.currentCustomer = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            notepad.currentCustomer = null;
        }
    }

    public void Serve(List<Ingredient> givenIngredients)
    {
        if (Time.time - lastRingTime < cooldown)
            return;

        lastRingTime = Time.time;
        //Debug.Log("This is how many is in the PLate: " + givenIngredients.Count);
        if (CheckOrder(givenIngredients))
        {
            Debug.Log("Correct order!");
            GameManager.Instance.AddScore(5);
            Leave();
        }
        else
        {
            Debug.Log("Wrong order!");
        }
    }

    bool CheckOrder(List<Ingredient> given)
    {
        Debug.Log("Current List" + given.Count);
        if (given.Count != currentRecipe.kitchenObjectSOList.Count)
            return false;

        foreach (var req in currentRecipe.kitchenObjectSOList)
        {
            bool found = given.Exists(i => i.currentState == req);
            if (!found) return false;
        }

        return true;
    }

    void Leave()
    {
        Destroy(spawnedNote);
        Destroy(gameObject, 1f);
    }

    public void SetRecipe(RecipeSO recipe)
    {
        currentRecipe = recipe;
    }

    public bool isComplete()
    {
        return alreadyCompleted;
    }
    public void Completed()
    {
        alreadyCompleted = true;
    }
}