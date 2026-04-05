using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public ServingWindow servingWindow;

    public RecipeSO[] possibleRecipes;

    public float spawnDelay = 10f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 2f, spawnDelay);
    }

    void SpawnCustomer()
    {
        if (servingWindow.currentCustomer == null)
        {
            GameObject custObj = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

            Customer cust = custObj.GetComponent<Customer>();
            RecipeSO randomOrder = possibleRecipes[Random.Range(0, possibleRecipes.Length)];

            cust.SetRecipe(randomOrder);
            servingWindow.SetCustomer(cust);
        }
    }
}
