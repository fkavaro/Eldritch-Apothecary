using System;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

namespace Apothecary.NPCs
{
	public enum Service
	{
		None,
		Sorcerer,
		Alchemist
	}

	public class Client : BehaviourRunner
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

		int _scaresCount = 0, minDistanceToCat = 3;
		float _maxSecondsWaiting;
		Transform _cat;
		BSRuntimeDebugger _debugger;

		protected override void Init()
		{
			_debugger = GetComponent<BSRuntimeDebugger>();
			wantedService = (Service)UnityEngine.Random.Range(0, 3); // Chooses a service randomly
			maxMinutesWaiting = UnityEngine.Random.Range(2, 11); // Chooses a random number of minutes to wait
			scareProbability = UnityEngine.Random.Range(0, 11); // Chooses a random scare probability
			maxScares = UnityEngine.Random.Range(1, 6); // Chooses a random number of supported scares

			_maxSecondsWaiting = maxMinutesWaiting * 60; // From minutes to seconds

			base.Init(); // Calls the base class method
		}

		protected override BehaviourGraph CreateGraph()
		{
			#region ACTIONS
			WalkAction shopping = new(ApothecaryManager.Instance.shopStands[0].position);
			WaitInLineAction waitingInLine = new();
			#endregion

			#region STATES
			FSM clientFSM = new();

			// Is startled by the grumpy cat 
			State StunnedByCat = clientFSM.CreateState("StunnedByCat");

			// Takes products on the stands (Optional)
			State Shopping = clientFSM.CreateState("Shopping", shopping);
			// Waits in line to be attended by the receptionist 
			State WaitingForReceptionist = clientFSM.CreateState("WaitingForReceptionist", waitingInLine);
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
			// Cat is close and client is scared
			ConditionPerception stunnedByCat = new(ReactToCat);
			// Timer
			UnityTimePerception desperate = new(_maxSecondsWaiting);
			// Turn for receptionist
			ConditionPerception receptionistTurn = new(CheckReceptionistTurn);
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
			clientFSM.CreateTransition(Shopping, WaitingForReceptionist, statusFlags: StatusFlags.Success);
			clientFSM.CreateTransition(WaitingForReceptionist, TalkingToReceptionist, receptionistTurn);
			clientFSM.CreateTransition(TalkingToReceptionist, WaitingForService, anyServiceIsWanted);
			clientFSM.CreateTransition(TalkingToReceptionist, Leaving, anyServiceIsWanted); //TODO!FIX: !anyServiceIsWanted
			clientFSM.CreateTransition(WaitingForService, VisitingSorcerer, sorcererTurn);
			clientFSM.CreateTransition(WaitingForService, CollectingPotion, isPotionReady);
			clientFSM.CreateTransition(VisitingSorcerer, Leaving, statusFlags: StatusFlags.Finished);
			clientFSM.CreateTransition(CollectingPotion, Leaving, statusFlags: StatusFlags.Finished);
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
		/// Checks if the cat is close enough to scare the client
		/// </summary>
		private bool ReactToCat()
		{
			// Cat is close and is scared
			if (Vector3.Distance(transform.position, _cat.transform.position) < minDistanceToCat &&
				UnityEngine.Random.Range(0, 10) < scareProbability)
			{
				_scaresCount++;
				return true;
			}
			return false;
		}
	}
}