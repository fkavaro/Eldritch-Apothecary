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
    [Tooltip("Simulation speed"), Range(0, 5)]
    public int simSpeed = 1;

    [Header("Positions")]
    [Tooltip("Grumpy cat")]
    public Transform cat;
    [Tooltip("Spot where clients complain")]
    public Transform complainingPosition;
    [Tooltip("Spot where clients enter the apothecary")]
    public Transform entrancePosition;
    [Tooltip("Spot where clients leave the apothecary")]
    public Transform exitPosition;
    [Tooltip("Parent of all shop stands gameobjects")]
    public Transform shopStandsParent;
    [Tooltip("Parent of all queue positions gameobjects")]
    public Transform queuePositionsParent;
    [Tooltip("Parent of all waiting seats gameobjects")]
    public Transform seatsPositionsParent;
    [Tooltip("Parent of all potion pick-up positions gameobjects")]
    public Transform pickUpPositionsParent;
    [Tooltip("Parent of all instantiated clients")]
    public Transform clientsParent;
    [Tooltip("Spot where clients sit while attended by the sorcerer")]
    public Transform sorcererSeat;

    [Header("Clients pool")]
    [Tooltip("All clients models to be spawned randomly")]
    public GameObject[] clientPrefabs;
    [Tooltip("Maximum number of clients in the apothecary at once")]
    public int maxClients = 10;
    [Tooltip("Maximum number of clients in the apothecary at once"), Range(5, 20)]
    public float spawnTimer = 5f;
    public ObjectPool<Client> clientsPool;

    [Header("Clients waiting queue")]
    public WaitingQueue waitingQueue;
    #endregion

    #region PRIVATE PROPERTIES
    List<Transform> shopStands = new(),
        queuePositions = new(),
        seatsPositions = new(),
        pickUpPositions = new();
    float nextClientTime = 0f;
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
            createFunc: CreateClient,
            actionOnGet: GetClient,
            actionOnRelease: ReleaseClient,
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
        if (Time.timeScale != simSpeed)
            Time.timeScale = simSpeed;

        // Clients keep coming if there's room for them
        if (Time.time >= nextClientTime && clientsPool.CountActive < maxClients)
        {
            nextClientTime = Time.time + spawnTimer; // Reset timer
            clientsPool.Get();
        }
    }
    #endregion

    #region PUBLIC METHODS
    public Vector3 RandomShopStand()
    {
        return RandomPosition(shopStands);
    }

    public Vector3 RandomWaitingSeat()
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

    /// <summary>
    /// Creation method for clients pool: instantiates a random client prefab.
    /// </summary>
    /// <returns>Instantiated client.</returns>
    Client CreateClient()
    {
        Client client = Instantiate(
            clientPrefabs[UnityEngine.Random.Range(0, clientPrefabs.Length)],
            entrancePosition.position,
            Quaternion.identity,
            clientsParent)
        .GetComponent<Client>();
        return client;
    }

    /// <summary>
    /// Get method for clients pool: resets client position and behaviour.
    /// </summary>>
    void GetClient(Client client)
    {
        client.transform.position = entrancePosition.position;
        client.Reset();
        client.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ReleaseClient(Client client)
    {
        client.Reset();
        client.gameObject.SetActive(true);
    }
    #endregion
}