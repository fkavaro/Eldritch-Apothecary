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

    readonly List<Transform> _queuePositions;
    readonly Queue<Client> _clientsQueue = new();
    readonly object _queueLock = new();

    public WaitingQueue(List<Transform> queuePositions)
    {
        this._queuePositions = queuePositions;
    }

    #region PUBLIC METHODS
    public void Enter(Client client)
    {
        if (Contains(client)) return;

        lock (_queueLock)
        {
            _clientsQueue.Enqueue(client);
            UpdateQueuePositions();
        }
    }

    public void NextTurn()
    {
        if (!HasAnyClient()) return;

        lock (_queueLock)
        {
            _clientsQueue.Dequeue();
            UpdateQueuePositions();

            if (HasAnyClient())
                NextTurnEvent?.Invoke(_clientsQueue.Peek());
            else
                NextTurnEvent?.Invoke(null);
        }
    }

    public Vector3 FirstInLinePos()
    {
        lock (_queueLock)
        {
            return _queuePositions[0].position;
        }
    }

    public Vector3 LastInLinePos()
    {
        lock (_queueLock)
        {
            return _queuePositions[^1].position;
        }
    }

    public bool Contains(Client clientContext)
    {
        lock (_queueLock)
        {
            return _clientsQueue.Contains(clientContext);
        }
    }

    public void FixRotation(Client clientContext)
    {
        if (clientContext == null) return;

        lock (_queueLock)
        {
            int index = _clientsQueue.ToArray().ToList().IndexOf(clientContext);
            if (index <= 0) return;

            // Look direction towards next position in queue
            Vector3 lookDirection = _queuePositions[index - 1].position - clientContext.transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            clientContext.ForceRotation(rotation);
        }
    }

    /// <summary>
    /// Returns the normalized time that the next client has been waiting in the queue.
    /// Normalized between 0 and the maximum waiting time of the client.
    /// </summary>
    public float GetNextClientNormalizedWaitingTime()
    {
        if (!HasAnyClient()) return 0f;

        lock (_queueLock)
        {
            return _clientsQueue.Peek().normalizedWaitingTime;
        }
    }

    public bool HasAnyClient()
    {
        lock (_queueLock)
        {
            return _clientsQueue.Count > 0;
        }
    }
    #endregion

    #region PRIVATE METHODS
    void UpdateQueuePositions()
    {
        lock (_queueLock)
        {
            if (_clientsQueue.Count == 0) return;

            int index = 0;
            foreach (Client client in _clientsQueue)
            {
                client.SetDestination(_queuePositions[index].position);
                index++;
            }
        }
    }
    #endregion
}