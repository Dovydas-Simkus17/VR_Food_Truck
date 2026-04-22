using UnityEngine;
using System.Collections.Generic;

public class ServingWindow : MonoBehaviour
{
    public Customer currentCustomer;
    public Notepad Notepad;
    private List<Ingredient> itemsOnPlate = new List<Ingredient>();

    public void SetCustomer(Customer newCustomer)
    {
        currentCustomer = newCustomer;
    }

    void FixedUpdate()
    {
        FindIngredients();
        if(currentCustomer == null || currentCustomer.currentState.ToString() == "Waiting") { return; }
        currentCustomer.increaseCurrentPaitence();
        Debug.Log("Ingredients in the plate " + itemsOnPlate.Count);

    }
    private Vector3 halfExtents = new Vector3(1, 1, 1);
    void FindIngredients()
    {
        itemsOnPlate.Clear();
        //Debug.Log("We are in MyCollisions");
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, halfExtents, gameObject.transform.rotation);
        //Debug.Log("Hit Count: " + hitColliders.Length);

        // Check when there is a new collider coming into contact with the box
        foreach (Collider collider in hitColliders)
        {
            Ingredient ing = collider.GetComponent<Ingredient>();

            if (ing != null && !itemsOnPlate.Contains(ing))
            {
                itemsOnPlate.Add(ing);
            }
        }

    }

    public void Serve()
    {
        if (currentCustomer != null)
        {
            currentCustomer.Serve(itemsOnPlate);
            foreach (Ingredient item in itemsOnPlate)
            {
                item.gameObject.SetActive(false);
            }

            itemsOnPlate.Clear();
            
            Notepad.ResetPad();
        }
    }
}