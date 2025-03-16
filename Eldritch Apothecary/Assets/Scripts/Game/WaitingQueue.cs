using UnityEngine;
using System.Collections.Generic;

public class WaitingQueue
{
    readonly List<Transform> queuePositions;
    readonly Queue<Client> clientsQueue = new();
    readonly object queueLock = new();

    public WaitingQueue(List<Transform> queuePositions)
    {
        this.queuePositions = queuePositions;
    }

    void UpdateQueuePositions()
    {
        lock (queueLock)
        {
            if (clientsQueue.Count == 0) return;

            ApothecaryManager.Instance.clientsInQueue = clientsQueue.Count;

            int index = 0;
            foreach (Client client in clientsQueue)
            {
                client.SetTarget(queuePositions[index].position);

                if (index == 0)
                    client.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
                else
                    client.transform.LookAt(queuePositions[index - 1].position);

                index++;
            }
        }
    }

    public void Add(Client client)
    {
        lock (queueLock)
        {
            clientsQueue.Enqueue(client);
            UpdateQueuePositions();
        }
    }

    public void Leave()
    {
        lock (queueLock)
        {
            if (clientsQueue.Count > 0)
            {
                clientsQueue.Dequeue();
                UpdateQueuePositions();
            }
        }
    }

    public Vector3 FirstInLine()
    {
        lock (queueLock)
        {
            return queuePositions[0].position;
        }
    }

    public Vector3 LastInLine()
    {
        lock (queueLock)
        {
            return queuePositions[^1].position;
        }
    }
}
