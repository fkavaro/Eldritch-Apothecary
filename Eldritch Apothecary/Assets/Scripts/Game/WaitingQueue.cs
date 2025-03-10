using UnityEngine;
using System.Collections.Generic;

public class WaitingQueue
{
    readonly List<Transform> queuePositions = new();
    readonly Queue<Client> clientQueue = new();

    public WaitingQueue(List<Transform> queuePositions)
    {
        this.queuePositions = queuePositions;
    }

    void UpdateQueuePositions()
    {
        if (clientQueue.Count == 0) return;

        int index = 0;
        foreach (Client client in clientQueue)
        {
            client.SetTarget(queuePositions[index].position);
            index++;
        }
    }

    public void Add(Client client)
    {
        clientQueue.Enqueue(client);
        UpdateQueuePositions();
    }

    public void Leave()
    {
        clientQueue.Dequeue();
        UpdateQueuePositions();
    }

    public Vector3 FirstInLine()
    {
        return queuePositions[0].position;
    }
}
