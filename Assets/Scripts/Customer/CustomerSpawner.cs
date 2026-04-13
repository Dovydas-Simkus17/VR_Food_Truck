using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public ServingWindow servingWindow;

    public RecipeSO[] possibleRecipes;
    private Coroutine spawnRoutine;
    void OnEnable()
    {
        Game_Manager.sharedInstance.OnDayStart += SetupDay;
        Game_Manager.sharedInstance.OnDayEnd += StopSpawning;
    }

    void OnDisable()
    {
        Game_Manager.sharedInstance.OnDayStart -= SetupDay;
        Game_Manager.sharedInstance.OnDayEnd -= StopSpawning;
    }

    void SetupDay(DayData day)
    {
        StopSpawning();

        spawnRoutine = StartCoroutine(SpawnRoutine(day));

    }
    IEnumerator SpawnRoutine(DayData day)
    {
        yield return new WaitForSeconds(2f);

        int spawned = 0;

        while (spawned < day.customerCount)
        {
            if (servingWindow.currentCustomer == null)
            {
                SpawnCustomer();
                spawned++;
            }

            yield return new WaitForSeconds(day.spawnInterval);
        }

        Debug.Log("Finished spawning customers");

        StopSpawning();
    }

    void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;

        }
    }

    void SpawnCustomer()
    {
        GameObject custObj = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        Customer cust = custObj.GetComponent<Customer>();
        RecipeSO randomOrder = possibleRecipes[Random.Range(0, possibleRecipes.Length)];

        cust.SetRecipe(randomOrder);
        servingWindow.SetCustomer(cust);
    }
}
