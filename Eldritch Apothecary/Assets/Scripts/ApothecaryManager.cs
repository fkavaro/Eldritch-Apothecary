using System.Collections.Generic;
using UnityEngine;

public class ApothecaryManager : MonoBehaviour
{
    public static ApothecaryManager Instance;
    public Transform[] shopStands, queuePositions;
    public Queue<Client> clientsWaiting = new();

    void Awake()
    {
        // Creates one instance if there isn't any (Singleton)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shopStands.Length == 0 || queuePositions.Length == 0)
        {
            Debug.LogError("Shop stands or queue positions are not assigned in the ApothecaryManager.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnqueueClient(Client client)
    {
        clientsWaiting.Enqueue(client);
        UpdateQueuePositions();
    }

    public void DequeueClient()
    {
        if (clientsWaiting.Count > 0)
        {
            Client client = clientsWaiting.Dequeue();
            // Method for when Client gets served
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (Client client in clientsWaiting)
        {
            // Move client to next position
            index++;
        }
    }
}