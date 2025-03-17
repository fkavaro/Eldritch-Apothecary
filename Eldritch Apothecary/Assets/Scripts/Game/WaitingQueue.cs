using UnityEngine;
using System.Collections.Generic;
using System;

public class WaitingQueue
{
    readonly List<Transform> queuePositions;
    readonly Queue<Client> clientsQueue = new();
    readonly object queueLock = new();

    public WaitingQueue(List<Transform> queuePositions)
    {
        this.queuePositions = queuePositions;
    }

    #region PUBLIC METHODS
    public void Enter(Client client)
    {
        lock (queueLock)
        {
            if (clientsQueue.Contains(client)) return;

            clientsQueue.Enqueue(client);
            UpdateQueuePositions();
        }
    }

    public void NextTurn()
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

    public bool Contains(Client clientContext)
    {
        lock (queueLock)
        {
            return clientsQueue.Contains(clientContext);
        }
    }
    #endregion

    #region PRIVATE METHODS
    void UpdateQueuePositions()
    {
        lock (queueLock)
        {
            if (clientsQueue.Count == 0) return;

            int index = 0;
            foreach (Client client in clientsQueue)
            {
                client.SetTarget(queuePositions[index].position);
                index++;
            }
        }
    }

    internal void FixRotation(Client clientContext)
    {
        throw new NotImplementedException();

        // if (index == 0)
        //     client.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
        // else
        //     client.transform.LookAt(queuePositions[index - 1].position);
    }
    #endregion
}
