using UnityEngine;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
namespace COMP396.Goap
{
	public class WanderAction : ActionBase<WanderAction.Data>, IInjectable
	{
		public WanderStatsSO wanderStats;
		public class Data : IActionData
		{
			public ITarget Target { get; set; }
			public float Timer { get; set; }
		}
		public override void Created() { }

		public override void Start(IMonoAgent agent, Data data)
		{
			data.Timer = Random.Range(wanderStats.MinTimeBetweenWandering, wanderStats.MaxTimeBetweenWandering);
		}
		public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
		{
			data.Timer -= context.DeltaTime;
			if (data.Timer > 0)
			{
				return ActionRunState.Continue;
			}
			return ActionRunState.Stop;
		}
		public override void End(IMonoAgent agent, Data data) { }
		public void Inject(DependencyInjection injection)
		{
			wanderStats = injection.wanderStats;
		}
	}
}