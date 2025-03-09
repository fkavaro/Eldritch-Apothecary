using System;
using System.Collections.Generic;
using UnityEngine;

public class ApothecaryManager : MonoBehaviour
{
    public static ApothecaryManager Instance;
    public Transform cat, complainingPosition, entrancePosition, exitPosition;

    public Transform[] shopStands, queuePositions;

    Queue<Client> clientQueue = new();

    public int clientsWaiting;

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

    public void AddToQueue(Client client)
    {
        clientQueue.Enqueue(client);
        UpdateQueuePositions();
    }

    public void LeaveQueue(Client client)
    {
        clientQueue.Dequeue();
        UpdateQueuePositions();
    }

    void UpdateQueuePositions()
    {
        clientsWaiting = clientQueue.Count;
        if (clientQueue.Count == 0) return;

        int index = 0;
        foreach (Client client in clientQueue)
        {
            client.SetTarget(queuePositions[index].position);
            index++;
        }
    }

    public Vector3 RandomShopStand()
    {
        return shopStands[UnityEngine.Random.Range(0, shopStands.Length)].position;
    }

    public Vector3 FirstInLine()
    {
        return queuePositions[0].position;
    }
}