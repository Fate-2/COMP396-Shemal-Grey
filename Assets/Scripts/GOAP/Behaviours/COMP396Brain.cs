using CrashKonijn.Goap.Behaviours;
using KBCore.Refs;
using UnityEngine;
namespace COMP396.Goap
{
	[RequireComponent(typeof(AgentBehaviour))]
	public class COMP396Brain : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private AgentBehaviour brain;
		[SerializeField, Scene] private GoapRunnerBehaviour runner;
		private void Awake()
		{
			brain.GoapSet = runner.GetGoapSet("COMP396");
		}
		private void Start()
		{
			brain.SetGoal<WanderGoal>(false);
		}
	}
}