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
	public float secondsWaiting = 0f;
	[Tooltip("Normalized between 0 and the maximum waiting time"), Range(0, 1)]
	public float normalizedWaitingTime;
	[Tooltip("Triggering distance to cat"), Range(0.5f, 2f)]
	public float minDistanceToCat = 1;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int fear = 10;
	[Tooltip("Minimum seconds between scares"), Range(10f, 120f)]
	public float minSecondsBetweenScares = 30f;

	[Tooltip("Maximum number of scares supported"), Range(1, 10)]
	public int maxScares = 10;
	public int scaresCount = 0;
	[HideInInspector] public float lastScareTime = -Mathf.Infinity;
	#endregion

	#region PRIVATE PROPERTIES

	StackFiniteStateMachine<Client> _clientSFSM;
	UtilitySystem<Client> _clientUS;
	TextMeshProUGUI _serviceText;
	#endregion

	#region STATES
	public Shopping_ClientState shoppingState;
	public WaitForReceptionist_ClientState waitForReceptionistState;
	public WaitForService_ClientState waitForServiceState;
	public AtSorcerer_ClientState atSorcererState;
	public PickPotionUp_ClientState pickPotionUpState;
	public Leaving_ClientState leavingState;
	#endregion

	#region ACTIONS
	public StateMachineAction<Client, StackFiniteStateMachine<Client>> fsmAction;
	public StunnedByCat_ClientAction stunnedByCatAction;
	public Complain_ClientAction complainAction;
	#endregion

	#region INHERITED METHODS
	protected override ADecisionSystem<Client> CreateDecisionSystem()
	{
		// Stack Finite State Machine
		_clientSFSM = new(this);

		// States initialization
		shoppingState = new(_clientSFSM);
		waitForReceptionistState = new(_clientSFSM);
		waitForServiceState = new(_clientSFSM);
		atSorcererState = new(_clientSFSM);
		pickPotionUpState = new(_clientSFSM);
		leavingState = new(_clientSFSM);

		// Initial state
		// If the client wants to shop, set the shopping state as the initial state
		// There's also a chance to also go shopping although a service is wanted
		if (wantedService == WantedService.OnlyShop ||
			UnityEngine.Random.Range(0, 11) < 5) // 50% chance
			_clientSFSM.SetInitialState(shoppingState);
		else
			_clientSFSM.SetInitialState(waitForReceptionistState);

		// Utility System
		_clientUS = new(this);

		fsmAction = new(_clientUS, _clientSFSM);
		stunnedByCatAction = new(_clientUS);
		complainAction = new(_clientUS);

		_clientUS.SetDefaultAction(fsmAction);

		return _clientUS;
	}

	protected override void OnUpdate()
	{
		if (_serviceText.gameObject.activeSelf != debugMode)
			_serviceText.gameObject.SetActive(debugMode);

		// Normalize time between 0 and the 1 as the maximum waiting time of the client
		normalizedWaitingTime = Mathf.Clamp01(secondsWaiting / (maxMinutesWaiting * 60f));
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
		return secondsWaiting >= maxMinutesWaiting * 60f;
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
		scaresCount = 0;
		secondsWaiting = 0f;
		normalizedWaitingTime = 0f;
		fear = 0;
	}

	public void ForceState(AState<Client, StackFiniteStateMachine<Client>> newState)
	{
		_clientSFSM.ForceState(newState);
	}

	public override bool CatIsBothering()
	{
		float currentDistanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);
		bool enoughTimeSinceLastScare = (Time.time - lastScareTime) >= minSecondsBetweenScares;

		if (currentDistanceToCat < minDistanceToCat // Cat is close
		&& _clientUS.IsCurrentAction(fsmAction) // Executing SFSM (not stunned nor complaining)
		&& !_clientSFSM.IsCurrentState(leavingState) // Not leaving
		&& enoughTimeSinceLastScare // Enough time has passed since last scare
									//&& UnityEngine.Random.Range(0, 10) < fear) // Checks scare probability
		)
		{
			Debug.Log("Cat is bothering " + name);
			return true;
		}
		// Cat is too far or client is not scared
		else return false;
	}

	/// <returns>If client has been scared enough times</returns>
	public bool HasReachedMaxScares()
	{
		return scaresCount >= maxScares;
	}
	#endregion

	#region PRIVATE	METHODS
	/// <summary>
	/// Randomizes client properties: wanted service, max minutes waiting, scare probability and max scares
	/// </summary>
	void RandomizeProperties()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(1, 5); // Chooses a random number of minutes to wait
		maxScares = UnityEngine.Random.Range(1, 11); // Chooses a random number of supported scares

		//minDistanceToCat = UnityEngine.Random.Range(0.5f, 2f); // Chooses a random distance to cat
		//fear = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability

		if (_serviceText == null)
			_serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

		_serviceText.text = wantedService.ToString();
	}
	#endregion
}