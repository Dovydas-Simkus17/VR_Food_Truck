using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public RecipeSO currentRecipe;

    public Transform noteSpawnPoint;
    public GameObject orderNotePrefab;

    private GameObject spawnedNote;

    public void SetOrder(RecipeSO recipe)
    {
        currentRecipe = recipe;

        SpawnNote();
    }

    void SpawnNote()
    {
        if (orderNotePrefab != null)
        {
            spawnedNote = Instantiate(orderNotePrefab, noteSpawnPoint.position, noteSpawnPoint.rotation);
            spawnedNote.GetComponent<OrderNote>().Setup(currentRecipe);
        }
    }

    public void Serve(List<Ingredient> givenIngredients)
    {
        //Debug.Log("This is how many is in the PLate: " + givenIngredients.Count);
        if (CheckOrder(givenIngredients))
        {
            Debug.Log("Correct order!");
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
        SpawnNote();
    }
}