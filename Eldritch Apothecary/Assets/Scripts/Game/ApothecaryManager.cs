using System.Collections.Generic;
using UnityEngine;

public class ApothecaryManager : MonoBehaviour
{
    public static ApothecaryManager Instance;
    public Transform[] shopStands, queuePositions;
    public Queue<int> clientsIdWaiting = new();
    public Transform nextClientPosition;

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

    public void EnqueueClient(int clientId)
    {
        clientsIdWaiting.Enqueue(clientId);
        UpdateNextPosition();
    }

    public void DequeueClient()
    {
        if (clientsIdWaiting.Count > 0)
        {
            clientsIdWaiting.Dequeue();
            UpdateNextPosition();
        }
    }

    private void UpdateNextPosition()
    {
        nextClientPosition = queuePositions[clientsIdWaiting.Count - 1];
    }
}