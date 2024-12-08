using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
namespace COMP396.Goap
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(AgentBehaviour))]
	public class AgentMoveBehaviour : ValidatedMonoBehaviour
	{
		[Header("References")]
		[SerializeField, Self] private NavMeshAgent agent;
		[SerializeField, Self] private AgentBehaviour behaviour;
		[Header("Variables")]
		[SerializeField] private float MinMoveDistance = 0.25f;
		private ITarget currentTarget;
		private Vector3 lastPosition;
		private void OnEnable() => behaviour.Events.OnTargetChanged += TargetChanged;
		private void OnDisable() => behaviour.Events.OnTargetChanged -= TargetChanged;
		private void TargetChanged(ITarget target, bool inRange)
		{
			currentTarget = target;
			lastPosition = currentTarget.Position;
			agent.SetDestination(currentTarget.Position);
		}
		private void Update()
		{
			if (currentTarget == null) { return; }
			if (MinMoveDistance <= Vector3.Distance(currentTarget.Position, lastPosition))
			{
				lastPosition = currentTarget.Position;
				agent.SetDestination(lastPosition);
			}
		}
	}
}