using UnityEngine;
namespace COMP396.Goap
{
	[CreateAssetMenu(menuName = "AI/Goap Wander Stats", fileName = "WanderStats")]
	public class WanderStatsSO : ScriptableObject
	{
		public float MinTimeBetweenWandering = 1f;
		public float MaxTimeBetweenWandering = 3f;
		public float WanderingRadius = 5f;
	}
}