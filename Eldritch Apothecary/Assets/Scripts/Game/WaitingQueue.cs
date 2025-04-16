using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class WaitingQueue
{
    /// <summary>
    /// A client has left the queue
    /// </summary>
    public event Action<Client> NextTurnEvent;

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
        //Debug.Log($"Client {client.name} entered the queue.");

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
                NextTurnEvent?.Invoke(clientsQueue.Peek());
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

    // TODO: FIX METHOD, bug: looks forward
    public void FixRotation(Client clientContext)
    {
        lock (queueLock)
        {
            int index = clientsQueue.ToArray().ToList().IndexOf(clientContext);
            if (index <= 0) return;

            // Rotate to next position
            Vector3 lookDirection = (queuePositions[index - 1].position - clientContext.transform.position).normalized;
            clientContext.transform.rotation = Quaternion.Euler(lookDirection);
        }
    }

    public float GetNextClientWaitingTime()
    {
        return clientsQueue.Count > 0 ? clientsQueue.Peek().timeWaiting : 0f;
    }

    public bool HasAnyClient()
    {
        lock (queueLock)
        {
            return clientsQueue.Count > 0;
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
                client.SetTargetPos(queuePositions[index].position);
                index++;
            }
        }
    }
    #endregion
}
