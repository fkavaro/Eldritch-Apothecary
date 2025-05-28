using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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

    [HideInInspector]
    public List<Shelf> shopShelves = new(),
            alchemistShelves = new(),
            sorcererShelves = new(),
            shopSuppliesShelves = new(),
            staffSuppliesShelves = new();

    [HideInInspector]
    public Transform complainingPosition,
        receptionistCalmDownPosition,
        queueExitPosition,
        exitPosition,
        entrancePosition;

    [HideInInspector]
    public Spot clientSeat,
        replenisherSeat,
        receptionistAttendingSpot,
        sorcererAttendingSpot;

    [HideInInspector] public Cat cat;
    [HideInInspector] public Alchemist alchemist;
    [HideInInspector] public Sorcerer sorcerer;
    [HideInInspector] public Replenisher replenisher;
    [HideInInspector] public Receptionist receptionist;

    [Header("Simulation")]
    [Tooltip("Simulation speed"), Range(0, 10)]
    public float simSpeed = 1;


    [Header("Positions parents")]
    [Tooltip("Parent of all queue positions gameobjects")]
    public Transform queuePositionsParent;


    [Header("Clients pool")]
    [Tooltip("All clients models to be spawned randomly")]
    public GameObject[] clientPrefabs;
    [Tooltip("Maximum number of clients in the apothecary at once")]
    public int maxClients = 10;
    [Tooltip("Time passed between clients spawning"), Range(5, 20)]
    public float spawnTimer = 5f;


    [Header("Shop supplies")]
    [Tooltip("Normalized between 0 and the maximum store capacity of shop"), Range(0f, 1f)]
    public float normalisedShopLack;
    public int shopLack;
    public int totalShopCapacity;


    [Header("Alchemist supplies")]
    [Tooltip("Normalized between 0 and the maximum store capacity of alchemist"), Range(0f, 1f)]
    public float normalisedAlchemistLack;
    public int alchemistLack;
    public int totalAlchemistCapacity;


    [Header("Sorcerer supplies")]
    [Tooltip("Normalized between 0 and the maximum store capacity of sorcerer"), Range(0f, 1f)]
    public float normalisedSorcererLack;
    public int sorcererLack;
    public int totalSorcererCapacity;

    [Header("Prepared potions")]
    [Tooltip("Normalized between 0 and the maximum capacity of prepared potions"), Range(0f, 1f)]
    public float normalisedPreparedPotions;
    public int totalPreparedPotions;
    public int preparedPotionsCapacity;
    #endregion

    #region PRIVATE PROPERTIES
    Transform _clientsParent;
    List<Transform> _queuePositions = new(),
        _potionServePositions = new(),
        _potionsPickUpPositions = new();
    List<Spot> _waitingSeats = new();
    float _lastSpawnTime = 0f;
    List<Client> _clientsComplaining = new();
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();// Ensures the Singleton logic runs

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

        //Staff
        receptionist = GameObject.FindGameObjectWithTag("Receptionist").GetComponent<Receptionist>();
        alchemist = GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Alchemist>();
        sorcerer = GameObject.FindGameObjectWithTag("Sorcerer").GetComponent<Sorcerer>();
        replenisher = GameObject.FindGameObjectWithTag("Replenisher").GetComponent<Replenisher>();
        cat = GameObject.FindGameObjectWithTag("Cat").GetComponent<Cat>();
        _clientsParent = GameObject.FindGameObjectWithTag("Clients parent").GetComponent<Transform>();

        //Spots
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop shelf"), shopShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Alchemist shelf"), alchemistShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Sorcerer shelf"), sorcererShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop supply shelf"), shopSuppliesShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Staff supply shelf"), staffSuppliesShelves);
        FillSpotList(GameObject.FindGameObjectsWithTag("Waiting seat"), _waitingSeats);
        clientSeat = GameObject.FindGameObjectWithTag("Client seat").GetComponent<Spot>();
        replenisherSeat = GameObject.FindGameObjectWithTag("Replenisher seat").GetComponent<Spot>();
        receptionistAttendingSpot = GameObject.FindGameObjectWithTag("Receptionist attending spot").GetComponent<Spot>();
        sorcererAttendingSpot = GameObject.FindGameObjectWithTag("Sorcerer attending spot").GetComponent<Spot>();

        //Positions
        entrancePosition = GameObject.FindGameObjectWithTag("Entrance").transform;
        exitPosition = GameObject.FindGameObjectWithTag("Exit").transform;
        complainingPosition = GameObject.FindGameObjectWithTag("Complain position").transform;
        receptionistCalmDownPosition = GameObject.FindGameObjectWithTag("Calm down position").transform;
        queueExitPosition = GameObject.FindGameObjectWithTag("Queue exit").transform;
        FillTranformList(GameObject.FindGameObjectsWithTag("Potion pick-up"), _potionsPickUpPositions);
        FillTranformList(GameObject.FindGameObjectsWithTag("Potion serve"), _potionServePositions);
    }

    void Start()
    {
        if (shopShelves.Count == 0 ||
            alchemistShelves.Count == 0 ||
            _queuePositions.Count == 0 ||
            sorcererShelves.Count == 0 ||
            shopSuppliesShelves.Count == 0 ||
            staffSuppliesShelves.Count == 0 ||
            _waitingSeats.Count == 0 ||
            _potionServePositions.Count == 0 ||
            _potionsPickUpPositions.Count == 0)
            Debug.LogError("A positions list is empty.");

        totalShopCapacity = CalculateTotalCapacity(shopShelves);
        totalAlchemistCapacity = CalculateTotalCapacity(alchemistShelves);
        totalSorcererCapacity = CalculateTotalCapacity(sorcererShelves);

        if (totalShopCapacity == 0
            || totalAlchemistCapacity == 0
            || totalSorcererCapacity == 0)
            Debug.LogWarning("A capacity is 0");
    }

    void Update()
    {
        // Update time scale
        if (Time.timeScale != simSpeed)
            Time.timeScale = simSpeed;

        // Clients keep coming if there's room for them
        if (Time.time >= _lastSpawnTime && clientsPool.CountActive < maxClients)
        {
            _lastSpawnTime = Time.time + spawnTimer; // Reset timer
            clientsPool.Get();
        }

        // Update lacks
        shopLack = CalculateTotalLack(shopShelves);
        alchemistLack = CalculateTotalLack(alchemistShelves);
        sorcererLack = CalculateTotalLack(sorcererShelves);

        // Update normalised values of supplies
        normalisedShopLack = Mathf.Clamp01((float)shopLack / totalShopCapacity);
        normalisedAlchemistLack = Mathf.Clamp01(alchemistLack / totalAlchemistCapacity);
        normalisedSorcererLack = Mathf.Clamp01(sorcererLack / totalSorcererCapacity);
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

    public Shelf RandomShopShelf()
    {
        return RandomShelf(shopShelves);
    }

    public Shelf RandomSorcererShelf()
    {
        return RandomShelf(sorcererShelves);
    }

    public Shelf RandomShelf(List<Shelf> shelves)
    {
        Shelf randomShelf = null;

        if (shelves.Count == 0 || shelves == null)
        {
            Debug.LogError("Shelves list is empty or null");
        }
        else
        {
            randomShelf = shelves[UnityEngine.Random.Range(0, shelves.Count)];

            if (randomShelf == null)
                Debug.LogError("Random shelf is null");
        }

        return randomShelf;
    }

    /// <summary>
    /// Adds client to complaining list if it's not already
    /// </summary>
    public void AddToComplains(Client client)
    {
        if (!_clientsComplaining.Contains(client))
            _clientsComplaining.Add(client);
    }

    /// <summary>
    /// Removes client from complaining list if it's already
    /// </summary>
    public void RemoveFromComplains(Client client)
    {
        if (_clientsComplaining.Contains(client))
            _clientsComplaining.Remove(client);
    }

    /// <returns>True if list contains any client</returns>
    public bool IsSomeoneComplaining()
    {
        return _clientsComplaining.Count > 0;
    }

    /// <returns>Firs client in complaining list</returns>
    public Client CurrentComplainingClient()
    {
        if (IsSomeoneComplaining())
            return _clientsComplaining[0];
        else
            return null;
    }

    public float GetNormalisedLack(List<Shelf> lackingShelves)
    {
        if (lackingShelves == shopShelves)
            return normalisedShopLack;
        else if (lackingShelves == alchemistShelves)
            return normalisedAlchemistLack;
        else if (lackingShelves == sorcererShelves)
            return normalisedSorcererLack;
        else
            return 0f;
    }

    public int GetTotalLack(List<Shelf> lackingShelves)
    {
        if (lackingShelves == shopShelves)
            return shopLack;
        else if (lackingShelves == alchemistShelves)
            return alchemistLack;
        else if (lackingShelves == sorcererShelves)
            return sorcererLack;
        else
            return 0;
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


    public bool ArePotionsReady()
    {
        return false; // TODO
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
        client.transform.position = entrancePosition.position;
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

    private int CalculateTotalCapacity(List<Shelf> shelfList)
    {
        int totalCapacity = 0;

        // Find all different shelves objetcs
        List<Shelves> uniqueShelves = new();

        // Each shelf
        foreach (Shelf shelf in shelfList)
            // If its shelves is new
            if (!uniqueShelves.Contains(shelf.shelves))
            {
                uniqueShelves.Add(shelf.shelves);
                // Add its capacity
                totalCapacity += shelf.shelves.capacity;
            }

        return totalCapacity;
    }

    private int CalculateTotalLack(List<Shelf> shelfList)
    {
        int totalLack = 0;

        // Find all different shelves objetcs
        List<Shelves> uniqueShelves = new();

        // Each shelf
        foreach (Shelf shelf in shelfList)
            // If its shelves is new
            if (!uniqueShelves.Contains(shelf.shelves))
            {
                uniqueShelves.Add(shelf.shelves);
                // Add its capacity
                totalLack += shelf.shelves.lackingAmount;
            }

        return totalLack;
    }
    #endregion
}