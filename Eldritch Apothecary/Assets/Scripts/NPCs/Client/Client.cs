using System;
using UnityEngine;
using TMPro;

public class Client : AHumanoid
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

	[Tooltip("Maximum waiting minutes"), Range(2, 10)]
	public int maxMinutesWaiting = 2;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int scareProbability = 3;
	[Tooltip("Maximum number of scares supported"), Range(1, 5)]
	public int maxScares = 3;
	#endregion

	#region PRIVATE PROPERTIES
	int _scaresCount = 0;
	StackFiniteStateMachine clientSFSM;
	TextMeshProUGUI serviceText;
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

	protected override ADecisionSystem CreateDecisionSystem()
	{
		// Stack Finite State Machine
		clientSFSM = new(this);

		// States initialization
		stunnedState = new(clientSFSM, this);
		shoppingState = new(clientSFSM, this);
		waitForReceptionistState = new(clientSFSM, this);
		complainingState = new(clientSFSM, this);
		waitForServiceState = new(clientSFSM, this);
		atSorcererState = new(clientSFSM, this);
		pickPotionUpState = new(clientSFSM, this);
		leavingState = new(clientSFSM, this);

		// Initial state according to client's wanted service
		// TODO: can shop although wanted service is sorcerer or alchemist
		if (wantedService == WantedService.OnlyShop)
			clientSFSM.SetInitialState(shoppingState);
		else
			clientSFSM.SetInitialState(waitForReceptionistState);

		return clientSFSM;
	}

	protected override void OnStart() { }

	protected override void OnUpdate()
	{
		if (serviceText.gameObject.activeSelf != debugMode)
			serviceText.gameObject.SetActive(debugMode);

		if (!HasReachedMaxScares()) ReactToCat();
	}

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
		ReactivateAgent(); // ANPC
		ResetBehaviour(); // ABehaviourController
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
			clientSFSM.SwitchState(stunnedState);
		}
	}

	/// <summary>
	/// Randomizes client properties: wanted service, max minutes waiting, scare probability and max scares
	/// </summary>
	void RandomizeProperties()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 11); // Chooses a random number of minutes to wait
		scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

		if (serviceText == null)
			serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

		serviceText.text = wantedService.ToString();
	}
	#endregion
}