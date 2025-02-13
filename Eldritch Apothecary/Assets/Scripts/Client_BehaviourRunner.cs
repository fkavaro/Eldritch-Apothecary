using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public enum Service
{
	None,
	Sorcerer,
	Alchemist
}

public class Client_BehaviourRunner : BehaviourRunner
{
	#region VARIABLES
	[Header("Client Properties")]
	[Tooltip("Desired service to be attended")]
	public Service wantedService;
	[Tooltip("Maximum minutes willing to wait"), Range(2, 10)]
	public int maxMinutesWaiting = 2;
	[Tooltip("Probability of being scared"), Range(0, 10)]
	public int scareProbability = 3;
	[Tooltip("Maximum number of scares supported"), Range(1, 5)]
	public int maxScares = 3;
	#endregion

	int _scaresCount = 0;
	Collider _visionCollider;
	Transform _cat;
	BSRuntimeDebugger _debugger;

	protected override void Init()
	{
		_debugger = GetComponent<BSRuntimeDebugger>();
		wantedService = (Service)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
		maxMinutesWaiting = UnityEngine.Random.Range(2, 11); // Chooses a random number of minutes to wait
		scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
		maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares
		base.Init(); // Calls the base class method
	}

	protected override BehaviourGraph CreateGraph()
	{
		FSM clientFSM = new();

		#region STATES
		// Takes products on the stands (Optional)
		State Shopping = clientFSM.CreateState("Shopping");
		// Is startled by the grumpy cat 
		State StunnedByCat = clientFSM.CreateState("StunnedByCat");
		// Waits in line to be attended by the receptionist 
		State WaitingForReceptionist = clientFSM.CreateState("WaitingForReceptionist");
		// Attended by the receptionist
		State TalkingToReceptionist = clientFSM.CreateState("AttendedByReceptionist");
		// Complains to the receptionist if waited too much time
		State Complaining = clientFSM.CreateState("Complaining");
		// Waits sat down to be attended by the sorcerer
		State WaitingForService = clientFSM.CreateState("WaitingForService");

		// a. SORCERER
		// Goes to the sorcerer's room
		//State GoingToSorcererRoom = clientFSM.CreateState("GoingToSorcererRoom");
		// Attended by sorcerer
		State VisitingSorcerer = clientFSM.CreateState("VisitingSorcerer");

		// b. ALCHEMIST
		// Takes the prepared potion
		State CollectingPotion = clientFSM.CreateState("TakingPotion");

		// Leaves the apothecary | Cancels the purchase if stunned too many times by the cat
		State Leaving = clientFSM.CreateState("Leaving");

		// Initial state
		clientFSM.SetEntryState(Shopping);
		#endregion

		#region PULL PERCEPTIONS
		ConditionPerception stunnedByCat = new(ReactToCat);
		UnityTimePerception desperate = new(maxMinutesWaiting * 60); // From minutes to seconds

		// Turn for receptionist
		//ConditionPerception receptionistTurn = new(CheckReceptionistTurn);
		// Something else (bool)
		ConditionPerception anyServiceIsWanted = new(CheckIfWantedService);
		// Turn for sorcerer
		ConditionPerception sorcererTurn = new(CheckSorcererTurn);
		// Potion ready
		ConditionPerception isPotionReady = new(IsPotionReady);
		#endregion

		#region PUSH PERCEPTIONS

		#endregion

		#region TRANSITIONS
		clientFSM.CreateTransition("Shopping -> WaitingForReceptionist", Shopping, WaitingForReceptionist, statusFlags: StatusFlags.Finished);
		clientFSM.CreateTransition("WaitingForReceptionist -> TalkingToReceptionist", WaitingForReceptionist, TalkingToReceptionist, statusFlags: StatusFlags.Finished);
		clientFSM.CreateTransition("TalkingToReceptionist -> WaitingForService", TalkingToReceptionist, WaitingForService, anyServiceIsWanted);
		clientFSM.CreateTransition("TalkingToReceptionist -> Leaving", TalkingToReceptionist, Leaving, anyServiceIsWanted); //!FIX: !anyServiceIsWanted
		clientFSM.CreateTransition("WaitingForService -> VisitingSorcerer", WaitingForService, VisitingSorcerer, sorcererTurn);
		clientFSM.CreateTransition("WaitingForService -> CollectingPotion", WaitingForService, CollectingPotion, isPotionReady);
		clientFSM.CreateTransition("VisitingSorcerer -> Leaving", VisitingSorcerer, Leaving, statusFlags: StatusFlags.Finished);
		clientFSM.CreateTransition("CollectingPotion -> Leaving", CollectingPotion, Leaving, statusFlags: StatusFlags.Finished);
		// TODO: StunnedByCat transitions FROM ALL with StackFSM?
		#endregion

		_debugger.RegisterGraph(clientFSM, "Client_FSM");
		return clientFSM;
	}

	private bool CheckReceptionistTurn()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Checks if the client wants the sorcerer or alchemist service
	/// </summary>
	/// <returns></returns>
	private bool CheckIfWantedService()
	{
		return wantedService == Service.Sorcerer || wantedService == Service.Alchemist;
	}

	private bool CheckSorcererTurn()
	{
		throw new NotImplementedException();
	}
	private bool IsPotionReady()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Checks if the client is stunned by the cat
	/// </summary>
	private bool ReactToCat()
	{
		// TODO: Check colission with tag //////////////////
		if (_visionCollider.bounds.Contains(_cat.position))
		{
			Vector3 direction = (_cat.position - transform.position).normalized;
			Ray ray = new Ray(transform.position + transform.up, direction * 20);

			bool watchingCat = Physics.Raycast(ray, out RaycastHit hit, 20) && hit.collider.gameObject.transform == _cat;

			if (watchingCat)
				// Calculate random number and check if it's less than fear property
				if (UnityEngine.Random.Range(0, 10) < scareProbability)
					return true;
		}
		return false;
	}
}
