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
		SHOPPING,
		SPELL,
		POTION
	}

	public enum Personality
	{

	}

	#region PUBLIC PROPERTIES
	[Header("Wanted Service Properties")]
	[Tooltip("Desired service to be attended")]
	public WantedService wantedService;
	[Tooltip("Turn number assigned to this client")]
	public int turnNumber = -1;

	[Header("Wait Properties")]
	[Tooltip("Normalized between 0 and the maximum waiting time"), Range(0f, 1f)]
	public float normalisedWaitingTime;
	[Tooltip("Seconds waiting first in line")]
	public float secondsWaiting = 0f;
	[Tooltip("Maximum waiting minutes"), Range(1, 3)]
	public int maxMinutesWaiting = 2;
	[Tooltip("Triggering distance to cat"), Range(0.5f, 2f)]
	public float minDistanceToCat = 1;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int fear = 10;
	[Tooltip("Minimum seconds between scares"), Range(10f, 120f)]
	public float minSecondsBetweenScares = 30f;
	[Tooltip("Maximum number of scares supported"), Range(1, 3)]
	public int maxScares = 2;
	public int scaresCount = 0;

	[HideInInspector] public float lastScareTime = -Mathf.Infinity;
	[HideInInspector] public TextMeshProUGUI turnText;
	#endregion

	#region PRIVATE PROPERTIES
	StackFiniteStateMachine<Client> _fsm;
	UtilitySystem<Client> _us;
	TextMeshProUGUI _serviceText;
	#endregion

	#region ACTIONS
	public FSM_ClientAction fsmAction;
	public StunnedByCat_ClientAction stunnedByCatAction;
	public Complain_ClientAction complainAction;
	#endregion

	#region INHERITED METHODS
	protected override ADecisionSystem<Client> CreateDecisionSystem()
	{
		// Finite State Machine
		_fsm = new(this);
		// Utility System
		_us = new(this);

		// Action
		fsmAction = new(_us, _fsm);
		stunnedByCatAction = new(_us);
		complainAction = new(_us);

		return _us;
	}

	protected override void OnAwake()
	{
		base.OnAwake();

		_serviceText = debugCanvas.Find("Service").GetComponent<TextMeshProUGUI>();
		turnText = debugCanvas.Find("Turn").GetComponent<TextMeshProUGUI>();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		// Normalise time between 0 and the 1 as the maximum waiting time of the client
		normalisedWaitingTime = Mathf.Clamp01(secondsWaiting / (maxMinutesWaiting * 60f));
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

	public void DontMindAnything()
	{
		scaresCount = 0;
		secondsWaiting = 0f;
		normalisedWaitingTime = 0f;
		fear = 0;
		isExecutionPaused = false;
	}

	public override bool CatIsBothering()
	{
		float currentDistanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);
		bool enoughTimeSinceLastScare = (Time.time - lastScareTime) >= minSecondsBetweenScares;

		if (currentDistanceToCat < minDistanceToCat // Cat is close
		&& _us.IsCurrentAction(fsmAction) // Executing SFSM (not stunned nor complaining)
		&& !_fsm.IsCurrentState(fsmAction.leavingState) // Not leaving
		&& enoughTimeSinceLastScare // Enough time has passed since last scare
		&& !ApothecaryManager.Instance.waitingQueue.Contains(this) // Not in waiting queue
																   //&& UnityEngine.Random.Range(0, 10) < fear) // Checks scare probability
		)
			return true;
		else
			return false;
	}

	/// <returns>If client has been scared enough times</returns>
	public bool TooScared()
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
		maxMinutesWaiting = UnityEngine.Random.Range(1, 4); // Chooses a random number of minutes to wait
		maxScares = UnityEngine.Random.Range(1, 4); // Chooses a random number of supported scares

		//minDistanceToCat = UnityEngine.Random.Range(0.5f, 2f); // Chooses a random distance to cat
		//fear = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability

		_serviceText.text = wantedService.ToString();
	}

	public void ResetWaitingTime()
	{
		secondsWaiting = 0f;
		normalisedWaitingTime = 0f;
	}
	#endregion
}