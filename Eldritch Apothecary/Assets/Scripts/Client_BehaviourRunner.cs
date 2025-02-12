using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class Client_BehaviourRunner : BehaviourRunner
{
	#region VARIABLES
	[Tooltip("Maximum minutes able to wait"), Range(2, 10)]
	public int patience = 2;
	[Tooltip("Maximum number of scares sopported"), Range(2, 5)]
	public int maxScares = 2;
	#endregion

	Collider _visionCollider;
	Transform _cat;
	BSRuntimeDebugger _debugger;

	protected override void Init()
	{
		_debugger = GetComponent<BSRuntimeDebugger>();
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
		State AttendedByReceptionist = clientFSM.CreateState("AttendedByReceptionist");
		// Complains to the receptionist if waited too much time
		State ComplainingToReceptionist = clientFSM.CreateState("ComplainingToReceptionist");

		// a. SORCERER
		// Waits sat down to be attended by the sorcerer
		State WaitingForSorcerer = clientFSM.CreateState("WaitingForSorcerer");
		// Goes to the sorcerer's room
		State GoingToSorcererRoom = clientFSM.CreateState("GoingToSorcererRoom");
		// Attended by sorcerer
		State VisitingSorcerer = clientFSM.CreateState("VisitingSorcerer");

		// b. ALCHEMIST		
		// Waits sat down while potion is made by the alchemist
		State WaitingForPotion = clientFSM.CreateState("WaitingForSorcerer");
		// Takes the prepared potion
		State TakingPotion = clientFSM.CreateState("TakingPotion");

		// Leaves the apothecary | Cancels the purchase if stunned too many times by the cat
		State Leaving = clientFSM.CreateState("Leaving");

		// Initial state
		clientFSM.SetEntryState(Shopping);
		#endregion

		#region PULL ERCEPTIONS
		ConditionPerception watchPlayer = new(CheckWatchCat);
		#endregion

		#region TRANSITIONS
		#endregion

		_debugger.RegisterGraph(clientFSM);
		return clientFSM;
	}

	/// <summary>
	/// Checks if the client is startled by the cat
	/// </summary>
	private bool CheckWatchCat()
	{
		// TODO: Check colission by tag. //////////////////
		if (_visionCollider.bounds.Contains(_cat.position))
		{
			Vector3 direction = (_cat.position - transform.position).normalized;
			Ray ray = new Ray(transform.position + transform.up, direction * 20);

			bool watchPlayer = Physics.Raycast(ray, out RaycastHit hit, 20) && hit.collider.gameObject.transform == _cat;

			return watchPlayer;
		}
		return false;
		//////////////////////////////////////////////////
	}
}
