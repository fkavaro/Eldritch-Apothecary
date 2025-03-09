using System;
using System.Collections.Generic;
using UnityEngine;

public class ApothecaryManager : MonoBehaviour
{
    public static ApothecaryManager Instance;
    public Transform cat, complainingPosition, entrancePosition, exitPosition;
    public Transform shopStandsParent,
        queuePositionsParent,
        seatsPositionsParent,
        pickUpPositionsParent,
        sorcererSeat;

    List<Transform> shopStands = new(),
        queuePositions = new(),
        seatsPositions = new(),
        pickUpPositions = new();
    Queue<Client> clientQueue = new();

    public int clientsWaiting;

    void Awake()
    {
        // Creates one instance if there isn't any (Singleton)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        FillChildrenList(shopStandsParent, shopStands);
        FillChildrenList(queuePositionsParent, queuePositions);
        FillChildrenList(seatsPositionsParent, seatsPositions);
        FillChildrenList(pickUpPositionsParent, pickUpPositions);
    }

    void Start()
    {
        if (shopStands.Count == 0 || queuePositions.Count == 0 || seatsPositions.Count == 0)
            Debug.LogError("Shop stands, queue or seats positions are empty.");
    }

    void Update()
    {

    }

    void FillChildrenList(Transform parent, List<Transform> childrenList)
    {
        foreach (Transform child in parent)
            childrenList.Add(child);
    }

    Vector3 RandomPosition(List<Transform> positions)
    {
        return positions[UnityEngine.Random.Range(0, positions.Count)].position;
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

    public void AddToQueue(Client client)
    {
        clientQueue.Enqueue(client);
        UpdateQueuePositions();
    }

    public void LeaveQueue()
    {
        clientQueue.Dequeue();
        UpdateQueuePositions();
    }

    public Vector3 RandomShopStand()
    {
        return RandomPosition(shopStands);
    }

    public Vector3 RandomSeat()
    {
        return RandomPosition(seatsPositions);
    }

    public Vector3 RandomPickUp()
    {
        return RandomPosition(pickUpPositions);
    }

    public Vector3 FirstInLine()
    {
        return queuePositions[0].position;
    }
}