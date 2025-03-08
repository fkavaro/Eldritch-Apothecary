using System;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEditor;


public class Client : ANPC
{
	public enum WantedService
	{
		OnlyShop,
		Sorcerer,
		Alchemist
	}
	int _scaresCount = 0;
	bool isTalking, isWaiting;
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
	#endregion

	#region STATES
	public Stunned_ClientState stunnedState; // Is startled by the grumpy cat 
	public Shopping_ClientState shoppingState; // Takes products on the stands (Optional)
	public WaitForReceptionist_ClientState waitForReceptionistState;// Waits in line to be attended by the receptionist 
	public Complaining_ClientState complainingState; // Complains to the receptionist
	#endregion

	#region ANIMATIONS
	readonly public int Talking = Animator.StringToHash("Talking"),
		PickUp = Animator.StringToHash("PickUp"),
		Complaining = Animator.StringToHash("Complaining"),
		Stunned = Animator.StringToHash("Stunned"),
		SatDown = Animator.StringToHash("SatDown");
	#endregion

	protected override void OnAwake()
	{
		wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 11); // Chooses a random number of minutes to wait
		scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

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
		#region ACTIONS
		//WalkAction shopping = new(ApothecaryManager.Instance.shopStands[0].position);
		//WaitInLineAction waitingInLine = new();
		#endregion

		#region STATES
		clientSFSM = new(this);

		// Is startled by the grumpy cat 
		//State StunnedByCat = clientFSM.CreateState("StunnedByCat");
		stunnedState = new(clientSFSM, this);

		// Takes products on the stands (Optional)
		//State Shopping = clientFSM.CreateState("Shopping", shopping);
		shoppingState = new(clientSFSM, this);

		// Waits in line to be attended by the receptionist 
		//State WaitingForReceptionist = clientFSM.CreateState("WaitingForReceptionist", waitingInLine);
		waitForReceptionistState = new(clientSFSM, this);

		// Attended by the receptionist
		//State TalkingToReceptionist = clientFSM.CreateState("AttendedByReceptionist");
		// Complains to the receptionist if waited too much time
		complainingState = new(clientSFSM, this);
		// Waits sat down to be attended by the sorcerer
		//State WaitingForService = clientFSM.CreateState("WaitingForService");

		// a. SORCERER
		// Goes to the sorcerer's room
		//State GoingToSorcererRoom = clientFSM.CreateState("GoingToSorcererRoom");
		// Attended by sorcerer
		//State VisitingSorcerer = clientFSM.CreateState("VisitingSorcerer");

		// b. ALCHEMIST
		// Takes the prepared potion
		//State CollectingPotion = clientFSM.CreateState("TakingPotion");

		// Leaves the apothecary | Cancels the purchase if stunned too many times by the cat
		//State Leaving = clientFSM.CreateState("Leaving");

		// Initial state
		clientSFSM.SetInitialState(shoppingState);
		#endregion

		#region PULL PERCEPTIONS
		// Cat is close and client is scared
		//ConditionPerception stunnedByCat = new(ReactToCat);
		// Timer
		//UnityTimePerception desperate = new(_maxSecondsWaiting);
		// Turn for receptionist
		//ConditionPerception receptionistTurn = new(CheckReceptionistTurn);
		// Something else (bool)
		//ConditionPerception anyServiceIsWanted = new(CheckIfWantedService);
		// Turn for sorcerer
		//ConditionPerception sorcererTurn = new(CheckSorcererTurn);
		// Potion ready
		//ConditionPerception isPotionReady = new(IsPotionReady);
		#endregion

		#region PUSH PERCEPTIONS

		#endregion

		#region TRANSITIONS
		// clientFSM.CreateTransition(Shopping, WaitingForReceptionist, statusFlags: StatusFlags.Success);
		// clientFSM.CreateTransition(WaitingForReceptionist, TalkingToReceptionist, receptionistTurn);
		// clientFSM.CreateTransition(TalkingToReceptionist, WaitingForService, anyServiceIsWanted);
		// clientFSM.CreateTransition(TalkingToReceptionist, Leaving, anyServiceIsWanted); //TODO!FIX: !anyServiceIsWanted
		// clientFSM.CreateTransition(WaitingForService, VisitingSorcerer, sorcererTurn);
		// clientFSM.CreateTransition(WaitingForService, CollectingPotion, isPotionReady);
		// clientFSM.CreateTransition(VisitingSorcerer, Leaving, statusFlags: StatusFlags.Finished);
		// clientFSM.CreateTransition(CollectingPotion, Leaving, statusFlags: StatusFlags.Finished);
		// TODO: StunnedByCat transitions FROM ALL with StackFSM?
		#endregion

		//_debugger.RegisterGraph(clientFSM, "Client_FSM");
		return clientSFSM;
	}

	/// <summary>
	/// Checks if the cat is close enough to scare the client
	/// </summary>
	private void ReactToCat()
	{
		// Cat is close and is scared
		if (Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position) < minDistanceToCat &&
			UnityEngine.Random.Range(0, 10) < scareProbability)
		{
			_scaresCount++;
			clientSFSM.SwitchState(stunnedState);
		}
	}

	public bool HasReachedMaxScares()
	{
		return _scaresCount >= maxScares;
	}

	// public override void CheckAnimation()
	// {
	// 	if (isTalking)
	// 	{
	// 		ChangeAnimationTo(Talking);
	// 		StopAgent();
	// 		//RotateTowardsNPC(otherNPC.position);
	// 	}
	// 	else
	// 	{
	// 		if (isWaiting)
	// 			ChangeAnimationTo(Idle);
	// 		else
	// 			ChangeAnimationTo(Moving);
	// 	}
	// }
}