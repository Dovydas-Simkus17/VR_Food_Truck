using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{
    public Transform[] queuePoints; // positions in line
    private List<Customer> customers = new List<Customer>();
    public static CustomerQueue sharedInstance;
    public ServingWindow servingWindow;
    void Awake()
    {
        // Singleton setup
        if (sharedInstance == null)
            sharedInstance = this;
        else
            Destroy(gameObject);
    }

    public bool HasFreeSlot()
    {
        return customers.Count < queuePoints.Length;
    }

    public int AddCustomer(Customer customer)
    {
        Debug.Log("We have added a customer");
        if (!HasFreeSlot()) return -1;

        customers.Add(customer);
        UpdateFrontCustomer();
        return customers.Count - 1;
    }

    public void RemoveCustomer(Customer customer)
    {
        int index = customers.IndexOf(customer);
        if (index == -1) return;

        customers.RemoveAt(index);

        // Move everyone forward
        for (int i = index; i < customers.Count; i++)
        {
            customers[i].MoveTo(queuePoints[i], i);
        }
        UpdateFrontCustomer();
    }

    public Transform GetPoint(int index)
    {
        return queuePoints[index];
    }

    public Customer GetFrontCustomer()
    {
        if (customers.Count == 0) return null;
        return customers[0];
    }
    void UpdateFrontCustomer()
    {
        if (customers.Count > 0)
        {
            servingWindow.SetCustomer(customers[0]);
        }
    }
}