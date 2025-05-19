using System;
using System.Collections.Generic;
using UnityEditor;
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

    [HideInInspector] public GameObject cat;
    [HideInInspector] public GameObject alchemist;
    [HideInInspector] public GameObject sorcerer;
    [HideInInspector] public GameObject replenisher;
    [HideInInspector] public Receptionist receptionist;
    [HideInInspector] public Spot clientSeat;
    [HideInInspector] public Transform complainingPosition;
    [HideInInspector] public Transform queueExitPosition;
    [HideInInspector] public Spot receptionistAttendingPos;

    [Header("Simulation")]
    [Tooltip("Simulation speed"), Range(0, 5)]
    public float simSpeed = 1;

    [Header("Positions parents")]
    [Tooltip("Parent of all queue positions gameobjects")]
    public Transform queuePositionsParent;

    [Header("Clients pool")]
    [HideInInspector]
    public Transform exitPosition;
    [Tooltip("All clients models to be spawned randomly")]
    public GameObject[] clientPrefabs;
    [Tooltip("Maximum number of clients in the apothecary at once")]
    public int maxClients = 10;
    [Tooltip("Time passed between clients spawning"), Range(5, 20)]
    public float spawnTimer = 5f;
    #endregion

    #region PRIVATE PROPERTIES
    Transform _clientsParent,
        _entrancePosition;

    List<Transform> _queuePositions = new(),
        _potionServePositions = new(),
        _potionsPickUpPositions = new();

    List<Spot> _shopStands = new(),
        _replenisherStands = new(),
        _alchemistStands = new(),
        _sorcererStands = new(),
        _waitingSeats = new();

    float _lastSpawnTime = 0f;
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();// Ensures the Singleton logic runs

        //Staff
        receptionist = GameObject.FindGameObjectsWithTag("Receptionist")[0].GetComponent<Receptionist>();
        alchemist = GameObject.FindGameObjectsWithTag("Alchemist")[0];
        sorcerer = GameObject.FindGameObjectsWithTag("Sorcerer")[0];
        replenisher = GameObject.FindGameObjectsWithTag("Replenisher")[0];
        cat = GameObject.FindGameObjectsWithTag("Cat")[0];
        _clientsParent = GameObject.FindGameObjectsWithTag("Clients parent")[0].GetComponent<Transform>();

        //Spots
        FillSpotList(GameObject.FindGameObjectsWithTag("Shop stand"), _shopStands);
        FillSpotList(GameObject.FindGameObjectsWithTag("Alchemist stand"), _alchemistStands);
        FillSpotList(GameObject.FindGameObjectsWithTag("Sorcerer stand"), _sorcererStands);
        FillSpotList(GameObject.FindGameObjectsWithTag("Storing stand"), _replenisherStands);
        FillSpotList(GameObject.FindGameObjectsWithTag("Waiting seat"), _waitingSeats);

        //Positions
        _entrancePosition = GameObject.FindGameObjectsWithTag("Entrance")[0].GetComponent<Transform>();
        exitPosition = GameObject.FindGameObjectsWithTag("Exit")[0].GetComponent<Transform>();
        clientSeat = GameObject.FindGameObjectsWithTag("Client seat")[0].GetComponent<Spot>();
        complainingPosition = GameObject.FindGameObjectsWithTag("Complain position")[0].GetComponent<Transform>();
        queueExitPosition = GameObject.FindGameObjectsWithTag("Queue exit")[0].GetComponent<Transform>();
        FillTranformList(GameObject.FindGameObjectsWithTag("Potion pick-up"), _potionsPickUpPositions);
        FillTranformList(GameObject.FindGameObjectsWithTag("Potion serve"), _potionServePositions);

        // Fill waiting queue positions in child order
        FillTransformChildrenList(queuePositionsParent, _queuePositions);

        // Waiting queue creation
        waitingQueue = new WaitingQueue(_queuePositions);

        // Clients pool creation
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
            _waitingSeats.Count == 0 ||
            _potionsPickUpPositions.Count == 0)
            Debug.LogError("A positions list is empty.");
    }

    void Update()
    {
        if (Time.timeScale != simSpeed)
            Time.timeScale = simSpeed;

        // Clients keep coming if there's room for them
        if (Time.time >= _lastSpawnTime && clientsPool.CountActive < maxClients)
        {
            _lastSpawnTime = Time.time + spawnTimer; // Reset timer
            clientsPool.Get();
        }
    }
    #endregion

    #region PUBLIC METHODS
    public Spot RandomWaitingSeat()
    {
        return RandomSpot(_waitingSeats);
    }

    public Vector3 RandomPickUp()
    {
        return RandomPosition(_potionsPickUpPositions);
    }

    public Spot RandomShopShelves()
    {
        return RandomSpot(_shopStands);
    }

    public bool SomeoneComplaining()
    {
        return false; // TODO
    }

    public float GetNormalizedPreparedPotionsNumber()
    {
        return 0f; // TODO
    }

    // TODO: IMPLEMENT TURNS SYSTEM
    internal bool IsTurn(Client client)
    {
        // Switch wanted service
        switch (client.wantedService)
        {
            case Client.WantedService.Sorcerer:
                return !clientSeat.IsOccupied();
            case Client.WantedService.Alchemist:
                return true;
            default:
                return true;
        }
    }
    #endregion

    #region PRIVATE METHODS
    void FillTransformChildrenList(Transform parent, List<Transform> childrenList)
    {
        foreach (Transform child in parent)
            childrenList.Add(child);
    }

    void FillSpotChildrenList(Transform parent, List<Spot> childrenList)
    {
        foreach (Transform child in parent)
            childrenList.Add(child.GetComponent<Spot>());
    }

    void FillSpotList(GameObject[] gameObjects, List<Spot> spots)
    {
        foreach (GameObject gameobject in gameObjects)
            spots.Add(gameobject.GetComponent<Spot>());
    }

    private void FillTranformList(GameObject[] gameObjects, List<Transform> transforms)
    {
        foreach (GameObject gameobject in gameObjects)
            transforms.Add(gameobject.transform);
    }

    Spot RandomSpot(List<Spot> spots)
    {
        return spots[UnityEngine.Random.Range(0, spots.Count)];
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
            _entrancePosition.position,
            Quaternion.identity,
            _clientsParent)
        .GetComponent<Client>();
        return client;
    }

    /// <summary>
    /// Get method for clients pool: resets client position and behaviour.
    /// </summary>>
    void GetClient(Client client)
    {
        client.transform.position = _entrancePosition.position;
        client.gameObject.SetActive(true);
        client.Reset();
    }

    /// <summary>
    /// Release method for clients pool: deactivates client gameobject.
    /// </summary>
    void ReleaseClient(Client client)
    {
        client.gameObject.SetActive(false);
    }
    #endregion
}