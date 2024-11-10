using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "WaypointsEmpty", story: "Is [Waypoints] empty?", category: "Conditions", id: "d95c9a2d5a56c75d7fec69af9bbfc1c4")]
public partial class WaypointsEmptyCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    public override bool IsTrue()
    {
        return Waypoints.Value.Count <= 0;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
