using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    [HideInInspector]
    public Spot clientSeat,
        receptionistCalmDownSpot,
        receptionistAttendingPos;
    [HideInInspector]
    public Transform complainingPosition,
        queueExitPosition;

    [Header("Simulation")]
    [Tooltip("Simulation speed"), Range(0, 10)]
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

    List<Shelf> _shopShelves = new(),
        _shopSuppliesShelves = new(),
        _staffSuppliesShelves = new(),
        _alchemistShelves = new(),
        _sorcererShelves = new();

    List<Spot> _waitingSeats = new();

    float _lastSpawnTime = 0f;
    List<Client> _clientsComplaining = new();
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
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop shelf"), _shopShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Alchemist shelf"), _alchemistShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Sorcerer shelf"), _sorcererShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop supply shelf"), _shopSuppliesShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Staff supply shelf"), _staffSuppliesShelves);
        FillSpotList(GameObject.FindGameObjectsWithTag("Waiting seat"), _waitingSeats);

        //Positions
        _entrancePosition = GameObject.FindGameObjectsWithTag("Entrance")[0].GetComponent<Transform>();
        exitPosition = GameObject.FindGameObjectsWithTag("Exit")[0].GetComponent<Transform>();
        clientSeat = GameObject.FindGameObjectsWithTag("Client seat")[0].GetComponent<Spot>();
        receptionistAttendingPos = GameObject.FindGameObjectsWithTag("Attending position")[0].GetComponent<Spot>();
        receptionistCalmDownSpot = GameObject.FindGameObjectsWithTag("Calm down position")[0].GetComponent<Spot>();
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
        if (_shopShelves.Count == 0 ||
            _alchemistShelves.Count == 0 ||
            _queuePositions.Count == 0 ||
            _sorcererShelves.Count == 0 ||
            _shopSuppliesShelves.Count == 0 ||
            _staffSuppliesShelves.Count == 0 ||
            _waitingSeats.Count == 0 ||
            _potionServePositions.Count == 0 ||
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

    public Shelf RandomShopShelves()
    {
        return RandomShelf(_shopShelves);
    }

    public void WantsToComplain(Client client)
    {
        if (!_clientsComplaining.Contains(client))
            _clientsComplaining.Add(client);
    }

    public void StopsComplaining(Client client)
    {
        if (_clientsComplaining.Contains(client))
            _clientsComplaining.Remove(client);
    }

    public bool IsSomeoneComplaining()
    {
        return _clientsComplaining.Count > 0;
    }


    public Client CurrentComplainingClient()
    {
        if (IsSomeoneComplaining())
            return _clientsComplaining[0];
        else
            return null;
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

    void FillShelfList(GameObject[] gameObjects, List<Shelf> shelfs)
    {
        foreach (GameObject gameobject in gameObjects)
            shelfs.Add(gameobject.GetComponent<Shelf>());
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

    Shelf RandomShelf(List<Shelf> shelves)
    {
        return shelves[UnityEngine.Random.Range(0, shelves.Count)];
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
        // TODO: change model
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