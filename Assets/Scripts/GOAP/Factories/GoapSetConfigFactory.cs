using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
namespace COMP396.Goap
{
	public class GoapSetConfigFactory : GoapSetFactoryBase
	{
		public override IGoapSetConfig Create()
		{
			var factory = new GoapSetBuilder("COMP396");
			factory.AddGoal<WanderGoal>().AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
			factory.AddAction<WanderAction>()
				.SetTarget<WanderTarget>()
				.AddEffect<IsWandering>(EffectType.Increase)
				.SetBaseCost(5)
				.SetInRange(10);
			factory.AddTargetSensor<WanderTargetSensor>()
				.SetTarget<WanderTarget>();
			return factory.Build();
		}
	}
}