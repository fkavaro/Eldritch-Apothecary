using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Client class that represents a client in the game.
/// </summary>
public class Client : AHumanoid<Client>
{
	public enum WantedService
	{
		OnlyShop,
		Sorcerer,
		Alchemist
	}

	#region PUBLIC PROPERTIES
	[Header("Client Properties")]
	[Tooltip("Desired service to be attended")]
	public WantedService wantedService;

	[Tooltip("Maximum waiting minutes"), Range(1, 4)]
	public int maxMinutesWaiting = 2;
	[Tooltip("Seconds waiting first in line")]
	public float timeWaiting = 0f;
	[Tooltip("Normalized between 0 and the maximum waiting time"), Range(0, 1)]
	public float normalizedWaitingTime;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int scareProbability = 3;
	[Tooltip("Maximum number of scares supported"), Range(1, 5)]
	public int maxScares = 3;
	#endregion

	#region PRIVATE PROPERTIES
	int _scaresCount = 0;
	StackFiniteStateMachine<Client> _clientSFSM;
	TextMeshProUGUI _serviceText;
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

	#region INHERITED METHODS
	protected override ADecisionSystem<Client> CreateDecisionSystem()
	{
		// Stack Finite State Machine
		_clientSFSM = new(this);

		// States initialization
		stunnedState = new(_clientSFSM);
		shoppingState = new(_clientSFSM);
		waitForReceptionistState = new(_clientSFSM);
		complainingState = new(_clientSFSM);
		waitForServiceState = new(_clientSFSM);
		atSorcererState = new(_clientSFSM);
		pickPotionUpState = new(_clientSFSM);
		leavingState = new(_clientSFSM);

		// Initial state according to client's wanted service
		// TODO: can shop although wanted service is sorcerer or alchemist
		if (wantedService == WantedService.OnlyShop)
			_clientSFSM.SetInitialState(shoppingState);
		else
			_clientSFSM.SetInitialState(waitForReceptionistState);

		return _clientSFSM;
	}

	protected override void OnUpdate()
	{
		if (_serviceText.gameObject.activeSelf != debugMode)
			_serviceText.gameObject.SetActive(debugMode);

		if (!HasReachedMaxScares()) ReactToCat();
	}
	#endregion

	#region PUBLIC METHODS
	/// <returns>If client has been scared enough times</returns>
	public bool HasReachedMaxScares()
	{
		return _scaresCount >= maxScares;
	}

	/// <summary>
	/// Resets the client's properties and behaviour
	/// </summary>
	public void Reset()
	{
		RandomizeProperties();
		IsStopped(false); // ANPC
		ResetBehaviour(); // ABehaviourController
	}

	public bool FirstInLineTooLong()
	{
		return timeWaiting >= maxMinutesWaiting * 60f;
	}

	public bool InWaitingQueue()
	{
		return ApothecaryManager.Instance.waitingQueue.Contains(this);
	}

	public void EnterWaitingQueue()
	{
		ApothecaryManager.Instance.waitingQueue.Enter(this);
	}

	public bool IsTurn()
	{
		return ApothecaryManager.Instance.IsTurn(this);
	}
	#endregion

	#region PRIVATE	METHODS
	/// <summary>
	/// Checks if the cat is close enough to scare the client
	/// </summary>
	void ReactToCat()
	{
		// Cat is close and client is scared
		if (Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position) < minDistanceToCat &&
			UnityEngine.Random.Range(0, 10) < scareProbability)
		{
			_scaresCount++;
			_clientSFSM.SwitchState(stunnedState);
		}
	}

	/// <summary>
	/// Randomizes client properties: wanted service, max minutes waiting, scare probability and max scares
	/// </summary>
	void RandomizeProperties()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 5); // Chooses a random number of minutes to wait
		scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

		if (_serviceText == null)
			_serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

		_serviceText.text = wantedService.ToString();
	}
	#endregion
}