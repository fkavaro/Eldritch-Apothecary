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
        entrancePosition,
        queuePositionsParent,
        dump;

    [HideInInspector]
    public Spot clientSeat,
        replenisherSeat,
        sorcererSeat,
        receptionistAttendingSpot,
        sorcererAttendingSpot;



    [Header("Simulation")]
    [Tooltip("Simulation speed"), Range(0, 10)]
    public float simSpeed = 1;

    [Header("Turns system")]
    public int generatedSorcererTurns = 0;
    public int currentSorcererTurn = 0;
    public List<Client> sorcererClientsQueue = new();
    public int generatedAlchemistTurns = 0;
    public int currentAlchemistTurn = 0;

    [Header("Staff")]
    public GameObject catPrefab;
    [HideInInspector] public Cat cat;
    [HideInInspector] public Alchemist alchemist;
    [HideInInspector] public Sorcerer sorcerer;
    [HideInInspector] public Replenisher replenisher;
    [HideInInspector] public Receptionist receptionist;

    [Header("Clients pool")]
    [Tooltip("All clients models to be spawned randomly")]
    public GameObject[] clientPrefabs;
    [Tooltip("Maximum number of clients in the apothecary at once")]
    public int maxClients = 10;
    public int minSecondsForNewClient = 2,
        maxSecondsForNewClient = 15;


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
    public int totalPotionsToBeServed;
    public int preparedPotionsCapacity;
    #endregion

    #region PRIVATE PROPERTIES
    Transform _clientsParent;
    List<Transform> _queuePositions = new(),
        _catSpawningPositions = new();
    List<Spot> _waitingSeats = new();
    List<Potion> _preparedPotions = new(),
        _readyPotions = new(),
        _leftPotions = new();
    float _lastSpawnTime = 0f;
    List<Client> _clientsComplaining = new();
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();// Ensures the Singleton logic runs

        FillTranformList(GameObject.FindGameObjectsWithTag("Cat spawn"), _catSpawningPositions);

        queuePositionsParent = GameObject.FindGameObjectWithTag("Queue positions").transform;
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
        _clientsParent = GameObject.FindGameObjectWithTag("Clients parent").GetComponent<Transform>();

        // Spawn cat in random position
        cat = Instantiate(
            catPrefab,
            RandomPosition(_catSpawningPositions),
            Quaternion.identity)
        .GetComponent<Cat>();

        //Spots
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop shelf"), shopShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Alchemist shelf"), alchemistShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Sorcerer shelf"), sorcererShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Shop supply shelf"), shopSuppliesShelves);
        FillShelfList(GameObject.FindGameObjectsWithTag("Staff supply shelf"), staffSuppliesShelves);
        FillSpotList(GameObject.FindGameObjectsWithTag("Waiting seat"), _waitingSeats);
        FillPotionList(GameObject.FindGameObjectsWithTag("Ready potion"), _readyPotions);
        FillPotionList(GameObject.FindGameObjectsWithTag("Prepared potion"), _preparedPotions);
        clientSeat = GameObject.FindGameObjectWithTag("Client seat").GetComponent<Spot>();
        replenisherSeat = GameObject.FindGameObjectWithTag("Replenisher seat").GetComponent<Spot>();
        sorcererSeat = GameObject.FindGameObjectWithTag("Sorcerer seat").GetComponent<Spot>();
        receptionistAttendingSpot = GameObject.FindGameObjectWithTag("Receptionist attending spot").GetComponent<Spot>();
        sorcererAttendingSpot = GameObject.FindGameObjectWithTag("Sorcerer attending spot").GetComponent<Spot>();

        //Positions
        entrancePosition = GameObject.FindGameObjectWithTag("Entrance").transform;
        exitPosition = GameObject.FindGameObjectWithTag("Exit").transform;
        complainingPosition = GameObject.FindGameObjectWithTag("Complain position").transform;
        receptionistCalmDownPosition = GameObject.FindGameObjectWithTag("Calm down position").transform;
        queueExitPosition = GameObject.FindGameObjectWithTag("Queue exit").transform;
        dump = GameObject.FindGameObjectWithTag("Dump").transform;

        // Setting how far in the future agents predict collisions for avoidance
        //UnityEngine.AI.NavMesh.avoidancePredictionTime = avoidancePredictionTime;
    }

    void Start()
    {
        totalShopCapacity = CalculateTotalCapacity(shopShelves);
        totalAlchemistCapacity = CalculateTotalCapacity(alchemistShelves);
        totalSorcererCapacity = CalculateTotalCapacity(sorcererShelves);
        preparedPotionsCapacity = _preparedPotions.Count;

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
            _lastSpawnTime = Time.time + UnityEngine.Random.Range(minSecondsForNewClient, maxSecondsForNewClient);
            clientsPool.Get();
        }

        // Update lacks
        shopLack = CalculateTotalLack(shopShelves);
        alchemistLack = CalculateTotalLack(alchemistShelves);
        sorcererLack = CalculateTotalLack(sorcererShelves);

        // Update normalised values of supplies
        normalisedShopLack = Mathf.Clamp01((float)shopLack / totalShopCapacity);
        normalisedAlchemistLack = Mathf.Clamp01((float)alchemistLack / totalAlchemistCapacity);
        normalisedSorcererLack = Mathf.Clamp01((float)sorcererLack / totalSorcererCapacity);

        // Update normalised value of prepared potions
        totalPotionsToBeServed = PotionsToBeServed();
        normalisedPreparedPotions = Mathf.Clamp01((float)totalPotionsToBeServed / preparedPotionsCapacity);
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Check if any ready potion has clients's turn number
    /// </summary>
    /// <returns>Corresponding potion</returns>
    public Potion AssignedPotion(Client client)
    {
        Potion clientPotion = null;

        foreach (Potion potion in _readyPotions)
            if (potion.ThisNumber(client.turnNumber))
                clientPotion = potion;

        return clientPotion;
    }

    /// <summary>
    /// Assigns a current turn to unassigned prepared potion
    /// </summary>
    public Potion AssignTurnToRandomPotion()
    {
        Potion randomPotion = RandomPreparedPotion(false);
        randomPotion.Assign(++currentAlchemistTurn);
        return randomPotion;
    }

    public Potion RandomPreparedPotion(bool isAssigned)
    {
        return RandomPotion(_preparedPotions, isAssigned);
    }

    public Potion RandomReadyPotion()
    {
        return RandomPotion(_readyPotions, false);
    }

    public Spot RandomWaitingSeat()
    {
        return RandomSpot(_waitingSeats, false);
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
        return shelves[UnityEngine.Random.Range(0, shelves.Count)];
    }

    /// <summary>
    /// Check if any prepared potion has any turn number
    /// </summary>
    /// <returns> Number of potions ready to be served</returns>
    public int PotionsToBeServed()
    {
        int number = 0;

        foreach (Potion potion in _preparedPotions)
            if (potion.IsAssigned())
                number++;

        return number;
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

    /// <summary>
    /// Assigns a turn number to client according to wanted service
    /// </summary>
    public void TakeTurn(Client client)
    {
        switch (client.wantedService)
        {
            case Client.WantedService.SPELL:
                client.turnNumber = ++generatedSorcererTurns;
                client.turnText.text = client.turnNumber.ToString();
                break;
            case Client.WantedService.POTION:
                client.turnNumber = ++generatedAlchemistTurns;
                client.turnText.text = client.turnNumber.ToString();
                AssignTurnToRandomPotion(); // TODO: should be called after potion is prepared by alchemist
                break;
            default: // SHOPPING
                break; // Nothing
        }
    }

    /// <returns>True if it's this client turn, according to wanted service</returns>
    internal bool IsTurn(Client client)
    {
        return client.wantedService switch
        {
            // If sorcerer is waiting client's turn
            Client.WantedService.SPELL => sorcerer.sfsm.IsCurrentState(sorcerer.waitForClientState)
                                        && client.turnNumber == currentSorcererTurn,
            // If a potion is ready for client's turn
            Client.WantedService.POTION => AssignedPotion(client),
            _ => true,
        };
    }

    public void NextSorcererTurn()
    {
        ++currentSorcererTurn;
    }


    public bool IsCurrentSorcererTurn(Client client)
    {
        return client.turnNumber == currentSorcererTurn;
    }

    public Potion ALeftPotion()
    {
        Potion leftPotion = null;

        if (_leftPotions.Count > 0)
            leftPotion = _leftPotions[0];

        return leftPotion;
    }

    public bool IsPotionLeft(Client client)
    {
        Potion clientsPotion = AssignedPotion(client);

        if (clientsPotion != null)
        {
            _leftPotions.Add(clientsPotion);
            return true;
        }
        else
            return false;
    }

    public void DumpPotion(Potion potion)
    {
        _leftPotions.Remove(potion);
    }
    #endregion

    #region PRIVATE METHODS
    void FillTransformChildrenList(Transform parent, List<Transform> childrenList)
    {
        foreach (Transform child in parent)
            childrenList.Add(child);
    }

    void FillSpotList(GameObject[] gameObjects, List<Spot> spots)
    {
        foreach (GameObject gameobject in gameObjects)
            spots.Add(gameobject.GetComponent<Spot>());
    }

    void FillPotionList(GameObject[] gameObjects, List<Potion> potions)
    {
        foreach (GameObject gameobject in gameObjects)
            potions.Add(gameobject.GetComponent<Potion>());
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

    Spot RandomSpot(List<Spot> spots, bool isOccupied = true)
    {
        Spot randomSpot = spots[UnityEngine.Random.Range(0, spots.Count)];

        if (isOccupied) // Random spot must be occupied
            while (!randomSpot.IsOccupied())
                randomSpot = spots[UnityEngine.Random.Range(0, spots.Count)];
        else // Random spot musn't be occupied
            while (randomSpot.IsOccupied())
                randomSpot = spots[UnityEngine.Random.Range(0, spots.Count)];

        return randomSpot;
    }

    Potion RandomPotion(List<Potion> potions, bool isAssigned = true)
    {
        Potion randomPotion = potions[UnityEngine.Random.Range(0, potions.Count)];

        if (isAssigned) // Random potion must be assigned
            while (!randomPotion.IsAssigned())
                randomPotion = potions[UnityEngine.Random.Range(0, potions.Count)];
        else // Mustn't be assigned
            while (randomPotion.IsAssigned())
                randomPotion = potions[UnityEngine.Random.Range(0, potions.Count)];

        return randomPotion;
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
        client.transform.position = entrancePosition.position;
        client.ChangeAnimationTo(client.walkAnim);
        client.turnText.text = "";
        client.turnNumber = -1;
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

    int CalculateTotalCapacity(List<Shelf> shelfList)
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

    int CalculateTotalLack(List<Shelf> shelfList)
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