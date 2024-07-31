using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePositions;
    private Queue<CustomerAI> customerQueue = new Queue<CustomerAI>();

    public void AddCustomerToQueue(CustomerAI customer)
    {
        customerQueue.Enqueue(customer);
        UpdateCustomerPositions();
    }

    public void RemoveCustomerFromQueue(CustomerAI customer)
    {
        if (customerQueue.Count > 0 && customerQueue.Peek() == customer)
        {
            customerQueue.Dequeue();
            UpdateCustomerPositions();
        }
    }

    public bool IsFirstCustomer(CustomerAI customer)
    {
        return customerQueue.Count > 0 && customerQueue.Peek() == customer;
    }

    private void UpdateCustomerPositions()
    {
        CustomerAI[] customers = customerQueue.ToArray();
        for (int i = 0; i < customers.Length; i++)
        {
            customers[i].SetDestination(queuePositions[i]);
        }
    }
}
