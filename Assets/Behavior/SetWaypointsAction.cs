using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetWaypoints", story: "Set [waypoints] from [patrollingLocations]", category: "Action", id: "db3c4a347e00f2db33d1f5235d90a273")]
public partial class SetWaypointsAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    [SerializeReference] public BlackboardVariable<GameObject> PatrollingLocations;

    protected override Status OnStart()
    {
        
        for (int i = 0; i < PatrollingLocations.Value.transform.childCount; i++)
        {
            Waypoints.Value.Add(PatrollingLocations.Value.transform.GetChild(i).gameObject);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

