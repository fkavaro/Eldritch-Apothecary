using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Manages the Apothecary information and systems
/// </summary>
public class ApothecaryManager : Singleton<ApothecaryManager>
{
    #region VARIABLES
    [Header("Positions")]

    public Transform cat;
    public Transform complainingPosition;
    public Transform entrancePosition;
    public Transform exitPosition;
    public Transform shopStandsParent;
    public Transform queuePositionsParent;
    public Transform seatsPositionsParent;
    public Transform pickUpPositionsParent;
    public Transform clientsParent;
    public Transform sorcererSeat;

    List<Transform> shopStands = new(),
        queuePositions = new(),
        seatsPositions = new(),
        pickUpPositions = new();

    [Header("Clients pool")]
    public GameObject clientPrefab;
    public int maxClients = 10;
    public ObjectPool<Client> clientsPool;

    [Header("Clients queue")]
    public WaitingQueue waitingQueue;
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();// Ensures the Singleton logic runs

        FillChildrenList(shopStandsParent, shopStands);
        FillChildrenList(queuePositionsParent, queuePositions);
        FillChildrenList(seatsPositionsParent, seatsPositions);
        FillChildrenList(pickUpPositionsParent, pickUpPositions);

        waitingQueue = new WaitingQueue(queuePositions);
        clientsPool = new ObjectPool<Client>(
            createFunc: () => Instantiate(clientPrefab, entrancePosition.position, Quaternion.identity).GetComponent<Client>(),
            actionOnGet: (client) => client.gameObject.SetActive(true),
            actionOnRelease: (client) => client.gameObject.SetActive(false),
            actionOnDestroy: (client) => Destroy(client.gameObject),
            maxSize: maxClients
        );
    }

    void Start()
    {
        if (shopStands.Count == 0 ||
            queuePositions.Count == 0 ||
            seatsPositions.Count == 0 ||
            pickUpPositions.Count == 0)
            Debug.LogError("A positions list is empty.");
    }

    void Update()
    {
        // Instantiate clients every 20 seconds if there are less than 10 clients
        if (Time.time % 20 == 0 && clientsPool.CountActive < maxClients)
        {
            Client client = clientsPool.Get();
            client.transform.position = entrancePosition.position;
            client.gameObject.SetActive(true);
        }
    }
    #endregion

    #region PUBLIC METHODS
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
    #endregion

    #region PRIVATE METHODS
    void FillChildrenList(Transform parent, List<Transform> childrenList)
    {
        foreach (Transform child in parent)
            childrenList.Add(child);
    }

    Vector3 RandomPosition(List<Transform> positions)
    {
        return positions[UnityEngine.Random.Range(0, positions.Count)].position;
    }
    #endregion
}