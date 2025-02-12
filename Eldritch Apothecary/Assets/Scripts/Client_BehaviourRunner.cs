using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;

public class Client_BehaviourRunner : BehaviourRunner
{

	protected override BehaviourGraph CreateGraph()
	{
		FSM Client_FSM = new FSM();

		State Waiting = Client_FSM.CreateState();

		return Client_FSM;
	}
}
