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
	#endregion

	#region PRIVATE PROPERTIES
	StackFiniteStateMachine<Client> _clientSFSM;
	UtilitySystem<Client> _clientUS;
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

	#region ACTIONS
	EstateMachineAction<Client, StackFiniteStateMachine<Client>> fsmAction;
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
		if (wantedService == WantedService.OnlyShop)
			_clientSFSM.SetInitialState(shoppingState);
		else
			_clientSFSM.SetInitialState(waitForReceptionistState);

		// Utility System
		_clientUS = new(this);

		fsmAction = new(_clientUS, _clientSFSM);

		_clientUS.SetDefaultAction(fsmAction);

		return _clientUS;
	}

	protected override void OnUpdate()
	{
		if (_serviceText.gameObject.activeSelf != debugMode)
			_serviceText.gameObject.SetActive(debugMode);

		//if (!HasReachedMaxScares()) CatIsTooClose();
	}
	#endregion

	#region PUBLIC METHODS
	/// <summary>
	/// Resets the client's properties and behaviour
	/// </summary>
	public void Reset()
	{
		RandomizeProperties();
		SetIfStopped(false); // ANPC
		ResetBehaviour(); // ABehaviourController
	}

	/// <returns> True if the client has waited too long, false otherwise.</returns>
	public bool WaitedTooLong()
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

	public void DontMindAnything()
	{
		_scaresCount = 0;
		timeWaiting = 0f;
		normalizedWaitingTime = 0f;
		fear = 0;
	}
	#endregion

	#region PRIVATE	METHODS
	/// <summary>
	/// Randomizes client properties: wanted service, max minutes waiting, scare probability and max scares
	/// </summary>
	void RandomizeProperties()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 5); // Chooses a random number of minutes to wait
		fear = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

		if (_serviceText == null)
			_serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

		_serviceText.text = wantedService.ToString();
	}
	#endregion
}