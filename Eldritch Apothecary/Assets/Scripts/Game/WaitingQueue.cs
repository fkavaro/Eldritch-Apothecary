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
        //Debug.Log($"Client {client.name} entered the queue.");

        lock (_queueLock)
        {
            if (_clientsQueue.Contains(client)) return;

            _clientsQueue.Enqueue(client);
            UpdateQueuePositions();
        }
    }

    public void NextTurn()
    {
        lock (_queueLock)
        {
            if (_clientsQueue.Count > 0)
            {
                _clientsQueue.Dequeue();
                UpdateQueuePositions();
                NextTurnEvent?.Invoke(_clientsQueue.Peek());
            }
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

    // TODO: FIX METHOD, bug: looks always forward
    public void FixRotation(Client clientContext)
    {
        lock (_queueLock)
        {
            int index = _clientsQueue.ToArray().ToList().IndexOf(clientContext);
            if (index <= 0) return;

            // Rotate to next position
            Vector3 lookDirection = (_queuePositions[index - 1].position - clientContext.transform.position).normalized;
            clientContext.transform.rotation = Quaternion.Euler(lookDirection);
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
                client.SetTargetPos(_queuePositions[index].position);
                index++;
            }
        }
    }
    #endregion
}
