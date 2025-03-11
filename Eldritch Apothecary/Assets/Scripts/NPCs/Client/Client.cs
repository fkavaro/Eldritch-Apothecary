using System;
using UnityEngine;

public class Client : AHumanoid
{
	public enum WantedService
	{
		OnlyShop,
		Sorcerer,
		Alchemist
	}
	int _scaresCount = 0;
	StackFiniteStateMachine clientSFSM;

	#region VARIABLES
	[Header("Client Properties")]
	[Tooltip("Desired service to be attended")]
	public WantedService wantedService;
	[Tooltip("Maximum waitin minutes"), Range(2, 10)]
	public int maxMinutesWaiting = 2;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int scareProbability = 3;
	[Tooltip("Maximum number of scares supported"), Range(1, 5)]
	public int maxScares = 3;
	[Tooltip("Triggering distance to cat"), Range(0f, 4f)]
	public float minDistanceToCat = 3f;
	//public GameObject[] clientModels;
	#endregion

	#region STATES
	public Stunned_ClientState stunnedState;
	public Shopping_ClientState shoppingState;
	public WaitForReceptionist_ClientState waitForReceptionistState;
	public Complaining_ClientState complainingState;
	public WaitForService_ClientState waitForServiceState;
	public AtSorcerer_ClientState atSorcererState;
	public PickPotionUp_ClientState pickPotionUpState;
	public Leaving_ClientState leavingState;
	#endregion

	protected override void OnAwake()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 11); // Chooses a random number of minutes to wait
		scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

		// Instantiates a random model
		//Instantiate(clientModels[UnityEngine.Random.Range(0, clientModels.Length)], transform.position, Quaternion.identity, transform);
		//clientModels[UnityEngine.Random.Range(0, clientModels.Length)].SetActive(true);

		base.OnAwake(); // Sets agent and animator components
	}

	protected override void OnStart()
	{

	}

	protected override void OnUpdate()
	{
		if (!HasReachedMaxScares()) ReactToCat();

		base.OnUpdate(); // Checks animation
	}

	protected override ADecisionSystem CreateDecisionSystem()
	{
		clientSFSM = new(this);

		stunnedState = new(clientSFSM, this);
		shoppingState = new(clientSFSM, this);
		waitForReceptionistState = new(clientSFSM, this);
		complainingState = new(clientSFSM, this);
		waitForServiceState = new(clientSFSM, this);
		atSorcererState = new(clientSFSM, this);
		pickPotionUpState = new(clientSFSM, this);
		leavingState = new(clientSFSM, this);

		clientSFSM.SetInitialState(shoppingState);

		return clientSFSM;
	}

	/// <summary>
	/// Checks if the cat is close enough to scare the client
	/// </summary>
	void ReactToCat()
	{
		// Cat is close and is scared
		if (Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position) < minDistanceToCat &&
			UnityEngine.Random.Range(0, 10) < scareProbability)
		{
			_scaresCount++;
			clientSFSM.SwitchState(stunnedState);
		}
	}

	/// <returns>If client has been scared enough times</returns>
	public bool HasReachedMaxScares()
	{
		return _scaresCount >= maxScares;
	}

	public void Release()
	{
		ApothecaryManager.Instance.clientsPool.Release(this);
	}
}