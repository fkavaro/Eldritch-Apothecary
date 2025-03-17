using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Manages the Apothecary information and systems
/// </summary>
public class ApothecaryManager : Singleton<ApothecaryManager>
{
    #region PUBLIC PROPERTIES
    public ObjectPool<Client> clientsPool;
    public WaitingQueue waitingQueue;
    public Shop shop;

    [Header("Simulation")]
    [Tooltip("Simulation speed"), Range(0, 5)]
    public int simSpeed = 1;

    [Header("Staff members")]
    public GameObject cat;
    [Tooltip("Receptionist position")]
    public GameObject receptionist;
    [Tooltip("Spot where clients complain")]

    [Header("Positions")]
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
    #endregion

    #region PRIVATE PROPERTIES
    List<Transform> _shopStands = new(),
        _queuePositions = new(),
        _seatsPositions = new(),
        _pickUpPositions = new();
    float _nextClientTime = 0f;
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();// Ensures the Singleton logic runs

        FillChildrenList(shopStandsParent, _shopStands);
        FillChildrenList(queuePositionsParent, _queuePositions);
        FillChildrenList(seatsPositionsParent, _seatsPositions);
        FillChildrenList(pickUpPositionsParent, _pickUpPositions);

        waitingQueue = new WaitingQueue(_queuePositions);
        shop = new Shop(_shopStands);
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
        if (_shopStands.Count == 0 ||
            _queuePositions.Count == 0 ||
            _seatsPositions.Count == 0 ||
            _pickUpPositions.Count == 0)
            Debug.LogError("A positions list is empty.");
    }

    void Update()
    {
        if (Time.timeScale != simSpeed)
            Time.timeScale = simSpeed;

        // Clients keep coming if there's room for them
        if (Time.time >= _nextClientTime && clientsPool.CountActive < maxClients)
        {
            _nextClientTime = Time.time + spawnTimer; // Reset timer
            clientsPool.Get();
        }
    }
    #endregion

    #region PUBLIC METHODS
    public Vector3 RandomWaitingSeat()
    {
        return RandomPosition(_seatsPositions);
    }

    public Vector3 RandomPickUp()
    {
        return RandomPosition(_pickUpPositions);
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
        client.gameObject.SetActive(true);
        client.Reset();
    }

    /// <summary>
    /// Release method for clients pool: deactivates client gameobject.
    /// </summary>
    void ReleaseClient(Client client)
    {
        //client.Reset();
        client.gameObject.SetActive(false);
        //client.transform.position = entrancePosition.position;
    }
    #endregion
}